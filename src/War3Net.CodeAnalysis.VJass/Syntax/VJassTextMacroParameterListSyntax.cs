// ------------------------------------------------------------------------------
// <copyright file="VJassTextMacroParameterListSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using War3Net.CodeAnalysis.VJass.Extensions;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassTextMacroParameterListSyntax : VJassSyntaxNode
    {
        internal VJassTextMacroParameterListSyntax(
            VJassSyntaxToken takesToken,
            SeparatedSyntaxList<VJassIdentifierNameSyntax, VJassSyntaxToken> parameterList)
        {
            TakesToken = takesToken;
            ParameterList = parameterList;
        }

        public VJassSyntaxToken TakesToken { get; }

        public SeparatedSyntaxList<VJassIdentifierNameSyntax, VJassSyntaxToken> ParameterList { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassTextMacroParameterListSyntax textMacroParameterList
                && ParameterList.IsEquivalentTo(textMacroParameterList.ParameterList);
        }

        public override void WriteTo(TextWriter writer)
        {
            TakesToken.WriteTo(writer);
            ParameterList.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            throw new NotSupportedException();
        }

        public override string ToString() => $"{TakesToken} {ParameterList}";

        public override VJassSyntaxToken GetFirstToken() => TakesToken;

        public override VJassSyntaxToken GetLastToken() => ParameterList.Items[^1].GetLastToken();

        protected internal override VJassTextMacroParameterListSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassTextMacroParameterListSyntax(
                newToken,
                ParameterList);
        }

        protected internal override VJassTextMacroParameterListSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassTextMacroParameterListSyntax(
                TakesToken,
                ParameterList.ReplaceLastItem(ParameterList.Items[^1].ReplaceLastToken(newToken)));
        }
    }
}