// ------------------------------------------------------------------------------
// <copyright file="PredefinedTypeFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Linq;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassPredefinedTypeSyntax PredefinedType(JassSyntaxToken keyword)
        {
            if (!JassSyntaxFacts.IsPredefinedTypeKeyword(keyword.SyntaxKind))
            {
                throw new ArgumentException($"The token's syntax kind must be one of: [ {string.Join(", ", JassSyntaxFacts.GetPredefinedTypeKeywordKinds().Select(predefinedType => $"'{predefinedType}'"))} ].", nameof(keyword));
            }

            return new JassPredefinedTypeSyntax(keyword);
        }
    }
}