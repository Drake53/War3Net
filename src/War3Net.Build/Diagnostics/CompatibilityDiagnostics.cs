// ------------------------------------------------------------------------------
// <copyright file="CompatibilityDiagnostics.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.CodeAnalysis;

namespace War3Net.Build.Diagnostics
{
    internal static class CompatibilityDiagnostics
    {
        internal const DiagnosticCategory Category = DiagnosticCategory.Compatibility;

        internal static readonly DiagnosticDescriptor TargetPatchNotSet = DiagnosticDescriptorFactory.Warning(
            "Target patch should be specified.",
            "The target patch has not been specified. Compatibility diagnostics will be disabled.",
            0,
            Category,
            WellKnownDiagnosticTags.Build);
    }
}