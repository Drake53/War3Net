// ------------------------------------------------------------------------------
// <copyright file="MapBuilder.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Text;

using War3Net.Build.Extensions;
using War3Net.IO.Mpq;

namespace War3Net.Build
{
    public class MapBuilder
    {
        private readonly Map _map;
        private readonly Encoding? _encoding;
        private readonly List<MpqFile> _files;

        public MapBuilder(Map map)
        {
            _map = map;
            _encoding = null;
            _files = new List<MpqFile>();
        }

        public Map Map => _map;

        public void AddFiles(MpqArchive archive)
        {
            archive.DiscoverFileNames();
            AddFiles(archive.GetMpqFiles());
        }

        public void AddFiles(params MpqFile[] files)
        {
            AddFiles((IEnumerable<MpqFile>)files);
        }

        public void AddFiles(IEnumerable<MpqFile> files)
        {
            foreach (var file in files)
            {
                if (file is not MpqKnownFile knownFile || !_map.SetFile(knownFile.FileName, false, knownFile.MpqStream))
                {
                    _files.Add(file);
                }
            }
        }

        public void AddFiles(string directory)
        {
            AddFiles(directory, Directory.EnumerateFiles(directory));
        }

        public void AddFiles(string directory, string searchPattern)
        {
            AddFiles(directory, Directory.EnumerateFiles(directory, searchPattern));
        }

        public void AddFiles(string directory, string searchPattern, EnumerationOptions enumerationOptions)
        {
            AddFiles(directory, Directory.EnumerateFiles(directory, searchPattern, enumerationOptions));
        }

        public void AddFiles(string directory, string searchPattern, SearchOption searchOption)
        {
            AddFiles(directory, Directory.EnumerateFiles(directory, searchPattern, searchOption));
        }

        public void Build(string path)
        {
            CreateArchiveBuilder().SaveWithPreArchiveData(path);
        }

        public void Build(string path, MpqArchiveCreateOptions createOptions)
        {
            CreateArchiveBuilder().SaveWithPreArchiveData(path, createOptions);
        }

        public void Build(Stream stream, bool leaveOpen = false)
        {
            CreateArchiveBuilder().SaveWithPreArchiveData(stream, leaveOpen);
        }

        public void Build(Stream stream, MpqArchiveCreateOptions createOptions, bool leaveOpen = false)
        {
            CreateArchiveBuilder().SaveWithPreArchiveData(stream, createOptions, leaveOpen);
        }

        protected virtual MpqArchiveBuilder CreateArchiveBuilder()
        {
            var archiveBuilder = new MpqArchiveBuilder();
            void AddFilesToArchiveBuilder(params MpqFile?[] files)
            {
                foreach (var file in files)
                {
                    if (file is not null)
                    {
                        archiveBuilder!.AddFile(file);
                    }
                }
            }

            AddFilesToArchiveBuilder(
                _map.GetDoodadsFile(_encoding),
                _map.GetScriptFile(_encoding),
                _map.GetPreviewIconsFile(_encoding),
                _map.GetShadowMapFile(_encoding),
                _map.GetAbilityObjectDataFile(_encoding),
                _map.GetDestructableObjectDataFile(_encoding),
                _map.GetCamerasFile(_encoding),
                _map.GetDoodadObjectDataFile(_encoding),
                _map.GetEnvironmentFile(_encoding),
                _map.GetBuffObjectDataFile(_encoding),
                _map.GetInfoFile(_encoding),
                _map.GetUpgradeObjectDataFile(_encoding),
                _map.GetRegionsFile(_encoding),
                _map.GetSoundsFile(_encoding),
                _map.GetItemObjectDataFile(_encoding),
                _map.GetUnitObjectDataFile(_encoding),
                _map.GetCustomTextTriggersFile(_encoding),
                _map.GetPathingMapFile(_encoding),
                _map.GetTriggersFile(_encoding),
                _map.GetTriggerStringsFile(_encoding),
                _map.GetUnitsFile(_encoding),
                _map.GetImportedFilesFile(_encoding));

            AddFilesToArchiveBuilder(_files.ToArray());

            return archiveBuilder;
        }

        private void AddFiles(string baseDirectory, IEnumerable<string> files)
        {
            var baseDirectoryLength = baseDirectory.Length;
            if (!baseDirectory.EndsWith('/') && !baseDirectory.EndsWith('\\'))
            {
                baseDirectoryLength++;
            }

            foreach (var file in files)
            {
                var fileName = file[baseDirectoryLength..];
                var fileStream = File.OpenRead(file);
                if (!_map.SetFile(fileName, false, fileStream))
                {
                    _files.Add(MpqFile.New(fileStream, fileName));
                }
            }
        }
    }
}