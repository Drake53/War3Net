// ------------------------------------------------------------------------------
// <copyright file="BinaryOperatorType.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    [Obsolete]
    public enum BinaryOperatorType
    {
        Add,
        Subtract,
        Multiplication,
        Division,
        GreaterThan,
        LessThan,
        Equals,
        NotEquals,
        GreaterOrEqual,
        LessOrEqual,
        And,
        Or,
    }
}