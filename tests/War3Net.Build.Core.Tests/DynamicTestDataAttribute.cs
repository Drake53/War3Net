// ------------------------------------------------------------------------------
// <copyright file="DynamicTestDataAttribute.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace War3Net.Build.Core.Tests
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class DynamicTestDataAttribute : Attribute, ITestDataSource
    {
        private readonly TestDataFileType _testDataFileType;

        public DynamicTestDataAttribute(TestDataFileType testDataFileType)
        {
            _testDataFileType = testDataFileType;
        }

        public IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            return TestDataFileProvider.GetFilePathsForTestDataType(_testDataFileType);
        }

        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            if (data.Length == 1 && data[0] is string filePath)
            {
                var fileName = Path.GetFileName(filePath);

                foreach (var knownFileName in TestDataFileParams.Get(_testDataFileType).KnownFileNames)
                {
                    if (string.Equals(fileName, knownFileName, StringComparison.OrdinalIgnoreCase))
                    {
                        return Path.GetFileName(Path.GetDirectoryName(filePath));
                    }
                }

                return fileName;
            }

            return methodInfo.Name;
        }
    }
}