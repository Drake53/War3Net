// ------------------------------------------------------------------------------
// <copyright file="BuildResult.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Microsoft.CodeAnalysis;

namespace War3Net.Build
{
    public sealed class BuildResult
    {
        private readonly bool _success;
        private readonly ImmutableArray<Diagnostic> _diagnostics;

        internal BuildResult(bool success, CompileResult? compileResult, IEnumerable<Diagnostic> diagnostics)
        {
            if (compileResult is null)
            {
                _success = success;
                _diagnostics = diagnostics.ToImmutableArray();
            }
            else
            {
                _success = success && compileResult.Success;
                _diagnostics = compileResult.Diagnostics.Concat(diagnostics).ToImmutableArray();
            }
        }

        public bool Success => _success;

        public ImmutableArray<Diagnostic> Diagnostics => _diagnostics;

        public static implicit operator bool(BuildResult buildResult)
        {
            return buildResult?.ToBoolean() ?? false;
        }

        public bool ToBoolean()
        {
            return _success;
        }
    }
}