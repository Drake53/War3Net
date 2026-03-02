using System.Linq;
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
        public MemberDeclarationSyntax Transpile(JassFunctionDeclarationSyntax functionDeclaration)
        {
            var declarator = functionDeclaration.FunctionDeclarator;

            var firstToken = declarator.ConstantToken ?? declarator.FunctionToken;
            var staticToken = declarator.ConstantToken is null
                ? TokenWithSpace(SyntaxKind.StaticKeyword)
                : Token(SyntaxKind.StaticKeyword, declarator.ConstantToken.TrailingTrivia);

            var functionNameToken = Transpile(declarator.IdentifierName.Token);
            var discardTakesTokenLeadingTrivia = false;
            if (IsSingleSpace(declarator.IdentifierName.Token.TrailingTrivia, declarator.ParameterList.GetTakesToken().LeadingTrivia))
            {
                functionNameToken = functionNameToken.WithoutTrailingTrivia();
                discardTakesTokenLeadingTrivia = true;
            }

            return SyntaxFactory.MethodDeclaration(
                default,
                new SyntaxTokenList(
                    TokenWithSpace(firstToken.LeadingTrivia, SyntaxKind.PublicKeyword),
                    staticToken),
                Transpile(
                    declarator.ConstantToken is null ? JassSyntaxTriviaList.Empty : declarator.FunctionToken.LeadingTrivia,
                    declarator.ReturnClause.ReturnType,
                    declarator.FunctionToken.TrailingTrivia),
                null,
                functionNameToken,
                null,
                Transpile(declarator.ParameterList, declarator.ReturnClause, discardTakesTokenLeadingTrivia),
                default,
                SyntaxFactory.Block(
                    Transpile(SyntaxKind.OpenBraceToken, declarator.ReturnClause.ReturnType.GetToken()),
                    SyntaxFactory.List(functionDeclaration.Statements.Select(Transpile)),
                    Transpile(SyntaxKind.CloseBraceToken, functionDeclaration.EndFunctionToken)),
                null);
        }
    }
}