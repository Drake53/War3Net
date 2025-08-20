// ------------------------------------------------------------------------------
// <copyright file="CascProduct.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.IO.Casc.Enums
{
    /// <summary>
    /// Represents Blizzard game product codes used in CASC/TACT systems.
    /// </summary>
    /// <remarks>
    /// <para>
    /// These product codes are used when accessing patch servers and CDNs to retrieve
    /// version information, configuration files, and game data through the TACT protocol.
    /// The codes are used in URLs like: http://{region}.patch.battle.net:1119/{product}/versions.
    /// </para>
    /// <para>
    /// Each product may have multiple variants (retail, PTR, beta, classic) that are
    /// tracked as separate products in the CDN system.
    /// </para>
    /// </remarks>
    public static class CascProduct
    {
        /// <summary>
        /// Warcraft franchise products.
        /// </summary>
        public static class Warcraft
        {
            /// <summary>
            /// Warcraft III: Reforged.
            /// </summary>
            public const string W3 = "w3";

            /// <summary>
            /// Warcraft III: Public Test.
            /// </summary>
            public const string W3T = "w3t";

            /// <summary>
            /// Warcraft III: Old version.
            /// </summary>
            public const string War3 = "war3";

            /// <summary>
            /// Warcraft III: Reforged Beta.
            /// </summary>
            public const string W3B = "w3b";
        }

        /// <summary>
        /// World of Warcraft products.
        /// </summary>
        public static class WoW
        {
            /// <summary>
            /// World of Warcraft (retail).
            /// </summary>
            public const string Retail = "wow";

            /// <summary>
            /// World of Warcraft: Test.
            /// </summary>
            public const string Test = "wowt";

            /// <summary>
            /// World of Warcraft: Beta.
            /// </summary>
            public const string Beta = "wow_beta";

            /// <summary>
            /// World of Warcraft: Classic.
            /// </summary>
            public const string Classic = "wow_classic";

            /// <summary>
            /// World of Warcraft: Classic Beta.
            /// </summary>
            public const string ClassicBeta = "wow_classic_beta";

            /// <summary>
            /// World of Warcraft: Classic Public Test Realm.
            /// </summary>
            public const string ClassicPTR = "wow_classic_ptr";

            /// <summary>
            /// World of Warcraft: Classic Era.
            /// </summary>
            public const string ClassicEra = "wow_classic_era";

            /// <summary>
            /// World of Warcraft: Development.
            /// </summary>
            public const string Dev = "wowdev";
        }

        /// <summary>
        /// Diablo franchise products.
        /// </summary>
        public static class Diablo
        {
            /// <summary>
            /// Diablo II: Resurrected.
            /// </summary>
            public const string D2R = "osi";

            /// <summary>
            /// Diablo II: Resurrected Beta.
            /// </summary>
            public const string D2RBeta = "osib";

            /// <summary>
            /// Diablo II: Resurrected Alpha.
            /// </summary>
            public const string D2RAlpha = "osia";

            /// <summary>
            /// Diablo II: Resurrected Development.
            /// </summary>
            public const string D2RDev = "osidev";

            /// <summary>
            /// Diablo II: Resurrected Vendor 1.
            /// </summary>
            public const string D2RVendor1 = "osiv1";

            /// <summary>
            /// Diablo II: Resurrected Vendor 2.
            /// </summary>
            public const string D2RVendor2 = "osiv2";

            /// <summary>
            /// Diablo II: Resurrected Vendor 3.
            /// </summary>
            public const string D2RVendor3 = "osiv3";

            /// <summary>
            /// Diablo II: Resurrected Vendor 4.
            /// </summary>
            public const string D2RVendor4 = "osiv4";

            /// <summary>
            /// Diablo II: Resurrected Vendor 5.
            /// </summary>
            public const string D2RVendor5 = "osiv5";

            /// <summary>
            /// Diablo II: Resurrected Vendor 6.
            /// </summary>
            public const string D2RVendor6 = "osiv6";

            /// <summary>
            /// Diablo III.
            /// </summary>
            public const string D3 = "d3";

            /// <summary>
            /// Diablo III: Beta.
            /// </summary>
            public const string D3Beta = "d3b";

            /// <summary>
            /// Diablo III: China.
            /// </summary>
            public const string D3China = "d3cn";

            /// <summary>
            /// Diablo III: Test.
            /// </summary>
            public const string D3Test = "d3t";

            /// <summary>
            /// Diablo IV.
            /// </summary>
            public const string D4 = "fenris";

            /// <summary>
            /// Diablo IV: Beta.
            /// </summary>
            public const string D4Beta = "fenrisb";

            /// <summary>
            /// Diablo IV: Pre-launch Event.
            /// </summary>
            public const string D4Event = "fenrise";

            /// <summary>
            /// Diablo IV: Development.
            /// </summary>
            public const string D4Dev = "fenrisdev";

            /// <summary>
            /// Diablo IV: Vendor.
            /// </summary>
            public const string D4Vendor = "fenrisvendor";
        }

        /// <summary>
        /// StarCraft franchise products.
        /// </summary>
        public static class StarCraft
        {
            /// <summary>
            /// StarCraft: Remastered.
            /// </summary>
            public const string SC1 = "s1";

            /// <summary>
            /// StarCraft: Remastered Alpha.
            /// </summary>
            public const string SC1Alpha = "s1a";

            /// <summary>
            /// StarCraft: Remastered Test.
            /// </summary>
            public const string SC1Test = "s1t";

            /// <summary>
            /// StarCraft II.
            /// </summary>
            public const string SC2 = "s2";

            /// <summary>
            /// StarCraft II: Beta.
            /// </summary>
            public const string SC2Beta = "s2b";

            /// <summary>
            /// StarCraft II: Test.
            /// </summary>
            public const string SC2Test = "s2t";
        }

        /// <summary>
        /// Heroes of the Storm products.
        /// </summary>
        public static class Heroes
        {
            /// <summary>
            /// Heroes of the Storm.
            /// </summary>
            public const string Retail = "hero";

            /// <summary>
            /// Heroes of the Storm: Tournament.
            /// </summary>
            public const string Tournament = "heroc";

            /// <summary>
            /// Heroes of the Storm: Test.
            /// </summary>
            public const string Test = "herot";
        }

        /// <summary>
        /// Hearthstone products.
        /// </summary>
        public static class Hearthstone
        {
            /// <summary>
            /// Hearthstone.
            /// </summary>
            public const string Retail = "hsb";

            /// <summary>
            /// Hearthstone: Tournament.
            /// </summary>
            public const string Tournament = "hsc";

            /// <summary>
            /// Hearthstone: Test.
            /// </summary>
            public const string Test = "hst";
        }

        /// <summary>
        /// Overwatch franchise products.
        /// </summary>
        public static class Overwatch
        {
            /// <summary>
            /// Overwatch.
            /// </summary>
            public const string OW1 = "pro";

            /// <summary>
            /// Overwatch: Tournament.
            /// </summary>
            public const string OW1Tournament = "proc";

            /// <summary>
            /// Overwatch: Development.
            /// </summary>
            public const string OW1Dev = "prodev";

            /// <summary>
            /// Overwatch: Test.
            /// </summary>
            public const string OW1Test = "prot";

            /// <summary>
            /// Overwatch: Vendor.
            /// </summary>
            public const string OW1Vendor = "prov";
        }

        /// <summary>
        /// Call of Duty franchise products.
        /// </summary>
        public static class CallOfDuty
        {
            /// <summary>
            /// Call of Duty: Black Ops 4.
            /// </summary>
            public const string BlackOps4 = "viper";

            /// <summary>
            /// Call of Duty: Modern Warfare.
            /// </summary>
            public const string ModernWarfare = "odin";

            /// <summary>
            /// Call of Duty: Black Ops Cold War.
            /// </summary>
            public const string BlackOpsColdWar = "zeus";

            /// <summary>
            /// Call of Duty: Vanguard.
            /// </summary>
            public const string Vanguard = "fore";
        }

        /// <summary>
        /// Other non-Blizzard games.
        /// </summary>
        public static class Other
        {
            /// <summary>
            /// Crash Bandicoot 4: It's About Time.
            /// </summary>
            public const string CrashBandicoot4 = "wlby";

            /// <summary>
            /// Crash Bandicoot 4: Development.
            /// </summary>
            public const string CrashBandicoot4Dev = "wlbydev";

            /// <summary>
            /// Crash Bandicoot 4: Vendor 1.
            /// </summary>
            public const string CrashBandicoot4Vendor1 = "wlbyv1";

            /// <summary>
            /// Crash Bandicoot 4: Vendor 2.
            /// </summary>
            public const string CrashBandicoot4Vendor2 = "wlbyv2";

            /// <summary>
            /// Crash Bandicoot 4: Vendor 3.
            /// </summary>
            public const string CrashBandicoot4Vendor3 = "wlbyv3";

            /// <summary>
            /// Crash Bandicoot 4: Vendor 4.
            /// </summary>
            public const string CrashBandicoot4Vendor4 = "wlbyv4";

            /// <summary>
            /// Crash Bandicoot 4: Vendor 5.
            /// </summary>
            public const string CrashBandicoot4Vendor5 = "wlbyv5";

            /// <summary>
            /// Crash Bandicoot 4: Vendor 6.
            /// </summary>
            public const string CrashBandicoot4Vendor6 = "wlbyv6";

            /// <summary>
            /// Destiny 2.
            /// </summary>
            public const string Destiny2 = "dst2";

            /// <summary>
            /// Destiny 2: Alpha.
            /// </summary>
            public const string Destiny2Alpha = "dst2a";

            /// <summary>
            /// Destiny 2: Public Test.
            /// </summary>
            public const string Destiny2Test = "dst2t";
        }

        /// <summary>
        /// Battle.net platform products.
        /// </summary>
        public static class BattleNet
        {
            /// <summary>
            /// Battle.net Agent.
            /// </summary>
            public const string Agent = "agent";

            /// <summary>
            /// Battle.net Agent Test.
            /// </summary>
            public const string AgentTest = "agent_test";

            /// <summary>
            /// Battle.net App.
            /// </summary>
            public const string App = "bna";

            /// <summary>
            /// Battle.net Bootstrapper.
            /// </summary>
            public const string Bootstrapper = "bts";

            /// <summary>
            /// Battle.net Catalogs.
            /// </summary>
            public const string Catalogs = "catalogs";

            /// <summary>
            /// Battle.net Client (deprecated).
            /// </summary>
            public const string Client = "clnt";

            /// <summary>
            /// Battle.net Demo.
            /// </summary>
            public const string Demo = "demo";

            /// <summary>
            /// Battle.net Test (deprecated).
            /// </summary>
            public const string Test = "test";
        }
    }
}