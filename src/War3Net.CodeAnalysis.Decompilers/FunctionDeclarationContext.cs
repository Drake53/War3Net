// ------------------------------------------------------------------------------
// <copyright file="FunctionDeclarationContext.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    internal sealed class FunctionDeclarationContext
    {
        public FunctionDeclarationContext(JassFunctionDeclarationSyntax functionDeclaration)
        {
            FunctionDeclaration = functionDeclaration;

            if (functionDeclaration.FunctionDeclarator.ParameterList is JassEmptyParameterListSyntax)
            {
                var returnTypeToken = functionDeclaration.FunctionDeclarator.ReturnClause.ReturnType.GetToken();
                IsActionsFunction = returnTypeToken.SyntaxKind == JassSyntaxKind.NothingKeyword;
                IsConditionsFunction = returnTypeToken.SyntaxKind == JassSyntaxKind.BooleanKeyword;
            }

            Handled = false;
        }

        public JassFunctionDeclarationSyntax FunctionDeclaration { get; }

        public bool IsActionsFunction { get; }

        public bool IsConditionsFunction { get; }

        public bool Handled { get; set; }
    }
}