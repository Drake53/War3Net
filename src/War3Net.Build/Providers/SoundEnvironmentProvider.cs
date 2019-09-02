// ------------------------------------------------------------------------------
// <copyright file="SoundEnvironmentProvider.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Providers
{
    public static class SoundEnvironmentProvider
    {
        // TODO: add strings for all environments
        public static string GetAmbientDaySound(SoundEnvironment soundEnvironment)
        {
            switch (soundEnvironment)
            {
                case SoundEnvironment.Cityscape: return "CityScapeDay";

                default: return null;
            }
        }

        public static string GetAmbientNightSound(SoundEnvironment soundEnvironment)
        {
            switch (soundEnvironment)
            {
                case SoundEnvironment.Cityscape: return "CityScapeNight";

                default: return null;
            }
        }
    }
}