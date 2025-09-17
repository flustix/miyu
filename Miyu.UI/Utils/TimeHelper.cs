// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace Miyu.UI.Utils;

public static class TimeHelper
{
    public static DateTimeOffset GetFromSeconds(long seconds)
    {
        var utc = DateTimeOffset.FromUnixTimeSeconds(seconds);
        return utc.ToLocalTime();
    }
}
