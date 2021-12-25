// ------------------------------------------------------------------------------
// <copyright file="KnownPlayerColorTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Drawing;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Common;
using War3Net.Build.Extensions;

namespace War3Net.Build.Core.Tests.Common
{
    [TestClass]
    public class KnownPlayerColorTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetTestData), DynamicDataSourceType.Method)]
        public void TestKnownPlayerColor(Color color, KnownPlayerColor playerColor)
        {
            Assert.IsTrue(color.TryGetKnownPlayerColor(out var actualPlayerColor));
            Assert.AreEqual(playerColor, actualPlayerColor);

            if (color != PlayerColor.YellowAlt)
            {
                Assert.AreEqual(color, PlayerColor.FromKnownColor(playerColor));
            }
        }

        private static IEnumerable<object[]> GetTestData()
        {
            yield return new object[] { PlayerColor.Red, KnownPlayerColor.Red };
            yield return new object[] { PlayerColor.Blue, KnownPlayerColor.Blue };
            yield return new object[] { PlayerColor.Teal, KnownPlayerColor.Teal };
            yield return new object[] { PlayerColor.Purple, KnownPlayerColor.Purple };
            yield return new object[] { PlayerColor.Yellow, KnownPlayerColor.Yellow };
            yield return new object[] { PlayerColor.YellowAlt, KnownPlayerColor.Yellow };
            yield return new object[] { PlayerColor.Orange, KnownPlayerColor.Orange };
            yield return new object[] { PlayerColor.Green, KnownPlayerColor.Green };
            yield return new object[] { PlayerColor.Pink, KnownPlayerColor.Pink };
            yield return new object[] { PlayerColor.Gray, KnownPlayerColor.Gray };
            yield return new object[] { PlayerColor.LightBlue, KnownPlayerColor.LightBlue };
            yield return new object[] { PlayerColor.DarkGreen, KnownPlayerColor.DarkGreen };
            yield return new object[] { PlayerColor.Brown, KnownPlayerColor.Brown };
            yield return new object[] { PlayerColor.Maroon, KnownPlayerColor.Maroon };
            yield return new object[] { PlayerColor.Navy, KnownPlayerColor.Navy };
            yield return new object[] { PlayerColor.Turquoise, KnownPlayerColor.Turquoise };
            yield return new object[] { PlayerColor.Violet, KnownPlayerColor.Violet };
            yield return new object[] { PlayerColor.Wheat, KnownPlayerColor.Wheat };
            yield return new object[] { PlayerColor.Peach, KnownPlayerColor.Peach };
            yield return new object[] { PlayerColor.Mint, KnownPlayerColor.Mint };
            yield return new object[] { PlayerColor.Lavender, KnownPlayerColor.Lavender };
            yield return new object[] { PlayerColor.Coal, KnownPlayerColor.Coal };
            yield return new object[] { PlayerColor.Snow, KnownPlayerColor.Snow };
            yield return new object[] { PlayerColor.Emerald, KnownPlayerColor.Emerald };
            yield return new object[] { PlayerColor.Peanut, KnownPlayerColor.Peanut };
            yield return new object[] { PlayerColor.Black, KnownPlayerColor.Black };
        }
    }
}