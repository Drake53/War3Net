# War3Net.Build Changelog

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
