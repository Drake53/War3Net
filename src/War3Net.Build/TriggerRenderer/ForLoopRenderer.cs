namespace War3Net.Build
{
    public partial class TriggerRenderer
    {
        private void RenderForLoopA(TriggerFunction function, TriggerRendererContext context)
        {
            RenderForLoop(function, context, "bj_forLoopAIndex", "bj_forLoopAIndexEnd");
        }

        private void RenderForLoopB(TriggerFunction function, TriggerRendererContext context)
        {
            RenderForLoop(function, context, "bj_forLoopBIndex", "bj_forLoopBIndexEnd");
        }

        private void RenderForLoop(TriggerFunction function, TriggerRendererContext context, string indexName, string indexEndName)
        {
            var argumentTypes = GetArgumentTypes(function);

            context.Writer.WriteSet(indexName, GetParameter(function.Parameters[0], argumentTypes[0], 0, context.TrigFunctionIdentifierBuilder));
            context.Writer.WriteSet(indexEndName, GetParameter(function.Parameters[1], argumentTypes[1], 1, context.TrigFunctionIdentifierBuilder));

            context.Writer.WriteLoop();
            context.Writer.WriteExitWhen(JassExpression.GreaterThan(
                indexName,
                indexEndName));

            RenderTriggerAction(function.Parameters[2].Function, context);

            context.Writer.WriteSet(
                indexName,
                JassExpression.Add(indexName, "1"));

            context.Writer.WriteEndLoop();
        }

        private void RenderForLoopVar(TriggerFunction function, TriggerRendererContext context)
        {
            throw new NotImplementedException();
        }

        private void RenderForLoopAMultiple(TriggerFunction function, TriggerRendererContext context)
        {
            throw new NotImplementedException();
        }

        private void RenderForLoopBMultiple(TriggerFunction function, TriggerRendererContext context)
        {
            throw new NotImplementedException();
        }

        private void RenderForLoopVarMultiple(TriggerFunction function, TriggerRendererContext context)
        {
            throw new NotImplementedException();
        }
    }
}