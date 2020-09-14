// ------------------------------------------------------------------------------
// <copyright file="GamePatchVersionProvider.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.ComponentModel;

using War3Net.Build.Common;

namespace War3Net.Build.Providers
{
    // https://wow.gamepedia.com/Warcraft_client_builds
    public static class GamePatchVersionProvider
    {
        public static Version GetGameVersion(GamePatch gamePatch)
        {
            return gamePatch switch
            {
#pragma warning disable SA1025 // Code should not contain multiple whitespace in a row
                GamePatch.v1_00   => new Version(1,  0, 0,  4448),
                GamePatch.v1_01   => new Version(1,  1, 0,  4482),
                GamePatch.v1_01b  => new Version(1,  1, 2,  4483),
                GamePatch.v1_01c  => new Version(1,  1, 3,  4484),
                GamePatch.v1_02   => new Version(1,  2, 0,  4531),
                GamePatch.v1_02a  => new Version(1,  2, 1,  4563),
                GamePatch.v1_03   => new Version(1,  3, 0,  4653),
                GamePatch.v1_04   => new Version(1,  4, 0,  4709),
                GamePatch.v1_04b  => new Version(1,  4, 2,  4709),
                GamePatch.v1_04c  => new Version(1,  4, 3,  4905),
                GamePatch.v1_05   => new Version(1,  5, 0,  4944),
                GamePatch.v1_06   => new Version(1,  6, 0,  5551),
                GamePatch.v1_07   => new Version(1,  7, 0,  5535),
                GamePatch.v1_10   => new Version(1, 10, 0,  5610),
                GamePatch.v1_11   => new Version(1, 11, 0,  5616),
                GamePatch.v1_12   => new Version(1, 12, 0,  5636),
                GamePatch.v1_13   => new Version(1, 13, 0,  5816),
                GamePatch.v1_13b  => new Version(1, 13, 2,  5818),
                GamePatch.v1_14   => new Version(1, 14, 0,  5840),
                GamePatch.v1_14b  => new Version(1, 14, 2,  5846),
                GamePatch.v1_15   => new Version(1, 15, 0,  5917),
                GamePatch.v1_16   => new Version(1, 16, 0,  5926),
                GamePatch.v1_17   => new Version(1, 17, 0,  5988),
                GamePatch.v1_18   => new Version(1, 18, 0,  6030),
                GamePatch.v1_19   => new Version(1, 19, 0,  6041),
                GamePatch.v1_19b  => new Version(1, 19, 2,  6046),
                GamePatch.v1_20a  => new Version(1, 20, 1,  6048),
                GamePatch.v1_20b  => new Version(1, 20, 2,  6056),
                GamePatch.v1_20c  => new Version(1, 20, 3,  6065),
                GamePatch.v1_20d  => new Version(1, 20, 4,  6070),
                GamePatch.v1_20e  => new Version(1, 20, 5,  6074),
                GamePatch.v1_21   => new Version(1, 21, 0,  6263),
                GamePatch.v1_21b  => new Version(1, 21, 2,  6300),
                GamePatch.v1_22   => new Version(1, 22, 0,  6328),
                GamePatch.v1_23   => new Version(1, 23, 0,  6352),
                GamePatch.v1_24a  => new Version(1, 24, 1,  6372),
                GamePatch.v1_24b  => new Version(1, 24, 2,  6374),
                GamePatch.v1_24c  => new Version(1, 24, 3,  6378),
                GamePatch.v1_24d  => new Version(1, 24, 4,  6384),
                GamePatch.v1_24e  => new Version(1, 24, 5,  6387),
                GamePatch.v1_25b  => new Version(1, 25, 2,  6394),
                GamePatch.v1_26a  => new Version(1, 26, 1,  6397),
                GamePatch.v1_27a  => new Version(1, 27, 1, 52240),
                GamePatch.v1_27b  => new Version(1, 27, 2,  7085),
                GamePatch.v1_28   => new Version(1, 28, 0,  7205),
                GamePatch.v1_28_1 => new Version(1, 28, 1,  7365),
                GamePatch.v1_28_2 => new Version(1, 28, 2,  7395),
                GamePatch.v1_28_3 => new Version(1, 28, 3,  7205),
                GamePatch.v1_28_4 => new Version(1, 28, 4,  7608),
                GamePatch.v1_28_5 => new Version(1, 28, 5,  7680),
                GamePatch.v1_29_0 => new Version(1, 29, 0,  9055),
                GamePatch.v1_29_1 => new Version(1, 29, 1,  9160),
                GamePatch.v1_29_2 => new Version(1, 29, 2,  9231),
                GamePatch.v1_30_0 => new Version(1, 30, 0,  9922),
                GamePatch.v1_30_1 => new Version(1, 30, 1, 10211),
                GamePatch.v1_30_2 => new Version(1, 30, 2, 11113),
                GamePatch.v1_30_3 => new Version(1, 30, 3, 11235),
                GamePatch.v1_30_4 => new Version(1, 30, 4, 11274),
                GamePatch.v1_31_0 => new Version(1, 31, 0, 12071),
                GamePatch.v1_31_1 => new Version(1, 31, 1, 12164),
                GamePatch.v1_32_0 => new Version(1, 32, 0, 14481),
                GamePatch.v1_32_1 => new Version(1, 32, 1, 14604),
                GamePatch.v1_32_2 => new Version(1, 32, 2, 14722),
                GamePatch.v1_32_3 => new Version(1, 32, 3, 14883),
                GamePatch.v1_32_4 => new Version(1, 32, 4, 15098),
                GamePatch.v1_32_5 => new Version(1, 32, 5, 15129),
                GamePatch.v1_32_6 => new Version(1, 32, 6, 15355),
                GamePatch.v1_32_7 => new Version(1, 32, 7, 15572),
                GamePatch.v1_32_8 => new Version(1, 32, 8, 15801),
                // GamePatch.v1_32_9 => new Version(1, 32, 9, ?????),
#pragma warning restore SA1025 // Code should not contain multiple whitespace in a row

                _ => throw new InvalidEnumArgumentException(nameof(gamePatch), (int)gamePatch, typeof(GamePatch)),
            };
        }

