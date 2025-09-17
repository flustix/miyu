// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Midori.Logging;

namespace Miyu.API;

public class MiyuAPIClient
{
    private MiyuClient client { get; }

    public MiyuAPIClient(MiyuClient client)
    {
        this.client = client;
    }

    public async Task<T?> Execute<T>(RestRequest<T> request)
        where T : class
    {
        try
        {
            await request.Perform(client);
        }
        catch (Exception e)
        {
            Logger.Error(e, "Failed to perform request!", LoggingTarget.Network);
            return null;
        }

        if (!request.Success)
            return null;

        return request.Response!;
    }
}
