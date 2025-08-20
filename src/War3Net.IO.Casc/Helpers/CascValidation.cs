// ------------------------------------------------------------------------------
// <copyright file="CascValidation.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using War3Net.IO.Casc.Enums;

namespace War3Net.IO.Casc.Helpers
{
    /// <summary>
    /// Provides validation for CASC product and region codes.
    /// </summary>
    public static class CascValidation
    {
        private static readonly HashSet<string> _validProducts;
        private static readonly HashSet<string> _validRegions;

        static CascValidation()
        {
            // Initialize valid products from CascProduct constants
            _validProducts = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                // Warcraft products
                CascProduct.Warcraft.W3,
                CascProduct.Warcraft.W3T,
                CascProduct.Warcraft.War3,
                CascProduct.Warcraft.W3B,

                // World of Warcraft products
                CascProduct.WoW.Retail,
                CascProduct.WoW.Test,
                CascProduct.WoW.Beta,
                CascProduct.WoW.Classic,
                CascProduct.WoW.ClassicBeta,
                CascProduct.WoW.ClassicPTR,
                CascProduct.WoW.ClassicEra,
                CascProduct.WoW.Dev,

                // Diablo products
                CascProduct.Diablo.D2R,
                CascProduct.Diablo.D2RBeta,
                CascProduct.Diablo.D2RAlpha,
                CascProduct.Diablo.D2RDev,
                CascProduct.Diablo.D3,
                CascProduct.Diablo.D3Beta,
                CascProduct.Diablo.D3China,
                CascProduct.Diablo.D3Test,
                CascProduct.Diablo.D4,
                CascProduct.Diablo.D4Beta,
                CascProduct.Diablo.D4Event,
                CascProduct.Diablo.D4Dev,
                CascProduct.Diablo.D4Vendor,

                // StarCraft products
                CascProduct.StarCraft.SC1,
                CascProduct.StarCraft.SC1Alpha,
                CascProduct.StarCraft.SC1Test,
                CascProduct.StarCraft.SC2,
                CascProduct.StarCraft.SC2Beta,
                CascProduct.StarCraft.SC2Test,

                // Heroes of the Storm products
                CascProduct.Heroes.Retail,
                CascProduct.Heroes.Tournament,
                CascProduct.Heroes.Test,

                // Hearthstone products
                CascProduct.Hearthstone.Retail,
                CascProduct.Hearthstone.Tournament,
                CascProduct.Hearthstone.Test,

                // Overwatch products
                CascProduct.Overwatch.OW1,
                CascProduct.Overwatch.OW1Tournament,
                CascProduct.Overwatch.OW1Dev,
                CascProduct.Overwatch.OW1Test,
                CascProduct.Overwatch.OW1Vendor,

                // Call of Duty products
                CascProduct.CallOfDuty.BlackOps4,
                CascProduct.CallOfDuty.ModernWarfare,
                CascProduct.CallOfDuty.BlackOpsColdWar,
                CascProduct.CallOfDuty.Vanguard,

                // Battle.net products
                CascProduct.BattleNet.Agent,
                CascProduct.BattleNet.AgentTest,
                CascProduct.BattleNet.App,
                CascProduct.BattleNet.Bootstrapper,
                CascProduct.BattleNet.Catalogs,
                CascProduct.BattleNet.Client,
                CascProduct.BattleNet.Demo,
                CascProduct.BattleNet.Test,
            };

            // Initialize valid regions from CascRegion constants
            _validRegions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                CascRegion.US,
                CascRegion.EU,
                CascRegion.KR,
                CascRegion.CN,
                CascRegion.TW,
            };
        }

        /// <summary>
        /// Gets the set of valid product codes.
        /// </summary>
        public static IReadOnlyCollection<string> ValidProducts => _validProducts;

        /// <summary>
        /// Gets the set of valid region codes.
        /// </summary>
        public static IReadOnlyCollection<string> ValidRegions => _validRegions;

        /// <summary>
        /// Determines whether the specified product code is valid.
        /// </summary>
        /// <param name="product">The product code to validate.</param>
        /// <returns><see langword="true"/> if the product code is valid; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="product"/> is <see langword="null"/>.</exception>
        public static bool IsValidProduct(string product)
        {
            if (product is null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            return _validProducts.Contains(product);
        }

        /// <summary>
        /// Determines whether the specified region code is valid.
        /// </summary>
        /// <param name="region">The region code to validate.</param>
        /// <returns><see langword="true"/> if the region code is valid; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="region"/> is <see langword="null"/>.</exception>
        public static bool IsValidRegion(string region)
        {
            if (region is null)
            {
                throw new ArgumentNullException(nameof(region));
            }

            return _validRegions.Contains(region);
        }
    }
}