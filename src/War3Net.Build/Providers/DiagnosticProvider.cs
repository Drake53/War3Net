// ------------------------------------------------------------------------------
// <copyright file="DiagnosticProvider.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using Microsoft.CodeAnalysis;

namespace War3Net.Build.Providers
{
    [Obsolete]
    internal static class DiagnosticProvider
    {
        // todo: make Campaign variants for Map-related diagnostics

        // Map file diagnostics
        internal static readonly DiagnosticDescriptor MissingMapFile = CreateMissingMapFileDescriptor("W3N1001");
        internal static readonly DiagnosticDescriptor InvalidMapFile = CreateInvalidMapFileDescriptor("W3N1002");
        internal static readonly DiagnosticDescriptor GenericMapFileError = CreateGenericMapFileErrorDescriptor("W3N1003");

        // Asset file diagnostics
        internal static readonly DiagnosticDescriptor MissingFileWithCustomMpqFlags = CreateMissingFileWithCustomMpqFlagsDescriptor("W3N1020");
        internal static readonly DiagnosticDescriptor MissingFileNeutralLocale = CreateMissingFileNeutralLocaleDescriptor("W3N1021");

        // Script (C#) diagnostics
        internal static readonly DiagnosticDescriptor MissingSourceDirectory = CreateMissingSourceDirectoryDescriptor("W3N2001");

        // Script (JASS) diagnostics
        internal static readonly DiagnosticDescriptor MissingPathJasshelper = CreateMissingPathDescriptor("W3N2101", "clijasshelper.exe");
        internal static readonly DiagnosticDescriptor MissingPathCommonJ = CreateMissingPathDescriptor("W3N2102", "common.j");
        internal static readonly DiagnosticDescriptor MissingPathBlizzardJ = CreateMissingPathDescriptor("W3N2103", "Blizzard.j");

        private static DiagnosticDescriptor CreateMissingMapFileDescriptor(string id)
        {
            return new DiagnosticDescriptor(
                id,
                $"Map should contain a '{{0}}' file.",
                $"The {{1}} file '{{0}}' was not found. Make sure that it gets added when building the map.",
                "Usage",
                DiagnosticSeverity.Error,
                true,
                null,
                null,
                WellKnownDiagnosticTags.Build);
        }

        private static DiagnosticDescriptor CreateInvalidMapFileDescriptor(string id)
        {
            return new DiagnosticDescriptor(
                id,
                $"File '{{0}}' is invalid.",
                $"Encountered invalid data when parsing the '{{0}}' file: {{1}}",
                string.Empty,
                DiagnosticSeverity.Error,
                true,
                null,
                null,
                WellKnownDiagnosticTags.Build);
        }

        private static DiagnosticDescriptor CreateGenericMapFileErrorDescriptor(string id)
        {
            return new DiagnosticDescriptor(
                id,
                $"File '{{0}}' could not be read.",
                $"An exception occured when parsing the '{{0}}' file: {{1}}",
                string.Empty,
                DiagnosticSeverity.Error,
                true,
                null,
                null,
                WellKnownDiagnosticTags.Build);
        }

        private static DiagnosticDescriptor CreateMissingFileWithCustomMpqFlagsDescriptor(string id)
        {
            return new DiagnosticDescriptor(
                id,
                $"Filenames with custom mpq file flags should be included.",
                $"The file '{{0}}' was not found, but its mpq file flags were set to: '{{1}}'.",
                "Usage",
                DiagnosticSeverity.Warning,
                true,
                null,
                null,
                WellKnownDiagnosticTags.Build);
        }

        private static DiagnosticDescriptor CreateMissingFileNeutralLocaleDescriptor(string id)
        {
            return new DiagnosticDescriptor(
                id,
                $"Files should have a locale-agnostic version.",
                $"The file '{{0}}' was added in one or more locales, but does not have a version for the neutral locale.",
                "Usage",
                DiagnosticSeverity.Warning,
                true,
                null,
                null,
                WellKnownDiagnosticTags.Build);
        }

        private static DiagnosticDescriptor CreateMissingSourceDirectoryDescriptor(string id)
        {
            return new DiagnosticDescriptor(
                id,
                $"Path to source code directory should be set.",
                $"The source code directory was not found at '{{0}}'. The directory does not exist.",
                "Usage",
                DiagnosticSeverity.Error,
                true,
                null,
                null,
                WellKnownDiagnosticTags.Compiler);
        }

        private static DiagnosticDescriptor CreateMissingPathDescriptor(string id, string fileName)
        {
            return new DiagnosticDescriptor(
                id,
                $"Path to '{fileName}' should be set.",
                $"The file '{fileName}' was not found at '{{0}}'. This file is required to compile vJass source code.",
                "Usage",
                DiagnosticSeverity.Error,
                true,
                null,
                null,
                WellKnownDiagnosticTags.Compiler);
        }
    }
}