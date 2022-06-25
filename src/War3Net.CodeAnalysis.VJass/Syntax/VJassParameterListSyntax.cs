// ------------------------------------------------------------------------------
// <copyright file="VJassParameterListSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

using War3Net.CodeAnalysis.VJass.Extensions;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassParameterListSyntax : VJassParameterListOrEmptyParameterListSyntax
    {
        internal VJassParameterListSyntax(
            VJassSyntaxToken takesToken,
            SeparatedSyntaxList<VJassParameterSyntax, VJassSyntaxToken> parameterList)
        {
            TakesToken = takesToken;
            ParameterList = parameterList;
        }

        public VJassSyntaxToken TakesToken { get; }

        public SeparatedSyntaxList<VJassParameterSyntax, VJassSyntaxToken> ParameterList { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassParameterListSyntax parameterList
                && ParameterList.IsEquivalentTo(parameterList.ParameterList);
        }

        public override void WriteTo(TextWriter writer)
        {
            TakesToken.WriteTo(writer);
            ParameterList.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            TakesToken.ProcessTo(writer, context);
            ParameterList.ProcessTo(writer, context);
        }

        public override string ToString() => $"{TakesToken} {ParameterList}";

        public override VJassSyntaxToken GetFirstToken() => TakesToken;

        public override VJassSyntaxToken GetLastToken() => ParameterList.Items[^1].GetLastToken();

        protected internal override VJassParameterListSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassParameterListSyntax(
                newToken,
                ParameterList);
        }

        protected internal override VJassParameterListSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassParameterListSyntax(
                TakesToken,
                ParameterList.ReplaceLastItem(ParameterList.Items[^1].ReplaceLastToken(newToken)));
        }
    }
}