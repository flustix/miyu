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
        SetDefault(ClientConfigEntry.Zoom, 1f, 0.5f, 2f);
    }
}

public enum ClientConfigEntry
{
    Token,
    Zoom
}
