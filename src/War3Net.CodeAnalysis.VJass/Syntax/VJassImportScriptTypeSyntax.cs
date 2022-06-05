// ------------------------------------------------------------------------------
// <copyright file="VJassImportScriptTypeSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassImportScriptTypeSyntax : IEquatable<VJassImportScriptTypeSyntax>
    {
        public VJassImportScriptTypeSyntax(string scriptTypeName)
        {
            ScriptTypeName = scriptTypeName;
        }

        public string ScriptTypeName { get; }

        public bool Equals(VJassImportScriptTypeSyntax? other)
        {
            return other is not null
                && string.Equals(ScriptTypeName, other.ScriptTypeName, StringComparison.Ordinal);
        }

        public override string ToString() => ScriptTypeName;
    }
}