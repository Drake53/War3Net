// ------------------------------------------------------------------------------
// <copyright file="DeveloperField.cs" company="shns">
// Copyright (c) 2016 shns. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

namespace War3Net.Drawing.Tga
{
    /// <summary>
    /// Represents a Developer Field.
    /// </summary>
    public class DeveloperField
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeveloperField"/> class.
        /// </summary>
        /// <param name="reader">
        /// A binary reader that contains TGA file. Caller must dipose the binary reader.
        /// A position of base stream of binary reader roll back in the constructor.
        /// </param>
        /// <param name="tag">A tag.</param>
        /// <param name="fieldOffset">A field offset.</param>
        /// <param name="fieldSize">A field size.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Throws if <paramref name="fieldSize"/> is greater than <see cref="int.MaxValue"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Throws if a base stream of <paramref name="reader"/> doesn't support Seek,
        /// because developer field exists in the position specified <paramref name="fieldOffset"/>.
        /// </exception>
        public DeveloperField(BinaryReader reader, ushort tag, uint fieldOffset, uint fieldSize)
        {
            if (fieldSize > int.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(fieldSize));
            }

            if (!reader.BaseStream.CanSeek)
            {
                throw new InvalidOperationException("Can't search developer field, because a base stream doesn't support Seek.");
            }

            Tag = tag;

            var originalPosition = reader.BaseStream.Position;
            try
            {
                reader.BaseStream.Seek(fieldOffset, SeekOrigin.Begin);
                Data = reader.ReadBytes((int)fieldSize);
            }
            finally
            {
                reader.BaseStream.Position = originalPosition;
            }
        }

        /// <summary>
        /// Gets or sets a tag.
        /// </summary>
        public ushort Tag { get; set; }

        /// <summary>
        /// Gets or sets developer field.
        /// </summary>
        public byte[] Data { get; set; }
    }
}