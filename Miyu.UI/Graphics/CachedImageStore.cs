// Copyright (c) flustix <me@flux.moe>.Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Security.Cryptography;
using System.Text;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Miyu.UI.Graphics;

public class CachedImageStore : IResourceStore<TextureUpload>
{
    private readonly Storage storage;
    private readonly OnlineStore online;

    public CachedImageStore(Storage storage)
    {
        this.storage = storage;
        online = new OnlineStore();
    }

    public TextureUpload Get(string name)
    {
        var hash = BitConverter.ToString(SHA256.HashData(Encoding.UTF8.GetBytes(name))).Replace("-", "").ToLower();
        var path = storage.GetFullPath(hash);

        if (File.Exists(path))
        {
            try
            {
                using var str = File.OpenRead(path);
                return new TextureUpload(str);
            }
            catch (Exception)
            {
            }
        }

        var pulled = online.Get(name);
        if (pulled == null) return null!;

        File.WriteAllBytes(path, pulled);
        return new TextureUpload(Image.Load<Rgba32>(pulled));
    }

    public Task<TextureUpload> GetAsync(string name, CancellationToken cancellationToken = default)
        => Task.Run(() => Get(name), cancellationToken);

    public Stream GetStream(string name) => throw new InvalidOperationException();

    public IEnumerable<string> GetAvailableResources() => new List<string>();

    public void Dispose()
    {
    }
}
