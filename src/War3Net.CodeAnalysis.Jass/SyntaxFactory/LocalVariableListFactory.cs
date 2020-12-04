// ------------------------------------------------------------------------------
// <copyright file="LocalVariableListFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Linq;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static LocalVariableListSyntax LocalVariableList(params LocalVariableDeclarationSyntax[] locals)
        {
            return locals.Any() ? new LocalVariableListSyntax(locals) : new LocalVariableListSyntax(Empty());
        }
    }
}