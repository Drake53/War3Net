// ------------------------------------------------------------------------------
// <copyright file="BinaryOperatorTypeExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass.Extensions
{
    public static class BinaryOperatorTypeExtensions
    {
        public static string GetString(this BinaryOperatorType binaryOperator)
        {
            return binaryOperator switch
            {
                BinaryOperatorType.Add => $"{JassSymbol.PlusSign}",
                BinaryOperatorType.Subtract => $"{JassSymbol.MinusSign}",
                BinaryOperatorType.Multiplication => $"{JassSymbol.Asterisk}",
                BinaryOperatorType.Division => $"{JassSymbol.Slash}",
                BinaryOperatorType.GreaterThan => $"{JassSymbol.GreaterThanSign}",
                BinaryOperatorType.LessThan => $"{JassSymbol.LessThanSign}",
                BinaryOperatorType.Equals => $"{JassSymbol.EqualsSign}{JassSymbol.EqualsSign}",
                BinaryOperatorType.NotEquals => $"{JassSymbol.ExclamationMark}{JassSymbol.EqualsSign}",
                BinaryOperatorType.GreaterOrEqual => $"{JassSymbol.GreaterThanSign}{JassSymbol.EqualsSign}",
                BinaryOperatorType.LessOrEqual => $"{JassSymbol.LessThanSign}{JassSymbol.EqualsSign}",
                BinaryOperatorType.And => JassKeyword.And,
                BinaryOperatorType.Or => JassKeyword.Or,
            };
        }
    }
}