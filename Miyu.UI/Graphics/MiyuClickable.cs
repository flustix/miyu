// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Platform;

namespace Miyu.UI.Graphics;

public partial class MiyuClickable : ClickableContainer, IHasCursorType
{
    public CursorType Cursor => CursorType.Hand;
}
