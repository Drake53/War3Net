// ------------------------------------------------------------------------------
// <copyright file="CascStorage.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using War3Net.IO.Casc.Compression;
using War3Net.IO.Casc.Encoding;
using War3Net.IO.Casc.Enums;
using War3Net.IO.Casc.Index;
using War3Net.IO.Casc.Structures;

namespace War3Net.IO.Casc.Storage
{
    /// <summary>
    /// Represents a CASC storage instance.
    /// </summary>
    public class CascStorage : IDisposable
    {
        private readonly CascStorageContext _context;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="CascStorage"/> class.
        /// </summary>
        /// <param name="storagePath">The path to the CASC storage.</param>
        /// <param name="localeFlags">The locale flags.</param>
        public CascStorage(string storagePath, CascLocaleFlags localeFlags = CascLocaleFlags.All)
        {
            _context = new CascStorageContext
            {
                StoragePath = storagePath,
                LocaleFlags = localeFlags,
                IndexManager = new IndexManager(),
            };
        }

        /// <summary>
        /// Gets the storage path.
        /// </summary>
        public string StoragePath => _context.StoragePath ?? string.Empty;

        /// <summary>
        /// Gets the storage features.
        /// </summary>
        public CascFeatures Features => _context.Features;

        /// <summary>
        /// Gets the product information.
        /// </summary>
        public CascStorageProduct? Product => _context.Product;

        /// <summary>
        /// Gets the locale flags.
        /// </summary>
        public CascLocaleFlags LocaleFlags => _context.LocaleFlags;

        /// <summary>
        /// Opens a local CASC storage.
        /// </summary>
        /// <param name="storagePath">The path to the storage.</param>
        /// <param name="localeFlags">The locale flags.</param>
        /// <returns>The opened storage.</returns>
        public static CascStorage OpenStorage(string storagePath, CascLocaleFlags localeFlags = CascLocaleFlags.All)
        {
            var storage = new CascStorage(storagePath, localeFlags);
            storage.Initialize();
            return storage;
        }

        /// <summary>
        /// Opens a file from the storage.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <param name="openFlags">The open flags.</param>
        /// <returns>A stream containing the file data.</returns>
        public Stream OpenFile(string fileName, CascOpenFlags openFlags = CascOpenFlags.OpenByName)
        {
            var openType = openFlags & CascOpenFlags.OpenTypeMask;

            switch (openType)
            {
                case CascOpenFlags.OpenByName:
                    return OpenFileByName(fileName);

                case CascOpenFlags.OpenByCKey:
                    return OpenFileByCKey(CascKey.Parse(fileName));

                case CascOpenFlags.OpenByEKey:
                    return OpenFileByEKey(EKey.Parse(fileName));

                case CascOpenFlags.OpenByFileId:
                    var fileId = uint.Parse(fileName);
                    return OpenFileByFileId(fileId);

                default:
                    throw new ArgumentException($"Invalid open type: {openType}");
            }
        }

        /// <summary>
        /// Opens a file by content key.
        /// </summary>
        /// <param name="ckey">The content key.</param>
        /// <returns>A stream containing the file data.</returns>
        public Stream OpenFileByCKey(CascKey ckey)
        {
            // Get EKey from encoding
            if (_context.EncodingFile == null)
            {
                throw new CascException("Encoding file not loaded");
            }

            var ekey = _context.EncodingFile.GetEKey(ckey);
            if (ekey == null)
            {
                throw new CascFileNotFoundException(ckey);
            }

            return OpenFileByEKey(ekey);
        }

        /// <summary>
        /// Opens a file by encoded key.
        /// </summary>
        /// <param name="ekey">The encoded key.</param>
        /// <returns>A stream containing the file data.</returns>
        public Stream OpenFileByEKey(EKey ekey)
        {
            if (_context.IndexManager == null)
            {
                throw new CascException("Index files not loaded");
            }

            // Find entry in index
            if (!_context.IndexManager.TryFindEntry(ekey, out var indexEntry))
            {
                throw new CascFileNotFoundException(ekey);
            }

            // Read data from data file
            var data = ReadDataFile(indexEntry!);

            // Check if data is BLTE-encoded
            if (BLTEDecoder.IsBLTE(data))
            {
                data = BLTEDecoder.Decode(data);
            }

            return new MemoryStream(data);
        }

        /// <summary>
        /// Opens a file by name.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <returns>A stream containing the file data.</returns>
        public Stream OpenFileByName(string fileName)
        {
            // TODO: Use root handler to resolve name to CKey/EKey
            throw new NotImplementedException("Opening files by name requires root handler implementation");
        }

