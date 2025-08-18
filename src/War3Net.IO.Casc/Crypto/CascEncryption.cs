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
            // Add some known keys (these are publicly available)
            // Format: KeyName (64-bit) -> Key (16 bytes)
            
            // Example keys (these would be game-specific)
            // AddKnownKey(0xFA505078126ACB3E, new byte[] { ... });
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
            // Extend IV to 8 bytes if needed
            var fullIv = new byte[8];
            Array.Copy(iv, fullIv, Math.Min(iv.Length, 8));

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
            // Extend IV to 8 bytes if needed
            var fullIv = new byte[8];
            Array.Copy(iv, fullIv, Math.Min(iv.Length, 8));

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
            if (KnownKeys.TryGetValue(keyName, out var key))
            {
                // Return a defensive copy to prevent external modifications
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
                        catch
                        {
                            // Skip invalid lines
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