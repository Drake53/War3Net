// ------------------------------------------------------------------------------
// <copyright file="JassParameterListOrEmptyParameterListSyntaxExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Immutable;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass.Extensions
{
    public static class JassParameterListOrEmptyParameterListSyntaxExtensions
    {
        public static ImmutableArray<JassParameterSyntax> GetParameters(this JassParameterListOrEmptyParameterListSyntax parameterListOrEmptyParameterList)
        {
            if (parameterListOrEmptyParameterList is JassParameterListSyntax parameterList)
            {
                return parameterList.ParameterList.Items;
            }
            else if (parameterListOrEmptyParameterList is JassEmptyParameterListSyntax)
            {
                return ImmutableArray<JassParameterSyntax>.Empty;
            }
            else
            {
                throw new ArgumentException("Unknown parameter list type.", nameof(parameterListOrEmptyParameterList));
            }
        }
    }
}