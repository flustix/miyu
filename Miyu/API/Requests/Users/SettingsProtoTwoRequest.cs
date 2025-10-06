// Copyright (c) flustix <me@flux.moe>.Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.API.Response;

namespace Miyu.API.Requests.Users;

public class SettingsProtoTwoRequest : RestRequest<SettingsProtoResponse>
{
    protected override string Path => "/users/@me/settings-proto/2";
}
