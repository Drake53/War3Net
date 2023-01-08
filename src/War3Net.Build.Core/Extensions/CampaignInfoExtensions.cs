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
using War3Net.Common.Providers;

namespace War3Net.Build.Extensions
{
    public static class CampaignInfoExtensions
    {
        private static readonly Encoding _defaultEncoding = UTF8EncodingProvider.StrictUTF8;

        public static void WriteArchiveHeaderToStream(this CampaignInfo campaignInfo, Stream stream)
        {
            campaignInfo.WriteArchiveHeaderToStream(stream, null, _defaultEncoding);
        }

        public static void WriteArchiveHeaderToStream(this CampaignInfo campaignInfo, Stream stream, TriggerStrings? campaignTriggerStrings)
        {
            campaignInfo.WriteArchiveHeaderToStream(stream, campaignTriggerStrings, _defaultEncoding);
        }

        public static void WriteArchiveHeaderToStream(this CampaignInfo campaignInfo, Stream stream, Encoding encoding)
        {
            campaignInfo.WriteArchiveHeaderToStream(stream, null, encoding);
        }

        public static void WriteArchiveHeaderToStream(this CampaignInfo campaignInfo, Stream stream, TriggerStrings? campaignTriggerStrings, Encoding encoding)
        {
            using (var writer = new BinaryWriter(stream, encoding, true))
            {
                writer.Write("HM3W".FromRawcode());
                writer.Write(0);
                writer.WriteString(campaignInfo.CampaignName.Localize(campaignTriggerStrings));
                writer.Write((int)campaignInfo.CampaignFlags);
                writer.Write(1);
            }
        }
    }
}