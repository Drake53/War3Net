namespace War3Net.Build
{
    public partial class TriggerRenderer
    {
        private void RenderWaitForCondition(TriggerFunction function, TriggerRendererContext context)
        {
            var argumentTypes = GetArgumentTypes(function);

            context.TrigFunctionIdentifierBuilder.Append(1);
            var conditionFunctionName = context.TrigFunctionIdentifierBuilder.ToString();
            RenderConditionFunction(context.TrigFunctionIdentifierBuilder, conditionFunctionName, function.Parameters[0]);
            context.TrigFunctionIdentifierBuilder.Remove();

            context.Writer.WriteLoop();
            context.Writer.WriteExitWhen(JassExpression.Parenthesized(JassExpression.Invoke(conditionFunctionName)));

            context.Writer.WriteCallCompact(
                WellKnownNatives.TriggerSleepAction,
                JassExpression.Invoke(
                    WellKnownFunctions.RMaxBJ,
                    "bj_WAIT_FOR_COND_MIN_INTERVAL",
                    GetParameter(function.Parameters[1], argumentTypes[1], 1, context.TrigFunctionIdentifierBuilder)));

            context.Writer.WriteEndLoop();
        }
    }
}