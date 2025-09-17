// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.UI.Components.Pages;
using osu.Framework.Graphics;

namespace Miyu.UI.Components.Channels;

public interface IChannelListItem : IDrawable
{
    bool Navigable { get; }

    bool MatchesPage(Page page);
    void Select();
}
