// ------------------------------------------------------------------------------
// <copyright file="TriggerFunctionParameter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Script
{
    public sealed partial class TriggerFunctionParameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TriggerFunctionParameter"/> class.
        /// </summary>
        public TriggerFunctionParameter()
        {
        }

        public TriggerFunctionParameterType Type { get; set; }

        public string Value { get; set; }

        public TriggerFunction? Function { get; set; }

        public TriggerFunctionParameter? ArrayIndexer { get; set; }

        public override string ToString()
        {
            return Type switch
            {
                TriggerFunctionParameterType.Preset => Value,
                TriggerFunctionParameterType.Variable => $"{Value}{(ArrayIndexer is null ? string.Empty : $"[{ArrayIndexer}]")}",
                TriggerFunctionParameterType.Function => Function?.ToString() ?? $"{Value}()",
                TriggerFunctionParameterType.String => $"\"{Value}\"",
                TriggerFunctionParameterType.Undefined => $"{{{Type}}}",
            };
        }
    }
}