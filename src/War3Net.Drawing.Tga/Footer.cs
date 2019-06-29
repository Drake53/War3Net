using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TgaLib
{
    /// <summary>
    /// Represents TGA footer.
    /// Footer is contained in TGA Version 2.0 only.
    /// </summary>
    public class Footer
    {
        #region constants

        /// <summary>
        /// Field length.
        /// </summary>
        private static class FieldLength
        {
            /// <summary>Extension area offset.</summary>
            public const int ExtensionAreaOffset = 4;

            /// <summary>Developer directory offset.</summary>
            public const int DeveloperDirectoryOffset = 4;

            /// <summary>Signature.</summary>
            public const int Signature = 16;

            /// <summary>Reserved character.</summary>
            public const int ReservedCharacter = 1;

            /// <summary>Binary zero string terminator.</summary>
            public const int BinaryZeroStringTerminator = 1;

            /// <summary>Footer total length.</summary>
            public const int FooterLength = ExtensionAreaOffset + DeveloperDirectoryOffset + Signature + ReservedCharacter + BinaryZeroStringTerminator;
        }

        /// <summary>Signature of TGA file.</summary>
        private const string TgaSignature = "TRUEVISION-XFILE.\0";

        #endregion  // constants


        #region properties

        /// <summary>
        /// Gets or sets an extension area offset.
        /// </summary>
        public uint ExtensionAreaOffset { get; set; }

        /// <summary>
        /// Gets or sets a developer directory offset.
        /// </summary>
        public uint DeveloperDirectoryOffset { get; set; }

        /// <summary>
        /// Gets or sets a signature.
        /// (Treats together with "Signature", "Reserved Character" and "Binary Zero String Terminator"
        /// in the specification as a signature.)
        /// </summary>
        public string Signature { get; set; }

        #endregion  // properties


        #region constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="reader">
        /// A binary reader that contains TGA file. Caller must dipose the binary reader.
        /// A position of base stream of binary reader roll back in the constructor.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Throws if a base stream of <paramref name="reader"/> doesn't support Seek,
        /// because footer exists last in the TGA file.
        /// </exception>
        public Footer(BinaryReader reader)
        {
            if (!reader.BaseStream.CanSeek)
            {
                throw new InvalidOperationException("Can't search footer, because a base stream doesn't support Seek.");
            }

            var originalPosition = reader.BaseStream.Position;
            try
            {
                reader.BaseStream.Seek(-FieldLength.FooterLength, SeekOrigin.End);

                ExtensionAreaOffset = reader.ReadUInt32();
                DeveloperDirectoryOffset = reader.ReadUInt32();
                Signature = ReadSignature(reader);
            }
            finally
            {
                reader.BaseStream.Position = originalPosition;
            }
        }

        #endregion  // constructors


        #region public methods

        /// <summary>
        /// Gets whether has a footer or not.
        /// </summary>
        /// <param name="reader">
        /// A binary reader that contains TGA file. Caller must dipose the binary reader.
        /// A position of base stream of binary reader roll back in the constructor.
        /// </param>
        /// <returns>
        /// Returns true, if TGA file has a footer.
        /// Returns false, if TGA file doesn't have a footer.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Throws if a base stream of <paramref name="reader"/> doesn't support Seek,
        /// because footer exists last in the TGA file.
        /// </exception>
        public static bool HasFooter(BinaryReader reader)
        {
            if (!reader.BaseStream.CanSeek)
            {
                throw new InvalidOperationException("Can't search footer, because a base stream doesn't support Seek.");
            }

            var originalPosition = reader.BaseStream.Position;
            try
            {
                var signature = ReadSignature(reader);
                return (signature == TgaSignature);
            }
            finally
            {
                // Roll back a position of stream
                reader.BaseStream.Position = originalPosition;
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>Returns a string that represents the current object.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("ExtensionAreaOffset     : {0}\r\n", ExtensionAreaOffset);
            sb.AppendFormat("DeveloperDirectoryOffset: {0}\r\n", DeveloperDirectoryOffset);
            sb.AppendFormat("Signature               : {0}\r\n", Signature);
            return sb.ToString();
        }

        #endregion  // public methods


        #region private methods

        /// <summary>
        /// Reads a signature.
        /// </summary>
        /// <param name="reader">A binary reader that contains TGA file. Caller must dipose the binary reader.</param>
        /// <returns>Returns a signature.</returns>
        private static string ReadSignature(BinaryReader reader)
        {
            // Seek to the position of signature
            var signatureSize = (FieldLength.Signature + FieldLength.ReservedCharacter + FieldLength.BinaryZeroStringTerminator);
            reader.BaseStream.Seek(-signatureSize, SeekOrigin.End);

            // Read the signature
            return reader.ReadString(signatureSize, Encoding.ASCII);
        }

        #endregion  // private methods
    }
}
