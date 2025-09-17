// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace Miyu.API.Requests.Channels;

public class StartTypingRequest : RestRequest<object>
{
    protected override string Path => $"/channels/{channelId}/typing";
    protected override HttpMethod Method => HttpMethod.Post;

    private ulong channelId { get; }

    public StartTypingRequest(ulong channelId)
    {
        this.channelId = channelId;
    }
}
