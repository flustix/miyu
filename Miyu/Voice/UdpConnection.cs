// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;

namespace Miyu.Voice;

public class UdpConnection
{
    private UdpClient client = null!;
    private IPEndPoint endpoint = null!;

    private BlockingCollection<byte[]> queue { get; } = new();
    private CancellationTokenSource source { get; } = new();

    private CancellationToken token => source.Token;

    public void Setup(IPEndPoint endpoint)
    {
        this.endpoint = endpoint;
        client = new UdpClient();
        _ = Task.Run(receiveLoop, token);
    }

    public Task Send(byte[] data, int len)
        => client.SendAsync(data, len, endpoint.Address.ToString(), endpoint.Port);

    public Task<byte[]> Receive()
        => Task.FromResult(queue.Take(token));

    public void Close()
    {
        source.Cancel();

        try
        {
            client.Close();
        }
        catch (Exception)
        {
        }

        queue.Dispose();
    }

    private async Task receiveLoop()
    {
        while (!token.IsCancellationRequested)
        {
            try
            {
                // ReSharper disable MethodSupportsCancellation
                var packet = await client.ReceiveAsync();
                queue.Add(packet.Buffer);
                // ReSharper enable MethodSupportsCancellation
            }
            catch (Exception)
            {
            }
        }
    }
}
