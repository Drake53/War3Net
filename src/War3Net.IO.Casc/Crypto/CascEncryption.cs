// ------------------------------------------------------------------------------
// <copyright file="CascEncryption.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace War3Net.IO.Casc.Crypto
{
    /// <summary>
    /// Provides encryption and decryption support for CASC files.
    /// </summary>
    public static class CascEncryption
    {
        private static readonly ConcurrentDictionary<ulong, byte[]> KnownKeys = new ConcurrentDictionary<ulong, byte[]>();
        private static readonly object KeyLoadLock = new object();

        static CascEncryption()
        {
            // Add known keys (these are publicly available)
            // Format: KeyName (64-bit) -> Key (16 bytes)

            // Warcraft III Reforged keys
            AddKnownKey(0x6E4296823E7D561E, new byte[] { 0xC0, 0xBF, 0xA2, 0x94, 0x3A, 0xC3, 0xE9, 0x22, 0x86, 0xE4, 0x44, 0x3E, 0xE3, 0x56, 0x0D, 0x65 }); // 1.32.0.13369 Base content
            AddKnownKey(0xE04D60E31DDEBF63, new byte[] { 0x26, 0x3D, 0xB5, 0xC4, 0x02, 0xDA, 0x8D, 0x4D, 0x68, 0x63, 0x09, 0xCB, 0x2E, 0x32, 0x54, 0xD0 }); // 1.32.0.13445 Base content

            // Battle.net app
            AddKnownKey(0x2C547F26A2613E01, new byte[] { 0x37, 0xC5, 0x0C, 0x10, 0x2D, 0x4C, 0x9E, 0x3A, 0x5A, 0xC0, 0x69, 0xF0, 0x72, 0xB1, 0x41, 0x7D });

            // StarCraft II
            AddKnownKey(0xD0CAE11366CEEA83, new byte[] { 0x00, 0x41, 0x61, 0x07, 0x8E, 0x5A, 0x61, 0x20, 0x32, 0x1E, 0xA5, 0xFF, 0xE4, 0xDC, 0xD1, 0x26 });

            // WoW streaming keys (commonly needed)
            AddKnownKey(0xFA505078126ACB3E, new byte[] { 0xBD, 0xC5, 0x18, 0x62, 0xAB, 0xED, 0x79, 0xB2, 0xDE, 0x48, 0xC8, 0xE7, 0xE6, 0x6C, 0x62, 0x00 });
            AddKnownKey(0xFF813F7D062AC0BC, new byte[] { 0xAA, 0x0B, 0x5C, 0x77, 0xF0, 0x88, 0xCC, 0xC2, 0xD3, 0x90, 0x49, 0xBD, 0x26, 0x7F, 0x06, 0x6D });
            AddKnownKey(0xD1E9B5EDF9283668, new byte[] { 0x8E, 0x4A, 0x25, 0x79, 0x89, 0x4E, 0x38, 0xB4, 0xAB, 0x90, 0x58, 0xBA, 0x5C, 0x73, 0x28, 0xEE });
            AddKnownKey(0xB76729641141CB34, new byte[] { 0x98, 0x49, 0xD1, 0xAA, 0x7B, 0x1F, 0xD0, 0x98, 0x19, 0xC5, 0xC6, 0x62, 0x83, 0xA3, 0x26, 0xEC });
            AddKnownKey(0xFFB9469FF16E6BF8, new byte[] { 0xD5, 0x14, 0xBD, 0x19, 0x09, 0xA9, 0xE5, 0xDC, 0x87, 0x03, 0xF4, 0xB8, 0xBB, 0x1D, 0xFD, 0x9A });
            AddKnownKey(0x23C5B5DF837A226C, new byte[] { 0x14, 0x06, 0xE2, 0xD8, 0x73, 0xB6, 0xFC, 0x99, 0x21, 0x7A, 0x18, 0x08, 0x81, 0xDA, 0x8D, 0x62 });

            // Additional Overwatch keys from CascLib
            AddKnownKey(0xFB680CB6A8BF81F3, new byte[] { 0x62, 0xD9, 0x0E, 0xFA, 0x7F, 0x36, 0xD7, 0x1C, 0x39, 0x8A, 0xE2, 0xF1, 0xFE, 0x37, 0xBD, 0xB9 });
            AddKnownKey(0x402CD9D8D6BFED98, new byte[] { 0xAE, 0xB0, 0xEA, 0xDE, 0xA4, 0x76, 0x12, 0xFE, 0x6C, 0x04, 0x1A, 0x03, 0x95, 0x8D, 0xF2, 0x41 });
            AddKnownKey(0xDBD3371554F60306, new byte[] { 0x34, 0xE3, 0x97, 0xAC, 0xE6, 0xDD, 0x30, 0xEE, 0xFD, 0xC9, 0x8A, 0x2A, 0xB0, 0x93, 0xCD, 0x3C });
            AddKnownKey(0x11A9203C9881710A, new byte[] { 0x2E, 0x2C, 0xB8, 0xC3, 0x97, 0xC2, 0xF2, 0x4E, 0xD0, 0xB5, 0xE4, 0x52, 0xF1, 0x8D, 0xC2, 0x67 });
            AddKnownKey(0xA19C4F859F6EFA54, new byte[] { 0x01, 0x96, 0xCB, 0x6F, 0x5E, 0xCB, 0xAD, 0x7C, 0xB5, 0x28, 0x38, 0x91, 0xB9, 0x71, 0x2B, 0x4B });
            AddKnownKey(0x87AEBBC9C4E6B601, new byte[] { 0x68, 0x5E, 0x86, 0xC6, 0x06, 0x3D, 0xFD, 0xA6, 0xC9, 0xE8, 0x52, 0x98, 0x07, 0x6B, 0x3D, 0x42 });
            AddKnownKey(0xDEE3A0521EFF6F03, new byte[] { 0xAD, 0x74, 0x0C, 0xE3, 0xFF, 0xFF, 0x92, 0x31, 0x46, 0x81, 0x26, 0x98, 0x57, 0x08, 0xE1, 0xB9 });
            AddKnownKey(0x8C9106108AA84F07, new byte[] { 0x53, 0xD8, 0x59, 0xDD, 0xA2, 0x63, 0x5A, 0x38, 0xDC, 0x32, 0xE7, 0x2B, 0x11, 0xB3, 0x2F, 0x29 });
            AddKnownKey(0x49166D358A34D815, new byte[] { 0x66, 0x78, 0x68, 0xCD, 0x94, 0xEA, 0x01, 0x35, 0xB9, 0xB1, 0x6C, 0x93, 0xB1, 0x12, 0x4A, 0xBA });
            AddKnownKey(0x1463A87356778D14, new byte[] { 0x69, 0xBD, 0x2A, 0x78, 0xD0, 0x5C, 0x50, 0x3E, 0x93, 0x99, 0x49, 0x59, 0xB3, 0x0E, 0x5A, 0xEC });
            AddKnownKey(0x5E152DE44DFBEE01, new byte[] { 0xE4, 0x5A, 0x17, 0x93, 0xB3, 0x7E, 0xE3, 0x1A, 0x8E, 0xB8, 0x5C, 0xEE, 0x0E, 0xEE, 0x1B, 0x68 });
            AddKnownKey(0x9B1F39EE592CA415, new byte[] { 0x54, 0xA9, 0x9F, 0x08, 0x1C, 0xAD, 0x0D, 0x08, 0xF7, 0xE3, 0x36, 0xF4, 0x36, 0x8E, 0x89, 0x4C });
            AddKnownKey(0x24C8B75890AD5917, new byte[] { 0x31, 0x10, 0x0C, 0x00, 0xFD, 0xE0, 0xCE, 0x18, 0xBB, 0xB3, 0x3F, 0x3A, 0xC1, 0x5B, 0x30, 0x9F });
            AddKnownKey(0xEA658B75FDD4890F, new byte[] { 0xDE, 0xC7, 0xA4, 0xE7, 0x21, 0xF4, 0x25, 0xD1, 0x33, 0x03, 0x98, 0x95, 0xC3, 0x60, 0x36, 0xF8 });
            AddKnownKey(0x026FDCDF8C5C7105, new byte[] { 0x8F, 0x41, 0x80, 0x9D, 0xA5, 0x53, 0x66, 0xAD, 0x41, 0x6D, 0x3C, 0x33, 0x74, 0x59, 0xEE, 0xE3 });
            AddKnownKey(0xCAE3FAC925F20402, new byte[] { 0x98, 0xB7, 0x8E, 0x87, 0x74, 0xBF, 0x27, 0x50, 0x93, 0xCB, 0x1B, 0x5F, 0xC7, 0x14, 0x51, 0x1B });
            AddKnownKey(0x061581CA8496C80C, new byte[] { 0xDA, 0x2E, 0xF5, 0x05, 0x2D, 0xB9, 0x17, 0x38, 0x0B, 0x8A, 0xA6, 0xEF, 0x7A, 0x5F, 0x8E, 0x6A });
            AddKnownKey(0xBE2CB0FAD3698123, new byte[] { 0x90, 0x2A, 0x12, 0x85, 0x83, 0x6C, 0xE6, 0xDA, 0x58, 0x95, 0x02, 0x0D, 0xD6, 0x03, 0xB0, 0x65 });
            AddKnownKey(0x57A5A33B226B8E0A, new byte[] { 0xFD, 0xFC, 0x35, 0xC9, 0x9B, 0x9D, 0xB1, 0x1A, 0x32, 0x62, 0x60, 0xCA, 0x24, 0x6A, 0xCB, 0x41 });
            AddKnownKey(0x42B9AB1AF5015920, new byte[] { 0xC6, 0x87, 0x78, 0x82, 0x3C, 0x96, 0x4C, 0x6F, 0x24, 0x7A, 0xCC, 0x0F, 0x4A, 0x25, 0x84, 0xF8 });
            AddKnownKey(0x4F0FE18E9FA1AC1A, new byte[] { 0x89, 0x38, 0x1C, 0x74, 0x8F, 0x65, 0x31, 0xBB, 0xFC, 0xD9, 0x77, 0x53, 0xD0, 0x6C, 0xC3, 0xCD });
            AddKnownKey(0x7758B2CF1E4E3E1B, new byte[] { 0x3D, 0xE6, 0x0D, 0x37, 0xC6, 0x64, 0x72, 0x35, 0x95, 0xF2, 0x7C, 0x5C, 0xDB, 0xF0, 0x8B, 0xFA });
            AddKnownKey(0xE5317801B3561125, new byte[] { 0x7D, 0xD0, 0x51, 0x19, 0x9F, 0x84, 0x01, 0xF9, 0x5E, 0x4C, 0x03, 0xC8, 0x84, 0xDC, 0xEA, 0x33 });
            AddKnownKey(0x16B866D7BA3A8036, new byte[] { 0x13, 0x95, 0xE8, 0x82, 0xBF, 0x25, 0xB4, 0x81, 0xF6, 0x1A, 0x4D, 0x62, 0x11, 0x41, 0xDA, 0x6E });
            AddKnownKey(0x11131FFDA0D18D30, new byte[] { 0xC3, 0x2A, 0xD1, 0xB8, 0x25, 0x28, 0xE0, 0xA4, 0x56, 0x89, 0x7B, 0x3C, 0xE1, 0xC2, 0xD2, 0x7E });
            AddKnownKey(0xCAC6B95B2724144A, new byte[] { 0x73, 0xE4, 0xBE, 0xA1, 0x45, 0xDF, 0x2B, 0x89, 0xB6, 0x5A, 0xEF, 0x02, 0xF8, 0x3F, 0xA2, 0x60 });
            AddKnownKey(0xB7DBC693758A5C36, new byte[] { 0xBC, 0x3A, 0x92, 0xBF, 0xE3, 0x02, 0x51, 0x8D, 0x91, 0xCC, 0x30, 0x79, 0x06, 0x71, 0xBF, 0x10 });
            AddKnownKey(0x90CA73B2CDE3164B, new byte[] { 0x5C, 0xBF, 0xF1, 0x1F, 0x22, 0x72, 0x0B, 0xAC, 0xC2, 0xAE, 0x6A, 0xAD, 0x8F, 0xE5, 0x33, 0x17 });
            AddKnownKey(0x6DD3212FB942714A, new byte[] { 0xE0, 0x2C, 0x16, 0x43, 0x60, 0x2E, 0xC1, 0x6C, 0x3A, 0xE2, 0xA4, 0xD2, 0x54, 0xA0, 0x8F, 0xD9 });
            AddKnownKey(0x11DDB470ABCBA130, new byte[] { 0x66, 0x19, 0x87, 0x66, 0xB1, 0xC4, 0xAF, 0x75, 0x89, 0xEF, 0xD1, 0x3A, 0xD4, 0xDD, 0x66, 0x7A });
            AddKnownKey(0x5BEF27EEE95E0B4B, new byte[] { 0x36, 0xBC, 0xD2, 0xB5, 0x51, 0xFF, 0x1C, 0x84, 0xAA, 0x3A, 0x39, 0x94, 0xCC, 0xEB, 0x03, 0x3E });
            AddKnownKey(0x9359B46E49D2DA42, new byte[] { 0x17, 0x3D, 0x65, 0xE7, 0xFC, 0xAE, 0x29, 0x8A, 0x93, 0x63, 0xBD, 0x6A, 0xA1, 0x89, 0xF2, 0x00 });
            AddKnownKey(0x1A46302EF8896F34, new byte[] { 0x80, 0x29, 0xAD, 0x54, 0x51, 0xD4, 0xBC, 0x18, 0xE9, 0xD0, 0xF5, 0xAC, 0x44, 0x9D, 0xC0, 0x55 });
            AddKnownKey(0x693529F7D40A064C, new byte[] { 0xCE, 0x54, 0x87, 0x3C, 0x62, 0xDA, 0xA4, 0x8E, 0xFF, 0x27, 0xFC, 0xC0, 0x32, 0xBD, 0x07, 0xE3 });
            AddKnownKey(0x388B85AEEDCB685D, new byte[] { 0xD9, 0x26, 0xE6, 0x59, 0xD0, 0x4A, 0x09, 0x6B, 0x24, 0xC1, 0x91, 0x51, 0x07, 0x6D, 0x37, 0x9A });
            AddKnownKey(0xE218F69AAC6C104D, new byte[] { 0xF4, 0x3D, 0x12, 0xC9, 0x4A, 0x9A, 0x52, 0x84, 0x97, 0x97, 0x1F, 0x1C, 0xBE, 0x41, 0xAD, 0x4D });
            AddKnownKey(0xF432F0425363F250, new byte[] { 0xBA, 0x69, 0xF2, 0xB3, 0x3C, 0x27, 0x68, 0xF5, 0xF2, 0x9B, 0xFE, 0x78, 0xA5, 0xA1, 0xFA, 0xD5 });
            AddKnownKey(0x061D52F86830B35D, new byte[] { 0xD7, 0x79, 0xF9, 0xC6, 0xCC, 0x9A, 0x4B, 0xE1, 0x03, 0xA4, 0xE9, 0x0A, 0x73, 0x38, 0xF7, 0x93 });
            AddKnownKey(0x1275C84CF113EF65, new byte[] { 0xCF, 0x58, 0xB6, 0x93, 0x3E, 0xAF, 0x98, 0xAF, 0x53, 0xE7, 0x6F, 0x84, 0x26, 0xCC, 0x7E, 0x6C });
        }

        /// <summary>
        /// Decrypts CASC data using Salsa20.
        /// </summary>
        /// <param name="data">The encrypted data.</param>
        /// <param name="keyName">The key name.</param>
        /// <param name="iv">The initialization vector.</param>
        /// <returns>The decrypted data.</returns>
        /// <exception cref="CascEncryptionException">Thrown when the decryption key is not found.</exception>
        public static byte[] Decrypt(byte[] data, ulong keyName, byte[] iv)
        {
            var key = GetKey(keyName);
            if (key == null)
            {
                throw new CascEncryptionException(keyName);
            }

            return Decrypt(data, key, iv);
        }

        /// <summary>
        /// Decrypts CASC data using Salsa20.
        /// </summary>
        /// <param name="data">The encrypted data.</param>
        /// <param name="key">The encryption key.</param>
        /// <param name="iv">The initialization vector.</param>
        /// <returns>The decrypted data.</returns>
        public static byte[] Decrypt(byte[] data, byte[] key, byte[] iv)
        {
            // CASC uses 4-byte IVs that need to be extended to 8 bytes for Salsa20
            // CascLib validates that IV is exactly 4 bytes
            if (iv == null)
            {
                throw new ArgumentNullException(nameof(iv));
            }

            if (iv.Length != 4)
            {
                throw new ArgumentException($"CASC encryption IV must be exactly 4 bytes, got {iv.Length} bytes", nameof(iv));
            }

            // Extend 4-byte IV to 8 bytes by padding with zeros (matching CascLib)
            var fullIv = new byte[8];
            Array.Copy(iv, 0, fullIv, 0, 4);
            // Bytes 4-7 remain zero-initialized

            var salsa20 = new Salsa20(key, fullIv);
            return salsa20.Process(data);
        }

        /// <summary>
        /// Decrypts CASC data in place using Salsa20.
        /// </summary>
        /// <param name="data">The encrypted data.</param>
        /// <param name="keyName">The key name.</param>
        /// <param name="iv">The initialization vector.</param>
        public static void DecryptInPlace(byte[] data, ulong keyName, byte[] iv)
        {
            var key = GetKey(keyName);
            if (key == null)
            {
                throw new CascEncryptionException(keyName);
            }

            DecryptInPlace(data, key, iv);
        }

        /// <summary>
        /// Decrypts CASC data in place using Salsa20.
        /// </summary>
        /// <param name="data">The encrypted data.</param>
        /// <param name="key">The encryption key.</param>
        /// <param name="iv">The initialization vector.</param>
        public static void DecryptInPlace(byte[] data, byte[] key, byte[] iv)
        {
            // CASC uses 4-byte IVs that need to be extended to 8 bytes for Salsa20
            // CascLib validates that IV is exactly 4 bytes
            if (iv == null)
            {
                throw new ArgumentNullException(nameof(iv));
            }

            if (iv.Length != 4)
            {
                throw new ArgumentException($"CASC encryption IV must be exactly 4 bytes, got {iv.Length} bytes", nameof(iv));
            }

            // Extend 4-byte IV to 8 bytes by padding with zeros (matching CascLib)
            var fullIv = new byte[8];
            Array.Copy(iv, 0, fullIv, 0, 4);
            // Bytes 4-7 remain zero-initialized

            var salsa20 = new Salsa20(key, fullIv);
            salsa20.ProcessInPlace(data);
        }

        /// <summary>
        /// Adds a known encryption key.
        /// </summary>
        /// <param name="keyName">The key name.</param>
        /// <param name="key">The encryption key.</param>
        public static void AddKnownKey(ulong keyName, byte[] key)
        {
            if (key == null || key.Length != CascConstants.KeyLength)
            {
                throw new ArgumentException($"Key must be {CascConstants.KeyLength} bytes", nameof(key));
            }

            // Create a defensive copy of the key to prevent external modifications
            var keyCopy = new byte[key.Length];
            Array.Copy(key, keyCopy, key.Length);
            KnownKeys.TryAdd(keyName, keyCopy);
        }

        /// <summary>
        /// Removes a known encryption key.
        /// </summary>
        /// <param name="keyName">The key name.</param>
        /// <returns>true if the key was removed; otherwise, false.</returns>
        public static bool RemoveKnownKey(ulong keyName)
        {
            if (KnownKeys.TryRemove(keyName, out var removedKey))
            {
                // Clear sensitive key data from memory
                if (removedKey != null)
                {
                    Array.Clear(removedKey, 0, removedKey.Length);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets an encryption key.
        /// </summary>
        /// <param name="keyName">The key name.</param>
        /// <returns>The encryption key, or null if not found.</returns>
        public static byte[]? GetKey(ulong keyName)
        {
            // ConcurrentDictionary already provides thread-safe access
            // Return the key directly since it's already stored as a defensive copy in AddKnownKey
            return KnownKeys.TryGetValue(keyName, out var key) ? key : null;
        }

        /// <summary>
        /// Gets a copy of an encryption key.
        /// </summary>
        /// <param name="keyName">The key name.</param>
        /// <returns>A copy of the encryption key, or null if not found.</returns>
        /// <remarks>
        /// Use this method when you need to modify the returned key array.
        /// For read-only access, use GetKey instead.
        /// </remarks>
        public static byte[]? GetKeyCopy(ulong keyName)
        {
            if (KnownKeys.TryGetValue(keyName, out var key))
            {
                // Return a defensive copy for cases where modification is needed
                var keyCopy = new byte[key.Length];
                Array.Copy(key, keyCopy, key.Length);
                return keyCopy;
            }
            return null;
        }

        /// <summary>
        /// Checks if a key is known.
        /// </summary>
        /// <param name="keyName">The key name.</param>
        /// <returns>true if the key is known; otherwise, false.</returns>
        public static bool HasKey(ulong keyName)
        {
            return KnownKeys.ContainsKey(keyName);
        }

        /// <summary>
        /// Gets all known key names.
        /// </summary>
        /// <returns>The collection of known key names.</returns>
        public static IEnumerable<ulong> GetKnownKeyNames()
        {
            return KnownKeys.Keys.ToArray();
        }

        /// <summary>
        /// Clears all known keys.
        /// </summary>
        public static void ClearKnownKeys()
        {
            // Clear sensitive data before removing
            foreach (var kvp in KnownKeys)
            {
                if (kvp.Value != null)
                {
                    Array.Clear(kvp.Value, 0, kvp.Value.Length);
                }
            }

            KnownKeys.Clear();
        }

        /// <summary>
        /// Loads encryption keys from a file.
        /// </summary>
        /// <param name="filePath">The path to the key file.</param>
        /// <returns>The number of keys loaded.</returns>
        public static int LoadKeysFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return 0;
            }

            // Use lock to ensure thread-safe bulk loading
            lock (KeyLoadLock)
            {
                var lines = File.ReadAllLines(filePath);
                var count = 0;

                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#") || line.StartsWith("//"))
                    {
                        continue;
                    }

                    var parts = line.Split(new[] { ' ', '\t', '=' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 2)
                    {
                        try
                        {
                            var keyName = Convert.ToUInt64(parts[0], 16);
                            var keyHex = parts[1].Replace("-", string.Empty).Replace(" ", string.Empty);

                            if (keyHex.Length == 32) // 16 bytes * 2 hex chars
                            {
                                var key = new byte[16];
                                for (int i = 0; i < 16; i++)
                                {
                                    key[i] = Convert.ToByte(keyHex.Substring(i * 2, 2), 16);
                                }

                                AddKnownKey(keyName, key);
                                count++;
                            }
                        }
                        catch (FormatException)
                        {
                            // Skip lines with invalid hex format - silently continue
                        }
                        catch (Exception)
                        {
                            // Skip lines that can't be parsed - silently continue
                        }
                    }
                }

                return count;
            }
        }

        /// <summary>
        /// Computes the Salsa20 hash for a given string (used for key generation).
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>The computed key.</returns>
        public static byte[] ComputeSalsa20Key(string input)
        {
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));

            // Take first 16 bytes of SHA256 hash as key
            var key = new byte[16];
            Array.Copy(hash, key, 16);
            return key;
        }
    }
}