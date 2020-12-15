# War3Net.Build.Core Changelog

## v1.5.3
### Changes
- Add UseNewFormat property to MapCustomTextTriggers.

## v1.5.2
### Changes
- Support parsing and serializing .wct files.

## v1.5.1
### Changes
- Add SoundFlags.UNK16 flag.
### Bugfixes
- Fix MapTriggers serializer for old format could serialize TriggerItemType.RootCategory items.

## v1.5.0
### Changes
- Support parsing and serializing .wtg files.
- Support additional format versions of .w3i files.
- Update target framework from .NET Standard to .NET Core.
- Include 1.32.9 in GamePatch enum.

## v1.4.0
### Changes
- Added GetTerrainTypes and GetCliffTypes methods to MapEnvironment.
- Added GetGamePatch method to GamePatchVersionProvider.
- Updated War3Net.IO.Mpq and CSharpLua packages.
### Bugfixes
- Fix parse error in war3mapUnits.doo when random data mode is -1.
### Breaking changes
- FileProvider class has been moved to War3Net.IO.Mpq namespace.
- Renamed GamePatchVersionProvider.GetPatchVersion to GetGameVersion.

## v1.3.10
### Changes
- Add event OnArchiveBuilding to MapBuilder.

## v1.3.9
### Bugfixes
- Fix lua global declarations.

## v1.3.8
### Bugfixes
- Fix string interpolation uses incorrect format string.

## v1.3.7
### Changes
- Preplaced units are now assigned to a global variable.
- Include 1.32.8 in GamePatch enum.

## v1.3.6 - Initial Version
