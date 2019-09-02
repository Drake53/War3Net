// ------------------------------------------------------------------------------
// <copyright file="LightEnvironmentProvider.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Providers
{
    public static class LightEnvironmentProvider
    {
        // TODO: add strings for all environments
        public static string GetTerrainLightEnvironmentModel(LightEnvironment lightEnvironment)
        {
            switch (lightEnvironment)
            {
                case LightEnvironment.LordaeronSummer: return @"Environment\\DNC\\DNCLordaeron\\DNCLordaeronTerrain\\DNCLordaeronTerrain.mdl";

                default: return null;
            }
        }

        public static string GetUnitLightEnvironmentModel(LightEnvironment lightEnvironment)
        {
            switch (lightEnvironment)
            {
                case LightEnvironment.LordaeronSummer: return @"Environment\\DNC\\DNCLordaeron\\DNCLordaeronUnit\\DNCLordaeronUnit.mdl";

                default: return null;
            }
        }
    }
}