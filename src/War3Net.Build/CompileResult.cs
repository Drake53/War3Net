// ------------------------------------------------------------------------------
// <copyright file="CompileResult.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Immutable;

using Microsoft.CodeAnalysis;

namespace War3Net.Build
{
    public sealed class CompileResult
    {
        private readonly bool _success;
        private readonly ImmutableArray<Diagnostic> _diagnostics;

        internal CompileResult(bool success, params Diagnostic[] diagnostics)
        {
            _success = success;
            _diagnostics = ImmutableArray.Create(diagnostics);
        }

        public bool Success => _success;

        public ImmutableArray<Diagnostic> Diagnostics => _diagnostics;

        public static implicit operator bool(CompileResult compileResult)
        {
            return compileResult?.ToBoolean() ?? false;
        }

        public bool ToBoolean()
        {
            return _success;
        }
    }
}