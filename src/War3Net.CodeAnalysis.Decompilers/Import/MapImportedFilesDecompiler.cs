// ------------------------------------------------------------------------------
// <copyright file="MapImportedFilesDecompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using War3Net.Build.Audio;
using War3Net.Build.Environment;
using War3Net.Build.Import;
using War3Net.Build.Info;
using War3Net.Build.Object;
using War3Net.Build.Script;
using War3Net.Build.Widget;
using War3Net.IO.Mpq;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        public bool TryDecompileMapImportedFiles(MpqArchive mpqArchive, ImportedFilesFormatVersion formatVersion, [NotNullWhen(true)] out MapImportedFiles? mapImportedFiles)
        {
            if (mpqArchive is null)
            {
                throw new ArgumentNullException(nameof(mpqArchive));
            }

            mapImportedFiles = new MapImportedFiles(formatVersion);

            var ignoreList = GetIgnoreList();

            mpqArchive.AddFileNames(Context.ImportedFileNames);
            foreach (var mpqFile in mpqArchive.GetMpqFiles())
            {
                if (mpqFile is MpqKnownFile mpqKnownFile && !ignoreList.Contains(mpqKnownFile.FileName))
                {
                    mapImportedFiles.Files.Add(new ImportedFile
                    {
                        Flags = ImportedFileFlags.UNK1 | ImportedFileFlags.UNK4 | ImportedFileFlags.UNK8,
                        FullPath = mpqKnownFile.FileName,
                    });
                }
            }

            return true;
        }

        private static HashSet<string> GetIgnoreList()
        {
            return new[]
            {
                Attributes.FileName,
                ListFile.FileName,
                Signature.FileName,
                "(user data)",
                "conversation.json",
                MapSounds.FileName,
                MapCameras.FileName,
                MapEnvironment.FileName,
                MapPathingMap.FileName,
                MapPreviewIcons.FileName,
                MapRegions.FileName,
                MapShadowMap.FileName,
                MapImportedFiles.FileName,
                MapInfo.FileName,
                MapAbilityObjectData.FileName,
                MapBuffObjectData.FileName,
                MapDestructableObjectData.FileName,
                MapDoodadObjectData.FileName,
                MapItemObjectData.FileName,
                MapUnitObjectData.FileName,
                MapUpgradeObjectData.FileName,
                MapCustomTextTriggers.FileName,
                MapTriggers.FileName,
                MapTriggerStrings.FileName,
                MapDoodads.FileName,
                MapUnits.FileName,
                "war3map.j",
                "war3map.lua",
                @"scripts\war3map.j",
                @"scripts\war3map.lua",
            }
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        }
    }
}