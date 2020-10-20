// ------------------------------------------------------------------------------
// <copyright file="MathApi.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.Runtime.Api.Common.Core
{
    public static class MathApi
    {
        public static float Deg2Rad(float degrees) => degrees * MathF.PI / 180f;

        public static float Rad2Deg(float radians) => radians * 180f / MathF.PI;

        public static float Sin(float radians) => MathF.Sin(radians);

        public static float Cos(float radians) => MathF.Cos(radians);

        public static float Tan(float radians) => MathF.Tan(radians);

        public static float Asin(float x)
        {
            var result = MathF.Asin(x);
            return float.IsNaN(result) ? 0f : result;
        }

        public static float Acos(float x)
        {
            var result = MathF.Acos(x);
            return float.IsNaN(result) ? 0f : result;
        }

        public static float Atan(float x) => MathF.Atan(x);

        public static float Atan2(float y, float x) => MathF.Atan2(y, x);

        public static float SquareRoot(float x)
        {
            var result = MathF.Sqrt(x);
            return float.IsNaN(result) ? 0f : result;
        }

        public static float Pow(float x, float power) => MathF.Pow(x, power);

        public static int BlzBitOr(int x, int y) => x | y;

        public static int BlzBitAnd(int x, int y) => x & y;

        public static int BlzBitXor(int x, int y) => x ^ y;
    }
}