// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Midori.Logging;
using Midori.Utils.Extensions;
using Miyu.API.Requests.Applications;
using Miyu.Attributes;
using Miyu.Events;
using Miyu.Events.Interactions;
using Miyu.Models.Channels;
using Miyu.Models.Channels.Messages.Attachment;
using Miyu.Models.Guilds.Roles;
using Miyu.Models.Interaction;
using Miyu.Models.Interaction.Application;
using Miyu.Models.Interaction.Data;
using Miyu.Models.Users;
using Miyu.Utils;
using Miyu.Utils.Extensions;

namespace Miyu;

public partial class MiyuClient
{
    private ConcurrentDictionary<SlashCommandAttribute, (object, MethodInfo)> commands { get; } = new();

    public void RegisterCommands(object obj)
    {
        const BindingFlags flags = BindingFlags.Default | BindingFlags.Instance | BindingFlags.Public;

        var type = obj.GetType();
        var methods = type.GetMethods(flags)
                          .Where(m => m.GetCustomAttributes(typeof(SlashCommandAttribute), false).Length > 0);

        foreach (var method in methods)
        {
            var attr = method.GetCustomAttribute<SlashCommandAttribute>()!;
            Logger.Add($"Registering /{attr.Name}.");
            commands[attr] = (obj, method);
        }
    }

    /// <summary>
    ///     Builds and updates all commands to discord.
    /// </summary>
    [EventListener(EventType.Ready)]
    public void FlushCommands()
    {
        if (!Config.RegisterCommands)
            return;

        var list = new List<DiscordAppCommand>();

        foreach (var (attr, (inst, method)) in commands)
        {
            var builder = new DiscordAppCommand
            {
                Type = DiscordAppCommandType.ChatInput,
                Name = attr.Name,
                Description = attr.Description
            };

            var param = method.GetParameters();

            foreach (var info in param)
            {
                DiscordAppCommandOptionType? type = null;

                var pType = info.ParameterType;
                var nType = Nullable.GetUnderlyingType(pType);

                if (nType is not null)
                    pType = nType;

                switch (pType.Name)
                {
                    case nameof(MiyuClient):
                    case nameof(DiscordInteraction):
                        continue;

                    case nameof(String):
                        type = DiscordAppCommandOptionType.String;
                        break;

                    case nameof(Int32):
                        type = DiscordAppCommandOptionType.Integer;
                        break;

                    case nameof(Boolean):
                        type = DiscordAppCommandOptionType.Boolean;
                        break;

                    case nameof(DiscordUser):
                        type = DiscordAppCommandOptionType.User;
                        break;

                    case nameof(DiscordChannel):
                        type = DiscordAppCommandOptionType.Channel;
                        break;

                    case nameof(DiscordRole):
                        type = DiscordAppCommandOptionType.Role;
                        break;

                    case nameof(Double):
                        type = DiscordAppCommandOptionType.Number;
                        break;

                    default:
                        Logger.Add($"{pType}");
                        continue;
                }

                if (type is null)
                    throw new ArgumentException($"Unexpected parameter of type {pType}.");

                var b = new DiscordAppCommandOption
                {
                    Type = type.Value,
                    Name = info.Name!,
                    Description = $"description for {info.Name}",
                    Required = !info.IsNullable()
                };

                if (type is DiscordAppCommandOptionType.Integer or DiscordAppCommandOptionType.Number)
                {
                    var range = info.GetCustomAttribute<RangeAttribute>();

                    if (range != null)
                    {
                        b.MinValue = Convert.ToDouble(range.Minimum);
                        b.MaxValue = Convert.ToDouble(range.Maximum);
                    }
                }
                else if (type == DiscordAppCommandOptionType.String)
                {
                    var length = info.GetCustomAttribute<LengthAttribute>();

                    if (length != null)
                    {
                        b.MinLength = length.MinimumLength;
                        b.MinLength = length.MaximumLength;
                    }
                }

                builder.Options ??= [];
                builder.Options.Add(b);
            }

            list.Add(builder);
        }

        _ = API.Execute(new OverwriteApplicationCommandsRequest(list));
    }

    [EventListener(EventType.InteractionCommand)]
    public Task HandleCommand(InteractionCommandEvent ev)
    {
        var interact = ev.Interaction;

        if (interact.Type != DiscordInteractionType.ApplicationCommand)
            return Task.CompletedTask;

        var data = interact.Data!.TurnTo<InteractionCommandData>();
        var handler = commands.FirstOrDefault(pair => pair.Key.Name.Equals(data.Name, StringComparison.InvariantCultureIgnoreCase));

        var (_, (target, method)) = handler;

        if (target is null || method is null)
        {
            Logger.Add($"No handler for /{data.Name}.", LogLevel.Warning);
            return Task.CompletedTask;
        }

        var param = method.GetParameters();
        var args = new List<object?>();

        foreach (var info in param)
        {
            var pName = info.ParameterType.Name;
            Logger.Add($"{pName} {info.Name}");

            switch (info.ParameterType.Name)
            {
                case nameof(DiscordInteraction):
                    args.Add(interact);
                    break;

                case nameof(MiyuClient):
                    args.Add(this);
                    break;

                default:
                {
                    if (!tryParseArgument(interact, args, info))
                        throw new Exception($"Unexpected argument of type {info.ParameterType}.");

                    break;
                }
            }
        }

        if (method.ReturnType == typeof(Task))
        {
            var ret = (Task)method.Invoke(target, args.ToArray())!;
            ret.Wait();

            Logger.Add($"{ret.IsCompleted} {ret.IsCanceled} {ret.IsFaulted} {ret.Exception}");

            if (ret.Exception is not null)
                throw ret.Exception;

            return Task.CompletedTask;
        }

        method.Invoke(target, args.ToArray());
        return Task.CompletedTask;
    }

    private bool tryParseArgument(DiscordInteraction interaction, List<object?> args, ParameterInfo parameter)
    {
        var nullable = parameter.IsNullable();
        object? result = null;

        var pType = parameter.ParameterType;
        var nType = Nullable.GetUnderlyingType(pType);

        if (nType is not null)
            pType = nType;

        switch (pType.Name)
        {
            case nameof(String): // 3
                result = interaction.GetString(parameter.Name!);
                break;

            case nameof(Int32): // 4
                result = interaction.GetInteger(parameter.Name!);
                break;

            case nameof(Boolean): // 5
                result = interaction.GetBool(parameter.Name!);
                break;

            case nameof(DiscordUser): // 6
                // result = interaction.GetUser(parameter.Name!);
                break;

            case nameof(DiscordChannel): // 7
                // result = interaction.GetChannel(parameter.Name!);
                break;

            case nameof(DiscordRole): // 8
                // result = interaction.GetRole(parameter.Name!);
                break;

            case nameof(Double): // 10
                result = interaction.GetNumber<double>(parameter.Name!);
                break;

            case nameof(DiscordAttachment): // 11
                // result = interaction.GetAttachment(parameter.Name!);
                break;

            default:
                return false;
        }

        Logger.Add($"{pType} {parameter.Name}: {result}");

        if (result is null && !nullable)
            throw new ArgumentException();

        args.Add(result);
        return true;
    }
}
