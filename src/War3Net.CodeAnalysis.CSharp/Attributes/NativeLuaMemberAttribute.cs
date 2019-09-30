// ------------------------------------------------------------------------------
// <copyright file="NativeLuaMemberAttribute.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.CSharp.Attributes
{
    /// <summary>
    /// Indicates that the member is natively available in lua, and therefore its namespace and class prefix should be omitted.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Method | AttributeTargets.Field, AllowMultiple = false)]
    public class NativeLuaMemberAttribute : Attribute
    {
    }
}