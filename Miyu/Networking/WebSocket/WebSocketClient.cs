// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Midori.Logging;
using Midori.Networking.WebSockets;
using Midori.Networking.WebSockets.Frame;

namespace Miyu.Networking.WebSocket;

public class WebSocketClient : IWebSocketClient
{
    public Action? OnConnect { get; set; }
    public Action? OnDisconnect { get; set; }
    public Action<string>? OnMessage { get; set; }
    public Action<Exception>? OnException { get; set; }

    public bool Connected => client?.State == WebSocketState.Open;

    private ClientWebSocket? client;

    private Task? receiveTask;

    public async Task ConnectAsync(string uri)
    {
        try
        {
            client?.Dispose();
            client = new ClientWebSocket();
            client.OnOpen += () => OnConnect?.Invoke();
            client.OnMessage += msg => OnMessage?.Invoke(msg.Text);
            client.OnClose += () =>
            {
                Logger.Log($"WebSocket has been closed. ({client.CloseReason})");
                OnDisconnect?.Invoke();
            };
            await client.ConnectAsync(uri);
        }
        catch (Exception e)
        {
            throw new Exception("Failed to connect to the WebSocket server", e);
        }
    }

    public Task DisconnectAsync(int code = 1000, string message = "")
    {
        if (client?.State == WebSocketState.Open)
            client.Close((WebSocketCloseCode)code, message);
        else
            MiyuClient.Logger.Add("err... you weren't connected", LogLevel.Error);

        client?.Dispose();
        client = null;

        receiveTask?.Wait();
        receiveTask = null;

        OnDisconnect?.Invoke();
        return Task.CompletedTask;
    }

    public async Task SendAsync(string message)
    {
        if (client?.State != WebSocketState.Open)
            throw new InvalidOperationException("not even connected");

        try
        {
            await client.SendTextAsync(message);
        }
        catch (Exception e)
        {
            throw new Exception("your honour, i plead oopsie daisy", e);
        }
    }
}
