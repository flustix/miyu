// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Text;
using Midori.Logging;
using Midori.Utils;
using Miyu.Utils;

namespace Miyu.API;

public abstract class RestRequest<T>
    where T : class
{
    protected abstract string Path { get; }
    protected virtual HttpMethod Method => HttpMethod.Get;

    protected MiyuClient Client { get; private set; } = null!;

    public bool Success => Response != null;
    public T? Response { get; protected set; }

    public Action<T> OnSuccess { get; set; } = _ => { };
    public Action<string> OnError { get; set; } = _ => { };

    private const string root_url = "https://discord.com/api/v10";

    internal async Task Perform(MiyuClient miyu)
    {
        Client = miyu;

        var client = new HttpClient();
        var token = FormattingUtils.FormatToken(Client.Config);

        if (!string.IsNullOrWhiteSpace(token))
            client.DefaultRequestHeaders.Add("Authorization", token);

        // client.DefaultRequestHeaders.Add("User-Agent", "Miyu Framework");

        try
        {
            var postJson = CreatePostData().Serialize();
            Logger.Log($"request: {Method} {Path} {postJson}", LoggingTarget.Network, LogLevel.Debug);
            var content = new StringContent(postJson, Encoding.UTF8, "application/json");

            var route = Path;
            var path = route.StartsWith("/") ? $"{root_url}{route}" : $"{root_url}/{route}";

            var res = Method.Method switch
            {
                "GET" => await client.GetAsync(path),
                "POST" => await client.PostAsync(path, content),
                "PUT" => await client.PutAsync(path, content),
                "DELETE" => await client.DeleteAsync(path),
                _ => throw new ArgumentOutOfRangeException()
            };

            var response = await res.Content.ReadAsStringAsync();

            if (!res.IsSuccessStatusCode)
            {
                MiyuClient.Logger.Add($"({res.StatusCode}) {response}\n{Environment.StackTrace}", LogLevel.Error);
                OnError(response);
                return;
            }

            Logger.Log($"response: {response}", LoggingTarget.Network, LogLevel.Debug);
            TriggerSuccess(Deserialize(response));
        }
        catch (Exception e)
        {
            MiyuClient.Logger.Add($"API request {GetType().Name.Split('.').Last()} failed!", LogLevel.Error, e);
            OnError(e.Message);
        }
    }

    protected void TriggerSuccess(T res)
    {
        Response = res;
        OnSuccess?.Invoke(res);
    }

    protected virtual object CreatePostData()
    {
        return new object();
    }

    protected virtual T Deserialize(string json)
    {
        return json.Deserialize<T>()!;
    }
}
