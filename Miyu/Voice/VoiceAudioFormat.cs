// Copyright (c) flustix <me@flux.moe>.Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace Miyu.Voice;

internal class VoiceAudioFormat
{
    public int SampleRate { get; } = 48000;
    public int ChannelCount { get; } = 2;

    public int MaxFrameSize => 120 * (SampleRate / 1000);
    public int SampleCountToSampleSize(int count) => count * ChannelCount * 2;
    public int GetSampleDuration(int size) => size / (SampleRate / 1000) / ChannelCount / 2;
    public int GetFrameSize(int duration) => duration * (SampleRate / 1000);
    public int GetSampleSize(int duration) => duration * ChannelCount * (SampleRate / 1000) * 2;
}
