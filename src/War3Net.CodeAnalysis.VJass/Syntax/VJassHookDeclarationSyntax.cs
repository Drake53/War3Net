// ------------------------------------------------------------------------------
// <copyright file="VJassHookDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassHookDeclarationSyntax : VJassScopedDeclarationSyntax
    {
        internal VJassHookDeclarationSyntax(
            VJassSyntaxToken hookToken,
            VJassIdentifierNameSyntax hookedFunction,
            VJassExpressionSyntax expression)
        {
            HookToken = hookToken;
            HookedFunction = hookedFunction;
            Expression = expression;
        }

        public VJassSyntaxToken HookToken { get; }

        public VJassIdentifierNameSyntax HookedFunction { get; }

        public VJassExpressionSyntax Expression { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassHookDeclarationSyntax hookDeclaration
                && HookedFunction.IsEquivalentTo(hookDeclaration.HookedFunction)
                && Expression.IsEquivalentTo(hookDeclaration.Expression);
        }

        public override void WriteTo(TextWriter writer)
        {
            HookToken.WriteTo(writer);
            HookedFunction.WriteTo(writer);
            Expression.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            HookToken.ProcessTo(writer, context);
            HookedFunction.ProcessTo(writer, context);
            Expression.ProcessTo(writer, context);
        }

        public override string ToString() => $"{HookToken} {HookedFunction} {Expression}";

        public override VJassSyntaxToken GetFirstToken() => HookToken;

        public override VJassSyntaxToken GetLastToken() => Expression.GetLastToken();

        protected internal override VJassHookDeclarationSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassHookDeclarationSyntax(
                newToken,
                HookedFunction,
                Expression);
        }

        protected internal override VJassHookDeclarationSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassHookDeclarationSyntax(
                HookToken,
                HookedFunction,
                Expression.ReplaceLastToken(newToken));
        }
    }
}