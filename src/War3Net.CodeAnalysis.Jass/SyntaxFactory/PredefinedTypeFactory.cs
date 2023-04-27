﻿// ------------------------------------------------------------------------------
// <copyright file="PredefinedTypeFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        private static readonly HashSet<JassSyntaxKind> _predefinedTypeSyntaxKinds = GetPredefinedTypeSyntaxKinds();

        public static JassPredefinedTypeSyntax PredefinedType(JassSyntaxToken keyword)
        {
            if (!_predefinedTypeSyntaxKinds.Contains(keyword.SyntaxKind))
            {
                throw new ArgumentException($"The token's syntax kind must be one of: [ {string.Join(", ", _predefinedTypeSyntaxKinds.Select(predefinedType => $"'{predefinedType}'"))} ].", nameof(keyword));
            }

            return new JassPredefinedTypeSyntax(keyword);
        }

        private static HashSet<JassSyntaxKind> GetPredefinedTypeSyntaxKinds()
        {
            return new HashSet<JassSyntaxKind>
            {
                JassSyntaxKind.BooleanKeyword,
                JassSyntaxKind.CodeKeyword,
                JassSyntaxKind.HandleKeyword,
                JassSyntaxKind.IntegerKeyword,
                JassSyntaxKind.NothingKeyword,
                JassSyntaxKind.RealKeyword,
                JassSyntaxKind.StringKeyword,
            };
        }
    }
}