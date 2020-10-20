// ------------------------------------------------------------------------------
// <copyright file="NLuaVirtualMachine.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Reflection;

using NLua;

using War3Net.Build.Common;
using War3Net.Runtime.Api.Blizzard;
using War3Net.Runtime.Api.Common;

namespace War3Net.Runtime.Lua
{
    public sealed class NLuaVirtualMachine : IVirtualMachine
    {
        private readonly NLua.Lua _luaState;

        public NLuaVirtualMachine(GamePatch? gamePatch, string mapScript)
            : this(
                  gamePatch,
                  mapScript,
#if DEBUG
                  true)
#else
                  false)
#endif
        {
        }

        internal NLuaVirtualMachine(GamePatch? gamePatch, bool debug)
            : this(gamePatch, null, debug)
        {
        }

        internal NLuaVirtualMachine(GamePatch? gamePatch, string? mapScript, bool debug)
        {
            _luaState = new NLua.Lua();

            var defaultMaximumRecursion = _luaState.MaximumRecursion;
            _luaState.MaximumRecursion = 0;

            const string ToStringFunction = @"
local ts = tostring
local tscs = tostringcs
tostring = function(v)
  if type(v) == 'userdata' then
    return tscs(v)
  end
  return ts(v)
end
";
            _luaState.RegisterFunction("tostringcs", typeof(NLuaVirtualMachine).GetMethod(nameof(UserDataToString)));
            _luaState.DoString(ToStringFunction, "tostring function");

            if (debug)
            {
                const string ErrorFunction = @"
local err = error
local traceback = debug.traceback
error = function(message, level)
  if message ~= nil then
    message = message.message or message
  end
  level = (level or 1) + 1
  err(traceback(message, level), level)
end
";
                _luaState.DoString(ErrorFunction, "error function");
            }

#if DEBUG
            State.RegisterFunction("print", GetType().GetMethod(nameof(LogPrintMessage)));
#endif

            _luaState.RemoveNonWhitelistedGlobals();

            if (gamePatch.HasValue)
            {
                this.InjectCommonApi(gamePatch.Value);
                this.InjectBlizzardApi(gamePatch.Value);
            }

            _luaState.MaximumRecursion = defaultMaximumRecursion;

            if (mapScript != null)
            {
                _luaState.DoString(mapScript, "war3map.lua");
                if (!(_luaState["config"] is LuaFunction configFunc && _luaState["main"] is LuaFunction mainFunc))
                {
                    throw new ArgumentException("Map script does not contain 'config' and/or 'main' function.");
                }
            }
        }

        internal NLua.Lua State => _luaState;

        public void Dispose()
        {
            _luaState.Dispose();
        }

        /// <inheritdoc/>
        public void InjectField(FieldInfo field)
        {
            _luaState[field.Name] = field.GetValue(null);
        }

        /// <inheritdoc/>
        public void InjectMethod(MethodInfo methodInfo)
        {
            _luaState.RegisterFunction(methodInfo.Name, methodInfo);
        }

        /// <inheritdoc/>
        public void RunMethod(string methodName)
        {
            ((LuaFunction)_luaState[methodName]).Invoke();
        }

        public static string UserDataToString(object v)
        {
            return $"{v.GetType().Name.ToLowerInvariant()}: {v.GetHashCode():X16}";
        }

        public static void LogPrintMessage(string s)
        {
#if DEBUG
            // Log.Information(s);
#endif
        }
    }
}