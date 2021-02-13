// ------------------------------------------------------------------------------
// <copyright file="CampaignInfoExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text;

using War3Net.Build.Info;
using War3Net.Build.Script;
using War3Net.Common.Extensions;

namespace War3Net.Build.Extensions
{
    public static class CampaignInfoExtensions
    {
        private static readonly Encoding _defaultEncoding = new UTF8Encoding(false, true);

        public static void WriteArchiveHeaderToStream(this CampaignInfo campaignInfo, Stream stream, byte[]? signData = null)
        {
            campaignInfo.WriteArchiveHeaderToStream(stream, null, _defaultEncoding, signData);
        }

        public static void WriteArchiveHeaderToStream(this CampaignInfo campaignInfo, Stream stream, CampaignTriggerStrings? campaignTriggerStrings, byte[]? signData = null)
        {
            campaignInfo.WriteArchiveHeaderToStream(stream, campaignTriggerStrings, _defaultEncoding, signData);
        }

        public static void WriteArchiveHeaderToStream(this CampaignInfo campaignInfo, Stream stream, Encoding encoding, byte[]? signData = null)
        {
            campaignInfo.WriteArchiveHeaderToStream(stream, null, encoding, signData);
        }

        public static void WriteArchiveHeaderToStream(this CampaignInfo campaignInfo, Stream stream, CampaignTriggerStrings? campaignTriggerStrings, Encoding encoding, byte[]? signData = null)
        {
            using (var writer = new BinaryWriter(stream, encoding, true))
            {
                writer.Write("HM3W".FromRawcode());
                writer.Write(0);
                writer.WriteString(campaignInfo.CampaignName.Localize(campaignTriggerStrings));
                writer.Write((int)campaignInfo.CampaignFlags);
                writer.Write(1);

                if (signData != null && signData.Length == 256)
                {
                    writer.Write("NGIS".FromRawcode());
                    writer.Write(signData);
                }
            }
        }
    }
}