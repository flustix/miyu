// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Midori.Logging;

namespace Miyu.Tests;

public static class Program
{
    public static Logger Logger { get; } = Logger.GetLogger("Miyu Tests");

    private static MiyuClient client { get; set; } = null!;

    public static async Task Main()
    {
        AppDomain.CurrentDomain.UnhandledException += (_, e) => { Logger.Add("An unhandled exception occurred!", LogLevel.Error, (Exception)e.ExceptionObject); };

        client = new MiyuClient(new MiyuConfig
        {
            Token = await File.ReadAllTextAsync("token.txt"),
            ClientToken = true,
            RegisterCommands = false
        });

        await client.ConnectAsync();
        await Task.Delay(-1);
    }
}
