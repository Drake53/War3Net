namespace War3Net.Build.Core.Tests.Providers
{
    [TestClass]
    public class RandomItemProviderTests
    {
        [TestMethod]
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