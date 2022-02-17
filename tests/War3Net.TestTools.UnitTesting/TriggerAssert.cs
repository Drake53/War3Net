// ------------------------------------------------------------------------------
// <copyright file="TriggerAssert.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass;

namespace War3Net.TestTools.UnitTesting
{
    public static class TriggerAssert
    {
        public static void AreEqual(TriggerDefinition expectedTriggerDefinition, TriggerDefinition actualTriggerDefinition)
        {
            Assert.AreEqual(expectedTriggerDefinition.IsComment, actualTriggerDefinition.IsComment);
            Assert.AreEqual(expectedTriggerDefinition.IsCustomTextTrigger, actualTriggerDefinition.IsCustomTextTrigger);
            Assert.AreEqual(expectedTriggerDefinition.IsEnabled, actualTriggerDefinition.IsEnabled);
            Assert.AreEqual(expectedTriggerDefinition.IsInitiallyOn, actualTriggerDefinition.IsInitiallyOn);
            Assert.AreEqual(expectedTriggerDefinition.RunOnMapInit, actualTriggerDefinition.RunOnMapInit);

            var expectedEvents = expectedTriggerDefinition.Functions.Where(function => function.Type == TriggerFunctionType.Event && function.IsEnabled && (expectedTriggerDefinition.IsInitiallyOn || !string.Equals(function.Name, "MapInitializationEvent", StringComparison.Ordinal))).ToList();
            var actualEvents = actualTriggerDefinition.Functions.Where(function => function.Type == TriggerFunctionType.Event).ToList();

            AreEqual(expectedEvents, actualEvents);

            var expectedConditions = expectedTriggerDefinition.Functions.Where(function => function.Type == TriggerFunctionType.Condition && function.IsEnabled).ToList();
            var actualConditions = actualTriggerDefinition.Functions.Where(function => function.Type == TriggerFunctionType.Condition).ToList();

            AreEqual(expectedConditions, actualConditions);

            var expectedActions = expectedTriggerDefinition.Functions.Where(function => function.Type == TriggerFunctionType.Action && function.IsEnabled).ToList();
            var actualActions = actualTriggerDefinition.Functions.Where(function => function.Type == TriggerFunctionType.Action).ToList();

            AreEqual(expectedActions, actualActions);
        }

        public static void AreEqual(TriggerFunctionParameter expectedFunctionParameter, TriggerFunctionParameter actualFunctionParameter)
        {
            Assert.AreEqual(expectedFunctionParameter.Type, actualFunctionParameter.Type);
            Assert.AreEqual(expectedFunctionParameter.Value, actualFunctionParameter.Value, ignoreCase: false, CultureInfo.InvariantCulture);

            Assert.AreEqual(expectedFunctionParameter.Function is null, actualFunctionParameter.Function is null);
            if (expectedFunctionParameter.Function is not null)
            {
                AreEqual(expectedFunctionParameter.Function, actualFunctionParameter.Function);
            }

            Assert.AreEqual(expectedFunctionParameter.ArrayIndexer is null, actualFunctionParameter.ArrayIndexer is null);
            if (expectedFunctionParameter.ArrayIndexer is not null)
            {
                AreEqual(expectedFunctionParameter.ArrayIndexer, actualFunctionParameter.ArrayIndexer);
            }
        }

        public static void AreEqual(TriggerFunction expectedFunction, TriggerFunction actualFunction)
        {
            Assert.AreEqual(expectedFunction.Type, actualFunction.Type);
            Assert.AreEqual(expectedFunction.Branch, actualFunction.Branch);
            Assert.AreEqual(expectedFunction.IsEnabled, actualFunction.IsEnabled);

            if (!string.Equals(expectedFunction.Name, actualFunction.Name, StringComparison.Ordinal))
            {
                Assert.AreEqual(expectedFunction.Name + "Multiple", actualFunction.Name, ignoreCase: false, CultureInfo.InvariantCulture);
                Assert.AreEqual(1, actualFunction.ChildFunctions.Count);

                var actualParameters = new List<TriggerFunctionParameter>();
                actualParameters.AddRange(actualFunction.Parameters);
                actualParameters.Add(new TriggerFunctionParameter
                {
                    Type = TriggerFunctionParameterType.Function,
                    Value = "DoNothing",
                    Function = new TriggerFunction
                    {
                        Type = actualFunction.ChildFunctions[0].Type,
                        Name = actualFunction.ChildFunctions[0].Name,
                        IsEnabled = actualFunction.ChildFunctions[0].IsEnabled,
                        Parameters = actualFunction.ChildFunctions[0].Parameters,
                        ChildFunctions = actualFunction.ChildFunctions[0].ChildFunctions,
                    },
                });

                AreEqual(expectedFunction.Parameters, actualParameters);
            }
            else if (string.Equals(expectedFunction.Name, "CustomScriptCode", StringComparison.Ordinal))
            {
                Assert.AreEqual(expectedFunction.Name, actualFunction.Name, ignoreCase: false, CultureInfo.InvariantCulture);

                var expectedFunctionParameter = expectedFunction.Parameters.Single();
                var actualFunctionParameter = actualFunction.Parameters.Single();

                var expectedCustomScriptAction = JassSyntaxFactory.ParseStatementLine(expectedFunctionParameter.Value);
                var actualCustomScriptAction = JassSyntaxFactory.ParseStatementLine(actualFunctionParameter.Value);

                Assert.AreEqual(expectedFunctionParameter.Type, actualFunctionParameter.Type);
                SyntaxAssert.AreEqual(expectedCustomScriptAction, actualCustomScriptAction);
                Assert.IsNull(expectedFunctionParameter.Function);
                Assert.IsNull(actualFunctionParameter.Function);
                Assert.IsNull(expectedFunctionParameter.ArrayIndexer);
                Assert.IsNull(actualFunctionParameter.ArrayIndexer);
            }
            else
            {
                Assert.AreEqual(expectedFunction.Name, actualFunction.Name, ignoreCase: false, CultureInfo.InvariantCulture);

                AreEqual(expectedFunction.Parameters, actualFunction.Parameters);
                AreEqual(expectedFunction.ChildFunctions, actualFunction.ChildFunctions);
            }
        }

        public static void AreEqual(List<TriggerFunctionParameter> expected, List<TriggerFunctionParameter> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count);
            for (var j = 0; j < expected.Count; j++)
            {
                AreEqual(expected[j], actual[j]);
            }
        }

        public static void AreEqual(List<TriggerFunction> expected, List<TriggerFunction> actual)
        {
            var expectedFunctions = expected.Where(triggerFunction => triggerFunction.IsEnabled).OrderBy(triggerFunction => triggerFunction.Branch).ToList();
            var actualFunctions = actual.OrderBy(triggerFunction => triggerFunction.Branch).ToList();

            Assert.AreEqual(expectedFunctions.Count, actualFunctions.Count);
            for (var j = 0; j < expectedFunctions.Count; j++)
            {
                AreEqual(expectedFunctions[j], actualFunctions[j]);
            }
        }
    }
}