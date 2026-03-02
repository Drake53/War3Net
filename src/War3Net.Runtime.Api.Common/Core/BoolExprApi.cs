using System;
using War3Net.Runtime.Core;

namespace War3Net.Runtime.Api.Common.Core
{
    public static class BoolExprApi
    {
        public static BoolExpr And(BoolExpr? operandA, BoolExpr? operandB) => throw new NotImplementedException();

        public static BoolExpr Or(BoolExpr? operandA, BoolExpr? operandB) => throw new NotImplementedException();

        public static BoolExpr Not(BoolExpr? operand) => throw new NotImplementedException();

        public static void DestroyBoolExpr(BoolExpr? e) => e.Dispose();
    }
}