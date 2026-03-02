namespace War3Net.Build
{
    public partial class TriggerRenderer
    {
        private void RenderConditionFunction(TrigFunctionIdentifierBuilder identifierBuilder, string functionName, TriggerFunctionParameter parameter)
        {
            if (parameter.Type != TriggerFunctionParameterType.Function || parameter.Function is null)
            {
                throw new ArgumentException("Parameter must have a function and be of type 'Function'.", nameof(parameter));
            }

            var function = parameter.Function;
            if (function.Type != TriggerFunctionType.Condition || !function.IsEnabled)
            {
                throw new ArgumentException("Parameter function must be enabled and of type 'Condition'.", nameof(parameter));
            }

            using var writer = IndentedTextWriter.New(_writer);

            var context = new TriggerRendererContext(writer, identifierBuilder);

            writer.WriteFilterFunction(functionName);

            var expression = GetTriggerConditionExpression(function, context);

            writer.WriteReturn(expression);
            writer.EndFunction();

            _writer.WriteLine(writer.ToString());
        }

        private void RenderConditionFunction(TrigFunctionIdentifierBuilder identifierBuilder, string functionName, bool returnValue, List<TriggerFunction> functions)
        {
            identifierBuilder.Append("Func");

            using var writer = IndentedTextWriter.New(_writer);

            var context = new TriggerRendererContext(writer, identifierBuilder);

            writer.WriteFilterFunction(functionName);

            for (var i = 0; i < functions.Count; i++)
            {
                var function = functions[i];
                if (function.Type != TriggerFunctionType.Condition || !function.IsEnabled)
                {
                    continue;
                }

                context.TrigFunctionIdentifierBuilder.Append(i + 1);
                var expression = GetTriggerConditionExpression(function, context);
                context.TrigFunctionIdentifierBuilder.Remove();

                if (returnValue)
                {
                    writer.WriteIf(JassExpression.Parenthesized(JassExpression.Not(expression)));
                    writer.WriteReturn(JassKeyword.False);
                    writer.WriteEndIf();
                }
                else
                {
                    writer.WriteIf(JassExpression.Parenthesized(expression));
                    writer.WriteReturn(JassKeyword.True);
                    writer.WriteEndIf();
                }
            }

            writer.WriteReturn(JassLiteral.Bool(returnValue));
            writer.EndFunction();

            _writer.WriteLine(writer.ToString());

            identifierBuilder.Remove();
        }

        private string GetTriggerConditionExpression(TriggerFunction function, TriggerRendererContext context)
        {
            if (function.Type != TriggerFunctionType.Condition || !function.IsEnabled)
            {
                throw new ArgumentException("Function must be enabled and of type 'Condition'.", nameof(function));
            }

            if (function.Name == TriggerConditionConstants.OrMultiple || function.Name == TriggerConditionConstants.AndMultiple)
            {
                var conditionFunctionName = $"{context.TrigFunctionIdentifierBuilder}C";
                RenderConditionFunction(context.TrigFunctionIdentifierBuilder, conditionFunctionName, function.Name == TriggerConditionConstants.AndMultiple, function.ChildFunctions);

                return JassExpression.Invoke(conditionFunctionName);
            }
            else if (function.Name == TriggerConditionConstants.GetBooleanAnd || function.Name == TriggerConditionConstants.GetBooleanOr)
            {
                context.TrigFunctionIdentifierBuilder.Append(1);
                var conditionFunctionName1 = context.TrigFunctionIdentifierBuilder.ToString();
                RenderConditionFunction(context.TrigFunctionIdentifierBuilder, conditionFunctionName1, function.Parameters[0]);
                context.TrigFunctionIdentifierBuilder.Remove();

                context.TrigFunctionIdentifierBuilder.Append(2);
                var conditionFunctionName2 = context.TrigFunctionIdentifierBuilder.ToString();
                RenderConditionFunction(context.TrigFunctionIdentifierBuilder, conditionFunctionName2, function.Parameters[1]);
                context.TrigFunctionIdentifierBuilder.Remove();

                return JassExpression.InvokeSpaced(
                    function.Name,
                    JassExpression.Invoke(conditionFunctionName1),
                    JassExpression.Invoke(conditionFunctionName2));
            }
            else
            {
                var parameters = GetParameters(function, context.TrigFunctionIdentifierBuilder).ToArray();

                var @operator = parameters[1];
                if (@operator.StartsWith('"') && @operator.EndsWith('"'))
                {
                    @operator = @operator[1..^1];
                }

                return JassExpression.Parenthesized(JassExpression.Binary(
                    parameters[0],
                    @operator,
                    parameters[2]));
            }
        }
    }
}