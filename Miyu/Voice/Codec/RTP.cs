// Copyright (c) flustix <me@flux.moe>.Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Buffers.Binary;
using Miyu.Native.libsodium;

namespace Miyu.Voice.Codec;

public static class RTP
{
    public const int HEADER_SIZE = 12;

    private const byte no_extension = 0x80;
    private const byte extension = 0x90;
    private const byte version = 0x78;

    public static bool IsHeader(ReadOnlySpan<byte> data)
        => data.Length >= HEADER_SIZE
           && (data[0] == no_extension || data[0] == extension)
           && data[1] == version;

    public static void DecodeHeader(ReadOnlySpan<byte> data, out ushort seq, out uint time, out uint ssrc, out bool ext)
    {
        if (data.Length < HEADER_SIZE)
            throw new ArgumentException();

        if ((data[0] != no_extension && data[0] != extension) || data[1] != version)
            throw new ArgumentException();

        ext = data[0] == extension;
        seq = BinaryPrimitives.ReadUInt16BigEndian(data[2..]);
        time = BinaryPrimitives.ReadUInt32BigEndian(data[4..]);
        ssrc = BinaryPrimitives.ReadUInt32BigEndian(data[8..]);
    }

    public static void EncodeHeader(ushort seq, uint time, uint ssrc, Span<byte> data)
    {
        if (data.Length < HEADER_SIZE)
            throw new ArgumentException();

        data[0] = no_extension;
        data[1] = version;

        BinaryPrimitives.WriteUInt16BigEndian(data[2..], seq);
        BinaryPrimitives.WriteUInt32BigEndian(data[4..], time);
        BinaryPrimitives.WriteUInt32BigEndian(data[8..], ssrc);
    }

    public static int GetPacketSize(int len, Encryption encryption) => encryption switch
    {
        Encryption.PolyLite => HEADER_SIZE + len,
        Encryption.PolySuffix => HEADER_SIZE + len + Sodium.SecretBox.NonceSize,
        Encryption.Poly => HEADER_SIZE + len + 4,
        _ => throw new ArgumentException()
    };
}
