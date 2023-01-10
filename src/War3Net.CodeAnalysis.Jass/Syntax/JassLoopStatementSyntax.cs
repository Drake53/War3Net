// ------------------------------------------------------------------------------
// <copyright file="JassLoopStatementSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassLoopStatementSyntax : JassStatementSyntax
    {
        internal JassLoopStatementSyntax(
            JassSyntaxToken loopToken,
            ImmutableArray<JassStatementSyntax> statements,
            JassSyntaxToken endLoopToken)
        {
            LoopToken = loopToken;
            Statements = statements;
            EndLoopToken = endLoopToken;
        }

        public JassSyntaxToken LoopToken { get; }

        public ImmutableArray<JassStatementSyntax> Statements { get; }

        public JassSyntaxToken EndLoopToken { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassLoopStatementSyntax loopStatement
                && Statements.IsEquivalentTo(loopStatement.Statements);
        }

        public override void WriteTo(TextWriter writer)
        {
            LoopToken.WriteTo(writer);
            Statements.WriteTo(writer);
            EndLoopToken.WriteTo(writer);
        }

        public override string ToString() => $"{LoopToken} [...]";

        public override JassSyntaxToken GetFirstToken() => LoopToken;

        public override JassSyntaxToken GetLastToken() => EndLoopToken;

        protected internal override JassLoopStatementSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassLoopStatementSyntax(
                newToken,
                Statements,
                EndLoopToken);
        }

        protected internal override JassLoopStatementSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassLoopStatementSyntax(
                LoopToken,
                Statements,
                newToken);
        }
    }
}