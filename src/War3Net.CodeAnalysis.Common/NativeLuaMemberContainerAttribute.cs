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
    /// <remarks>
    /// This attribute is only useful when it's applied to classes whose .cs source files are passed to the compiler.
    /// It has no effect on classes that are referenced through a .dll file.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class NativeLuaMemberContainerAttribute : Attribute
    {
    }
}