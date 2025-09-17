// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace Miyu.API.Requests.Channels;

public class CreateMessageRequest : RestRequest<object>
{
    protected override string Path => $"/channels/{id}/messages";
    protected override HttpMethod Method => HttpMethod.Post;

    private ulong id { get; }
    private string content { get; }

    public CreateMessageRequest(ulong id, string content)
    {
        this.id = id;
        this.content = content;
    }

    protected override object CreatePostData()
    {
        return new
        {
            content
        };
    }
}
