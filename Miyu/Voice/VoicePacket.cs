// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace Miyu.Voice;

internal class VoicePacket
{
    public Memory<byte> Bytes { get; }
    public int Duration { get; }
    public bool Silence { get; }

    public byte[]? Rented { get; }

    public VoicePacket(Memory<byte> bytes, int duration, bool silence, byte[]? rented = null)
    {
        Bytes = bytes;
        Duration = duration;
        Silence = silence;
        Rented = rented;
    }
}
