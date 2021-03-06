// ------------------------------------------------------------------------------
// <copyright file="LimitOp.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CS0626 // Method, operator, or accessor is marked external and has no attributes on it
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor

namespace War3Net.Runtime.Api.Enums
{
    public class LimitOp : EventId
    {
        /// @CSharpLua.Template = "LESS_THAN"
        public static readonly LimitOp LessThan;

        /// @CSharpLua.Template = "LESS_THAN_OR_EQUAL"
        public static readonly LimitOp LessThanOrEqual;

        /// @CSharpLua.Template = "EQUAL"
        public static readonly LimitOp Equal;

        /// @CSharpLua.Template = "GREATER_THAN_OR_EQUAL"
        public static readonly LimitOp GreaterThanOrEqual;

        /// @CSharpLua.Template = "GREATER_THAN"
        public static readonly LimitOp GreaterThan;

        /// @CSharpLua.Template = "NOT_EQUAL"
        public static readonly LimitOp NotEqual;

        /// @CSharpLua.Template = "ConvertLimitOp({0})"
        public extern LimitOp(int id);

        /// @CSharpLua.Template = "ConvertLimitOp({0})"
        public static extern LimitOp GetById(int id);
    }
}