        /// <summary>
        /// Opens a file by file data ID.
        /// </summary>
        /// <param name="fileDataId">The file data ID.</param>
        /// <returns>A stream containing the file data.</returns>
        public Stream OpenFileByFileId(uint fileDataId)
        {
            // TODO: Use root handler to resolve FileDataId to CKey/EKey
            throw new NotImplementedException("Opening files by FileDataId requires root handler implementation");
        }

        /// <summary>
        /// Adds an encryption key.
        /// </summary>
        /// <param name="keyName">The key name.</param>
        /// <param name="key">The encryption key.</param>
        public void AddEncryptionKey(ulong keyName, byte[] key)
        {
            if (key == null || key.Length != CascConstants.KeyLength)
            {
                throw new ArgumentException($"Encryption key must be {CascConstants.KeyLength} bytes", nameof(key));
            }

            _context.EncryptionKeys[keyName] = key;
        }

        /// <summary>
        /// Gets an encryption key.
        /// </summary>
        /// <param name="keyName">The key name.</param>
        /// <returns>The encryption key, or null if not found.</returns>
        public byte[]? GetEncryptionKey(ulong keyName)
        {
            return _context.EncryptionKeys.TryGetValue(keyName, out var key) ? key : null;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _context.IndexManager?.Clear();
            _context.EncodingFile?.Clear();

            _disposed = true;
        }

        private void Initialize()
        {
            // Set up paths
            _context.DataPath = Path.Combine(_context.StoragePath!, "data");
            _context.ConfigPath = Path.Combine(_context.StoragePath!, "config");

            // Load .build.info
            var buildInfoPath = Path.Combine(_context.StoragePath!, ".build.info");
            if (File.Exists(buildInfoPath))
            {
                _context.BuildInfo = BuildInfo.ParseFile(buildInfoPath);
                _context.ActiveBuild = _context.BuildInfo.GetActiveBuild();

                if (_context.ActiveBuild != null)
                {
                    _context.Product = new CascStorageProduct(
                        _context.ActiveBuild.Product ?? "unknown",
                        uint.TryParse(_context.ActiveBuild.BuildId, out var buildId) ? buildId : 0);
                }
            }

            // Load index files
            if (Directory.Exists(_context.DataPath))
            {
                _context.IndexManager!.LoadIndexFiles(_context.DataPath);
            }

            // Load encoding file
            LoadEncodingFile();

            // Set features based on what we found
            UpdateFeatures();
        }

        private void LoadEncodingFile()
        {
            if (_context.ActiveBuild == null || string.IsNullOrEmpty(_context.ActiveBuild.BuildKey))
            {
                return;
            }

            // Build key is the MD5 hash of the build config file
            // The encoding file EKey is stored in the build config
            // For now, we'll try to find the encoding file directly
            var configPath = _context.ConfigPath;
            if (Directory.Exists(configPath))
            {
                // Look for encoding files
                var encodingFiles = Directory.GetFiles(configPath, "*", SearchOption.AllDirectories)
                    .Where(f => Path.GetFileName(f).StartsWith("encoding", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (encodingFiles.Count > 0)
                {
                    try
                    {
                        _context.EncodingFile = EncodingFile.ParseFile(encodingFiles[0]);
                    }
                    catch
                    {
                        // Failed to load encoding file
                    }
                }
            }
        }

        private void UpdateFeatures()
        {
            _context.Features = CascFeatures.None;

            // Check for data archives
            if (Directory.Exists(_context.DataPath))
            {
                if (Directory.GetFiles(_context.DataPath, "data.*").Length > 0)
                {
                    _context.Features |= CascFeatures.DataArchives;
                }
            }

            // Check for encoding support
            if (_context.EncodingFile != null)
            {
                _context.Features |= CascFeatures.RootCKey;
            }
        }

        private byte[] ReadDataFile(EKeyEntry indexEntry)
        {
            var dataFilePath = IndexManager.GetDataFilePath(indexEntry, _context.DataPath!);

            if (!File.Exists(dataFilePath))
            {
                throw new FileNotFoundException($"Data file not found: {dataFilePath}");
            }

            using var stream = File.OpenRead(dataFilePath);
            stream.Seek((long)indexEntry.DataFileOffset, SeekOrigin.Begin);

            var data = new byte[indexEntry.EncodedSize];
            var bytesRead = stream.Read(data, 0, data.Length);

            if (bytesRead != data.Length)
            {
                throw new CascException($"Failed to read complete data from {dataFilePath}");
            }

            return data;
        }
    }
}