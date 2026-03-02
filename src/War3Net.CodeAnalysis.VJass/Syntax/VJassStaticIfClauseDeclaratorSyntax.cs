using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassStaticIfClauseDeclaratorSyntax : VJassSyntaxNode
    {
        internal VJassStaticIfClauseDeclaratorSyntax(
            VJassSyntaxToken staticToken,
            VJassSyntaxToken ifToken,
            VJassExpressionSyntax condition,
            VJassSyntaxToken thenToken)
        {
            StaticToken = staticToken;
            IfToken = ifToken;
            Condition = condition;
            ThenToken = thenToken;
        }

        public VJassSyntaxToken StaticToken { get; }

        public VJassSyntaxToken IfToken { get; }

        public VJassExpressionSyntax Condition { get; }

        public VJassSyntaxToken ThenToken { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassStaticIfClauseDeclaratorSyntax staticIfClauseDeclarator
                && Condition.IsEquivalentTo(staticIfClauseDeclarator.Condition);
        }

        public override void WriteTo(TextWriter writer)
        {
            StaticToken.WriteTo(writer);
            IfToken.WriteTo(writer);
            Condition.WriteTo(writer);
            ThenToken.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            StaticToken.ProcessTo(writer, context);
            IfToken.ProcessTo(writer, context);
            Condition.ProcessTo(writer, context);
            ThenToken.ProcessTo(writer, context);
        }

        public override string ToString() => $"{StaticToken} {IfToken} {Condition} {ThenToken}";

        public override VJassSyntaxToken GetFirstToken() => StaticToken;

        public override VJassSyntaxToken GetLastToken() => ThenToken;

        protected internal override VJassStaticIfClauseDeclaratorSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassStaticIfClauseDeclaratorSyntax(
                newToken,
                IfToken,
                Condition,
                ThenToken);
        }

        protected internal override VJassStaticIfClauseDeclaratorSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassStaticIfClauseDeclaratorSyntax(
                StaticToken,
                IfToken,
                Condition,
                newToken);
        }
    }
}