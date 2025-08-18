// ------------------------------------------------------------------------------
// <copyright file="FlakyDynamicTestDataAttribute.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using War3Net.Build;

namespace War3Net.CodeAnalysis.Decompilers.Tests
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class FlakyDynamicTestDataAttribute : DynamicTestDataAttribute
    {
        private readonly HashSet<string> _flakyTestNames;

        public FlakyDynamicTestDataAttribute(MapFiles mapFilesToOpen, params string[] flakyTestNames)
            : base(mapFilesToOpen)
        {
            _flakyTestNames = flakyTestNames.ToHashSet(StringComparer.OrdinalIgnoreCase);
        }

#if !ENABLE_FLAKY_TESTS
        public override IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            foreach (var data in base.GetData(methodInfo))
            {
                if (data.Length == 1 &&
                    data[0] is string filePath &&
                    _flakyTestNames.Contains(GetFileDisplayName(filePath)))
                {
                    continue;
                }

                yield return data;
            }
        }
#endif
    }
}