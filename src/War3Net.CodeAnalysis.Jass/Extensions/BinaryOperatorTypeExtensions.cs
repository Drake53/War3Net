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
                BinaryOperatorType.Add => "+",
                BinaryOperatorType.Subtract => "-",
                BinaryOperatorType.Multiplication => "*",
                BinaryOperatorType.Division => "/",
                BinaryOperatorType.GreaterThan => ">",
                BinaryOperatorType.LessThan => "<",
                BinaryOperatorType.Equals => "==",
                BinaryOperatorType.NotEquals => "!=",
                BinaryOperatorType.GreaterOrEqual => ">=",
                BinaryOperatorType.LessOrEqual => "<=",
                BinaryOperatorType.And => "and",
                BinaryOperatorType.Or => "or",
            };
        }
    }
}