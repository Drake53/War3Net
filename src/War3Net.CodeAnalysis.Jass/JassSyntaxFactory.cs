// ------------------------------------------------------------------------------
// <copyright file="JassSyntaxFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

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

        public static BinaryOperatorType ParseBinaryOperator(string binaryOperator)
        {
            return JassParser.Instance.BinaryOperatorParser.ParseOrThrow(binaryOperator);
        }

        public static JassCompilationUnitSyntax ParseCompilationUnit(string compilationUnit)
        {
            return JassParser.Instance.CompilationUnitParser.ParseOrThrow(compilationUnit);
        }

        public static ICustomScriptAction ParseCustomScriptAction(string customScriptAction)
        {
            return JassParser.Instance.CustomScriptActionParser.ParseOrThrow(customScriptAction);
        }

        public static IDeclarationSyntax ParseDeclaration(string declaration)
        {
            return JassParser.Instance.DeclarationParser.ParseOrThrow(declaration);
        }

        public static IExpressionSyntax ParseExpression(string expression)
        {
            return JassParser.Instance.ExpressionParser.ParseOrThrow(expression);
        }

        public static JassFunctionDeclarationSyntax ParseFunctionDeclaration(string functionDeclaration)
        {
            return JassParser.Instance.FunctionDeclarationParser.ParseOrThrow(functionDeclaration);
        }

        public static JassIdentifierNameSyntax ParseIdentifierName(string identifierName)
        {
            return JassParser.Instance.IdentifierNameParser.ParseOrThrow(identifierName);
        }

        public static JassParameterListSyntax ParseParameterList(string parameterList)
        {
            return JassParser.Instance.ParameterListParser.ParseOrThrow(parameterList);
        }

        public static IStatementSyntax ParseStatement(string statement)
        {
            return JassParser.Instance.StatementParser.ParseOrThrow(statement);
        }

        public static JassTypeSyntax ParseTypeName(string typeName)
        {
            return JassParser.Instance.TypeParser.ParseOrThrow(typeName);
        }

        public static UnaryOperatorType ParseUnaryOperator(string unaryOperator)
        {
            return JassParser.Instance.UnaryOperatorParser.ParseOrThrow(unaryOperator);
        }

        public static bool TryParseArgumentList(string argumentList, [NotNullWhen(true)] out JassArgumentListSyntax? result)
        {
            return TryParse(argumentList, JassParser.Instance.ArgumentListParser, out result);
        }

        public static bool TryParseBinaryOperator(string binaryOperator, [NotNullWhen(true)] out BinaryOperatorType? result)
        {
            return TryParse(binaryOperator, JassParser.Instance.BinaryOperatorParser, out result);
        }

        public static bool TryParseCompilationUnit(string compilationUnit, [NotNullWhen(true)] out JassCompilationUnitSyntax? result)
        {
            return TryParse(compilationUnit, JassParser.Instance.CompilationUnitParser, out result);
        }

        public static bool TryParseCustomScriptAction(string customScriptAction, [NotNullWhen(true)] out ICustomScriptAction? result)
        {
            return TryParse(customScriptAction, JassParser.Instance.CustomScriptActionParser, out result);
        }

        public static bool TryParseDeclaration(string declaration, [NotNullWhen(true)] out IDeclarationSyntax? result)
        {
            return TryParse(declaration, JassParser.Instance.DeclarationParser, out result);
        }

        public static bool TryParseExpression(string expression, [NotNullWhen(true)] out IExpressionSyntax? result)
        {
            return TryParse(expression, JassParser.Instance.ExpressionParser, out result);
        }

        public static bool TryParseFunctionDeclaration(string functionDeclaration, [NotNullWhen(true)] out JassFunctionDeclarationSyntax? result)
        {
            return TryParse(functionDeclaration, JassParser.Instance.FunctionDeclarationParser, out result);
        }

        public static bool TryParseIdentifierName(string identifierName, [NotNullWhen(true)] out JassIdentifierNameSyntax? result)
        {
            return TryParse(identifierName, JassParser.Instance.IdentifierNameParser, out result);
        }

        public static bool TryParseParameterList(string parameterList, [NotNullWhen(true)] out JassParameterListSyntax? result)
        {
            return TryParse(parameterList, JassParser.Instance.ParameterListParser, out result);
        }

        public static bool TryParseStatement(string statement, [NotNullWhen(true)] out IStatementSyntax? result)
        {
            return TryParse(statement, JassParser.Instance.StatementParser, out result);
        }

        public static bool TryParseTypeName(string typeName, [NotNullWhen(true)] out JassTypeSyntax? result)
        {
            return TryParse(typeName, JassParser.Instance.TypeParser, out result);
        }

        public static bool TryParseUnaryOperator(string unaryOperator, [NotNullWhen(true)] out UnaryOperatorType? result)
        {
            return TryParse(unaryOperator, JassParser.Instance.UnaryOperatorParser, out result);
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