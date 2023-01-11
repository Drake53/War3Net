// ------------------------------------------------------------------------------
// <copyright file="JassDebugStatementSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassDebugStatementSyntax : JassStatementSyntax
    {
        internal JassDebugStatementSyntax(
            JassSyntaxToken debugToken,
            JassStatementSyntax statement)
        {
            DebugToken = debugToken;
            Statement = statement;
        }

        public JassSyntaxToken DebugToken { get; }

        public JassStatementSyntax Statement { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassDebugStatementSyntax debugStatement
                && Statement.IsEquivalentTo(debugStatement.Statement);
        }

        public override void WriteTo(TextWriter writer)
        {
            DebugToken.WriteTo(writer);
            Statement.WriteTo(writer);
        }

        public override string ToString() => $"{DebugToken} {Statement}";

        public override JassSyntaxToken GetFirstToken() => DebugToken;

        public override JassSyntaxToken GetLastToken() => Statement.GetLastToken();

        protected internal override JassDebugStatementSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassDebugStatementSyntax(
                newToken,
                Statement);
        }

        protected internal override JassDebugStatementSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassDebugStatementSyntax(
                DebugToken,
                Statement.ReplaceLastToken(newToken));
        }
    }
}