// ------------------------------------------------------------------------------
// <copyright file="FileFactory.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1649 // File name should match first type name

using System.Collections.Generic;
using System.Linq;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static FileSyntax File(params NewDeclarationSyntax[] declarations)
        {
            return new FileSyntax(
                new EmptyNode(0),
                new DeclarationListSyntax(declarations),
                new FunctionListSyntax(new EmptyNode(0)),
                new TokenNode(new SyntaxToken(SyntaxTokenType.EndOfFile), 0));
        }

        public static FileSyntax File(params FunctionSyntax[] functions)
        {
            return new FileSyntax(
                new EmptyNode(0),
                new DeclarationListSyntax(new EmptyNode(0)),
                new FunctionListSyntax(functions),
                new TokenNode(new SyntaxToken(SyntaxTokenType.EndOfFile), 0));
        }

        public static FileSyntax File(IEnumerable<NewDeclarationSyntax> declarations)
        {
            return new FileSyntax(
                new EmptyNode(0),
                new DeclarationListSyntax(declarations.ToArray()),
                new FunctionListSyntax(new EmptyNode(0)),
                new TokenNode(new SyntaxToken(SyntaxTokenType.EndOfFile), 0));
        }

        public static FileSyntax File(IEnumerable<FunctionSyntax> functions)
        {
            return new FileSyntax(
                new EmptyNode(0),
                new DeclarationListSyntax(new EmptyNode(0)),
                new FunctionListSyntax(functions.ToArray()),
                new TokenNode(new SyntaxToken(SyntaxTokenType.EndOfFile), 0));
        }

        public static FileSyntax File(IEnumerable<NewDeclarationSyntax> declarations, IEnumerable<FunctionSyntax> functions)
        {
            return new FileSyntax(
                new EmptyNode(0),
                new DeclarationListSyntax(declarations.ToArray()),
                new FunctionListSyntax(functions.ToArray()),
                new TokenNode(new SyntaxToken(SyntaxTokenType.EndOfFile), 0));
        }
    }
}