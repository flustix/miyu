// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Models.Channels.Messages;

namespace Miyu.API.Requests.Channels;

public class GetMessagesRequest : RestRequest<List<DiscordMessage>>
{
    protected override string Path => $"/channels/{id}/messages?limit={limit}";

    private ulong id { get; }
    private int limit { get; }

    public GetMessagesRequest(ulong id, int limit = 50)
    {
        this.id = id;
        this.limit = limit;
    }
}
