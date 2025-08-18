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
        }

        /// <summary>
        /// Decrypts CASC data using Salsa20.
        /// </summary>
        /// <param name="data">The encrypted data.</param>
        /// <param name="keyName">The key name.</param>
        /// <param name="iv">The initialization vector.</param>
        /// <returns>The decrypted data.</returns>
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
            return KnownKeys.TryRemove(keyName, out _);
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
                            // Skip lines with invalid hex format
                            System.Diagnostics.Trace.TraceWarning($"Invalid key format in line: {line}");
                        }
                        catch (Exception ex)
                        {
                            // Log unexpected errors
                            System.Diagnostics.Trace.TraceError($"Error parsing key from line '{line}': {ex.Message}");
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