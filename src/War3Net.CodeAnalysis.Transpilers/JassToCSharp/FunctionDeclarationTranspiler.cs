// ------------------------------------------------------------------------------
// <copyright file="FunctionDeclarationTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        private const string JassEntryPointName = "main";
        private const string CSharpEntryPointName = "Main";

        // TODO: define these smwhere else
        public const string FilterNativeName = "Filter";
        public const string ConditionNativeName = "Condition";

        public static MethodDeclarationSyntax Transpile(this Jass.Syntax.FunctionDeclarationSyntax functionDeclarationNode)
        {
            _ = functionDeclarationNode ?? throw new ArgumentNullException(nameof(functionDeclarationNode));

            var functionName = functionDeclarationNode.IdentifierNode.ValueText;

            var functionDeclaration = SyntaxFactory.MethodDeclaration(
                default,
                default,
                functionDeclarationNode.ReturnTypeNode.Transpile(),
                null,
                functionName == JassEntryPointName
                ? SyntaxFactory.Identifier(CSharpEntryPointName)
                : SyntaxFactory.Identifier(
                    SyntaxTriviaList.Empty,
                    SyntaxKind.IdentifierToken,
                    functionDeclarationNode.IdentifierNode.TranspileIdentifier(),
                    functionName,
                    SyntaxTriviaList.Empty),
                null,
                SyntaxFactory.ParameterList(default),
                default,
                null,
                null);

            return functionDeclaration.AddParameterListParameters((
                functionName == FilterNativeName || functionName == ConditionNativeName
                ? functionDeclarationNode.ParameterListReferenceNode.Transpile(TokenTranspileFlags.ReturnBoolFunc)
                : functionDeclarationNode.ParameterListReferenceNode.Transpile()).ToArray());
        }
    }
}