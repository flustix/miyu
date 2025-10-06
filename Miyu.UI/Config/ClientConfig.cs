// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Configuration;
using osu.Framework.Platform;

namespace Miyu.UI.Config;

public class ClientConfig : IniConfigManager<ClientConfigEntry>
{
    public ClientConfig(Storage storage)
        : base(storage)
    {
    }

    protected override void InitialiseDefaults()
    {
        base.InitialiseDefaults();

        SetDefault(ClientConfigEntry.Token, "");
        SetDefault(ClientConfigEntry.Zoom, 1f, 0.8f, 2f, 0.1f);
        SetDefault(ClientConfigEntry.Animations, true);
        SetDefault(ClientConfigEntry.Theme, "frappe");
    }

    public double AnimLen(double duration)
    {
        if (Get<bool>(ClientConfigEntry.Animations))
            return duration;

        return 0;
    }
}

public enum ClientConfigEntry
{
    Token,
    Zoom,
    Animations,
    Theme
}
