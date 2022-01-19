// ------------------------------------------------------------------------------
// <copyright file="IInvocationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public interface IInvocationSyntax
    {
        public JassIdentifierNameSyntax IdentifierName { get; init; }

        public JassArgumentListSyntax Arguments { get; init; }
    }
}