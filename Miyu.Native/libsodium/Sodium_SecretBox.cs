// Copyright (c) flustix <me@flux.moe>.Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Runtime.InteropServices;

namespace Miyu.Native.libsodium;

public static partial class Sodium
{
    private const string lib = "libsodium";

    public static partial class SecretBox
    {
        public static int KeySize => (int)SecretBoxKeySize();
        public static int NonceSize => (int)SecretBoxNonceSize();
        public static int MacSize => (int)SecretBoxMacSize();

        [LibraryImport(lib, EntryPoint = "crypto_secretbox_xsalsa20poly1305_keybytes")]
        [return: MarshalAs(UnmanagedType.SysUInt)]
        private static partial UIntPtr SecretBoxKeySize();

        [LibraryImport(lib, EntryPoint = "crypto_secretbox_xsalsa20poly1305_noncebytes")]
        [return: MarshalAs(UnmanagedType.SysUInt)]
        private static partial UIntPtr SecretBoxNonceSize();

        [LibraryImport(lib, EntryPoint = "crypto_secretbox_xsalsa20poly1305_macbytes")]
        [return: MarshalAs(UnmanagedType.SysUInt)]
        private static partial UIntPtr SecretBoxMacSize();

        [LibraryImport(lib, EntryPoint = "crypto_secretbox_easy")]
        private static unsafe partial int SecretBoxCreate(byte* buffer, byte* message, ulong messageLength, byte* nonce, byte* key);

        public static unsafe int Encrypt(ReadOnlySpan<byte> source, Span<byte> target, ReadOnlySpan<byte> key, ReadOnlySpan<byte> nonce)
        {
            var status = 0;

            fixed (byte* sPtr = &source.GetPinnableReference())
            fixed (byte* tPtr = &target.GetPinnableReference())
            fixed (byte* kPtr = &key.GetPinnableReference())
            fixed (byte* nPtr = &nonce.GetPinnableReference())
                status = SecretBoxCreate(tPtr, sPtr, (ulong)source.Length, nPtr, kPtr);

            return status;
        }
    }
}
