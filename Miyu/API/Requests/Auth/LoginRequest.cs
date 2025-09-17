// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.API.Payloads.Auth;
using Miyu.API.Response.Auth;

namespace Miyu.API.Requests.Auth;

public class LoginRequest : RestRequest<LoginResponse>
{
    protected override string Path => "/auth/login";
    protected override HttpMethod Method => HttpMethod.Post;

    private string email { get; }
    private string password { get; }

    public LoginRequest(string email, string password)
    {
        this.email = email;
        this.password = password;
    }

    protected override object CreatePostData()
    {
        return new LoginPayload
        {
            Email = email,
            Password = password
        };
    }
}
