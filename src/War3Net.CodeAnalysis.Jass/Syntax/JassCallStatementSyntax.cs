// ------------------------------------------------------------------------------
// <copyright file="JassCallStatementSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassCallStatementSyntax : JassStatementSyntax
    {
        internal JassCallStatementSyntax(
            JassSyntaxToken callToken,
            JassIdentifierNameSyntax identifierName,
            JassArgumentListSyntax argumentList)
        {
            CallToken = callToken;
            IdentifierName = identifierName;
            ArgumentList = argumentList;
        }

        public JassSyntaxToken CallToken { get; }

        public JassIdentifierNameSyntax IdentifierName { get; }

        public JassArgumentListSyntax ArgumentList { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassCallStatementSyntax callStatement
                && IdentifierName.IsEquivalentTo(callStatement.IdentifierName)
                && ArgumentList.IsEquivalentTo(callStatement.ArgumentList);
        }

        public override void WriteTo(TextWriter writer)
        {
            CallToken.WriteTo(writer);
            IdentifierName.WriteTo(writer);
            ArgumentList.WriteTo(writer);
        }

        public override string ToString() => $"{CallToken} {IdentifierName}{ArgumentList}";

        public override JassSyntaxToken GetFirstToken() => CallToken;

        public override JassSyntaxToken GetLastToken() => ArgumentList.GetLastToken();

        protected internal override JassCallStatementSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassCallStatementSyntax(
                newToken,
                IdentifierName,
                ArgumentList);
        }

        protected internal override JassCallStatementSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassCallStatementSyntax(
                CallToken,
                IdentifierName,
                ArgumentList.ReplaceLastToken(newToken));
        }
    }
}