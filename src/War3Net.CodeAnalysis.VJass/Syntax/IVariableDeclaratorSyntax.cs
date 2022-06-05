// ------------------------------------------------------------------------------
// <copyright file="IVariableDeclaratorSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public interface IVariableDeclaratorSyntax : IEquatable<IVariableDeclaratorSyntax>
    {
        VJassTypeSyntax Type { get; }

        VJassIdentifierNameSyntax IdentifierName { get; }
    }
}