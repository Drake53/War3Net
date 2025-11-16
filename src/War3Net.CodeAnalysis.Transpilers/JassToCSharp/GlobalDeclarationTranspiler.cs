// ------------------------------------------------------------------------------
// <copyright file="GlobalDeclarationTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.CodeAnalysis.CSharp.Syntax;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public MemberDeclarationSyntax Transpile(
            JassGlobalDeclarationSyntax globalDeclaration)
        {
            return globalDeclaration switch
            {
                JassGlobalConstantDeclarationSyntax globalConstantDeclaration => Transpile(globalConstantDeclaration),
                JassGlobalVariableDeclarationSyntax globalVariableDeclaration => Transpile(globalVariableDeclaration),
            };
        }

        public MemberDeclarationSyntax Transpile(
            JassSyntaxTriviaList leadingTrivia,
            JassGlobalDeclarationSyntax globalDeclaration)
        {
            return globalDeclaration switch
            {
                JassGlobalConstantDeclarationSyntax globalConstantDeclaration => Transpile(leadingTrivia, globalConstantDeclaration),
                JassGlobalVariableDeclarationSyntax globalVariableDeclaration => Transpile(leadingTrivia, globalVariableDeclaration),
            };
        }

        public MemberDeclarationSyntax Transpile(
            JassGlobalDeclarationSyntax globalDeclaration,
            JassSyntaxTriviaList trailingTrivia)
        {
            return globalDeclaration switch
            {
                JassGlobalConstantDeclarationSyntax globalConstantDeclaration => Transpile(globalConstantDeclaration, trailingTrivia),
                JassGlobalVariableDeclarationSyntax globalVariableDeclaration => Transpile(globalVariableDeclaration, trailingTrivia),
            };
        }

        public MemberDeclarationSyntax Transpile(
            JassSyntaxTriviaList leadingTrivia,
            JassGlobalDeclarationSyntax globalDeclaration,
            JassSyntaxTriviaList trailingTrivia)
        {
            return globalDeclaration switch
            {
                JassGlobalConstantDeclarationSyntax globalConstantDeclaration => Transpile(leadingTrivia, globalConstantDeclaration, trailingTrivia),
                JassGlobalVariableDeclarationSyntax globalVariableDeclaration => Transpile(leadingTrivia, globalVariableDeclaration, trailingTrivia),
            };
        }
    }
}