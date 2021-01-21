// ------------------------------------------------------------------------------
// <copyright file="JassArgumentListSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Immutable;
using System.Linq;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassArgumentListSyntax : IEquatable<JassArgumentListSyntax>
    {
        public JassArgumentListSyntax(ImmutableArray<IExpressionSyntax> arguments)
        {
            Arguments = arguments;
        }

        public ImmutableArray<IExpressionSyntax> Arguments { get; init; }

        public bool Equals(JassArgumentListSyntax? other)
        {
            return other is not null
                && Arguments.SequenceEqual(other.Arguments);
        }

        public override string ToString() => string.Join($"{JassSymbol.Comma} ", Arguments);
    }
}