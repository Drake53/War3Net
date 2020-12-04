// ------------------------------------------------------------------------------
// <copyright file="FileFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static FileSyntax File(params NewDeclarationSyntax[] declarations)
        {
            return File(declarations, Array.Empty<FunctionSyntax>());
        }

        public static FileSyntax File(IEnumerable<NewDeclarationSyntax> declarations)
        {
            return File(declarations, Array.Empty<FunctionSyntax>());
        }

        public static FileSyntax File(params FunctionSyntax[] functions)
        {
            return File(Array.Empty<NewDeclarationSyntax>(), functions);
        }

        public static FileSyntax File(IEnumerable<FunctionSyntax> functions)
        {
            return File(Array.Empty<NewDeclarationSyntax>(), functions);
        }

        public static FileSyntax File(IEnumerable<NewDeclarationSyntax> declarations, IEnumerable<FunctionSyntax> functions)
        {
            return new FileSyntax(
                Empty(),
                new DeclarationListSyntax(declarations.ToArray()),
                new FunctionListSyntax(functions.ToArray()),
                Token(SyntaxTokenType.EndOfFile));
        }

        public static FileSyntax File(LineDelimiterSyntax header, IEnumerable<NewDeclarationSyntax> declarations, IEnumerable<FunctionSyntax> functions)
        {
            return new FileSyntax(
                header,
                new DeclarationListSyntax(declarations.ToArray()),
                new FunctionListSyntax(functions.ToArray()),
                Token(SyntaxTokenType.EndOfFile));
        }
    }
}