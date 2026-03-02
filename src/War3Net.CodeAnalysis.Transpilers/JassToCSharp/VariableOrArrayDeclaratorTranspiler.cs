using Microsoft.CodeAnalysis.CSharp.Syntax;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public VariableDeclarationSyntax Transpile(
            JassVariableOrArrayDeclaratorSyntax declarator,
            bool isGlobalDeclaration)
        {
            return declarator switch
            {
                JassArrayDeclaratorSyntax arrayDeclarator => Transpile(arrayDeclarator, isGlobalDeclaration),
                JassVariableDeclaratorSyntax variableDeclarator => Transpile(variableDeclarator, isGlobalDeclaration),
            };
        }

        public VariableDeclarationSyntax Transpile(
            JassSyntaxTriviaList leadingTrivia,
            JassVariableOrArrayDeclaratorSyntax declarator,
            bool isGlobalDeclaration)
        {
            return declarator switch
            {
                JassArrayDeclaratorSyntax arrayDeclarator => Transpile(leadingTrivia, arrayDeclarator, isGlobalDeclaration),
                JassVariableDeclaratorSyntax variableDeclarator => Transpile(leadingTrivia, variableDeclarator, isGlobalDeclaration),
            };
        }

        public VariableDeclarationSyntax Transpile(
            JassVariableOrArrayDeclaratorSyntax declarator,
            JassSyntaxTriviaList trailingTrivia,
            bool isGlobalDeclaration)
        {
            return declarator switch
            {
                JassArrayDeclaratorSyntax arrayDeclarator => Transpile(arrayDeclarator, trailingTrivia, isGlobalDeclaration),
                JassVariableDeclaratorSyntax variableDeclarator => Transpile(variableDeclarator, trailingTrivia, isGlobalDeclaration),
            };
        }

        public VariableDeclarationSyntax Transpile(
            JassSyntaxTriviaList leadingTrivia,
            JassVariableOrArrayDeclaratorSyntax declarator,
            JassSyntaxTriviaList trailingTrivia,
            bool isGlobalDeclaration)
        {
            return declarator switch
            {
                JassArrayDeclaratorSyntax arrayDeclarator => Transpile(leadingTrivia, arrayDeclarator, trailingTrivia, isGlobalDeclaration),
                JassVariableDeclaratorSyntax variableDeclarator => Transpile(leadingTrivia, variableDeclarator, trailingTrivia, isGlobalDeclaration),
            };
        }
    }
}