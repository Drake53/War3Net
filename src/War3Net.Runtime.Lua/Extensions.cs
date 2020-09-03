// ------------------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using NLua;
using NLua.Exceptions;

using Serilog;

namespace War3Net.Runtime.Lua
{
    public static class Extensions
    {
        public static void Invoke(this LuaFunction luaFunction)
        {
            try
            {
                luaFunction.Call();
            }
            catch (LuaScriptException e)
            {
                Log.Error(string.Join("\r\n", e.EnumerateInnerExceptions().Select(exc => $"  {exc}").Prepend("Unhandled exception")));

                return;
            }
            catch
            {
                throw;
            }
        }

        public static bool InvokeCondition(this LuaFunction luaFunction)
        {
            try
            {
                return (bool)luaFunction.Call()[0];
            }
            catch (LuaScriptException e)
            {
                Log.Error(string.Join("\r\n", e.EnumerateInnerExceptions().Select(exc => $"  {exc}").Prepend("Unhandled exception")));

                return false;
            }
            catch
            {
                throw;
            }
        }

        private static IEnumerable<Exception> EnumerateInnerExceptions(this Exception e)
        {
            do
            {
                yield return e;
                e = e.InnerException;
            }
            while (e != null);
        }
    }
}