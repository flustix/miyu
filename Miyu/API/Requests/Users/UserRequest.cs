// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Midori.Utils;
using Miyu.Models.Users;

namespace Miyu.API.Requests.Users;

public class UserRequest : RestRequest<DiscordUser>
{
    protected override string Path => $"/users/{id}";

    private ulong id { get; }

    public UserRequest(ulong id)
    {
        this.id = id;
    }

    protected override DiscordUser Deserialize(string json)
    {
        var channel = json.Deserialize<DiscordUser>();
        channel.Client = Client;
        return channel;
    }
}
