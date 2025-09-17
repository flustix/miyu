// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Midori.Utils;
using Miyu.Models.Channels;

namespace Miyu.API.Requests.Channels;

public class GetChannelRequest : RestRequest<DiscordChannel>
{
    protected override string Path => $"/channels/{channelId}";

    private ulong channelId { get; }

    public GetChannelRequest(ulong channelId)
    {
        this.channelId = channelId;
    }

    protected override DiscordChannel Deserialize(string json)
    {
        var channel = json.Deserialize<DiscordChannel>();
        channel.Client = Client;
        return channel;
    }
}
