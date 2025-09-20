// Copyright (c) flustix <me@flux.moe>.Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Native.libopus;

namespace Miyu.Voice.Codec;

internal static class MiyuOpus
{
    internal static void Encode(IntPtr encoder, VoiceAudioFormat format, Span<byte> pcm, ref Span<byte> target)
    {
        if (pcm.Length != target.Length)
            throw new InvalidOperationException("pcm and target size do not match");

        var duration = format.GetSampleDuration(pcm.Length);
        var frameSize = format.GetFrameSize(duration);
        var sampleSize = format.GetSampleSize(duration);

        if (sampleSize != pcm.Length)
            throw new InvalidOperationException("invalid pcm sample size");

        Opus.MiyuEncode(encoder, pcm, frameSize, ref target);
    }

    internal static IntPtr CreateEncoder(VoiceAudioFormat format)
    {
        var enc = Opus.EncoderCreate(format.SampleRate, format.ChannelCount, 2048, out var error);
        return error != OpusError.Ok ? throw new Exception($"Failed to create encoder. opus error: {error} ({(int)error})") : enc;
    }
}
