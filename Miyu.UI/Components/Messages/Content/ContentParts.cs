// Copyright (c) flustix <me@flux.moe>.Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace Miyu.UI.Components.Messages.Content;

public static class ContentParts
{
    public interface IBase
    {
    }

    public class Text : IBase
    {
        public string Value { get; }

        public Text(string value)
        {
            Value = value;
        }
    }

    public class Emote : IBase
    {
        public ulong ID { get; }
        public bool Animated { get; }

        public Emote(ulong id, bool animated)
        {
            ID = id;
            Animated = animated;
        }
    }

    public class UserMention : IBase
    {
        public ulong ID { get; }

        public UserMention(ulong id)
        {
            ID = id;
        }
    }
}
