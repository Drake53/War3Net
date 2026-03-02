using System.Collections.Immutable;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(ImmutableArray<JassStatementSyntax> statementList)
        {
            foreach (var statement in statementList)
            {
                Render(statement);
            }
        }
    }
}