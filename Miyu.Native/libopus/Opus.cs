// Copyright (c) flustix <me@flux.moe>.Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Runtime.InteropServices;

namespace Miyu.Native.libopus;

public static partial class Opus
{
    private const string lib = "libopus";

    [LibraryImport(lib, EntryPoint = "opus_encoder_create")]
    public static partial IntPtr EncoderCreate(int sampleRate, int channels, int application, out OpusError error);

    [LibraryImport(lib, EntryPoint = "opus_encode")]
    private static unsafe partial int Encode(IntPtr encoder, byte* pcmData, int frameSize, byte* data, int maxDataBytes);

    public static unsafe void MiyuEncode(IntPtr encoder, Span<byte> pcm, int frameSize, ref Span<byte> target)
    {
        int length;

        fixed (byte* pPtr = &pcm.GetPinnableReference())
        fixed (byte* tPtr = &target.GetPinnableReference())
            length = Encode(encoder, pPtr, frameSize, tPtr, target.Length);

        if (length < 0)
            throw new Exception($"Failed to encode PCM data: {(OpusError)length} ({length})");

        target = target[..length];
    }
}

public enum OpusError
{
    Ok = 0,
    BadArgument = -1,
    BufferTooSmall = -2,
    InternalError = -3,
    InvalidPacket = -4,
    Unimplemented = -5,
    InvalidState = -6,
    AllocationFailure = -7
}
