// ------------------------------------------------------------------------------
// <copyright file="JassSyntaxFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

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
    }
}