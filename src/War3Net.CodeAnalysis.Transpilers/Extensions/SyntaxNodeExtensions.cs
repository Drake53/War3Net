// ------------------------------------------------------------------------------
// <copyright file="SyntaxNodeExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Transpilers.Extensions
{
    public static class SyntaxNodeExtensions
    {
        public static TSyntax WithCSharpLuaTemplateAttribute<TSyntax>(this TSyntax node, string template)
            where TSyntax : SyntaxNode
        {
            return node.WithLeadingTrivia(
                SyntaxFactory.Trivia(
                    SyntaxFactory.DocumentationCommentTrivia(
                        SyntaxKind.SingleLineDocumentationCommentTrivia,
                        SyntaxFactory.List(new XmlNodeSyntax[]
                        {
                            SyntaxFactory.XmlText(
                                SyntaxFactory.XmlTextLiteral(
                                    SyntaxFactory.TriviaList(
                                        SyntaxFactory.DocumentationCommentExterior("///")),
                                    " ",
                                    " ",
                                    default)),
                            SyntaxFactory.XmlText(
                                $"@CSharpLua.Template = \"{template}\""),
                            SyntaxFactory.XmlText(
                                SyntaxFactory.XmlTextNewLine("\r\n", false)),
                        }))));
        }
    }
}