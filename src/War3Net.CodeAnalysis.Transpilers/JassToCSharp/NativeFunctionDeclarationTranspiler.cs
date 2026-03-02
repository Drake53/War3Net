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
        public MemberDeclarationSyntax Transpile(JassNativeFunctionDeclarationSyntax nativeFunctionDeclaration)
        {
            var firstToken = nativeFunctionDeclaration.ConstantToken ?? nativeFunctionDeclaration.NativeToken;
            var externToken = nativeFunctionDeclaration.ConstantToken is null
                ? TokenWithSpace(SyntaxKind.ExternKeyword)
                : Token(SyntaxKind.ExternKeyword, nativeFunctionDeclaration.ConstantToken.TrailingTrivia);

            var functionNameToken = Transpile(nativeFunctionDeclaration.IdentifierName.Token);
            var discardTakesTokenLeadingTrivia = false;
            if (IsSingleSpace(nativeFunctionDeclaration.IdentifierName.Token.TrailingTrivia, nativeFunctionDeclaration.ParameterList.GetTakesToken().LeadingTrivia))
            {
                functionNameToken = functionNameToken.WithoutTrailingTrivia();
                discardTakesTokenLeadingTrivia = true;
            }

            var parameterList = Transpile(
                nativeFunctionDeclaration.ParameterList,
                nativeFunctionDeclaration.ReturnClause,
                discardTakesTokenLeadingTrivia);

            return SyntaxFactory.MethodDeclaration(
                default,
                new SyntaxTokenList(
                    TokenWithSpace(firstToken.LeadingTrivia, SyntaxKind.PublicKeyword),
                    TokenWithSpace(SyntaxKind.StaticKeyword),
                    externToken),
                Transpile(
                    nativeFunctionDeclaration.ConstantToken is null ? JassSyntaxTriviaList.Empty : nativeFunctionDeclaration.NativeToken.LeadingTrivia,
                    nativeFunctionDeclaration.ReturnClause.ReturnType,
                    nativeFunctionDeclaration.NativeToken.TrailingTrivia),
                null,
                functionNameToken,
                null,
                parameterList.WithoutTrailingTrivia(),
                default,
                null,
                null,
                SyntaxFactory.Token(
                    MergeTrivia(parameterList.GetTrailingTrivia(), Transpile(nativeFunctionDeclaration.ReturnClause.ReturnType.GetLeadingTrivia())),
                    SyntaxKind.SemicolonToken,
                    Transpile(nativeFunctionDeclaration.ReturnClause.ReturnType.GetTrailingTrivia())));
        }
    }
}