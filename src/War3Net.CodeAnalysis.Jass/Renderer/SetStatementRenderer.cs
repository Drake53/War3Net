// ------------------------------------------------------------------------------
// <copyright file="SetStatementRenderer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassSetStatementSyntax setStatement)
        {
            Write($"{JassKeyword.Set} ");
            Render(setStatement.IdentifierName);

            if (setStatement.Indexer is not null)
            {
                Write(JassSymbol.LeftSquareBracket);
                Render(setStatement.Indexer);
                Write(JassSymbol.RightSquareBracket);
            }

            Write(' ');
            Render(setStatement.Value);
        }
    }
}