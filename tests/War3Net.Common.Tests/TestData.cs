// ------------------------------------------------------------------------------
// <copyright file="TestData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#nullable enable

using System;
using System.Collections.Generic;

namespace War3Net.Common.Tests
{
    internal static class TestData
    {
        internal static IEnumerable<object?[]> GetTestStrings()
        {
            yield return new[] { "Hello world" };
            yield return new[] { "Hello world!" };
            yield return new[] { string.Empty };
            yield return new[] { (string?)null };

            yield return new object?[] { "String cannot contain \0 characters.", typeof(ArgumentException) };
            yield return new[] { "But it can if it's the last character: \0" };
            yield return new[] { "\0" };

            yield return new object?[] { "\uD83D", typeof(ArgumentException) };
            yield return new object?[] { "\uD83D\uD83D", typeof(ArgumentException) };
            yield return new object?[] { "\uDCAB", typeof(ArgumentException) };
            yield return new object?[] { "\uDCAB\uDCAB", typeof(ArgumentException) };
            yield return new object?[] { "\uDCAB\uD83D", typeof(ArgumentException) };
            yield return new[] { "\uD83D\uDCAB" };
            yield return new[] { "a\uD83D\uDCAB" };
            yield return new[] { "\uD83D\uDCABa" };
            yield return new[] { "a\uD83D\uDCABa" };
        }
    }
}