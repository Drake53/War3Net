// ------------------------------------------------------------------------------
// <copyright file="GlobalsDeclarationFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Immutable;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassGlobalsDeclarationSyntax GlobalsDeclaration(params JassGlobalDeclarationSyntax[] globalDeclarations)
        {
            return new JassGlobalsDeclarationSyntax(
                Token(JassSyntaxKind.GlobalsKeyword),
                globalDeclarations.ToImmutableArray(),
                Token(JassSyntaxKind.EndGlobalsKeyword));
        }

        public static JassGlobalsDeclarationSyntax GlobalsDeclaration(IEnumerable<JassGlobalDeclarationSyntax> globalDeclarations)
        {
            return new JassGlobalsDeclarationSyntax(
                Token(JassSyntaxKind.GlobalsKeyword),
                globalDeclarations.ToImmutableArray(),
                Token(JassSyntaxKind.EndGlobalsKeyword));
        }

        public static JassGlobalsDeclarationSyntax GlobalsDeclaration(ImmutableArray<JassGlobalDeclarationSyntax> globalDeclarations)
        {
            return new JassGlobalsDeclarationSyntax(
                Token(JassSyntaxKind.GlobalsKeyword),
                globalDeclarations,
                Token(JassSyntaxKind.EndGlobalsKeyword));
        }
    }
}