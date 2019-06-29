using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TgaLib
{
    /// <summary>
    /// Represents TGA developer area.
    /// </summary>
    public class DeveloperArea
    {
        #region properties

        /// <summary>
        /// Gets or sets developer fields.
        /// </summary>
        public DeveloperField[] Fields { get; set; }

        #endregion  // properties


        #region constructors

        /// <summary>
        /// Constructor.
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
            if (!reader.BaseStream.CanSeek)
            {
                throw new InvalidOperationException("Can't search developer area, because a base stream doesn't support Seek.");
            }

            var originalPosition = reader.BaseStream.Position;
            try
            {
                reader.BaseStream.Seek(developerAreaOffset, SeekOrigin.Begin);

                var tagCount = reader.ReadUInt16();
                Fields = new DeveloperField[tagCount];
                for (int i = 0; i < tagCount; ++i)
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

        #endregion  // constructors
    }
}
