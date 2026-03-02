namespace War3Net.Runtime.Api.Common.Core
{
    public static class ConditionFuncApi
    {
        public static ConditionFunc Condition(Func<bool>? func) => new ConditionFunc(func);

        public static void DestroyCondition(ConditionFunc? c) => c.Dispose();
    }
}