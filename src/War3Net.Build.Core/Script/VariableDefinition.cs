// ------------------------------------------------------------------------------
// <copyright file="VariableDefinition.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Script
{
    public sealed partial class VariableDefinition
    {
        public VariableDefinition()
        {
        }

        public string Name { get; set; }

        public string Type { get; set; }

        public int Unk { get; set; }

        public bool IsArray { get; set; }

        public int ArraySize { get; set; }

        public bool IsInitialized { get; set; }

        public string InitialValue { get; set; }

        public int Id { get; set; }

        public int ParentId { get; set; }

        public override string ToString()
        {
            return $"{Type} {Name}{(IsArray ? $"[{(ArraySize > 0 ? $"{ArraySize}" : string.Empty)}]" : string.Empty)}{(IsInitialized ? $" = {InitialValue}" : string.Empty)}";
        }
    }
}