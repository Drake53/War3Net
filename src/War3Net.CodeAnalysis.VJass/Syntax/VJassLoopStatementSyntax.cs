using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using War3Net.CodeAnalysis.VJass.Extensions;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassLoopStatementSyntax : VJassStatementSyntax
    {
        internal VJassLoopStatementSyntax(
            VJassSyntaxToken loopToken,
            ImmutableArray<VJassStatementSyntax> statements,
            VJassSyntaxToken endLoopToken)
        {
            LoopToken = loopToken;
            Statements = statements;
            EndLoopToken = endLoopToken;
        }

        public VJassSyntaxToken LoopToken { get; }

        public ImmutableArray<VJassStatementSyntax> Statements { get; }

        public VJassSyntaxToken EndLoopToken { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassLoopStatementSyntax loopStatement
                && Statements.IsEquivalentTo(loopStatement.Statements);
        }

        public override void WriteTo(TextWriter writer)
        {
            LoopToken.WriteTo(writer);
            Statements.WriteTo(writer);
            EndLoopToken.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            LoopToken.ProcessTo(writer, context);
            Statements.ProcessTo(writer, context);
            EndLoopToken.ProcessTo(writer, context);
        }

        public override string ToString() => $"{LoopToken} [...]";

        public override VJassSyntaxToken GetFirstToken() => LoopToken;

        public override VJassSyntaxToken GetLastToken() => EndLoopToken;

        protected internal override VJassLoopStatementSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassLoopStatementSyntax(
                newToken,
                Statements,
                EndLoopToken);
        }

        protected internal override VJassLoopStatementSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassLoopStatementSyntax(
                LoopToken,
                Statements,
                newToken);
        }
    }
}