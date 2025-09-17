// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Models.Interaction.Response;

namespace Miyu.API.Requests.Interactions;

public class InteractionCallbackRequest : RestRequest<object>
{
    protected override string Path => $"/interactions/{id}/{token}/callback";
    protected override HttpMethod Method => HttpMethod.Post;

    private ulong id { get; }
    private string token { get; }
    private DiscordInteractionResponse response { get; }

    public InteractionCallbackRequest(ulong id, string token, DiscordInteractionResponse response)
    {
        this.id = id;
        this.token = token;
        this.response = response;
    }

    protected override object CreatePostData()
    {
        return response;
    }
}
