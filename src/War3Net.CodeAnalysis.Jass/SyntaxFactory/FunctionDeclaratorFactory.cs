// ------------------------------------------------------------------------------
// <copyright file="FunctionDeclaratorFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassFunctionDeclaratorSyntax FunctionDeclarator(JassIdentifierNameSyntax identifierName)
        {
            return new JassFunctionDeclaratorSyntax(
                null,
                Token(JassSyntaxKind.FunctionKeyword),
                identifierName,
                JassEmptyParameterListSyntax.Value,
                ReturnClause(JassTypeSyntax.Nothing));
        }

        public static JassFunctionDeclaratorSyntax FunctionDeclarator(string name)
        {
            return new JassFunctionDeclaratorSyntax(
                null,
                Token(JassSyntaxKind.FunctionKeyword),
                ParseIdentifierName(name),
                JassEmptyParameterListSyntax.Value,
                ReturnClause(JassTypeSyntax.Nothing));
        }

        public static JassFunctionDeclaratorSyntax FunctionDeclarator(JassIdentifierNameSyntax identifierName, JassReturnClauseSyntax returnClause)
        {
            return new JassFunctionDeclaratorSyntax(
                null,
                Token(JassSyntaxKind.FunctionKeyword),
                identifierName,
                JassEmptyParameterListSyntax.Value,
                returnClause);
        }

        public static JassFunctionDeclaratorSyntax FunctionDeclarator(JassIdentifierNameSyntax identifierName, JassTypeSyntax returnType)
        {
            return new JassFunctionDeclaratorSyntax(
                null,
                Token(JassSyntaxKind.FunctionKeyword),
                identifierName,
                JassEmptyParameterListSyntax.Value,
                ReturnClause(returnType));
        }

        public static JassFunctionDeclaratorSyntax FunctionDeclarator(JassIdentifierNameSyntax identifierName, string returnType)
        {
            return new JassFunctionDeclaratorSyntax(
                null,
                Token(JassSyntaxKind.FunctionKeyword),
                identifierName,
                JassEmptyParameterListSyntax.Value,
                ReturnClause(ParseTypeName(returnType)));
        }

        public static JassFunctionDeclaratorSyntax FunctionDeclarator(string name, JassReturnClauseSyntax returnClause)
        {
            return new JassFunctionDeclaratorSyntax(
                null,
                Token(JassSyntaxKind.FunctionKeyword),
                ParseIdentifierName(name),
                JassEmptyParameterListSyntax.Value,
                returnClause);
        }

        public static JassFunctionDeclaratorSyntax FunctionDeclarator(string name, JassTypeSyntax returnType)
        {
            return new JassFunctionDeclaratorSyntax(
                null,
                Token(JassSyntaxKind.FunctionKeyword),
                ParseIdentifierName(name),
                JassEmptyParameterListSyntax.Value,
                ReturnClause(returnType));
        }

        public static JassFunctionDeclaratorSyntax FunctionDeclarator(string name, string returnType)
        {
            return new JassFunctionDeclaratorSyntax(
                null,
                Token(JassSyntaxKind.FunctionKeyword),
                ParseIdentifierName(name),
                JassEmptyParameterListSyntax.Value,
                ReturnClause(ParseTypeName(returnType)));
        }

        public static JassFunctionDeclaratorSyntax FunctionDeclarator(JassIdentifierNameSyntax identifierName, JassReturnClauseSyntax returnClause, JassParameterListOrEmptyParameterListSyntax parameterList)
        {
            return new JassFunctionDeclaratorSyntax(
                null,
                Token(JassSyntaxKind.FunctionKeyword),
                identifierName,
                parameterList,
                returnClause);
        }

        public static JassFunctionDeclaratorSyntax FunctionDeclarator(JassIdentifierNameSyntax identifierName, JassReturnClauseSyntax returnClause, params JassParameterSyntax[] parameters)
        {
            return new JassFunctionDeclaratorSyntax(
                null,
                Token(JassSyntaxKind.FunctionKeyword),
                identifierName,
                ParameterList(parameters),
                returnClause);
        }

        public static JassFunctionDeclaratorSyntax FunctionDeclarator(JassIdentifierNameSyntax identifierName, JassReturnClauseSyntax returnClause, IEnumerable<JassParameterSyntax> parameters)
        {
            return new JassFunctionDeclaratorSyntax(
                null,
                Token(JassSyntaxKind.FunctionKeyword),
                identifierName,
                ParameterList(parameters),
                returnClause);
        }

        public static JassFunctionDeclaratorSyntax FunctionDeclarator(JassIdentifierNameSyntax identifierName, JassTypeSyntax returnType, JassParameterListOrEmptyParameterListSyntax parameterList)
        {
            return new JassFunctionDeclaratorSyntax(
                null,
                Token(JassSyntaxKind.FunctionKeyword),
                identifierName,
                parameterList,
                ReturnClause(returnType));
        }

        public static JassFunctionDeclaratorSyntax FunctionDeclarator(JassIdentifierNameSyntax identifierName, JassTypeSyntax returnType, params JassParameterSyntax[] parameters)
        {
            return new JassFunctionDeclaratorSyntax(
                null,
                Token(JassSyntaxKind.FunctionKeyword),
                identifierName,
                ParameterList(parameters),
                ReturnClause(returnType));
        }

        public static JassFunctionDeclaratorSyntax FunctionDeclarator(JassIdentifierNameSyntax identifierName, JassTypeSyntax returnType, IEnumerable<JassParameterSyntax> parameters)
        {
            return new JassFunctionDeclaratorSyntax(
                null,
                Token(JassSyntaxKind.FunctionKeyword),
                identifierName,
                ParameterList(parameters),
                ReturnClause(returnType));
        }

        public static JassFunctionDeclaratorSyntax FunctionDeclarator(JassIdentifierNameSyntax identifierName, string returnType, JassParameterListOrEmptyParameterListSyntax parameterList)
        {
            return new JassFunctionDeclaratorSyntax(
                null,
                Token(JassSyntaxKind.FunctionKeyword),
                identifierName,
                parameterList,
                ReturnClause(ParseTypeName(returnType)));
        }

        public static JassFunctionDeclaratorSyntax FunctionDeclarator(JassIdentifierNameSyntax identifierName, string returnType, params JassParameterSyntax[] parameters)
        {
            return new JassFunctionDeclaratorSyntax(
                null,
                Token(JassSyntaxKind.FunctionKeyword),
                identifierName,
                ParameterList(parameters),
                ReturnClause(ParseTypeName(returnType)));
        }

        public static JassFunctionDeclaratorSyntax FunctionDeclarator(JassIdentifierNameSyntax identifierName, string returnType, IEnumerable<JassParameterSyntax> parameters)
        {
            return new JassFunctionDeclaratorSyntax(
                null,
                Token(JassSyntaxKind.FunctionKeyword),
                identifierName,
                ParameterList(parameters),
                ReturnClause(ParseTypeName(returnType)));
        }

        public static JassFunctionDeclaratorSyntax FunctionDeclarator(string name, JassReturnClauseSyntax returnClause, JassParameterListOrEmptyParameterListSyntax parameterList)
        {
            return new JassFunctionDeclaratorSyntax(
                null,
                Token(JassSyntaxKind.FunctionKeyword),
                ParseIdentifierName(name),
                parameterList,
                returnClause);
        }

        public static JassFunctionDeclaratorSyntax FunctionDeclarator(string name, JassReturnClauseSyntax returnClause, params JassParameterSyntax[] parameters)
        {
            return new JassFunctionDeclaratorSyntax(
                null,
                Token(JassSyntaxKind.FunctionKeyword),
                ParseIdentifierName(name),
                ParameterList(parameters),
                returnClause);
        }

        public static JassFunctionDeclaratorSyntax FunctionDeclarator(string name, JassReturnClauseSyntax returnClause, IEnumerable<JassParameterSyntax> parameters)
        {
            return new JassFunctionDeclaratorSyntax(
                null,
                Token(JassSyntaxKind.FunctionKeyword),
                ParseIdentifierName(name),
                ParameterList(parameters),
                returnClause);
        }

        public static JassFunctionDeclaratorSyntax FunctionDeclarator(string name, JassTypeSyntax returnType, JassParameterListOrEmptyParameterListSyntax parameterList)
        {
            return new JassFunctionDeclaratorSyntax(
                null,
                Token(JassSyntaxKind.FunctionKeyword),
                ParseIdentifierName(name),
                parameterList,
                ReturnClause(returnType));
        }

        public static JassFunctionDeclaratorSyntax FunctionDeclarator(string name, JassTypeSyntax returnType, params JassParameterSyntax[] parameters)
        {
            return new JassFunctionDeclaratorSyntax(
                null,
                Token(JassSyntaxKind.FunctionKeyword),
                ParseIdentifierName(name),
                ParameterList(parameters),
                ReturnClause(returnType));
        }

        public static JassFunctionDeclaratorSyntax FunctionDeclarator(string name, JassTypeSyntax returnType, IEnumerable<JassParameterSyntax> parameters)
        {
            return new JassFunctionDeclaratorSyntax(
                null,
                Token(JassSyntaxKind.FunctionKeyword),
                ParseIdentifierName(name),
                ParameterList(parameters),
                ReturnClause(returnType));
        }

        public static JassFunctionDeclaratorSyntax FunctionDeclarator(string name, string returnType, JassParameterListOrEmptyParameterListSyntax parameterList)
        {
            return new JassFunctionDeclaratorSyntax(
                null,
                Token(JassSyntaxKind.FunctionKeyword),
                ParseIdentifierName(name),
                parameterList,
                ReturnClause(ParseTypeName(returnType)));
        }

        public static JassFunctionDeclaratorSyntax FunctionDeclarator(string name, string returnType, params JassParameterSyntax[] parameters)
        {
            return new JassFunctionDeclaratorSyntax(
                null,
                Token(JassSyntaxKind.FunctionKeyword),
                ParseIdentifierName(name),
                ParameterList(parameters),
                ReturnClause(ParseTypeName(returnType)));
        }

        public static JassFunctionDeclaratorSyntax FunctionDeclarator(string name, string returnType, IEnumerable<JassParameterSyntax> parameters)
        {
            return new JassFunctionDeclaratorSyntax(
                null,
                Token(JassSyntaxKind.FunctionKeyword),
                ParseIdentifierName(name),
                ParameterList(parameters),
                ReturnClause(ParseTypeName(returnType)));
        }

        public static JassFunctionDeclaratorSyntax FunctionDeclarator(JassSyntaxToken functionToken, JassIdentifierNameSyntax identifierName, JassReturnClauseSyntax returnClause, JassParameterListOrEmptyParameterListSyntax parameterList)
        {
            ThrowHelper.ThrowIfInvalidToken(functionToken, JassSyntaxKind.FunctionKeyword);

            return new JassFunctionDeclaratorSyntax(
                null,
                functionToken,
                identifierName,
                parameterList,
                returnClause);
        }

        public static JassFunctionDeclaratorSyntax ConstantFunctionDeclarator(JassIdentifierNameSyntax identifierName)
        {
            return new JassFunctionDeclaratorSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.FunctionKeyword),
                identifierName,
                JassEmptyParameterListSyntax.Value,
                ReturnClause(JassTypeSyntax.Nothing));
        }

        public static JassFunctionDeclaratorSyntax ConstantFunctionDeclarator(string name)
        {
            return new JassFunctionDeclaratorSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.FunctionKeyword),
                ParseIdentifierName(name),
                JassEmptyParameterListSyntax.Value,
                ReturnClause(JassTypeSyntax.Nothing));
        }

        public static JassFunctionDeclaratorSyntax ConstantFunctionDeclarator(JassIdentifierNameSyntax identifierName, JassReturnClauseSyntax returnClause)
        {
            return new JassFunctionDeclaratorSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.FunctionKeyword),
                identifierName,
                JassEmptyParameterListSyntax.Value,
                returnClause);
        }

        public static JassFunctionDeclaratorSyntax ConstantFunctionDeclarator(JassIdentifierNameSyntax identifierName, JassTypeSyntax returnType)
        {
            return new JassFunctionDeclaratorSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.FunctionKeyword),
                identifierName,
                JassEmptyParameterListSyntax.Value,
                ReturnClause(returnType));
        }

        public static JassFunctionDeclaratorSyntax ConstantFunctionDeclarator(JassIdentifierNameSyntax identifierName, string returnType)
        {
            return new JassFunctionDeclaratorSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.FunctionKeyword),
                identifierName,
                JassEmptyParameterListSyntax.Value,
                ReturnClause(ParseTypeName(returnType)));
        }

        public static JassFunctionDeclaratorSyntax ConstantFunctionDeclarator(string name, JassReturnClauseSyntax returnClause)
        {
            return new JassFunctionDeclaratorSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.FunctionKeyword),
                ParseIdentifierName(name),
                JassEmptyParameterListSyntax.Value,
                returnClause);
        }

        public static JassFunctionDeclaratorSyntax ConstantFunctionDeclarator(string name, JassTypeSyntax returnType)
        {
            return new JassFunctionDeclaratorSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.FunctionKeyword),
                ParseIdentifierName(name),
                JassEmptyParameterListSyntax.Value,
                ReturnClause(returnType));
        }

        public static JassFunctionDeclaratorSyntax ConstantFunctionDeclarator(string name, string returnType)
        {
            return new JassFunctionDeclaratorSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.FunctionKeyword),
                ParseIdentifierName(name),
                JassEmptyParameterListSyntax.Value,
                ReturnClause(ParseTypeName(returnType)));
        }

        public static JassFunctionDeclaratorSyntax ConstantFunctionDeclarator(JassIdentifierNameSyntax identifierName, JassReturnClauseSyntax returnClause, JassParameterListOrEmptyParameterListSyntax parameterList)
        {
            return new JassFunctionDeclaratorSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.FunctionKeyword),
                identifierName,
                parameterList,
                returnClause);
        }

        public static JassFunctionDeclaratorSyntax ConstantFunctionDeclarator(JassIdentifierNameSyntax identifierName, JassReturnClauseSyntax returnClause, params JassParameterSyntax[] parameters)
        {
            return new JassFunctionDeclaratorSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.FunctionKeyword),
                identifierName,
                ParameterList(parameters),
                returnClause);
        }

        public static JassFunctionDeclaratorSyntax ConstantFunctionDeclarator(JassIdentifierNameSyntax identifierName, JassReturnClauseSyntax returnClause, IEnumerable<JassParameterSyntax> parameters)
        {
            return new JassFunctionDeclaratorSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.FunctionKeyword),
                identifierName,
                ParameterList(parameters),
                returnClause);
        }

        public static JassFunctionDeclaratorSyntax ConstantFunctionDeclarator(JassIdentifierNameSyntax identifierName, JassTypeSyntax returnType, JassParameterListOrEmptyParameterListSyntax parameterList)
        {
            return new JassFunctionDeclaratorSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.FunctionKeyword),
                identifierName,
                parameterList,
                ReturnClause(returnType));
        }

        public static JassFunctionDeclaratorSyntax ConstantFunctionDeclarator(JassIdentifierNameSyntax identifierName, JassTypeSyntax returnType, params JassParameterSyntax[] parameters)
        {
            return new JassFunctionDeclaratorSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.FunctionKeyword),
                identifierName,
                ParameterList(parameters),
                ReturnClause(returnType));
        }

        public static JassFunctionDeclaratorSyntax ConstantFunctionDeclarator(JassIdentifierNameSyntax identifierName, JassTypeSyntax returnType, IEnumerable<JassParameterSyntax> parameters)
        {
            return new JassFunctionDeclaratorSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.FunctionKeyword),
                identifierName,
                ParameterList(parameters),
                ReturnClause(returnType));
        }

        public static JassFunctionDeclaratorSyntax ConstantFunctionDeclarator(JassIdentifierNameSyntax identifierName, string returnType, JassParameterListOrEmptyParameterListSyntax parameterList)
        {
            return new JassFunctionDeclaratorSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.FunctionKeyword),
                identifierName,
                parameterList,
                ReturnClause(ParseTypeName(returnType)));
        }

        public static JassFunctionDeclaratorSyntax ConstantFunctionDeclarator(JassIdentifierNameSyntax identifierName, string returnType, params JassParameterSyntax[] parameters)
        {
            return new JassFunctionDeclaratorSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.FunctionKeyword),
                identifierName,
                ParameterList(parameters),
                ReturnClause(ParseTypeName(returnType)));
        }

        public static JassFunctionDeclaratorSyntax ConstantFunctionDeclarator(JassIdentifierNameSyntax identifierName, string returnType, IEnumerable<JassParameterSyntax> parameters)
        {
            return new JassFunctionDeclaratorSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.FunctionKeyword),
                identifierName,
                ParameterList(parameters),
                ReturnClause(ParseTypeName(returnType)));
        }

        public static JassFunctionDeclaratorSyntax ConstantFunctionDeclarator(string name, JassReturnClauseSyntax returnClause, JassParameterListOrEmptyParameterListSyntax parameterList)
        {
            return new JassFunctionDeclaratorSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.FunctionKeyword),
                ParseIdentifierName(name),
                parameterList,
                returnClause);
        }

        public static JassFunctionDeclaratorSyntax ConstantFunctionDeclarator(string name, JassReturnClauseSyntax returnClause, params JassParameterSyntax[] parameters)
        {
            return new JassFunctionDeclaratorSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.FunctionKeyword),
                ParseIdentifierName(name),
                ParameterList(parameters),
                returnClause);
        }

        public static JassFunctionDeclaratorSyntax ConstantFunctionDeclarator(string name, JassReturnClauseSyntax returnClause, IEnumerable<JassParameterSyntax> parameters)
        {
            return new JassFunctionDeclaratorSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.FunctionKeyword),
                ParseIdentifierName(name),
                ParameterList(parameters),
                returnClause);
        }

        public static JassFunctionDeclaratorSyntax ConstantFunctionDeclarator(string name, JassTypeSyntax returnType, JassParameterListOrEmptyParameterListSyntax parameterList)
        {
            return new JassFunctionDeclaratorSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.FunctionKeyword),
                ParseIdentifierName(name),
                parameterList,
                ReturnClause(returnType));
        }

        public static JassFunctionDeclaratorSyntax ConstantFunctionDeclarator(string name, JassTypeSyntax returnType, params JassParameterSyntax[] parameters)
        {
            return new JassFunctionDeclaratorSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.FunctionKeyword),
                ParseIdentifierName(name),
                ParameterList(parameters),
                ReturnClause(returnType));
        }

        public static JassFunctionDeclaratorSyntax ConstantFunctionDeclarator(string name, JassTypeSyntax returnType, IEnumerable<JassParameterSyntax> parameters)
        {
            return new JassFunctionDeclaratorSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.FunctionKeyword),
                ParseIdentifierName(name),
                ParameterList(parameters),
                ReturnClause(returnType));
        }

        public static JassFunctionDeclaratorSyntax ConstantFunctionDeclarator(string name, string returnType, JassParameterListOrEmptyParameterListSyntax parameterList)
        {
            return new JassFunctionDeclaratorSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.FunctionKeyword),
                ParseIdentifierName(name),
                parameterList,
                ReturnClause(ParseTypeName(returnType)));
        }

        public static JassFunctionDeclaratorSyntax ConstantFunctionDeclarator(string name, string returnType, params JassParameterSyntax[] parameters)
        {
            return new JassFunctionDeclaratorSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.FunctionKeyword),
                ParseIdentifierName(name),
                ParameterList(parameters),
                ReturnClause(ParseTypeName(returnType)));
        }

        public static JassFunctionDeclaratorSyntax ConstantFunctionDeclarator(string name, string returnType, IEnumerable<JassParameterSyntax> parameters)
        {
            return new JassFunctionDeclaratorSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.FunctionKeyword),
                ParseIdentifierName(name),
                ParameterList(parameters),
                ReturnClause(ParseTypeName(returnType)));
        }

        public static JassFunctionDeclaratorSyntax ConstantFunctionDeclarator(JassSyntaxToken constantToken, JassSyntaxToken functionToken, JassIdentifierNameSyntax identifierName, JassTypeSyntax returnType, JassParameterListOrEmptyParameterListSyntax parameterList)
        {
            ThrowHelper.ThrowIfInvalidToken(constantToken, JassSyntaxKind.ConstantKeyword);
            ThrowHelper.ThrowIfInvalidToken(functionToken, JassSyntaxKind.FunctionKeyword);

            return new JassFunctionDeclaratorSyntax(
                constantToken,
                functionToken,
                identifierName,
                parameterList,
                ReturnClause(returnType));
        }

        public static JassFunctionDeclaratorSyntax ConditionFunctionDeclarator(string name)
        {
            return new JassFunctionDeclaratorSyntax(
                null,
                Token(JassSyntaxKind.FunctionKeyword),
                ParseIdentifierName(name),
                JassEmptyParameterListSyntax.Value,
                ReturnClause(JassTypeSyntax.Boolean));
        }
    }
}