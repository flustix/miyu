// Copyright (c) flustix <me@flux.moe>.Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace Miyu.Voice.Codec;

public enum Encryption
{
    PolyLite, // xsalsa20_poly1305_lite
    PolySuffix, // xsalsa20_poly1305_suffix
    Poly // xsalsa20_poly1305
}
