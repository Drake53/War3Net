// ------------------------------------------------------------------------------
// <copyright file="InitGlobals.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Linq;

using War3Net.Build.Extensions;
using War3Net.Build.Info;
using War3Net.Build.Script;
using War3Net.CodeAnalysis;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual void GenerateInitGlobals(Map map, IndentedTextWriter writer)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var mapTriggers = map.Triggers;
            if (mapTriggers is null)
            {
                throw new ArgumentException($"Function '{GeneratedFunctionName.InitGlobals}' cannot be generated without {nameof(MapTriggers)}.", nameof(map));
            }

            writer.WriteFunction(GeneratedFunctionName.InitGlobals);

            if (mapTriggers.Variables.Any(variable =>
                (variable.IsArray ||
                 map.Info.FormatVersion >= MapInfoFormatVersion.v31) &&
                (variable.IsInitialized ||
                 TriggerData.TriggerTypeDefaults.TryGetValue(variable.Type, out _) ||
                 string.Equals(variable.Type, JassKeyword.String, StringComparison.Ordinal))))
            {
                writer.WriteLocal(
                    JassKeyword.Integer,
                    "i",
                    "0");
            }

            foreach (var variable in mapTriggers.Variables)
            {
                string? initialValueExpression = null;

                if (variable.IsInitialized)
                {
                    initialValueExpression = TriggerData.TriggerParams.TryGetValue(variable.InitialValue, out var triggerParam) && string.Equals(triggerParam.VariableType, variable.Type, StringComparison.Ordinal)
                        ? triggerParam.ScriptText
                        : string.Equals(variable.Type, JassKeyword.String, StringComparison.Ordinal)
                            ? $"\"{EscapedStringProvider.GetEscapedString(variable.InitialValue)}\""
                            : variable.InitialValue;
                }
                else if (TriggerData.TriggerTypeDefaults.TryGetValue(variable.Type, out var triggerTypeDefault))
                {
                    initialValueExpression = triggerTypeDefault.ScriptText;
                }
                else if (string.Equals(variable.Type, JassKeyword.String, StringComparison.Ordinal))
                {
                    initialValueExpression = "\"\"";
                }

                if (initialValueExpression is not null)
                {
                    WriteInitGlobal(variable, initialValueExpression, writer);
                }
            }

            writer.EndFunction();
        }

        protected internal virtual void WriteInitGlobal(VariableDefinition variable, string initialValueExpression, IndentedTextWriter writer)
        {
            if (variable is null)
            {
                throw new ArgumentNullException(nameof(variable));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (variable.IsArray)
            {
                writer.WriteSet(
                    "i",
                    "0");

                writer.WriteLoop();
                writer.WriteExitWhen(JassExpression.ParenthesizedCompact(JassExpression.GreaterThan(
                    "i",
                    JassLiteral.Int(variable.ArraySize))));

                writer.WriteSet(
                    JassExpression.ElementAccess(variable.GetVariableName(), "i"),
                    initialValueExpression);

                writer.WriteSet(
                    "i",
                    JassExpression.Add("i", "1"));

                writer.WriteEndLoop();
                writer.WriteLine();
            }
            else
            {
                writer.WriteSet(
                    variable.GetVariableName(),
                    initialValueExpression);
            }
        }

        protected internal virtual bool ShouldGenerateInitGlobals(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Triggers is not null;
        }
    }
}