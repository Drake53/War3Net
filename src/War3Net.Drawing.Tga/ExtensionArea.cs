using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TgaLib
{
    /// <summary>
    /// Represents TGA extension area.
    /// </summary>
    public class ExtensionArea
    {
        #region constants

        /// <summary>
        /// Field length.
        /// </summary>
        private static class FieldLength
        {
            /// <summary>Author name.</summary>
            public const int AuthorName = 41;

            /// <summary>Author comments.</summary>
            public const int AuthorComments = 324;

            /// <summary>Job name/ID.</summary>
            public const int JobNameID = 41;

            /// <summary>Software ID.</summary>
            public const int SoftwareID = 41;

            /// <summary>Software version letter.</summary>
            public const int SoftwareVersionLetter = 1;
        }

        #endregion  // constants


        #region properties

        /// <summary>
        /// Gets or sets a size of extension area.
        /// </summary>
        public ushort ExtensionSize { get; set; }

        /// <summary>
        /// Gets or sets an author name.
        /// </summary>
        public string AuthorName { get; set; }

        /// <summary>
        /// Gets or sets author comments.
        /// </summary>
        public string AuthorComments { get; set; }

        /// <summary>
        /// Gets or sets a time-stamp.
        /// </summary>
        public DateTime? TimeStamp { get; set; }

        /// <summary>
        /// Gets or sets a job name/ID.
        /// </summary>
        public string JobNameID { get; set; }

        /// <summary>
        /// Gets or sets a job time.
        /// </summary>
        public TimeSpan JobTime { get; set; }

        /// <summary>
        /// Gets or sets a software ID.
        /// </summary>
        public string SoftwareID { get; set; }

        /// <summary>
        /// Gets or sets a software version.
        /// </summary>
        public string SoftwareVersion { get; set; }

        /// <summary>
        /// Gets or sets a key color.
        /// </summary>
        public uint KeyColor { get; set; }

        /// <summary>
        /// Gets or sets an aspect ratio(width) of pixel.
        /// </summary>
        public ushort PixelAspectRatioWidth { get; set; }

        /// <summary>
        /// Gets or sets an aspect ratio(height) of pixel.
        /// </summary>
        public ushort PixelAspectRatioHeight { get; set; }

        /// <summary>
        /// Gets or sets a gamma value numerator.
        /// </summary>
        public ushort GammaNumerator { get; set; }

        /// <summary>
        /// Gets or sets a gamma value denominator.
        /// </summary>
        public ushort GammaDenominator { get; set; }

        /// <summary>
        /// Gets or sets a color correction offset.
        /// </summary>
        public uint ColorCorrectionOffset { get; set; }

        /// <summary>
        /// Gets or sets a postage stamp offset.
        /// </summary>
        public uint PostageStampOffset { get; set; }

        /// <summary>
        /// Gets or sets a scan line offset.
        /// </summary>
        public uint ScanLineOffset { get; set; }

        /// <summary>
        /// Gets or sets attributes type.
        /// </summary>
        public byte AttributesType { get; set; }

        #endregion  // properties


        #region constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="reader">
        /// A binary reader that contains TGA file. Caller must dipose the binary reader.
        /// A position of base stream of binary reader roll back in the constructor.
        /// </param>
        /// <param name="extensionAreaOffset">An extension area offset.</param>
        /// <exception cref="InvalidOperationException">
        /// Throws if a base stream of <paramref name="reader"/> doesn't support Seek,
        /// because extension area exists in the position specified an extension area offset in the footer.
        /// </exception>
        public ExtensionArea(BinaryReader reader, uint extensionAreaOffset)
        {
            if (!reader.BaseStream.CanSeek)
            {
                throw new InvalidOperationException("Can't search extension area, because a base stream doesn't support Seek.");
            }

            var originalPosition = reader.BaseStream.Position;
            try
            {
                reader.BaseStream.Seek(extensionAreaOffset, SeekOrigin.Begin);

                ExtensionSize = reader.ReadUInt16();
                AuthorName = reader.ReadString(FieldLength.AuthorName, Encoding.ASCII);
                AuthorComments = reader.ReadString(FieldLength.AuthorComments, Encoding.ASCII);
                TimeStamp = ReadTimeStamp(reader);
                JobNameID = reader.ReadString(FieldLength.JobNameID, Encoding.ASCII);
                JobTime = ReadJobTime(reader);
                SoftwareID = reader.ReadString(FieldLength.SoftwareID, Encoding.ASCII);
                SoftwareVersion = ReadSoftwareVersion(reader);
                KeyColor = reader.ReadUInt32();
                PixelAspectRatioWidth = reader.ReadUInt16();
                PixelAspectRatioHeight = reader.ReadUInt16();
                GammaNumerator = reader.ReadUInt16();
                GammaDenominator = reader.ReadUInt16();
                ColorCorrectionOffset = reader.ReadUInt32();
                PostageStampOffset = reader.ReadUInt32();
                ScanLineOffset = reader.ReadUInt32();
                AttributesType = reader.ReadByte();
            }
            finally
            {
                reader.BaseStream.Position = originalPosition;
            }
        }

        #endregion  // constructors


        #region public methods

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>Returns a string that represents the current object.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("ExtensionSize        : {0}\r\n", ExtensionSize);
            sb.AppendFormat("AuthorName           : {0}\r\n", AuthorName.TrimEnd('\0'));
            sb.AppendFormat("AuthorComments       : {0}\r\n", AuthorComments.TrimEnd('\0'));
            if (TimeStamp.HasValue)
            {
                sb.AppendFormat("TimeStamp            : {0:yyyy/MM/dd HH:mm:ss}\r\n", TimeStamp.Value);
            }
            else
            {
                sb.AppendLine("TimeStamp            : not specified");
            }
            sb.AppendFormat("Job Name/ID          : {0}\r\n", JobNameID.TrimEnd('\0'));
            sb.AppendFormat("JobTime              : {0:hh\\:mm\\:ss}\r\n", JobTime);
            sb.AppendFormat("SoftwareID           : {0}\r\n", SoftwareID);
            sb.AppendFormat("SoftwareVersion      : {0}\r\n", SoftwareVersion);
            sb.AppendFormat("KeyColor             : #{0:X08}\r\n", KeyColor);
            if (PixelAspectRatioHeight != 0)
            {
                sb.AppendFormat("PixelAspectRatio     : {0}:{1}\r\n", PixelAspectRatioWidth, PixelAspectRatioHeight);
            }
            else
            {
                sb.AppendLine("PixelAspectRatio     : not specified");
            }
            if (GammaDenominator != 0)
            {
                sb.AppendFormat("GammaValue           : {0:0.0}\r\n", ((double)GammaNumerator) / ((double)GammaDenominator));
            }
            else
            {
                sb.AppendLine("GammaValue           : not specified");
            }
            sb.AppendFormat("ColorCorrectionOffset: {0}\r\n", ColorCorrectionOffset);
            sb.AppendFormat("PostageStampOffset   : {0}\r\n", PostageStampOffset);
            sb.AppendFormat("ScanLineOffset       : {0}\r\n", ScanLineOffset);
            sb.AppendFormat("AttributesType       : {0}\r\n", AttributesType);
            return sb.ToString();
        }

        #endregion  // public methods


        #region private methods

        /// <summary>
        /// Reads a time-stamp.
        /// </summary>
        /// <param name="reader">A binary reader that contains TGA file. Caller must dipose the binary reader.</param>
        /// <returns>Returns a time-stamp.</returns>
        private DateTime? ReadTimeStamp(BinaryReader reader)
        {
            var month = reader.ReadUInt16();
            var day = reader.ReadUInt16();
            var year = reader.ReadUInt16();
            var hour = reader.ReadUInt16();
            var minute = reader.ReadUInt16();
            var second = reader.ReadUInt16();

            if ((year == 0) && (month == 0) && (day == 0) && (hour == 0) && (minute == 0) && (second == 0))
            {
                return null;
            }
            return new DateTime(year, month, day, hour, minute, second);
        }

        /// <summary>
        /// Read a job time.
        /// </summary>
        /// <param name="reader">A binary reader that contains TGA file. Caller must dipose the binary reader.</param>
        /// <returns>Returns a job time.</returns>
        private TimeSpan ReadJobTime(BinaryReader reader)
        {
            var hours = reader.ReadUInt16();
            var minutes = reader.ReadUInt16();
            var seconds = reader.ReadUInt16();
            return new TimeSpan(hours, minutes, seconds);
        }

        /// <summary>
        /// Reads a software version.
        /// </summary>
        /// <param name="reader">A binary reader that contains TGA file. Caller must dipose the binary reader.</param>
        /// <returns>Returns a software version.</returns>
        private string ReadSoftwareVersion(BinaryReader reader)
        {
            var versionNumber = reader.ReadUInt16();
            var versionLetter = reader.ReadString(FieldLength.SoftwareVersionLetter, Encoding.ASCII);
            return string.Format("{0:0.00}{1}", ((double)versionNumber) / 100.0, versionLetter.TrimEnd('\0').TrimEnd(' '));
        }

        #endregion  // private methods
    }
}
