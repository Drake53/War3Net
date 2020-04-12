// ------------------------------------------------------------------------------
// <copyright file="DiagnosticDescriptorFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using Microsoft.CodeAnalysis;

namespace War3Net.Build.Diagnostics
{
    internal static class DiagnosticDescriptorFactory
    {
        internal static DiagnosticDescriptor Create(LocalizableString title, LocalizableString messageFormat, int id, DiagnosticCategory category, DiagnosticSeverity defaultSeverity, params string[] customTags)
        {
            if (!Enum.IsDefined(typeof(DiagnosticCategory), category))
            {
                throw new ArgumentOutOfRangeException(nameof(category));
            }

            if (id < 0 || id >= 1000)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            return new DiagnosticDescriptor($"W3N{(int)category}{id: D3}", title, messageFormat, category.ToString(), defaultSeverity, true, null, null, customTags);
        }

        internal static DiagnosticDescriptor Create(string title, string messageFormat, int id, DiagnosticCategory category, DiagnosticSeverity defaultSeverity, params string[] customTags)
        {
            if (!Enum.IsDefined(typeof(DiagnosticCategory), category))
            {
                throw new ArgumentOutOfRangeException(nameof(category));
            }

            if (id < 0 || id >= 1000)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            return new DiagnosticDescriptor($"W3N{(int)category}{id: D3}", title, messageFormat, category.ToString(), defaultSeverity, true, null, null, customTags);
        }

        internal static DiagnosticDescriptor Info(LocalizableString title, LocalizableString messageFormat, int id, DiagnosticCategory category, params string[] customTags)
        {
            return Create(title, messageFormat, id, category, DiagnosticSeverity.Info, customTags);
        }

        internal static DiagnosticDescriptor Info(string title, string messageFormat, int id, DiagnosticCategory category, params string[] customTags)
        {
            return Create(title, messageFormat, id, category, DiagnosticSeverity.Info, customTags);
        }

        internal static DiagnosticDescriptor Warning(LocalizableString title, LocalizableString messageFormat, int id, DiagnosticCategory category, params string[] customTags)
        {
            return Create(title, messageFormat, id, category, DiagnosticSeverity.Warning, customTags);
        }

        internal static DiagnosticDescriptor Warning(string title, string messageFormat, int id, DiagnosticCategory category, params string[] customTags)
        {
            return Create(title, messageFormat, id, category, DiagnosticSeverity.Warning, customTags);
        }

        internal static DiagnosticDescriptor Error(LocalizableString title, LocalizableString messageFormat, int id, DiagnosticCategory category, params string[] customTags)
        {
            return Create(title, messageFormat, id, category, DiagnosticSeverity.Error, customTags);
        }

        internal static DiagnosticDescriptor Error(string title, string messageFormat, int id, DiagnosticCategory category, params string[] customTags)
        {
            return Create(title, messageFormat, id, category, DiagnosticSeverity.Error, customTags);
        }
    }
}