// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using JetBrains.Annotations;
using Miyu.Models;

namespace Miyu.Attributes;

[MeansImplicitUse]
[AttributeUsage(AttributeTargets.Method)]
public class SlashCommandAttribute : Attribute
{
    public string Name { get; }
    public string Description { get; }

    public bool AllowInDM { get; set; } = true;
    public DiscordPermissions Permissions { get; set; } = 0;

    public SlashCommandAttribute(string name, string description)
    {
        Name = name;
        Description = description;
    }
}
