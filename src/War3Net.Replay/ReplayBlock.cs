// ------------------------------------------------------------------------------
// <copyright file="ReplayBlock.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using War3Net.Common.Extensions;

namespace War3Net.Replay
{
    public abstract class ReplayBlock
    {
        internal ReplayBlock()
        {
        }

        public static ReplayBlock Parse(Stream data)
        {
            return Parse(data, (ReplayBlockType)data.ReadByte());
        }

        public static ReplayBlock Parse(Stream data, ReplayBlockType replayBlockType)
        {
            switch (replayBlockType)
            {
                case ReplayBlockType.StartBlockOne:
                case ReplayBlockType.StartBlockTwo:
                case ReplayBlockType.StartBlockThree:
                    return null;

                case ReplayBlockType.TimeslotOld:
                case ReplayBlockType.Timeslot:
                    return new TimeslotBlock(data);

                default:
                    return null;
            }
        }

        /// <summary>
        /// Seek the next <see cref="ReplayBlock"/> in the <paramref name="data"/>.
        /// </summary>
        internal static void SkipCurrentBlock(Stream data, out ReplayBlockType replayBlockType)
        {
            replayBlockType = (ReplayBlockType)data.ReadByte();

            switch (replayBlockType)
            {
                case ReplayBlockType.LeaveGame:
                    data.Seek(13, SeekOrigin.Current);
                    break;

                case ReplayBlockType.StartBlockOne:
                case ReplayBlockType.StartBlockTwo:
                case ReplayBlockType.StartBlockThree:
                    data.Seek(4, SeekOrigin.Current);
                    break;

                case ReplayBlockType.TimeslotOld:
                case ReplayBlockType.Timeslot:
                    data.Seek(data.ReadWordAsInt(), SeekOrigin.Current);
                    break;

                case ReplayBlockType.ChatMessage:
                    data.Seek(1, SeekOrigin.Current);
                    data.Seek(data.ReadWordAsInt(), SeekOrigin.Current);
                    break;

                case ReplayBlockType.ChecksumOrSeed:
                    data.Seek(5, SeekOrigin.Current);
                    break;

                case ReplayBlockType.Unknown:
                    data.Seek(10, SeekOrigin.Current);
                    break;

                case ReplayBlockType.ForcedGameEnd:
                    data.Seek(8, SeekOrigin.Current);
                    break;

                default:
                    throw new InvalidDataException($"Unknown replay block type: {replayBlockType}.");
            }
        }
    }
}