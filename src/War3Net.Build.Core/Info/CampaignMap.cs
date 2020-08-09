// ------------------------------------------------------------------------------
// <copyright file="CampaignMap.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text;

using War3Net.Common.Extensions;

namespace War3Net.Build.Info
{
    public sealed class CampaignMap
    {
        private string _unk;
        private string _mapFilePath;

        public string Unk
        {
            get => _unk;
            set => _unk = value;
        }

        public string MapFilePath
        {
            get => _mapFilePath;
            set => _mapFilePath = value;
        }

        public static CampaignMap Parse(Stream stream, bool leaveOpen = false)
        {
            var data = new CampaignMap();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                data._unk = reader.ReadChars();
                data._mapFilePath = reader.ReadChars();
            }

            return data;
        }

        public void WriteTo(BinaryWriter writer)
        {
            writer.WriteString(_unk);
            writer.WriteString(_mapFilePath);
        }
    }
}