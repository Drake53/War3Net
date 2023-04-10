// ------------------------------------------------------------------------------
// <copyright file="SyntaxTriviaListRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="triviaList">The <see cref="JassSyntaxTriviaList"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassSyntaxTriviaList"/>, or the input <paramref name="triviaList"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="triviaList"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteLeadingTrivia(JassSyntaxTriviaList triviaList, out JassSyntaxTriviaList result)
        {
            result = triviaList;
            return false;
        }

        /// <param name="triviaList">The <see cref="JassSyntaxTriviaList"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassSyntaxTriviaList"/>, or the input <paramref name="triviaList"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="triviaList"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteTrailingTrivia(JassSyntaxTriviaList triviaList, out JassSyntaxTriviaList result)
        {
            result = triviaList;
            return false;
        }
    }
}