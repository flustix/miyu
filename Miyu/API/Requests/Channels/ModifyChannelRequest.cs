// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.API.Payloads.Channel;

namespace Miyu.API.Requests.Channels;

public class ModifyChannelRequest : RestRequest<object>
{
    protected override string Path => $"/channels/{id}";
    protected override HttpMethod Method => HttpMethod.Patch;

    private ulong id { get; }
    private GuildChannelEditPayload data { get; }

    public ModifyChannelRequest(ulong id, GuildChannelEditPayload data)
    {
        this.id = id;
        this.data = data;
    }

    protected override object CreatePostData()
    {
        return data;
    }
}
