// ------------------------------------------------------------------------------
// <copyright file="PlayerControllerProvider.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.Build.Info;

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