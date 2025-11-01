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
            Render(setStatement.SetToken);
            WriteSpace();
            Render(setStatement.IdentifierName);

            if (setStatement.ElementAccessClause is not null)
            {
                Render(setStatement.ElementAccessClause);
            }

            WriteSpace();
            Render(setStatement.Value);
        }
    }
}