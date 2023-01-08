﻿// ------------------------------------------------------------------------------
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
        public bool TryDecompileMapImportedFiles(MpqArchive mpqArchive, ImportedFilesFormatVersion formatVersion, [NotNullWhen(true)] out ImportedFiles? mapImportedFiles)
        {
            if (mpqArchive is null)
            {
                throw new ArgumentNullException(nameof(mpqArchive));
            }

            mapImportedFiles = new ImportedFiles(formatVersion);

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
                UserData.FileName,
                "conversation.json",
                MapSounds.FileName,
                MapCameras.FileName,
                MapEnvironment.FileName,
                MapPathingMap.FileName,
                MapPreviewIcons.FileName,
                MapRegions.FileName,
                MapShadowMap.FileName,
                ImportedFiles.MapFileName,
                MapInfo.FileName,
                AbilityObjectData.MapFileName,
                BuffObjectData.MapFileName,
                DestructableObjectData.MapFileName,
                DoodadObjectData.MapFileName,
                ItemObjectData.MapFileName,
                UnitObjectData.MapFileName,
                UpgradeObjectData.MapFileName,
                AbilityObjectData.MapSkinFileName,
                BuffObjectData.MapSkinFileName,
                DestructableObjectData.MapSkinFileName,
                DoodadObjectData.MapSkinFileName,
                ItemObjectData.MapSkinFileName,
                UnitObjectData.MapSkinFileName,
                UpgradeObjectData.MapSkinFileName,
                MapCustomTextTriggers.FileName,
                MapTriggers.FileName,
                TriggerStrings.MapFileName,
                MapDoodads.FileName,
                MapUnits.FileName,
                JassMapScript.FileName,
                JassMapScript.FullName,
                LuaMapScript.FileName,
                LuaMapScript.FullName,
            }
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        }
    }
}