        public static GamePatch GetGamePatch(Version gameVersion)
        {
            return gameVersion.Major switch
            {
                1 => gameVersion.Minor switch
                {
                    // TODO: earlier versions

                    28 => gameVersion.Build switch
                    {
                        0 => GamePatch.v1_28,
                        1 => GamePatch.v1_28_1,
                        2 => GamePatch.v1_28_2,
                        3 => GamePatch.v1_28_3,
                        4 => GamePatch.v1_28_4,
                        5 => GamePatch.v1_28_5,

                        _ => throw new ArgumentOutOfRangeException(nameof(gameVersion)),
                    },

                    29 => gameVersion.Build switch
                    {
                        0 => GamePatch.v1_29_0,
                        1 => GamePatch.v1_29_1,
                        2 => GamePatch.v1_29_2,

                        _ => throw new ArgumentOutOfRangeException(nameof(gameVersion)),
                    },

                    30 => gameVersion.Build switch
                    {
                        0 => GamePatch.v1_30_0,
                        1 => GamePatch.v1_30_1,
                        2 => GamePatch.v1_30_2,
                        3 => GamePatch.v1_30_3,
                        4 => GamePatch.v1_30_4,

                        _ => throw new ArgumentOutOfRangeException(nameof(gameVersion)),
                    },

                    31 => gameVersion.Build switch
                    {
                        0 => GamePatch.v1_31_0,
                        1 => GamePatch.v1_31_1,

                        _ => throw new ArgumentOutOfRangeException(nameof(gameVersion)),
                    },

                    32 => gameVersion.Build switch
                    {
                        0 => GamePatch.v1_32_0,
                        1 => GamePatch.v1_32_1,
                        2 => GamePatch.v1_32_2,
                        3 => GamePatch.v1_32_3,
                        4 => GamePatch.v1_32_4,
                        5 => GamePatch.v1_32_5,
                        6 => GamePatch.v1_32_6,
                        7 => GamePatch.v1_32_7,
                        8 => GamePatch.v1_32_8,
                        // 9 => GamePatch.v1_32_9,

                        _ => throw new NotSupportedException(),
                    },

                    _ => throw new NotSupportedException(),
                },

                _ => throw new NotSupportedException(),
            };
        }
    }
}