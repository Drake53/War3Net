// ------------------------------------------------------------------------------
// <copyright file="TranspileStringConcatenationHandler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static class TranspileStringConcatenationHandler
    {
        private static readonly HashSet<string> _functions = new HashSet<string>();
        private static readonly HashSet<string> _globals = new HashSet<string>();
        private static readonly HashSet<string> _locals = new HashSet<string>();

        public static void RegisterFunctionWithStringReturnType(string variableName)
        {
            _functions.Add(variableName);
        }

        public static void RegisterGlobalStringVariable(string variableName)
        {
            _globals.Add(variableName);
        }

        internal static void RegisterLocalStringVariable(string variableName)
        {
            _locals.Add(variableName);
        }

        internal static bool IsFunctionStringReturnType(string functionName)
        {
            return _functions.Contains(functionName);
        }

        internal static bool IsStringVariable(string variableName)
        {
            return _globals.Contains(variableName) || _locals.Contains(variableName);
        }

        internal static void ResetLocalVariables()
        {
            _locals.Clear();
        }

        internal static void Reset()
        {
            _globals.Clear();
            _locals.Clear();
            _functions.Clear();
        }
    }
}