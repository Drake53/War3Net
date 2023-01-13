// ------------------------------------------------------------------------------
// <copyright file="UnaryOperatorTypeExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.ComponentModel;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass.Extensions
{
    public static class UnaryOperatorTypeExtensions
    {
        public static string GetSymbol(this UnaryOperatorType unaryOperator)
        {
            return unaryOperator switch
            {
                UnaryOperatorType.Plus => JassSymbol.Plus,
                UnaryOperatorType.Minus => JassSymbol.Minus,
                UnaryOperatorType.Not => JassKeyword.Not,

                _ => throw new InvalidEnumArgumentException(nameof(unaryOperator), (int)unaryOperator, typeof(UnaryOperatorType)),
            };
        }
    }
}