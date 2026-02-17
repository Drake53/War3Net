// ------------------------------------------------------------------------------
// <copyright file="CustomScriptCodeDecompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.Build.Script;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private TriggerFunction DecompileCustomScriptAction(string customScriptCode)
        {
            return new TriggerFunction
            {
                Type = TriggerFunctionType.Action,
                IsEnabled = true,
                Name = "CustomScriptCode",
                Parameters = new()
                {
                    new TriggerFunctionParameter
                    {
                        Type = TriggerFunctionParameterType.String,
                        Value = customScriptCode,
                    },
                },
            };
        }
    }
}