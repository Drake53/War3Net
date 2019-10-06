// ------------------------------------------------------------------------------
// <copyright file="NativeLuaMemberContainerAttribute.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Common
{
    /// <summary>
    /// Indicates that the class is a container for members with the <see cref="NativeLuaMemberAttribute"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    [Obsolete("This attribute is not used during transpilation")]
    public class NativeLuaMemberContainerAttribute : Attribute
    {
    }
}