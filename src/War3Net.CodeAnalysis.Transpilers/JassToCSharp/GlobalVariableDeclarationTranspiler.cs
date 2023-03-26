// ------------------------------------------------------------------------------
// <copyright file="GlobalVariableDeclarationTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;
using War3Net.CodeAnalysis.Transpilers.Extensions;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public MemberDeclarationSyntax Transpile(JassGlobalVariableDeclarationSyntax globalDeclaration)
        {
            var declaration = SyntaxFactory.FieldDeclaration(
                default,
                new SyntaxTokenList(
                    SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                    SyntaxFactory.Token(SyntaxKind.StaticKeyword)),
                Transpile(globalDeclaration.Declarator));

            if (ApplyCSharpLuaTemplateAttribute)
            {
                var jassToLuaTranspiler = JassToLuaTranspiler ?? new JassToLuaTranspiler();
                declaration = declaration.WithCSharpLuaTemplateAttribute(jassToLuaTranspiler.Transpile(globalDeclaration.Declarator.GetIdentifierName()));
            }

            return declaration;
        }
    }
}