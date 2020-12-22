// ------------------------------------------------------------------------------
// <copyright file="IfStatementRenderer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1649 // File name should match first type name

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass.Renderer
{
    public partial class JassRenderer
    {
        public void Render(IfStatementSyntax ifStatement)
        {
            Render(ifStatement.IfKeywordToken);
            WriteSpace();
            Render(ifStatement.ConditionExpressionNode);
            WriteSpace();
            Render(ifStatement.ThenKeywordToken);
            Render(ifStatement.LineDelimiterNode, true);
            Render(ifStatement.StatementListNode);
            Outdent();
            if (ifStatement.ElseClauseNode is not null)
            {
                Render(ifStatement.ElseClauseNode);
            }

            Render(ifStatement.EndifKeywordToken);
        }
    }
}