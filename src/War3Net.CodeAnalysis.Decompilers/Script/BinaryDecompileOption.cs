// ------------------------------------------------------------------------------
// <copyright file="BinaryDecompileOption.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.Build.Script;

namespace War3Net.CodeAnalysis.Decompilers
{
    internal sealed class BinaryDecompileOption
    {
        public string Type { get; set; }

        public TriggerFunctionParameter LeftParameter { get; set; }

        public TriggerFunctionParameter RightParameter { get; set; }

        public override string ToString() => $"{Type}: [{LeftParameter}] [{RightParameter}]";
    }
}