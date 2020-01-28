// ------------------------------------------------------------------------------
// <copyright file="CreateAllDestructablesHelperFunctionGenerator.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

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
                yield return builder.GenerateAssignmentStatement(
                    LocalDestructableVariableName,
                    builder.GenerateInvocationExpression(
                        nameof(War3Api.Common.CreateDestructable),
                        builder.GenerateFourCCExpression(destructable.TypeId),
                        builder.GenerateFloatLiteralExpression(destructable.PositionX),
                        builder.GenerateFloatLiteralExpression(destructable.PositionY),
                        builder.GenerateFloatLiteralExpression(destructable.FacingDeg),
                        builder.GenerateFloatLiteralExpression(destructable.ScaleX),
                        builder.GenerateIntegerLiteralExpression(destructable.Variation)));

                if (destructable.Life != 100)
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