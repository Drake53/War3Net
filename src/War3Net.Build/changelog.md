# War3Net.Build Changelog

## v1.3.7
### Changes
- Preplaced units are now assigned to a global variable.
- Include 1.32.8 in GamePatch enum.

## v1.3.6
### Changes
- Created new project 'War3Net.Build.Core', and moved useful files there to reduce dependencies.
- Include 1.32.6 and 1.32.7 in GamePatch enum.

## v1.3.5
### Changes
- Can now parse and serialize war3map.w3s file format version 3. Meaning of the added data not yet known, nor stored in the MapSounds object.
- Object data file parsers no longer validate the object modifications.
- Updated War3Net.Common and War3Net.IO.Mpq packages.
### Bugfixes
- Changed/added some exception messages.

## v1.3.4
### Changes
- Added DecompilePackageLibs to ScriptCompilerOptions.
- Updated War3Net.CodeAnalysis.Jass, CSharpLua, and War3Api packages.

## v1.3.3
### Changes
- Include latest Reforged patches in GamePatch enum.
### Bugfixes
- Added default value and property for ObjectData format version, so its value is not stuck at 0, which is invalid.

## v1.3.2
### Changes
- Added setter indexer to ObjectDataModification.
- Added property MapObjectData to ScriptCompilerOptions.

## v1.3.1
### Changes
- Update CSharpLua and MPQ packages.
### Bugfixes
- Can now run MapBuilder's Build method multiple times, without needing to restart the application.

## v1.3.0
### Changes
- Support .w3u, .w3t, .w3b, .w3d, .w3a, .w3h, .w3q, .w3o, and .w3f files.
- Added ObjectData and TargetPatch properties to ScriptCompilerOptions. Not setting the TargetPatch will generate a new warning diagnostic.
- FileProvider can now search recursively in MPQ archives (useful for campaigns). Also added FileExists method.
- CreateAllDestructables can now generate the dead and withZ variants of CreateDestructable.
- Added property HasSkin to unit and doodad data.
- Include latest Reforged patches in GamePatch enum.
- Added SetGameVersion method to MapInfo.

## v1.2.0
### Changes
- Support war3map.mmp files.
- Support water tinting color.

## v1.1.4
### Changes
- Added setters for Sound properties.
- Added get/set methods in MapSounds to handle sound collection.
- Update CSharpLua to v1.5.10, and War3Api to v1.32.2

## v1.1.3
### Bugfixes
- Fix MapSounds reforged file format was not parsed correctly.

## v1.1.2
### Bugfixes
- Fix MapRegions containing regions without sound generates invalid syntax.

## v1.1.1
### Bugfixes
- Replace Regex.Escape, which escapes too many characters (eg '.').
- Update CSharpLua to v1.5.9, which has some more bug fixes.

## v1.1.0
### Changes
- Default FormatVersion for MapInfo set back to v1.31 format.
- MapInfo property setters will now check if the property is available for the current FormatVersion.
- Added FormatVersion enum and property for all map files.
- Map script generator will set unit/item/destructable skin if it's different from its type ID.
- Added GamePatch enum.
### Bugfixes
- MapDoodads is now parsed correctly, similar to MapUnits.
### Breaking changes
- MapSounds constructor now takes MapSoundsFormatVersion instead of uint.

## v1.0.2
### Changes
- Added reforged sound channels (not tested).
- Added setters for most MapUnits properties, and added the Skin property.
### Breaking changes
- Subversion type for .doo headers changed from uint to MapWidgetsSubVersion enum.

## v1.0.1
### Changes
- Can now parse and serialize war3map.w3s file format version 2. Meaning of the added data not yet known, nor stored in the MapSounds object.
- Added overload for PlayerData.Create method, making it easier to create a copy of an existing playerData object.

## v1.0.0
### Changes
- Updated War3Api and MapInfo.Default for reforged.
### Bugfixes
- Unit and doodad rotation data from .doo files is now correctly converted from radians to degrees.
- MapUnits and MapDoodads IEnumerable constructor now sets the .doo header to its default value.
- Sounds paths in war3map.w3s files are now correctly escaped.
### Breaking changes
- Strings from MapInfo.MapName, MapInfo.MapDescription, and Options.LobbyMusic are now automatically escaped in the map script.
	- This change introduced a bug, that has been fixed in v1.1.1
