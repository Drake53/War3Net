// ------------------------------------------------------------------------------
// <copyright file="VJassSyntaxFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

using Pidgin;

using War3Net.CodeAnalysis.VJass.Syntax;

namespace War3Net.CodeAnalysis.VJass
{
    public static partial class VJassSyntaxFactory
    {
        public static VJassCompilationUnitSyntax ParseCompilationUnit(string compilationUnit)
        {
            return VJassParser.Instance.CompilationUnitParser.ParseOrThrow(compilationUnit);
        }

        public static bool TryParseCompilationUnit(string compilationUnit, [NotNullWhen(true)] out VJassCompilationUnitSyntax? result)
        {
            return TryParse(compilationUnit, VJassParser.Instance.CompilationUnitParser, out result);
        }

        private static bool TryParse<TSyntax>(string input, Parser<char, TSyntax> parser, [NotNullWhen(true)] out TSyntax? result)
            where TSyntax : class
        {
            var parseResult = parser.Parse(input);
            if (parseResult.Success)
            {
                result = parseResult.Value;
                return true;
            }

            result = null;
            return false;
        }
    }
}