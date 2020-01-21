// ------------------------------------------------------------------------------
// <copyright file="DiagnosticProvider.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.CodeAnalysis;

using War3Net.Build.Audio;
using War3Net.Build.Environment;
using War3Net.Build.Info;
using War3Net.Build.Widget;

namespace War3Net.Build.Providers
{
    internal static class DiagnosticProvider
    {
        internal static readonly DiagnosticDescriptor MissingMapInfo = CreateMissingMapFileDescriptor("W3N1001", MapInfo.FileName, DiagnosticSeverity.Error);
        internal static readonly DiagnosticDescriptor MissingMapEnvironment = CreateMissingMapFileDescriptor("W3N1002", MapEnvironment.FileName, DiagnosticSeverity.Warning);
        internal static readonly DiagnosticDescriptor MissingMapDoodads = CreateMissingMapFileDescriptor("W3N1003", MapDoodads.FileName, DiagnosticSeverity.Info);
        internal static readonly DiagnosticDescriptor MissingMapUnits = CreateMissingMapFileDescriptor("W3N1004", MapUnits.FileName, DiagnosticSeverity.Info);
        internal static readonly DiagnosticDescriptor MissingMapRegions = CreateMissingMapFileDescriptor("W3N1005", MapRegions.FileName, DiagnosticSeverity.Info);
        internal static readonly DiagnosticDescriptor MissingMapSounds = CreateMissingMapFileDescriptor("W3N1006", MapSounds.FileName, DiagnosticSeverity.Info);

        internal static readonly DiagnosticDescriptor MissingFileWithCustomMpqFlags = CreateMissingFileWithCustomMpqFlagsDescriptor("W3N1020");
        internal static readonly DiagnosticDescriptor MissingFileNeutralLocale = CreateMissingFileNeutralLocaleDescriptor("W3N1021");

        internal static readonly DiagnosticDescriptor MissingSourceDirectory = CreateMissingSourceDirectoryDescriptor("W3N2001");

        internal static readonly DiagnosticDescriptor MissingPathJasshelper = CreateMissingPathDescriptor("W3N2101", "clijasshelper.exe");
        internal static readonly DiagnosticDescriptor MissingPathCommonJ = CreateMissingPathDescriptor("W3N2102", "common.j");
        internal static readonly DiagnosticDescriptor MissingPathBlizzardJ = CreateMissingPathDescriptor("W3N2103", "Blizzard.j");

        private static DiagnosticDescriptor CreateMissingMapFileDescriptor(string id, string fileName, DiagnosticSeverity severity)
        {
            return new DiagnosticDescriptor(
                id,
                $"Map should contain a '{fileName}' file.",
                $"The {(severity == DiagnosticSeverity.Info ? "optional" : "required")} file '{fileName}' was not found. Make sure that it gets added when building the map.",
                "Usage",
                severity,
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