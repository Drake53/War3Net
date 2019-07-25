// ------------------------------------------------------------------------------
// <copyright file="JassParseTest.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace War3Net.CodeAnalysis.Jass.Tests
{
    [TestClass]
    public class JassParseTest
    {
        [Timeout(1000)]
        [DataTestMethod]
        [DynamicData(nameof(GetJassSourceFiles), DynamicDataSourceType.Method)]
        public void TestParseJassFile(string inputFilePath)
        {
            var tokenizer = new JassTokenizer(File.ReadAllText(inputFilePath)/*, null*/);

            foreach (var token in tokenizer.Tokenize())
            {
                Console.WriteLine(token);
            }

            // Assert.Fail();
        }

        private static IEnumerable<object[]> GetJassSourceFiles()
        {
            // yield return new object[] { "TestData/common.j" };
            // yield return new object[] { "TestData/Blizzard.j" };

            yield break;
        }
    }
}