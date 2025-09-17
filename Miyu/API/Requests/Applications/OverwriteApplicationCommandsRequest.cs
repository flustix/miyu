// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Midori.Utils;
using Miyu.Models.Interaction.Application;
using Miyu.Utils;
using Newtonsoft.Json.Linq;

namespace Miyu.API.Requests.Applications;

public class OverwriteApplicationCommandsRequest : RestRequest<List<DiscordAppCommand>>
{
    protected override string Path => $"/applications/{Client.Self.ID}/commands";
    protected override HttpMethod Method => HttpMethod.Put;

    private List<DiscordAppCommand> commands { get; }

    public OverwriteApplicationCommandsRequest(List<DiscordAppCommand> commands)
    {
        this.commands = commands;
    }

    protected override object CreatePostData()
    {
        return commands.Select(x =>
        {
            var o = x.TurnTo<JObject>();
            o.Remove("application_id");
            o.Remove("id");
            o.Remove("version");
            return o;
        });
    }

    protected override List<DiscordAppCommand> Deserialize(string json)
    {
        var cmds = json.Deserialize<List<DiscordAppCommand>>()!;
        cmds.ForEach(x => x.Client = Client);
        return cmds.ToList();
    }
}
