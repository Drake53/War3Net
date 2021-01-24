// ------------------------------------------------------------------------------
// <copyright file="RealLiteralExpressionParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using War3Net.CodeAnalysis.Jass.Syntax;

using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, IExpressionSyntax> GetRealLiteralExpressionParser()
        {
#if true
            return Try(Token(char.IsDigit).AtLeastOnceString().Before(Symbol.FullStop))
                .Then(Token(char.IsDigit).ManyString(), (intPart, fracPart) => (IExpressionSyntax)new JassRealLiteralExpressionSyntax(intPart, fracPart))
                .Or(Symbol.FullStop.Then(Token(char.IsDigit).AtLeastOnceString())
                    .Select<IExpressionSyntax>(fracPart => new JassRealLiteralExpressionSyntax($"{JassSymbol.Zero}", fracPart)))
                .Labelled("real literal");
#else
            return Try(Token(char.IsDigit).AtLeastOnceString().Before(Symbol.FullStop))
                .Then(Token(char.IsDigit).ManyString(), (intPart, fracPart) => (IExpressionSyntax)new JassRealLiteralExpressionSyntax(float.Parse($"{intPart}{JassSymbol.FullStop}{fracPart}", CultureInfo.InvariantCulture)))
                .Or(Symbol.FullStop.Then(Token(char.IsDigit).AtLeastOnceString())
                    .Select<IExpressionSyntax>(fracPart => new JassRealLiteralExpressionSyntax(float.Parse($"{JassSymbol.Zero}{JassSymbol.FullStop}{fracPart}", CultureInfo.InvariantCulture))))
                .Labelled("real literal");
#endif
        }
    }
}