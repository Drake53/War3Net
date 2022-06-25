// ------------------------------------------------------------------------------
// <copyright file="VJassExitStatementSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassExitStatementSyntax : VJassStatementSyntax
    {
        internal VJassExitStatementSyntax(
            VJassSyntaxToken exitWhenToken,
            VJassExpressionSyntax condition)
        {
            ExitWhenToken = exitWhenToken;
            Condition = condition;
        }

        public VJassSyntaxToken ExitWhenToken { get; }

        public VJassExpressionSyntax Condition { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassExitStatementSyntax exitStatement
                && Condition.IsEquivalentTo(exitStatement.Condition);
        }

        public override void WriteTo(TextWriter writer)
        {
            ExitWhenToken.WriteTo(writer);
            Condition.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            ExitWhenToken.ProcessTo(writer, context);
            Condition.ProcessTo(writer, context);
        }

        public override string ToString() => $"{ExitWhenToken} {Condition}";

        public override VJassSyntaxToken GetFirstToken() => ExitWhenToken;

        public override VJassSyntaxToken GetLastToken() => Condition.GetLastToken();

        protected internal override VJassExitStatementSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassExitStatementSyntax(
                newToken,
                Condition);
        }

        protected internal override VJassExitStatementSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassExitStatementSyntax(
                ExitWhenToken,
                Condition.ReplaceLastToken(newToken));
        }
    }
}