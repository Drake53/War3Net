// ------------------------------------------------------------------------------
// <copyright file="PlayerControllerProvider.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Providers
{
    internal static class PlayerControllerProvider
    {
        public static string GetPlayerControllerString(PlayerController playerController)
        {
            switch (playerController)
            {
                case PlayerController.User: return nameof(War3Api.Common.MAP_CONTROL_USER);
                case PlayerController.Computer: return nameof(War3Api.Common.MAP_CONTROL_COMPUTER);
                case PlayerController.Rescuable: return nameof(War3Api.Common.MAP_CONTROL_RESCUABLE);
                case PlayerController.Neutral: return nameof(War3Api.Common.MAP_CONTROL_NEUTRAL);
                // case Creep: return nameof(War3Api.Common.MAP_CONTROL_CREEP);
                case PlayerController.None:
                default:
                    return nameof(War3Api.Common.MAP_CONTROL_NONE);
            }
        }
    }
}