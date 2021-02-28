// ------------------------------------------------------------------------------
// <copyright file="PlayerControllerExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.Build.Info;

namespace War3Net.Build.Extensions
{
    public static class PlayerControllerExtensions
    {
        public static string GetVariableName(this PlayerController playerController)
        {
            return playerController switch
            {
                PlayerController.User => MapControlName.User,
                PlayerController.Computer => MapControlName.Computer,
                PlayerController.Rescuable => MapControlName.Rescuable,
                PlayerController.Neutral => MapControlName.Neutral,
                // PlayerController.Creep => MapControlName.Creep;
                _ => MapControlName.None,
            };
        }

        private class MapControlName
        {
            internal const string User = "MAP_CONTROL_USER";
            internal const string Computer = "MAP_CONTROL_COMPUTER";
            internal const string Rescuable = "MAP_CONTROL_RESCUABLE";
            internal const string Neutral = "MAP_CONTROL_NEUTRAL";
            internal const string Creep = "MAP_CONTROL_CREEP";
            internal const string None = "MAP_CONTROL_NONE";
        }
    }
}