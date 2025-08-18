// ------------------------------------------------------------------------------
// <copyright file="CascEncryptionException.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.IO.Casc
{
    /// <summary>
    /// Exception thrown when CASC encryption/decryption fails.
    /// </summary>
    public class CascEncryptionException : CascException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CascEncryptionException"/> class.
        /// </summary>
        public CascEncryptionException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CascEncryptionException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        public CascEncryptionException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CascEncryptionException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        public CascEncryptionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CascEncryptionException"/> class.
        /// </summary>
        /// <param name="keyName">The name of the missing encryption key.</param>
        public CascEncryptionException(ulong keyName)
            : base($"Missing encryption key: 0x{keyName:X16}")
        {
            KeyName = keyName;
        }

        /// <summary>
        /// Gets the name of the missing encryption key.
        /// </summary>
        public ulong? KeyName { get; }
    }
}