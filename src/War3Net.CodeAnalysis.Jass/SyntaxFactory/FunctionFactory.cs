// ------------------------------------------------------------------------------
// <copyright file="FunctionFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static FunctionSyntax Function(FunctionDeclarationSyntax functionDeclaration, params NewStatementSyntax[] statements)
        {
            return Function(functionDeclaration, LocalVariableList(), StatementList(statements));
        }

        public static FunctionSyntax Function(FunctionDeclarationSyntax functionDeclaration, LineDelimiterSyntax afterDeclaration, params NewStatementSyntax[] statements)
        {
            return Function(functionDeclaration, LocalVariableList(), StatementList(statements), afterDeclaration);
        }

        public static FunctionSyntax Function(FunctionDeclarationSyntax functionDeclaration, LocalVariableListSyntax locals, params NewStatementSyntax[] statements)
        {
            return Function(functionDeclaration, locals, StatementList(statements));
        }

        public static FunctionSyntax Function(FunctionDeclarationSyntax functionDeclaration, LineDelimiterSyntax afterDeclaration, LocalVariableListSyntax locals, params NewStatementSyntax[] statements)
        {
            return Function(functionDeclaration, locals, StatementList(statements), afterDeclaration);
        }

        public static FunctionSyntax Function(FunctionDeclarationSyntax functionDeclaration, IEnumerable<NewStatementSyntax> statements)
        {
            return Function(functionDeclaration, statements.ToArray());
        }

        public static FunctionSyntax Function(FunctionDeclarationSyntax functionDeclaration, IEnumerable<LocalVariableDeclarationSyntax> localDeclarations, IEnumerable<NewStatementSyntax> statements)
        {
            return Function(functionDeclaration, LocalVariableList(localDeclarations.ToArray()), statements.ToArray());
        }

        private static FunctionSyntax Function(
            FunctionDeclarationSyntax functionDeclaration,
            LocalVariableListSyntax locals,
            StatementListSyntax statements,
            LineDelimiterSyntax? afterDeclaration = null,
            LineDelimiterSyntax? afterFunction = null)
        {
            return new FunctionSyntax(
                Empty(),
                Token(SyntaxTokenType.FunctionKeyword),
                functionDeclaration,
                afterDeclaration ?? Newlines(),
                locals,
                statements,
                Token(SyntaxTokenType.EndfunctionKeyword),
                afterFunction ?? Newlines(2));
        }
    }
}