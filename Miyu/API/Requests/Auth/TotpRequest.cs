// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.API.Response.Auth;

namespace Miyu.API.Requests.Auth;

public class TotpRequest : RestRequest<TotpResponse>
{
    protected override string Path => "/auth/mfa/totp";
    protected override HttpMethod Method => HttpMethod.Post;

    private string code { get; }
    private string ticket { get; }
    private string instance { get; }

    public TotpRequest(string code, string ticket, string instance)
    {
        this.code = code;
        this.ticket = ticket;
        this.instance = instance;
    }

    protected override object CreatePostData() => new
    {
        code,
        login_instance_id = instance,
        ticket
    };
}
