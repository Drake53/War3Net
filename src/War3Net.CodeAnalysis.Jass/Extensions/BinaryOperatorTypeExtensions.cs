// ------------------------------------------------------------------------------
// <copyright file="BinaryOperatorTypeExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.ComponentModel;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass.Extensions
{
    public static class BinaryOperatorTypeExtensions
    {
        public static string GetSymbol(this BinaryOperatorType binaryOperator)
        {
            return binaryOperator switch
            {
                BinaryOperatorType.Add => JassSymbol.Plus,
                BinaryOperatorType.Subtract => JassSymbol.Minus,
                BinaryOperatorType.Multiplication => JassSymbol.Asterisk,
                BinaryOperatorType.Division => JassSymbol.Slash,
                BinaryOperatorType.GreaterThan => JassSymbol.GreaterThan,
                BinaryOperatorType.LessThan => JassSymbol.LessThan,
                BinaryOperatorType.Equals => JassSymbol.EqualsEquals,
                BinaryOperatorType.NotEquals => JassSymbol.ExclamationEquals,
                BinaryOperatorType.GreaterOrEqual => JassSymbol.GreaterThanEquals,
                BinaryOperatorType.LessOrEqual => JassSymbol.LessThanEquals,
                BinaryOperatorType.And => JassKeyword.And,
                BinaryOperatorType.Or => JassKeyword.Or,

                _ => throw new InvalidEnumArgumentException(nameof(binaryOperator), (int)binaryOperator, typeof(BinaryOperatorType)),
            };
        }
    }
}