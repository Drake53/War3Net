// ------------------------------------------------------------------------------
// <copyright file="JassCompilationUnitBuilder.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Immutable;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass.Builders
{
    public class JassCompilationUnitBuilder : JassSyntaxBuilder
    {
        private readonly ImmutableArray<JassTopLevelDeclarationSyntax>.Builder _declarationsBuilder;

        private JassGlobalsDeclarationBuilder? _globalsDeclarationBuilder;
        private JassFunctionDeclarationBuilder? _functionDeclarationBuilder;

        public JassCompilationUnitBuilder()
        {
            _declarationsBuilder = ImmutableArray.CreateBuilder<JassTopLevelDeclarationSyntax>();
        }

        public JassCompilationUnitSyntax ToCompilationUnit(JassSyntaxToken endOfFileToken)
        {
            if (_globalsDeclarationBuilder is not null ||
                _functionDeclarationBuilder is not null)
            {
                throw new InvalidOperationException();
            }

            JassSyntaxFactory.ThrowHelper.ThrowIfInvalidToken(endOfFileToken, JassSyntaxKind.EndOfFileToken);

            return new JassCompilationUnitSyntax(
                _declarationsBuilder.ToImmutable(),
                endOfFileToken.PrependLeadingTrivia(BuildTriviaList()));
        }

        public void AddDeclaration(JassTopLevelDeclarationSyntax declaration)
        {
            if (_globalsDeclarationBuilder is not null ||
                _functionDeclarationBuilder is not null)
            {
                throw new InvalidOperationException();
            }

            _declarationsBuilder.Add(declaration.PrependLeadingTrivia(BuildTriviaList()));
        }

        public JassGlobalsDeclarationBuilder BeginGlobalsDeclaration(JassSyntaxToken globalsToken)
        {
            if (_functionDeclarationBuilder is not null)
            {
                throw new InvalidOperationException();
            }

            return _globalsDeclarationBuilder = new JassGlobalsDeclarationBuilder(globalsToken.PrependLeadingTrivia(BuildTriviaList()));
        }

        public void EndGlobalsDeclaration(JassSyntaxToken endGlobalsToken)
        {
            if (_globalsDeclarationBuilder is null)
            {
                throw new InvalidOperationException();
            }

            _declarationsBuilder.Add(_globalsDeclarationBuilder.ToGlobalsDeclaration(endGlobalsToken));
            _globalsDeclarationBuilder = null;
        }

        public JassFunctionDeclarationBuilder BeginFunctionDeclaration(JassFunctionDeclaratorSyntax functionDeclarator)
        {
            if (_globalsDeclarationBuilder is not null)
            {
                throw new InvalidOperationException();
            }

            return _functionDeclarationBuilder = new JassFunctionDeclarationBuilder(functionDeclarator.PrependLeadingTrivia(BuildTriviaList()));
        }

        public void EndFunctionDeclaration(JassSyntaxToken endFunctionToken)
        {
            if (_functionDeclarationBuilder is null)
            {
                throw new InvalidOperationException();
            }

            _declarationsBuilder.Add(_functionDeclarationBuilder.ToFunctionDeclaration(endFunctionToken));
            _functionDeclarationBuilder = null;
        }
    }
}