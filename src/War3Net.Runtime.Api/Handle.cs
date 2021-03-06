﻿// ------------------------------------------------------------------------------
// <copyright file="Handle.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Runtime.Api
{
    public abstract class Handle
    {
        /// @CSharpLua.Template.get = "GetHandleId({0})"
        public extern int GetHandleId();
    }
}