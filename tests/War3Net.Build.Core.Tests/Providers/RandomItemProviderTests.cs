// ------------------------------------------------------------------------------
// <copyright file="RandomItemProviderTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Providers;
using War3Net.Build.Widget;
using War3Net.Common.Extensions;

namespace War3Net.Build.Core.Tests.Providers
{
    [TestClass]
    public class RandomItemProviderTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetRandomItems), DynamicDataSourceType.Method)]
        public void TestGetRandomItemTypeCode(string expectedRawcode, ItemClass itemClass, int level)
        {
            var actualTypeCode = RandomItemProvider.GetRandomItemTypeCode(itemClass, level);
            Assert.AreEqual(expectedRawcode, actualTypeCode.ToRawcode());

            Assert.IsTrue(RandomItemProvider.IsRandomItem(actualTypeCode, out var actualItemClass, out var actualLevel));
            Assert.AreEqual(itemClass, actualItemClass);
            Assert.AreEqual(level, actualLevel);
        }

        private static IEnumerable<object[]> GetRandomItems()
        {
            yield return new object[] { "YiI5", ItemClass.Permanent, 5 };
            yield return new object[] { "YYI7", ItemClass.Any, 7 };
        }
    }
}