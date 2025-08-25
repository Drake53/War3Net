// ------------------------------------------------------------------------------
// <copyright file="FlakyTestMethodAttribute.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

#if ENABLE_FLAKY_TESTS
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace War3Net.TestTools.UnitTesting
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class FlakyTestMethodAttribute
#if ENABLE_FLAKY_TESTS
        : TestMethodAttribute
#else
        : Attribute
#endif
    {
        public FlakyTestMethodAttribute(string? reason = null)
        {
            Reason = reason;
        }

        public string? Reason { get; }
    }
}