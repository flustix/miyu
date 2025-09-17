// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Text.RegularExpressions;

namespace Miyu.UI.Utils;

public static partial class Regexes
{
    [GeneratedRegex(@"(<a?):\w+:(\d+>)")]
    public static partial Regex CustomEmote();
}
