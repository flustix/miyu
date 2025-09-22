// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Utils;
using Newtonsoft.Json.Linq;

namespace Miyu.API.Requests.Channels;

public class CreateMessageRequest : RestRequest<object>
{
    protected override string Path => $"/channels/{id}/messages";
    protected override HttpMethod Method => HttpMethod.Post;

    private ulong id { get; }
    private JObject data { get; } = new();

    public CreateMessageRequest(ulong id, string content)
    {
        this.id = id;
        data["content"] = content;
    }

    public void ReplyTo(ulong message) => data.Add("message_reference", new
    {
        type = 0,
        message_id = message,
        fail_if_not_exists = false
    }.TurnTo<JObject>());

    protected override object CreatePostData() => data;
}
