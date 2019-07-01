// ------------------------------------------------------------------------------
// <copyright file="DeveloperArea.cs" company="shns">
// Copyright (c) 2016 shns. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

namespace War3Net.Drawing.Tga
{
    /// <summary>
    /// Represents TGA developer area.
    /// </summary>
    public class DeveloperArea
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeveloperArea"/> class.
        /// </summary>
        /// <param name="reader">
        /// A binary reader that contains TGA file. Caller must dipose the binary reader.
        /// A position of base stream of binary reader roll back in the constructor.
        /// </param>
        /// <param name="developerAreaOffset">A developer area offset.</param>
        /// <exception cref="InvalidOperationException">
        /// Throws if a base stream of <paramref name="reader"/> doesn't support Seek,
        /// because developer area exists in the position specified a developer area offset in the footer.
        /// </exception>
        public DeveloperArea(BinaryReader reader, uint developerAreaOffset)
        {
            var originalPosition = reader?.BaseStream.Position ?? throw new ArgumentNullException(nameof(reader));

            if (!reader.BaseStream.CanSeek)
            {
                throw new InvalidOperationException("Can't search developer area, because a base stream doesn't support Seek.");
            }

            try
            {
                reader.BaseStream.Seek(developerAreaOffset, SeekOrigin.Begin);

                var tagCount = reader.ReadUInt16();
                Fields = new DeveloperField[tagCount];
                for (var i = 0; i < tagCount; ++i)
                {
                    var tag = reader.ReadUInt16();
                    var offset = reader.ReadUInt32();
                    var size = reader.ReadUInt32();
                    var field = new DeveloperField(reader, tag, offset, size);
                    Fields[i] = field;
                }
            }
            finally
            {
                reader.BaseStream.Position = originalPosition;
            }
        }

        /// <summary>
        /// Gets or sets developer fields.
        /// </summary>
        public DeveloperField[] Fields { get; set; }
    }
}