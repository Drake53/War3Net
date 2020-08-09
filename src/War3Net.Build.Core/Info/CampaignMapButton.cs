// ------------------------------------------------------------------------------
// <copyright file="CampaignMapButton.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text;

using War3Net.Common.Extensions;

namespace War3Net.Build.Info
{
    public sealed class CampaignMapButton
    {
        private int _isVisibleInitially;
        private string _chapter;
        private string _title;
        private string _mapFilePath;

        public bool IsVisibleInitially
        {
            get => (_isVisibleInitially & 0x1) != 0;
            set => _isVisibleInitially = value ? 1 : 0;
        }

        public string Chapter
        {
            get => _chapter;
            set => _chapter = value;
        }

        public string Title
        {
            get => _title;
            set => _title = value;
        }

        public string MapFilePath
        {
            get => _mapFilePath;
            set => _mapFilePath = value;
        }

        public static CampaignMapButton Parse(Stream stream, bool leaveOpen = false)
        {
            var data = new CampaignMapButton();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                data._isVisibleInitially = reader.ReadInt32();
                data._chapter = reader.ReadChars();
                data._title = reader.ReadChars();
                data._mapFilePath = reader.ReadChars();
            }

            return data;
        }

        public void WriteTo(BinaryWriter writer)
        {
            writer.Write(_isVisibleInitially);
            writer.WriteString(_chapter);
            writer.WriteString(_title);
            writer.WriteString(_mapFilePath);
        }
    }
}