// ------------------------------------------------------------------------------
// <copyright file="FunctionDeclarationContext.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

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
                IsActionsFunction = string.Equals(functionDeclaration.FunctionDeclarator.ReturnClause.ReturnType.GetToken().Text, JassKeyword.Nothing, StringComparison.Ordinal);
                IsConditionsFunction = string.Equals(functionDeclaration.FunctionDeclarator.ReturnClause.ReturnType.GetToken().Text, JassKeyword.Boolean, StringComparison.Ordinal);
            }

            Handled = false;
        }

        public JassFunctionDeclarationSyntax FunctionDeclaration { get; }

        public bool IsActionsFunction { get; }

        public bool IsConditionsFunction { get; }

        public bool Handled { get; set; }
    }
}