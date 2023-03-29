// ------------------------------------------------------------------------------
// <copyright file="NativeFunctionDeclarationFactory.cs" company="Drake53">
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
        public static JassNativeFunctionDeclarationSyntax NativeFunctionDeclaration(JassIdentifierNameSyntax identifierName)
        {
            return new JassNativeFunctionDeclarationSyntax(
                null,
                Token(JassSyntaxKind.NativeKeyword),
                identifierName,
                JassEmptyParameterListSyntax.Value,
                ReturnClause(JassTypeSyntax.Nothing));
        }

        public static JassNativeFunctionDeclarationSyntax NativeFunctionDeclaration(string name)
        {
            return new JassNativeFunctionDeclarationSyntax(
                null,
                Token(JassSyntaxKind.NativeKeyword),
                ParseIdentifierName(name),
                JassEmptyParameterListSyntax.Value,
                ReturnClause(JassTypeSyntax.Nothing));
        }

        public static JassNativeFunctionDeclarationSyntax NativeFunctionDeclaration(JassIdentifierNameSyntax identifierName, JassReturnClauseSyntax returnClause)
        {
            return new JassNativeFunctionDeclarationSyntax(
                null,
                Token(JassSyntaxKind.NativeKeyword),
                identifierName,
                JassEmptyParameterListSyntax.Value,
                returnClause);
        }

        public static JassNativeFunctionDeclarationSyntax NativeFunctionDeclaration(JassIdentifierNameSyntax identifierName, JassTypeSyntax returnType)
        {
            return new JassNativeFunctionDeclarationSyntax(
                null,
                Token(JassSyntaxKind.NativeKeyword),
                identifierName,
                JassEmptyParameterListSyntax.Value,
                ReturnClause(returnType));
        }

        public static JassNativeFunctionDeclarationSyntax NativeFunctionDeclaration(JassIdentifierNameSyntax identifierName, string returnType)
        {
            return new JassNativeFunctionDeclarationSyntax(
                null,
                Token(JassSyntaxKind.NativeKeyword),
                identifierName,
                JassEmptyParameterListSyntax.Value,
                ReturnClause(ParseTypeName(returnType)));
        }

        public static JassNativeFunctionDeclarationSyntax NativeFunctionDeclaration(string name, JassReturnClauseSyntax returnClause)
        {
            return new JassNativeFunctionDeclarationSyntax(
                null,
                Token(JassSyntaxKind.NativeKeyword),
                ParseIdentifierName(name),
                JassEmptyParameterListSyntax.Value,
                returnClause);
        }

        public static JassNativeFunctionDeclarationSyntax NativeFunctionDeclaration(string name, JassTypeSyntax returnType)
        {
            return new JassNativeFunctionDeclarationSyntax(
                null,
                Token(JassSyntaxKind.NativeKeyword),
                ParseIdentifierName(name),
                JassEmptyParameterListSyntax.Value,
                ReturnClause(returnType));
        }

        public static JassNativeFunctionDeclarationSyntax NativeFunctionDeclaration(string name, string returnType)
        {
            return new JassNativeFunctionDeclarationSyntax(
                null,
                Token(JassSyntaxKind.NativeKeyword),
                ParseIdentifierName(name),
                JassEmptyParameterListSyntax.Value,
                ReturnClause(ParseTypeName(returnType)));
        }

        public static JassNativeFunctionDeclarationSyntax NativeFunctionDeclaration(JassIdentifierNameSyntax identifierName, JassReturnClauseSyntax returnClause, JassParameterListOrEmptyParameterListSyntax parameterList)
        {
            return new JassNativeFunctionDeclarationSyntax(
                null,
                Token(JassSyntaxKind.NativeKeyword),
                identifierName,
                parameterList,
                returnClause);
        }

        public static JassNativeFunctionDeclarationSyntax NativeFunctionDeclaration(JassIdentifierNameSyntax identifierName, JassReturnClauseSyntax returnClause, params JassParameterSyntax[] parameters)
        {
            return new JassNativeFunctionDeclarationSyntax(
                null,
                Token(JassSyntaxKind.NativeKeyword),
                identifierName,
                ParameterList(parameters),
                returnClause);
        }

        public static JassNativeFunctionDeclarationSyntax NativeFunctionDeclaration(JassIdentifierNameSyntax identifierName, JassReturnClauseSyntax returnClause, IEnumerable<JassParameterSyntax> parameters)
        {
            return new JassNativeFunctionDeclarationSyntax(
                null,
                Token(JassSyntaxKind.NativeKeyword),
                identifierName,
                ParameterList(parameters),
                returnClause);
        }

        public static JassNativeFunctionDeclarationSyntax NativeFunctionDeclaration(JassIdentifierNameSyntax identifierName, JassTypeSyntax returnType, JassParameterListOrEmptyParameterListSyntax parameterList)
        {
            return new JassNativeFunctionDeclarationSyntax(
                null,
                Token(JassSyntaxKind.NativeKeyword),
                identifierName,
                parameterList,
                ReturnClause(returnType));
        }

        public static JassNativeFunctionDeclarationSyntax NativeFunctionDeclaration(JassIdentifierNameSyntax identifierName, JassTypeSyntax returnType, params JassParameterSyntax[] parameters)
        {
            return new JassNativeFunctionDeclarationSyntax(
                null,
                Token(JassSyntaxKind.NativeKeyword),
                identifierName,
                ParameterList(parameters),
                ReturnClause(returnType));
        }

        public static JassNativeFunctionDeclarationSyntax NativeFunctionDeclaration(JassIdentifierNameSyntax identifierName, JassTypeSyntax returnType, IEnumerable<JassParameterSyntax> parameters)
        {
            return new JassNativeFunctionDeclarationSyntax(
                null,
                Token(JassSyntaxKind.NativeKeyword),
                identifierName,
                ParameterList(parameters),
                ReturnClause(returnType));
        }

        public static JassNativeFunctionDeclarationSyntax NativeFunctionDeclaration(JassIdentifierNameSyntax identifierName, string returnType, JassParameterListOrEmptyParameterListSyntax parameterList)
        {
            return new JassNativeFunctionDeclarationSyntax(
                null,
                Token(JassSyntaxKind.NativeKeyword),
                identifierName,
                parameterList,
                ReturnClause(ParseTypeName(returnType)));
        }

        public static JassNativeFunctionDeclarationSyntax NativeFunctionDeclaration(JassIdentifierNameSyntax identifierName, string returnType, params JassParameterSyntax[] parameters)
        {
            return new JassNativeFunctionDeclarationSyntax(
                null,
                Token(JassSyntaxKind.NativeKeyword),
                identifierName,
                ParameterList(parameters),
                ReturnClause(ParseTypeName(returnType)));
        }

        public static JassNativeFunctionDeclarationSyntax NativeFunctionDeclaration(JassIdentifierNameSyntax identifierName, string returnType, IEnumerable<JassParameterSyntax> parameters)
        {
            return new JassNativeFunctionDeclarationSyntax(
                null,
                Token(JassSyntaxKind.NativeKeyword),
                identifierName,
                ParameterList(parameters),
                ReturnClause(ParseTypeName(returnType)));
        }

        public static JassNativeFunctionDeclarationSyntax NativeFunctionDeclaration(string name, JassReturnClauseSyntax returnClause, JassParameterListOrEmptyParameterListSyntax parameterList)
        {
            return new JassNativeFunctionDeclarationSyntax(
                null,
                Token(JassSyntaxKind.NativeKeyword),
                ParseIdentifierName(name),
                parameterList,
                returnClause);
        }

        public static JassNativeFunctionDeclarationSyntax NativeFunctionDeclaration(string name, JassReturnClauseSyntax returnClause, params JassParameterSyntax[] parameters)
        {
            return new JassNativeFunctionDeclarationSyntax(
                null,
                Token(JassSyntaxKind.NativeKeyword),
                ParseIdentifierName(name),
                ParameterList(parameters),
                returnClause);
        }

        public static JassNativeFunctionDeclarationSyntax NativeFunctionDeclaration(string name, JassReturnClauseSyntax returnClause, IEnumerable<JassParameterSyntax> parameters)
        {
            return new JassNativeFunctionDeclarationSyntax(
                null,
                Token(JassSyntaxKind.NativeKeyword),
                ParseIdentifierName(name),
                ParameterList(parameters),
                returnClause);
        }

        public static JassNativeFunctionDeclarationSyntax NativeFunctionDeclaration(string name, JassTypeSyntax returnType, JassParameterListOrEmptyParameterListSyntax parameterList)
        {
            return new JassNativeFunctionDeclarationSyntax(
                null,
                Token(JassSyntaxKind.NativeKeyword),
                ParseIdentifierName(name),
                parameterList,
                ReturnClause(returnType));
        }

        public static JassNativeFunctionDeclarationSyntax NativeFunctionDeclaration(string name, JassTypeSyntax returnType, params JassParameterSyntax[] parameters)
        {
            return new JassNativeFunctionDeclarationSyntax(
                null,
                Token(JassSyntaxKind.NativeKeyword),
                ParseIdentifierName(name),
                ParameterList(parameters),
                ReturnClause(returnType));
        }

        public static JassNativeFunctionDeclarationSyntax NativeFunctionDeclaration(string name, JassTypeSyntax returnType, IEnumerable<JassParameterSyntax> parameters)
        {
            return new JassNativeFunctionDeclarationSyntax(
                null,
                Token(JassSyntaxKind.NativeKeyword),
                ParseIdentifierName(name),
                ParameterList(parameters),
                ReturnClause(returnType));
        }

        public static JassNativeFunctionDeclarationSyntax NativeFunctionDeclaration(string name, string returnType, JassParameterListOrEmptyParameterListSyntax parameterList)
        {
            return new JassNativeFunctionDeclarationSyntax(
                null,
                Token(JassSyntaxKind.NativeKeyword),
                ParseIdentifierName(name),
                parameterList,
                ReturnClause(ParseTypeName(returnType)));
        }

        public static JassNativeFunctionDeclarationSyntax NativeFunctionDeclaration(string name, string returnType, params JassParameterSyntax[] parameters)
        {
            return new JassNativeFunctionDeclarationSyntax(
                null,
                Token(JassSyntaxKind.NativeKeyword),
                ParseIdentifierName(name),
                ParameterList(parameters),
                ReturnClause(ParseTypeName(returnType)));
        }

        public static JassNativeFunctionDeclarationSyntax NativeFunctionDeclaration(string name, string returnType, IEnumerable<JassParameterSyntax> parameters)
        {
            return new JassNativeFunctionDeclarationSyntax(
                null,
                Token(JassSyntaxKind.NativeKeyword),
                ParseIdentifierName(name),
                ParameterList(parameters),
                ReturnClause(ParseTypeName(returnType)));
        }

        public static JassNativeFunctionDeclarationSyntax NativeFunctionDeclaration(JassSyntaxToken nativeToken, JassIdentifierNameSyntax identifierName, JassReturnClauseSyntax returnClause, JassParameterListOrEmptyParameterListSyntax parameterList)
        {
            ThrowHelper.ThrowIfInvalidToken(nativeToken, JassSyntaxKind.NativeKeyword);

            return new JassNativeFunctionDeclarationSyntax(
                null,
                nativeToken,
                identifierName,
                parameterList,
                returnClause);
        }

        public static JassNativeFunctionDeclarationSyntax ConstantNativeFunctionDeclaration(JassIdentifierNameSyntax identifierName)
        {
            return new JassNativeFunctionDeclarationSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.NativeKeyword),
                identifierName,
                JassEmptyParameterListSyntax.Value,
                ReturnClause(JassTypeSyntax.Nothing));
        }

        public static JassNativeFunctionDeclarationSyntax ConstantNativeFunctionDeclaration(string name)
        {
            return new JassNativeFunctionDeclarationSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.NativeKeyword),
                ParseIdentifierName(name),
                JassEmptyParameterListSyntax.Value,
                ReturnClause(JassTypeSyntax.Nothing));
        }

        public static JassNativeFunctionDeclarationSyntax ConstantNativeFunctionDeclaration(JassIdentifierNameSyntax identifierName, JassReturnClauseSyntax returnClause)
        {
            return new JassNativeFunctionDeclarationSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.NativeKeyword),
                identifierName,
                JassEmptyParameterListSyntax.Value,
                returnClause);
        }

        public static JassNativeFunctionDeclarationSyntax ConstantNativeFunctionDeclaration(JassIdentifierNameSyntax identifierName, JassTypeSyntax returnType)
        {
            return new JassNativeFunctionDeclarationSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.NativeKeyword),
                identifierName,
                JassEmptyParameterListSyntax.Value,
                ReturnClause(returnType));
        }

        public static JassNativeFunctionDeclarationSyntax ConstantNativeFunctionDeclaration(JassIdentifierNameSyntax identifierName, string returnType)
        {
            return new JassNativeFunctionDeclarationSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.NativeKeyword),
                identifierName,
                JassEmptyParameterListSyntax.Value,
                ReturnClause(ParseTypeName(returnType)));
        }

        public static JassNativeFunctionDeclarationSyntax ConstantNativeFunctionDeclaration(string name, JassReturnClauseSyntax returnClause)
        {
            return new JassNativeFunctionDeclarationSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.NativeKeyword),
                ParseIdentifierName(name),
                JassEmptyParameterListSyntax.Value,
                returnClause);
        }

        public static JassNativeFunctionDeclarationSyntax ConstantNativeFunctionDeclaration(string name, JassTypeSyntax returnType)
        {
            return new JassNativeFunctionDeclarationSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.NativeKeyword),
                ParseIdentifierName(name),
                JassEmptyParameterListSyntax.Value,
                ReturnClause(returnType));
        }

        public static JassNativeFunctionDeclarationSyntax ConstantNativeFunctionDeclaration(string name, string returnType)
        {
            return new JassNativeFunctionDeclarationSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.NativeKeyword),
                ParseIdentifierName(name),
                JassEmptyParameterListSyntax.Value,
                ReturnClause(ParseTypeName(returnType)));
        }

        public static JassNativeFunctionDeclarationSyntax ConstantNativeFunctionDeclaration(JassIdentifierNameSyntax identifierName, JassReturnClauseSyntax returnClause, JassParameterListOrEmptyParameterListSyntax parameterList)
        {
            return new JassNativeFunctionDeclarationSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.NativeKeyword),
                identifierName,
                parameterList,
                returnClause);
        }

        public static JassNativeFunctionDeclarationSyntax ConstantNativeFunctionDeclaration(JassIdentifierNameSyntax identifierName, JassReturnClauseSyntax returnClause, params JassParameterSyntax[] parameters)
        {
            return new JassNativeFunctionDeclarationSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.NativeKeyword),
                identifierName,
                ParameterList(parameters),
                returnClause);
        }

        public static JassNativeFunctionDeclarationSyntax ConstantNativeFunctionDeclaration(JassIdentifierNameSyntax identifierName, JassReturnClauseSyntax returnClause, IEnumerable<JassParameterSyntax> parameters)
        {
            return new JassNativeFunctionDeclarationSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.NativeKeyword),
                identifierName,
                ParameterList(parameters),
                returnClause);
        }

        public static JassNativeFunctionDeclarationSyntax ConstantNativeFunctionDeclaration(JassIdentifierNameSyntax identifierName, JassTypeSyntax returnType, JassParameterListOrEmptyParameterListSyntax parameterList)
        {
            return new JassNativeFunctionDeclarationSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.NativeKeyword),
                identifierName,
                parameterList,
                ReturnClause(returnType));
        }

        public static JassNativeFunctionDeclarationSyntax ConstantNativeFunctionDeclaration(JassIdentifierNameSyntax identifierName, JassTypeSyntax returnType, params JassParameterSyntax[] parameters)
        {
            return new JassNativeFunctionDeclarationSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.NativeKeyword),
                identifierName,
                ParameterList(parameters),
                ReturnClause(returnType));
        }

        public static JassNativeFunctionDeclarationSyntax ConstantNativeFunctionDeclaration(JassIdentifierNameSyntax identifierName, JassTypeSyntax returnType, IEnumerable<JassParameterSyntax> parameters)
        {
            return new JassNativeFunctionDeclarationSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.NativeKeyword),
                identifierName,
                ParameterList(parameters),
                ReturnClause(returnType));
        }

        public static JassNativeFunctionDeclarationSyntax ConstantNativeFunctionDeclaration(JassIdentifierNameSyntax identifierName, string returnType, JassParameterListOrEmptyParameterListSyntax parameterList)
        {
            return new JassNativeFunctionDeclarationSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.NativeKeyword),
                identifierName,
                parameterList,
                ReturnClause(ParseTypeName(returnType)));
        }

        public static JassNativeFunctionDeclarationSyntax ConstantNativeFunctionDeclaration(JassIdentifierNameSyntax identifierName, string returnType, params JassParameterSyntax[] parameters)
        {
            return new JassNativeFunctionDeclarationSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.NativeKeyword),
                identifierName,
                ParameterList(parameters),
                ReturnClause(ParseTypeName(returnType)));
        }

        public static JassNativeFunctionDeclarationSyntax ConstantNativeFunctionDeclaration(JassIdentifierNameSyntax identifierName, string returnType, IEnumerable<JassParameterSyntax> parameters)
        {
            return new JassNativeFunctionDeclarationSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.NativeKeyword),
                identifierName,
                ParameterList(parameters),
                ReturnClause(ParseTypeName(returnType)));
        }

        public static JassNativeFunctionDeclarationSyntax ConstantNativeFunctionDeclaration(string name, JassReturnClauseSyntax returnClause, JassParameterListOrEmptyParameterListSyntax parameterList)
        {
            return new JassNativeFunctionDeclarationSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.NativeKeyword),
                ParseIdentifierName(name),
                parameterList,
                returnClause);
        }

        public static JassNativeFunctionDeclarationSyntax ConstantNativeFunctionDeclaration(string name, JassReturnClauseSyntax returnClause, params JassParameterSyntax[] parameters)
        {
            return new JassNativeFunctionDeclarationSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.NativeKeyword),
                ParseIdentifierName(name),
                ParameterList(parameters),
                returnClause);
        }

        public static JassNativeFunctionDeclarationSyntax ConstantNativeFunctionDeclaration(string name, JassReturnClauseSyntax returnClause, IEnumerable<JassParameterSyntax> parameters)
        {
            return new JassNativeFunctionDeclarationSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.NativeKeyword),
                ParseIdentifierName(name),
                ParameterList(parameters),
                returnClause);
        }

        public static JassNativeFunctionDeclarationSyntax ConstantNativeFunctionDeclaration(string name, JassTypeSyntax returnType, JassParameterListOrEmptyParameterListSyntax parameterList)
        {
            return new JassNativeFunctionDeclarationSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.NativeKeyword),
                ParseIdentifierName(name),
                parameterList,
                ReturnClause(returnType));
        }

        public static JassNativeFunctionDeclarationSyntax ConstantNativeFunctionDeclaration(string name, JassTypeSyntax returnType, params JassParameterSyntax[] parameters)
        {
            return new JassNativeFunctionDeclarationSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.NativeKeyword),
                ParseIdentifierName(name),
                ParameterList(parameters),
                ReturnClause(returnType));
        }

        public static JassNativeFunctionDeclarationSyntax ConstantNativeFunctionDeclaration(string name, JassTypeSyntax returnType, IEnumerable<JassParameterSyntax> parameters)
        {
            return new JassNativeFunctionDeclarationSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.NativeKeyword),
                ParseIdentifierName(name),
                ParameterList(parameters),
                ReturnClause(returnType));
        }

        public static JassNativeFunctionDeclarationSyntax ConstantNativeFunctionDeclaration(string name, string returnType, JassParameterListOrEmptyParameterListSyntax parameterList)
        {
            return new JassNativeFunctionDeclarationSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.NativeKeyword),
                ParseIdentifierName(name),
                parameterList,
                ReturnClause(ParseTypeName(returnType)));
        }

        public static JassNativeFunctionDeclarationSyntax ConstantNativeFunctionDeclaration(string name, string returnType, params JassParameterSyntax[] parameters)
        {
            return new JassNativeFunctionDeclarationSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.NativeKeyword),
                ParseIdentifierName(name),
                ParameterList(parameters),
                ReturnClause(ParseTypeName(returnType)));
        }

        public static JassNativeFunctionDeclarationSyntax ConstantNativeFunctionDeclaration(string name, string returnType, IEnumerable<JassParameterSyntax> parameters)
        {
            return new JassNativeFunctionDeclarationSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                Token(JassSyntaxKind.NativeKeyword),
                ParseIdentifierName(name),
                ParameterList(parameters),
                ReturnClause(ParseTypeName(returnType)));
        }

        public static JassNativeFunctionDeclarationSyntax ConstantNativeFunctionDeclaration(JassSyntaxToken constantToken, JassSyntaxToken nativeToken, JassIdentifierNameSyntax identifierName, JassReturnClauseSyntax returnClause, JassParameterListOrEmptyParameterListSyntax parameterList)
        {
            ThrowHelper.ThrowIfInvalidToken(constantToken, JassSyntaxKind.ConstantKeyword);
            ThrowHelper.ThrowIfInvalidToken(nativeToken, JassSyntaxKind.NativeKeyword);

            return new JassNativeFunctionDeclarationSyntax(
                constantToken,
                nativeToken,
                identifierName,
                parameterList,
                returnClause);
        }
    }
}