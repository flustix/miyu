// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace Miyu.UI.Android;

public partial class AndroidApp : MiyuApp
{
    protected override float ZoomValue { get; }

    public AndroidApp(float density)
    {
        ZoomValue = density;
    }
}
