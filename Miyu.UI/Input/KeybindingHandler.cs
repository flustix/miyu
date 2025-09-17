// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Input.Bindings;

namespace Miyu.UI.Input;

public partial class KeybindingHandler : KeyBindingContainer<MiyuBind>
{
    public override IEnumerable<IKeyBinding> DefaultKeyBindings => new List<IKeyBinding>
    {
        new KeyBinding(new KeyCombination(InputKey.Control, InputKey.Up), MiyuBind.SwitchGuildUp),
        new KeyBinding(new KeyCombination(InputKey.Control, InputKey.Down), MiyuBind.SwitchGuildDown),
        new KeyBinding(new KeyCombination(InputKey.Shift, InputKey.Up), MiyuBind.SwitchChannelUp),
        new KeyBinding(new KeyCombination(InputKey.Shift, InputKey.Down), MiyuBind.SwitchChannelDown),

        new KeyBinding(new KeyCombination(InputKey.LShift, InputKey.Tab), MiyuBind.ToggleLeftSide),
        new KeyBinding(new KeyCombination(InputKey.RShift, InputKey.Tab), MiyuBind.ToggleRightSide)
    };
}
