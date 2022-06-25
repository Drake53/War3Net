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
            VJassIdentifierNameSyntax identifierName)
        {
            HookToken = hookToken;
            HookedFunction = hookedFunction;
            IdentifierName = identifierName;
        }

        public VJassSyntaxToken HookToken { get; }

        public VJassIdentifierNameSyntax HookedFunction { get; }

        public VJassIdentifierNameSyntax IdentifierName { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassHookDeclarationSyntax hookDeclaration
                && HookedFunction.IsEquivalentTo(hookDeclaration.HookedFunction)
                && IdentifierName.IsEquivalentTo(hookDeclaration.IdentifierName);
        }

        public override void WriteTo(TextWriter writer)
        {
            HookToken.WriteTo(writer);
            HookedFunction.WriteTo(writer);
            IdentifierName.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            HookToken.ProcessTo(writer, context);
            HookedFunction.ProcessTo(writer, context);
            IdentifierName.ProcessTo(writer, context);
        }

        public override string ToString() => $"{HookToken} {HookedFunction} {IdentifierName}";

        public override VJassSyntaxToken GetFirstToken() => HookToken;

        public override VJassSyntaxToken GetLastToken() => IdentifierName.GetLastToken();

        protected internal override VJassHookDeclarationSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassHookDeclarationSyntax(
                newToken,
                HookedFunction,
                IdentifierName);
        }

        protected internal override VJassHookDeclarationSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassHookDeclarationSyntax(
                HookToken,
                HookedFunction,
                IdentifierName.ReplaceLastToken(newToken));
        }
    }
}