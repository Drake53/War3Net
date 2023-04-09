// ------------------------------------------------------------------------------
// <copyright file="SyntaxTriviaRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="trivia">The <see cref="JassSyntaxTriviaList"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassSyntaxTriviaList"/>, or the input <paramref name="trivia"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="trivia"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteLeadingTrivia(JassSyntaxTriviaList trivia, out JassSyntaxTriviaList result)
        {
            result = trivia;
            return false;
        }

        /// <param name="trivia">The <see cref="JassSyntaxTriviaList"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassSyntaxTriviaList"/>, or the input <paramref name="trivia"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="trivia"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteTrailingTrivia(JassSyntaxTriviaList trivia, out JassSyntaxTriviaList result)
        {
            result = trivia;
            return false;
        }
    }
}