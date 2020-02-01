# War3Net.Build Changelog

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