// ------------------------------------------------------------------------------
// <copyright file="NativeLuaMemberContainerAttribute.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.CSharp.Attributes
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