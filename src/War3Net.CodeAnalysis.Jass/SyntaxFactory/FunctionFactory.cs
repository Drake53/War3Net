// ------------------------------------------------------------------------------
// <copyright file="FunctionFactory.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1649 // File name should match first type name

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static FunctionSyntax Function(FunctionDeclarationSyntax functionDeclaration, params NewStatementSyntax[] statements)
        {
            return new FunctionSyntax(
                new EmptyNode(0),
                new TokenNode(new SyntaxToken(SyntaxTokenType.FunctionKeyword), 0),
                functionDeclaration,
                new LineDelimiterSyntax(new EndOfLineSyntax(new TokenNode(new SyntaxToken(SyntaxTokenType.NewlineSymbol), 0))),
                new LocalVariableListSyntax(new EmptyNode(0)),
                new StatementListSyntax(statements),
                new TokenNode(new SyntaxToken(SyntaxTokenType.EndfunctionKeyword), 0),
                new LineDelimiterSyntax(new EndOfLineSyntax(new TokenNode(new SyntaxToken(SyntaxTokenType.NewlineSymbol), 0))));
        }

        public static FunctionSyntax Function(FunctionDeclarationSyntax functionDeclaration, LocalVariableListSyntax locals, params NewStatementSyntax[] statements)
        {
            return new FunctionSyntax(
                new EmptyNode(0),
                new TokenNode(new SyntaxToken(SyntaxTokenType.FunctionKeyword), 0),
                functionDeclaration,
                new LineDelimiterSyntax(new EndOfLineSyntax(new TokenNode(new SyntaxToken(SyntaxTokenType.NewlineSymbol), 0))),
                locals,
                new StatementListSyntax(statements),
                new TokenNode(new SyntaxToken(SyntaxTokenType.EndfunctionKeyword), 0),
                new LineDelimiterSyntax(new EndOfLineSyntax(new TokenNode(new SyntaxToken(SyntaxTokenType.NewlineSymbol), 0))));
        }

        public static FunctionSyntax Function(FunctionDeclarationSyntax functionDeclaration, IEnumerable<NewStatementSyntax> statements)
        {
            throw new NotImplementedException();
        }

        public static FunctionSyntax Function(FunctionDeclarationSyntax functionDeclaration, IEnumerable<LocalVariableDeclarationSyntax> localDeclarations, IEnumerable<NewStatementSyntax> statements)
        {
            throw new NotImplementedException();
        }
    }
}