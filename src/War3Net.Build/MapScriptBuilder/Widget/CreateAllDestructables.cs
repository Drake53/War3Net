// ------------------------------------------------------------------------------
// <copyright file="CreateAllDestructables.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using War3Net.Build.Common;
using War3Net.Build.Extensions;
using War3Net.Build.Info;
using War3Net.Build.Widget;
using War3Net.CodeAnalysis;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual void GenerateCreateAllDestructables(Map map, IndentedTextWriter writer)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var mapDoodads = map.Doodads;
            if (mapDoodads is null)
            {
                throw new ArgumentException($"Function '{GeneratedFunctionName.CreateAllDestructables}' cannot be generated without {nameof(MapDoodads)}.", nameof(map));
            }

            writer.WriteFunction(GeneratedFunctionName.CreateAllDestructables);

            if (!ForceGenerateGlobalDestructableVariable)
            {
                writer.WriteLocal(TypeName.Destructable, VariableName.Destructable);
            }

            writer.WriteLocal(TypeName.Trigger, VariableName.Trigger);

            if (UseLifeVariable)
            {
                writer.WriteLocal(JassKeyword.Real, VariableName.Life);
            }

            var createFunctions = new[]
            {
                NativeName.CreateDestructable,
                NativeName.CreateDeadDestructable,
                NativeName.CreateDestructableZ,
                NativeName.CreateDeadDestructableZ,
                NativeName.BlzCreateDestructableWithSkin,
                NativeName.BlzCreateDeadDestructableWithSkin,
                NativeName.BlzCreateDestructableZWithSkin,
                NativeName.BlzCreateDeadDestructableZWithSkin,
            };

            foreach (var (destructable, id) in mapDoodads.Doodads.Where(ShouldGenerateDestructableVariable).IncludeId())
            {
                var destructableVariableName = destructable.GetVariableName();
                if (!ForceGenerateGlobalDestructableVariable && (!TriggerVariableReferences.TryGetValue(destructableVariableName, out var value) || !value))
                {
                    destructableVariableName = VariableName.Destructable;
                }

                var isDead = destructable.Life == 0;
                var hasZ = destructable.State.HasFlag(DoodadState.WithZ);
                var skinId = destructable.SkinId == 0 ? destructable.TypeId : destructable.SkinId;
                var hasSkin = ForceGenerateUnitWithSkin || skinId != destructable.TypeId;
                var createFunctionIndex = isDead ? 1 : 0;

                var arguments = new List<string>();
                arguments.Add(JassLiteral.FourCC(destructable.TypeId));
                arguments.Add(JassLiteral.Real(destructable.Position.X));
                arguments.Add(JassLiteral.Real(destructable.Position.Y));
                if (hasZ)
                {
                    arguments.Add(JassLiteral.Real(destructable.Position.Z));
                    createFunctionIndex += 2;
                }

                arguments.Add(JassLiteral.Real(destructable.Rotation * W3MathF.Rad2Deg, 3));
                arguments.Add(JassLiteral.Real(destructable.Scale.X, 3));
                arguments.Add(JassLiteral.Int(destructable.Variation));
                if (hasSkin)
                {
                    arguments.Add(JassLiteral.FourCC(destructable.SkinId));
                    createFunctionIndex += 4;
                }

                writer.WriteSet(
                    destructableVariableName,
                    JassExpression.InvokeSpaced(
                        createFunctions[createFunctionIndex],
                        arguments.ToArray()));

                if (!isDead && destructable.Life != 100)
                {
                    var lifePercentLiteral = JassLiteral.Real(destructable.Life * 0.01f, 2);

                    if (UseLifeVariable)
                    {
                        writer.WriteSet(
                            VariableName.Life,
                            JassExpression.InvokeSpaced(
                                NativeName.GetDestructableLife,
                                destructableVariableName));

                        writer.WriteCall(
                            NativeName.SetDestructableLife,
                            destructableVariableName,
                            JassExpression.Multiply(
                                lifePercentLiteral,
                                VariableName.Life));
                    }
                    else
                    {
                        writer.WriteCall(
                            NativeName.SetDestructableLife,
                            destructableVariableName,
                            JassExpression.Multiply(
                                lifePercentLiteral,
                                JassExpression.Invoke(NativeName.GetDestructableLife, destructableVariableName)));
                    }
                }

                if (destructable.HasItemTable())
                {
                    writer.WriteSet(
                        VariableName.Trigger,
                        JassExpression.InvokeSpaced(NativeName.CreateTrigger));

                    writer.WriteCall(
                        NativeName.TriggerRegisterDeathEvent,
                        VariableName.Trigger,
                        destructableVariableName);

                    writer.WriteCall(
                        NativeName.TriggerAddAction,
                        VariableName.Trigger,
                        JassExpression.FunctionRef(FunctionName.SaveDyingWidget));

                    writer.WriteCall(
                        NativeName.TriggerAddAction,
                        VariableName.Trigger,
                        JassExpression.FunctionRef(destructable.GetDropItemsFunctionName(id)));
                }
            }

            writer.EndFunction();
        }

        protected internal virtual bool ShouldGenerateCreateAllDestructables(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (map.Info is not null && map.Info.FormatVersion == MapInfoFormatVersion.v8)
            {
                return true;
            }

            return map.Doodads is not null
                && map.Doodads.Doodads.Any(ShouldGenerateDestructableVariable);
        }
    }
}