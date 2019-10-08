// ------------------------------------------------------------------------------
// <copyright file="NativeLuaMemberAttribute.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Common
{
    /// <summary>
    /// Indicates that the member is natively available in lua, and therefore its namespace and class prefix should be omitted.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Method | AttributeTargets.Field, AllowMultiple = false)]
    public class NativeLuaMemberAttribute : Attribute
    {
        public NativeLuaMemberAttribute()
        {
        }

        public NativeLuaMemberAttribute(string nativeFunctionName)
        {
        }
    }
}