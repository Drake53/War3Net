// ------------------------------------------------------------------------------
// <copyright file="JassExpression.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass
{
    public static class JassExpression
    {
        public static string FunctionRef(string functionName)
        {
            return $"{JassKeyword.Function} {functionName}";
        }

        public static string Invoke(string functionName)
        {
            return $"{functionName}()";
        }

        public static string Invoke(string functionName, params string[] arguments)
        {
            return $"{functionName}({string.Join(", ", arguments)})";
        }

        public static string InvokeCompact(string functionName, params string[] arguments)
        {
            return $"{functionName}({string.Join(",", arguments)})";
        }

        public static string InvokeSpaced(string functionName, params string[] arguments)
        {
            return $"{functionName}( {string.Join(", ", arguments)} )";
        }

        public static string Not(string expression)
        {
            return $"{JassKeyword.Not} {expression}";
        }

        public static string Negate(string expression)
        {
            return $"-{expression}";
        }

        public static string And(string left, string right)
        {
            return $"{left} {JassKeyword.And} {right}";
        }

        public static string Or(string left, string right)
        {
            return $"{left} {JassKeyword.Or} {right}";
        }

        public static string Equal(string left, string right)
        {
            return $"{left} == {right}";
        }

        public static string EqualCompact(string left, string right)
        {
            return $"{left}=={right}";
        }

        public static string NotEqual(string left, string right)
        {
            return $"{left} != {right}";
        }

        public static string Add(string left, string right)
        {
            return $"{left} + {right}";
        }

        public static string Subtract(string left, string right)
        {
            return $"{left} - {right}";
        }

        public static string Multiply(string left, string right)
        {
            return $"{left} * {right}";
        }

        public static string Divide(string left, string right)
        {
            return $"{left} / {right}";
        }

        public static string LessThan(string left, string right)
        {
            return $"{left} < {right}";
        }

        public static string LessThanOrEqual(string left, string right)
        {
            return $"{left} <= {right}";
        }

        public static string GreaterThan(string left, string right)
        {
            return $"{left} > {right}";
        }

        public static string GreaterThanOrEqual(string left, string right)
        {
            return $"{left} >= {right}";
        }

        public static string Binary(string left, string @operator, string right)
        {
            return $"{left} {@operator} {right}";
        }

        public static string Parenthesized(string expression)
        {
            return $"( {expression} )";
        }

        public static string ParenthesizedCompact(string expression)
        {
            return $"({expression})";
        }

        public static string ElementAccess(string arrayName, int index)
        {
            return $"{arrayName}[{index}]";
        }

        public static string ElementAccess(string arrayName, string index)
        {
            return $"{arrayName}[{index}]";
        }
    }
}