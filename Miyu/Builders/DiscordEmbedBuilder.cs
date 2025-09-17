// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Models;
using Miyu.Models.Channels.Messages.Embed;

namespace Miyu.Builders;

public class DiscordEmbedBuilder
{
    private string? title;
    private string? description;
    private string? thumbnail;

    public string? Title
    {
        get => title;
        set
        {
            if (value?.Length > 256)
                throw new ArgumentOutOfRangeException(nameof(value), "Title cannot be longer than 256 characters.");

            title = value;
        }
    }

    public string? Description
    {
        get => description;
        set
        {
            if (value?.Length > 2048)
                throw new ArgumentOutOfRangeException(nameof(value), "Description cannot be longer than 2048 characters.");

            description = value;
        }
    }

    public string? Thumbnail
    {
        get => description;
        set => thumbnail = value;
    }

    public Uri? Url { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public DiscordColor? Color { get; set; }

    private (string n, string? v, string? i)? author;
    private (string t, string? i)? footer;
    private readonly List<(string n, string v, bool i)> fields = new();

    public DiscordEmbedBuilder WithTitle(string t)
    {
        Title = t;
        return this;
    }

    public DiscordEmbedBuilder WithDescription(string d)
    {
        Description = d;
        return this;
    }

    public DiscordEmbedBuilder WithThumbnail(string t)
    {
        thumbnail = t;
        return this;
    }

    public DiscordEmbedBuilder WithColor(DiscordColor color)
    {
        Color = color;
        return this;
    }

    public DiscordEmbedBuilder WithFooter(string text, string? icon = null)
    {
        footer = (text, icon);
        return this;
    }

    public DiscordEmbedBuilder WithAuthor(string name, string? url = null, string? icon = null)
    {
        if (name.Length > 256) throw new ArgumentOutOfRangeException(nameof(name), "Author name cannot be longer than 256 characters.");

        author = (name, url, icon);
        return this;
    }

    public DiscordEmbedBuilder WithField(string name, string value, bool inline = false)
    {
        if (fields.Count >= 25) throw new InvalidOperationException("Embeds cannot have more than 25 fields.");
        if (name.Length > 256) throw new ArgumentOutOfRangeException(nameof(name), "Field name cannot be longer than 256 characters.");
        if (value.Length > 1024) throw new ArgumentOutOfRangeException(nameof(value), "Field value cannot be longer than 1024 characters.");

        fields.Add((name, value, inline));
        return this;
    }

    public DiscordEmbed Build()
    {
        var embed = new DiscordEmbed
        {
            Title = Title,
            Description = Description,
            Url = Url,
            Timestamp = Timestamp,
            Color = Color
        };

        if (author != null)
        {
            embed.Author = new DiscordEmbedAuthor
            {
                Name = author.Value.n,
                Url = author.Value.v,
                IconUrl = author.Value.i
            };
        }

        if (!string.IsNullOrWhiteSpace(thumbnail))
            embed.Thumbnail = new DiscordEmbedThumbnail { Url = thumbnail };

        if (footer != null)
        {
            embed.Footer = new DiscordEmbedFooter
            {
                Text = footer.Value.t,
                IconUrl = footer.Value.i
            };
        }

        if (fields.Count > 0)
        {
            embed.Fields = fields.Select(f => new DiscordEmbedField
            {
                Name = f.n,
                Value = f.v,
                Inline = f.i
            }).ToList();
        }

        return embed;
    }
}
