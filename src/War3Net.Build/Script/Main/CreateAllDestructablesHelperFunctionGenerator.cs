// ------------------------------------------------------------------------------
// <copyright file="CreateAllDestructablesHelperFunctionGenerator.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using War3Net.Build.Widget;

namespace War3Net.Build.Script.Main
{
    internal static partial class MainFunctionGenerator<TBuilder, TGlobalDeclarationSyntax, TFunctionSyntax, TStatementSyntax, TExpressionSyntax>
    {
        private const string LocalDestructableVariableName = "d";

        private static TFunctionSyntax GenerateCreateAllDestructablesHelperFunction(TBuilder builder)
        {
            var locals = new List<(string, string)>()
            {
                (nameof(War3Api.Common.destructable), LocalDestructableVariableName),
                (nameof(War3Api.Common.trigger), LocalTriggerVariableName),
            };

            return builder.Build("CreateAllDestructables", locals, GetCreateAllDestructablesHelperFunctionStatements(builder));
        }

        private static IEnumerable<TStatementSyntax> GetCreateAllDestructablesHelperFunctionStatements(TBuilder builder)
        {
            foreach (var destructable in builder.Data.MapDoodads.Where(mapDoodad => mapDoodad.DroppedItemData.FirstOrDefault() != null))
            {
                var isDead = destructable.Life == 0;
                var hasZ = destructable.State.HasFlag(DoodadState.WithZ);
                var hasSkin = destructable.HasSkin;

                var args = new List<TExpressionSyntax>();
                args.Add(builder.GenerateFourCCExpression(destructable.TypeId));
                args.Add(builder.GenerateFloatLiteralExpression(destructable.PositionX));
                args.Add(builder.GenerateFloatLiteralExpression(destructable.PositionY));
                if (hasZ)
                {
                    args.Add(builder.GenerateFloatLiteralExpression(destructable.PositionZ));
                }

                args.Add(builder.GenerateFloatLiteralExpression(destructable.FacingDeg));
                args.Add(builder.GenerateFloatLiteralExpression(destructable.ScaleX));
                args.Add(builder.GenerateIntegerLiteralExpression(destructable.Variation));
                if (hasSkin)
                {
                    args.Add(builder.GenerateFourCCExpression(destructable.Skin));
                }

                yield return builder.GenerateAssignmentStatement(
                    LocalDestructableVariableName,
                    builder.GenerateInvocationExpression(
                        hasSkin
                            ? hasZ
                                ? isDead
                                    ? nameof(War3Api.Common.BlzCreateDeadDestructableZWithSkin)
                                    : nameof(War3Api.Common.BlzCreateDestructableZWithSkin)
                                : isDead
                                    ? nameof(War3Api.Common.BlzCreateDeadDestructableWithSkin)
                                    : nameof(War3Api.Common.BlzCreateDestructableWithSkin)
                            : hasZ
                                ? isDead
                                    ? nameof(War3Api.Common.CreateDeadDestructableZ)
                                    : nameof(War3Api.Common.CreateDestructableZ)
                                : isDead
                                    ? nameof(War3Api.Common.CreateDeadDestructable)
                                    : nameof(War3Api.Common.CreateDestructable),
                        args.ToArray()));

                if (!isDead && destructable.Life != 100)
                {
                    yield return builder.GenerateInvocationStatement(
                        nameof(War3Api.Common.SetDestructableLife),
                        builder.GenerateVariableExpression(LocalDestructableVariableName),
                        builder.GenerateBinaryExpression(
                            BinaryOperator.Multiplication,
                            builder.GenerateFloatLiteralExpression(destructable.Life * 0.01f),
                            builder.GenerateInvocationExpression(
                                nameof(War3Api.Common.GetDestructableLife),
                                builder.GenerateVariableExpression(LocalDestructableVariableName))));
                }

                foreach (var droppedItem in destructable.DroppedItemData)
                {
                    yield return builder.GenerateAssignmentStatement(
                        LocalTriggerVariableName,
                        builder.GenerateInvocationExpression(nameof(War3Api.Common.CreateTrigger)));

                    yield return builder.GenerateInvocationStatement(
                        nameof(War3Api.Common.TriggerRegisterDeathEvent),
                        builder.GenerateVariableExpression(LocalTriggerVariableName),
                        builder.GenerateVariableExpression(LocalDestructableVariableName));

                    yield return builder.GenerateInvocationStatement(
                        nameof(War3Api.Common.TriggerAddAction),
                        builder.GenerateVariableExpression(LocalTriggerVariableName),
                        builder.GenerateFunctionReferenceExpression(nameof(War3Api.Blizzard.SaveDyingWidget)));

                    yield return builder.GenerateInvocationStatement(
                        nameof(War3Api.Common.TriggerAddAction),
                        builder.GenerateVariableExpression(LocalTriggerVariableName),
                        builder.GenerateFunctionReferenceExpression($"Doodad{destructable.CreationNumber.ToString("D6")}_DropItems"));
                }
            }
        }
    }
}