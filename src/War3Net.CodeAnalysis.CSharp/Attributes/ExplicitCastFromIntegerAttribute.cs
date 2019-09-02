// ------------------------------------------------------------------------------
// <copyright file="ExplicitCastFromIntegerAttribute.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.CSharp.Attributes
{
    /// <summary>
    /// Indicates that this enum type can not be cast to from an integer directly, and instead requires the invocation of a certain method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum, AllowMultiple = false)]
    [Obsolete("Use attribute defined in CSharp.lua instead", true)]
    public class ExplicitCastFromIntegerAttribute : Attribute
    {
        public ExplicitCastFromIntegerAttribute(string castMethodName)
        {
            ConvertFunction = castMethodName;
        }

        public string ConvertFunction { get; private set; }
    }
}