// ------------------------------------------------------------------------------
// <copyright file="VariableReferences.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using War3Net.Build.Script;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        public virtual void FindVariableReferences(Map map)
        {
            var mapTriggers = map.Triggers;
            if (mapTriggers is not null)
            {
                foreach (var trigger in mapTriggers.TriggerItems)
                {
                    if (trigger is TriggerDefinition triggerDefinition)
                    {
                        foreach (var function in triggerDefinition.Functions)
                        {
                            FindVariableReferences(function, triggerDefinition.IsEnabled);
                        }
                    }
                }
            }
        }

        protected internal virtual void FindVariableReferences(TriggerFunction function, bool isEnabled)
        {
            isEnabled = isEnabled && function.IsEnabled;

            foreach (var parameter in function.Parameters)
            {
                FindVariableReferences(parameter, isEnabled);
            }

            foreach (var childFunction in function.ChildFunctions)
            {
                FindVariableReferences(childFunction, isEnabled);
            }
        }

        protected internal virtual void FindVariableReferences(TriggerFunctionParameter parameter, bool isEnabled)
        {
            if (parameter.Type == TriggerFunctionParameterType.Variable)
            {
                if (parameter.Value.StartsWith("gg_", StringComparison.Ordinal))
                {
                    if (TriggerVariableReferences.TryGetValue(parameter.Value, out var value))
                    {
                        TriggerVariableReferences[parameter.Value] = value || isEnabled;
                    }
                    else
                    {
                        TriggerVariableReferences.Add(parameter.Value, isEnabled);
                    }
                }

                if (parameter.ArrayIndexer is not null)
                {
                    FindVariableReferences(parameter.ArrayIndexer, isEnabled);
                }
            }
            else if (parameter.Function is not null)
            {
                FindVariableReferences(parameter.Function, isEnabled);
            }
        }
    }
}