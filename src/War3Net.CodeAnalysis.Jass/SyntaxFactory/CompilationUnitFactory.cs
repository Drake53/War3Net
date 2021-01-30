// ------------------------------------------------------------------------------
// <copyright file="CompilationUnitFactory.cs" company="Drake53">
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
        public static JassCompilationUnitSyntax CompilationUnit(IEnumerable<IDeclarationSyntax> declarations)
        {
            return new JassCompilationUnitSyntax(declarations.ToImmutableArray());
        }

        public static JassCompilationUnitSyntax CompilationUnit(params IDeclarationSyntax[] declarations)
        {
            return new JassCompilationUnitSyntax(declarations.ToImmutableArray());
        }
    }
}