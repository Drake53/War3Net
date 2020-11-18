// ------------------------------------------------------------------------------
// <copyright file="MapInfoExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text;

using War3Net.Build.Info;
using War3Net.Common.Extensions;

namespace War3Net.Build.Extensions
{
    public static class MapInfoExtensions
    {
        public static void WriteArchiveHeaderToStream(this MapInfo mapInfo, Stream stream, byte[]? signData = null)
        {
            using (var writer = new BinaryWriter(stream, new UTF8Encoding(false, true), true))
            {
                writer.Write("HM3W".FromRawcode());
                writer.Write(0);
                writer.WriteString(mapInfo.MapName);
                writer.Write((int)mapInfo.MapFlags);
                writer.Write(mapInfo.Players.Count);

                if (signData != null && signData.Length == 256)
                {
                    writer.Write("NGIS".FromRawcode());
                    writer.Write(signData);
                }
            }
        }
    }
}