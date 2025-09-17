// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Reflection;
using Midori.Logging;
using Miyu.Events;

namespace Miyu.Networking.Gateway;

public class GatewayEventHandler
{
    private MiyuClient client { get; }

    private Dictionary<EventType, List<(object, MethodInfo)>> handlers { get; } = new();

    public GatewayEventHandler(MiyuClient client)
    {
        this.client = client;
    }

    public void RegisterListener(EventType type, object target, MethodInfo handler)
    {
        handlers.TryAdd(type, new List<(object, MethodInfo)>());
        var list = handlers[type];
        list.Add((target, handler));
    }

    public void UnregisterListener(object target)
    {
        foreach (var (_, list) in handlers)
            list.RemoveAll(x => x.Item1 == target);
    }

    internal void DispatchEvent(GenericEvent ev)
    {
        if (!handlers.TryGetValue(ev.Type, out var list))
            return;

        Logger.Log($"Calling {ev} on {string.Join(", ", list.Select(h => $"{h.Item1.GetType().Name}.{h.Item2.Name}"))}", LoggingTarget.Info, LogLevel.Debug);

        foreach (var (target, method) in list)
        {
            try
            {
                var args = new List<object>();
                var expected = method.GetParameters();

                foreach (var p in expected)
                {
                    switch (p.ParameterType)
                    {
                        case { } t when t == typeof(MiyuClient):
                            args.Add(client);
                            break;

                        case { } t when t.BaseType == typeof(GenericEvent):
                            args.Add(ev);
                            break;

                        default:
                            throw new ArgumentException($"Unexpected type {p.ParameterType}.");
                    }
                }

                if (method.ReturnType == typeof(Task))
                {
                    var task = (Task)method.Invoke(target, args.ToArray())!;
                    task.Wait();

                    if (task.IsFaulted) throw task.Exception;
                }
                else
                    method.Invoke(target, args.ToArray());
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Failed to handle {ev.Type} event with '{target.GetType().Name}.{method.Name}()'!");
            }
        }
    }
}
