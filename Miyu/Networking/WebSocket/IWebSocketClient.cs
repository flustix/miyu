// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace Miyu.Networking.WebSocket;

public interface IWebSocketClient
{
    Action? OnConnect { get; set; }
    Action? OnDisconnect { get; set; }
    Action<string>? OnMessage { get; set; }
    Action<Exception>? OnException { get; set; }

    bool Connected { get; }

    Task ConnectAsync(string uri);
    Task DisconnectAsync(int code = 1000, string message = "");

    Task SendAsync(string message);
}
