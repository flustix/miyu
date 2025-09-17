// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using osu.Framework;

namespace Miyu.UI.Desktop;

internal static class Program
{
    private static void Main(string[] args)
    {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

        using var host = Host.GetSuitableDesktopHost("miyu", new HostOptions
        {
            PortableInstallation = true,
            FriendlyGameName = "Miyu UI",
            IPCPipeName = "miyu"
        });

        host.Run(new MiyuApp());
    }
}
