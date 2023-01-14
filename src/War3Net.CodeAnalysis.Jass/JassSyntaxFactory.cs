// ------------------------------------------------------------------------------
// <copyright file="JassSyntaxFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

using Pidgin;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassArgumentListSyntax ParseArgumentList(string argumentList)
        {
            return JassParser.Instance.ArgumentListParser.ParseOrThrow(argumentList);
        }

        public static JassCompilationUnitSyntax ParseCompilationUnit(string compilationUnit)
        {
            return JassParser.Instance.CompilationUnitParser.ParseOrThrow(compilationUnit);
        }

        public static JassExpressionSyntax ParseExpression(string expression)
        {
            return JassParser.Instance.ExpressionParser.ParseOrThrow(expression);
        }

        public static JassFunctionDeclarationSyntax ParseFunctionDeclaration(string functionDeclaration)
        {
            return JassParser.Instance.FunctionDeclarationParser.ParseOrThrow(functionDeclaration);
        }

        public static JassGlobalDeclarationSyntax ParseGlobalDeclaration(string globalDeclaration)
        {
            return JassParser.Instance.GlobalDeclarationParser.ParseOrThrow(globalDeclaration);
        }

        public static JassIdentifierNameSyntax ParseIdentifierName(string identifierName)
        {
            return JassParser.Instance.IdentifierNameParser.ParseOrThrow(identifierName);
        }

        public static JassParameterListOrEmptyParameterListSyntax ParseParameterList(string parameterList)
        {
            return JassParser.Instance.ParameterListParser.ParseOrThrow(parameterList);
        }

        public static JassStatementSyntax ParseStatement(string statement)
        {
            return JassParser.Instance.StatementParser.ParseOrThrow(statement);
        }

        public static JassTopLevelDeclarationSyntax ParseTopLevelDeclaration(string topLevelDeclaration)
        {
            return JassParser.Instance.TopLevelDeclarationParser.ParseOrThrow(topLevelDeclaration);
        }

        public static JassTypeSyntax ParseTypeName(string typeName)
        {
            return JassParser.Instance.TypeParser.ParseOrThrow(typeName);
        }

        public static bool TryParseArgumentList(string argumentList, [NotNullWhen(true)] out JassArgumentListSyntax? result)
        {
            return TryParse(argumentList, JassParser.Instance.ArgumentListParser, out result);
        }

        public static bool TryParseCompilationUnit(string compilationUnit, [NotNullWhen(true)] out JassCompilationUnitSyntax? result)
        {
            return TryParse(compilationUnit, JassParser.Instance.CompilationUnitParser, out result);
        }

        public static bool TryParseExpression(string expression, [NotNullWhen(true)] out JassExpressionSyntax? result)
        {
            return TryParse(expression, JassParser.Instance.ExpressionParser, out result);
        }

        public static bool TryParseFunctionDeclaration(string functionDeclaration, [NotNullWhen(true)] out JassFunctionDeclarationSyntax? result)
        {
            return TryParse(functionDeclaration, JassParser.Instance.FunctionDeclarationParser, out result);
        }

        public static bool TryParseGlobalDeclaration(string globalDeclaration, [NotNullWhen(true)] out JassGlobalDeclarationSyntax? result)
        {
            return TryParse(globalDeclaration, JassParser.Instance.GlobalDeclarationParser, out result);
        }

        public static bool TryParseIdentifierName(string identifierName, [NotNullWhen(true)] out JassIdentifierNameSyntax? result)
        {
            return TryParse(identifierName, JassParser.Instance.IdentifierNameParser, out result);
        }

        public static bool TryParseParameterList(string parameterList, [NotNullWhen(true)] out JassParameterListOrEmptyParameterListSyntax? result)
        {
            return TryParse(parameterList, JassParser.Instance.ParameterListParser, out result);
        }

        public static bool TryParseStatement(string statement, [NotNullWhen(true)] out JassStatementSyntax? result)
        {
            return TryParse(statement, JassParser.Instance.StatementParser, out result);
        }

        public static bool TryParseTopLevelDeclaration(string topLevelDeclaration, [NotNullWhen(true)] out JassTopLevelDeclarationSyntax? result)
        {
            return TryParse(topLevelDeclaration, JassParser.Instance.TopLevelDeclarationParser, out result);
        }

        public static bool TryParseTypeName(string typeName, [NotNullWhen(true)] out JassTypeSyntax? result)
        {
            return TryParse(typeName, JassParser.Instance.TypeParser, out result);
        }

        private static bool TryParse<TSyntax>(string input, Parser<char, TSyntax> parser, [NotNullWhen(true)] out TSyntax? result)
            where TSyntax : class
        {
            var parseResult = parser.Parse(input);
            if (parseResult.Success)
            {
                result = parseResult.Value;
                return true;
            }

            result = null;
            return false;
        }

        private static bool TryParse<TSyntax>(string input, Parser<char, TSyntax> parser, [NotNullWhen(true)] out TSyntax? result)
            where TSyntax : struct
        {
            var parseResult = parser.Parse(input);
            if (parseResult.Success)
            {
                result = parseResult.Value;
                return true;
            }

            result = null;
            return false;
        }
    }
}