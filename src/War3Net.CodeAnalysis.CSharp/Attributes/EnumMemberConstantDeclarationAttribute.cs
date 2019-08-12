// ------------------------------------------------------------------------------
// <copyright file="EnumMemberConstantDeclarationAttribute.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.CSharp.Attributes
{
    /// <summary>
    /// For use with <see cref="ExplicitCastFromIntegerAttribute"/>.
    /// This attribute points to the constant declaration that precomputes the explicit cast from integer to this enum member.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class EnumMemberConstantDeclarationAttribute : Attribute
    {
        public EnumMemberConstantDeclarationAttribute(string constantDeclarationName)
        {
            Declaration = constantDeclarationName;
        }

        public string Declaration { get; private set; }
    }
}