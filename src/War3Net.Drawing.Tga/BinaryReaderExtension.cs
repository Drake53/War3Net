// ------------------------------------------------------------------------------
// <copyright file="BinaryReaderExtension.cs" company="shns">
// Copyright (c) 2016 shns. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text;

namespace TgaLib
{
    /// <summary>
    /// Extends <see cref="BinaryReader"/>.
    /// </summary>
    internal static class BinaryReaderExtension
    {
        /// <summary>
        /// Reads a string from <see cref="BinaryReader"/>.
        /// </summary>
        /// <param name="reader">The input <see cref="BinaryReader"/>.</param>
        /// <param name="length">The number of bytes to read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <returns>Returns a string to read from <paramref name="reader"/>.</returns>
        public static string ReadString(this BinaryReader reader, int length, Encoding encoding)
        {
            var bytes = reader.ReadBytes(length);
            return encoding.GetString(bytes);
        }
    }
}