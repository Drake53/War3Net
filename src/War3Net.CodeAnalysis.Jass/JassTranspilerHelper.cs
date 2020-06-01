// ------------------------------------------------------------------------------
// <copyright file="JassTranspilerHelper.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static class JassTranspilerHelper
    {
        public static CompilationUnitSyntax GetCompilationUnit(SyntaxList<UsingDirectiveSyntax> usingDirectives, params MemberDeclarationSyntax[] namespaceOrClassDeclarations)
        {
            return SyntaxFactory.CompilationUnit(
                default,
                usingDirectives,
                default,
                new SyntaxList<MemberDeclarationSyntax>(namespaceOrClassDeclarations));
        }

        public static MemberDeclarationSyntax GetNamespaceDeclaration(string identifier, params ClassDeclarationSyntax[] classDeclarations)
        {
            return identifier is null
                ? (MemberDeclarationSyntax)(classDeclarations.Length == 1 ? classDeclarations[0] : throw new System.Exception())
                : SyntaxFactory.NamespaceDeclaration(
                    SyntaxFactory.IdentifierName(SyntaxFactory.Identifier(identifier)),
                    default,
                    new SyntaxList<UsingDirectiveSyntax>(classDeclarations.Select(@class => GetUsingStaticClassDirective(identifier, @class.Identifier.ValueText))),
                    new SyntaxList<MemberDeclarationSyntax>(classDeclarations));
        }

        public static ClassDeclarationSyntax[] GetClassDeclaration(string identifier, IEnumerable<MemberDeclarationSyntax> members, bool applyNativeMemberAttributes)
        {
            if (applyNativeMemberAttributes)
            {
                return new[]
                {
                    SyntaxFactory.ClassDeclaration(
                    default,
                    new SyntaxTokenList(
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                        SyntaxFactory.Token(SyntaxKind.StaticKeyword)),
                    SyntaxFactory.Identifier(identifier),
                    null,
                    null,
                    default,
                    // .AddAttributeByName(nameof(CSharpLua.NativeLuaMemberContainerAttribute)),
                    new SyntaxList<MemberDeclarationSyntax>(
                        members.Select(declr => declr is ClassDeclarationSyntax
                        ? declr.WithLeadingTrivia(
                            SyntaxFactory.TriviaList(
                                SyntaxFactory.Trivia(
                                    GetSingleLineDocumentationCommentTrivia(
                                        SyntaxFactory.XmlText("@CSharpLua.Ignore")))))
                        : declr.WithLeadingTrivia(
                            SyntaxFactory.TriviaList(
                                SyntaxFactory.Trivia(
                                    GetSingleLineDocumentationCommentTrivia(
                                        SyntaxFactory.XmlText($"@CSharpLua.Template = \"{GetDeclarationTemplateText(declr)}\""))))))))
                    .WithLeadingTrivia(
                        SyntaxFactory.TriviaList(
                            SyntaxFactory.Trivia(
                                GetSingleLineDocumentationCommentTrivia(
                                    SyntaxFactory.XmlText("@CSharpLua.Ignore"))))),
                };
            }
            else
            {
                // If native attributes not applied, assume the C# code will get transpiled to lua.
                // In lua, there is a limitation of around 200 local declarations in a scope.
                // In order to not reach this limit, the members will get spread out over multiple classes if needed.
                const int DeclarationLimit = 150;

                var classList = new List<ClassDeclarationSyntax>();
                var memberList = members.ToArray();
                var enumerator = members.GetEnumerator();
                for (var i = 0; i * DeclarationLimit <= memberList.Length; i++)
                {
                    var remaining = memberList.Length - (i * DeclarationLimit);
                    var size = remaining > DeclarationLimit ? DeclarationLimit : remaining;
                    var membersSubset = new MemberDeclarationSyntax[size];
                    for (var j = 0; j < size; j++)
                    {
                        enumerator.MoveNext();
                        membersSubset[j] = enumerator.Current;
                    }

                    classList.Add(
                        SyntaxFactory.ClassDeclaration(
                        default,
                        new SyntaxTokenList(
                            SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                            SyntaxFactory.Token(SyntaxKind.StaticKeyword)),
                        SyntaxFactory.Identifier($"{identifier}{i}"),
                        null,
                        null,
                        default,
                        new SyntaxList<MemberDeclarationSyntax>(membersSubset)));
                }

                return classList.ToArray();
            }
        }

        private static DocumentationCommentTriviaSyntax GetSingleLineDocumentationCommentTrivia(XmlTextSyntax xmlTextSyntax)
        {
            return SyntaxFactory.DocumentationCommentTrivia(
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
                    xmlTextSyntax,
                    SyntaxFactory.XmlText(
                        SyntaxFactory.XmlTextNewLine("\r\n", false)),
                }),
                SyntaxFactory.Token(SyntaxKind.EndOfDocumentationCommentToken));
        }

        private static string GetDeclarationTemplateText(MemberDeclarationSyntax memberDeclaration)
        {
            if (memberDeclaration is MethodDeclarationSyntax methodDeclaration)
            {
                var @params = methodDeclaration.ParameterList.Parameters.Count == 0
                    ? string.Empty
                    : methodDeclaration.ParameterList.Parameters.Select((_, index) => $"{{{index}}}").Aggregate((accum, next) => $"{accum}, {next}");
                return $"{methodDeclaration.Identifier.ValueText}({@params})";
            }
            else if (memberDeclaration is FieldDeclarationSyntax fieldDeclaration)
            {
                return fieldDeclaration.Declaration.Variables.Single().Identifier.ValueText;
            }
            else
            {
                throw new NotSupportedException("The attribute @CSharpLua.Template only supports methods and fields.");
            }
        }

        private static T AddAttributeByName<T>(this T memberDeclaration, string attributeName)
            where T : MemberDeclarationSyntax
        {
            return (T)memberDeclaration.AddAttributeLists(
                SyntaxFactory.AttributeList(
                    default(SeparatedSyntaxList<AttributeSyntax>).Add(
                        SyntaxFactory.Attribute(
                            SyntaxFactory.ParseName(attributeName)))));
        }

        private static UsingDirectiveSyntax GetUsingStaticClassDirective(string namespaceName, string className)
        {
            return SyntaxFactory.UsingDirective(SyntaxFactory.Token(SyntaxKind.StaticKeyword), null, SyntaxFactory.ParseName($"{namespaceName}.{className}"));
        }
    }
}