// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace Miyu.Utils;

public static class FormattingUtils
{
    public static string FormatToken(MiyuConfig config)
    {
        if (config.ClientToken)
            return config.Token;

        return $"Bot {config.Token}";
    }
}
