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
using System.Threading;

using War3Net.IO.Casc.Compression;
using War3Net.IO.Casc.Encoding;
using War3Net.IO.Casc.Enums;
using War3Net.IO.Casc.Index;
using War3Net.IO.Casc.IO;
using War3Net.IO.Casc.Root;
using War3Net.IO.Casc.Structures;

namespace War3Net.IO.Casc.Storage
{
    /// <summary>
    /// Represents a CASC storage instance.
    /// </summary>
    public class CascStorage : IDisposable
    {
        private readonly CascStorageContext _context;
        private readonly ReaderWriterLockSlim _storageLock;
        private int _referenceCount = 1;
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
            _storageLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
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
        /// Gets the root handler.
        /// </summary>
        internal IRootHandler? RootHandler => _context.RootHandler;

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
            return OpenFileByEKey(ekey, false);
        }

        /// <summary>
        /// Opens a file by encoded key with streaming option.
        /// </summary>
        /// <param name="ekey">The encoded key.</param>
        /// <param name="useStreaming">If true, returns a streaming reader; if false, loads entire file to memory.</param>
        /// <returns>A stream containing the file data.</returns>
        public Stream OpenFileByEKey(EKey ekey, bool useStreaming)
        {
            _storageLock.EnterReadLock();
            try
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

                if (useStreaming)
                {
                    // Return a streaming reader
                    return new CascStream(_context, indexEntry!);
                }
                else
                {
                    // Read data from data file
                    var data = ReadDataFile(indexEntry!);

                    // Check if data is BLTE-encoded
                    if (BLTEDecoder.IsBLTE(data))
                    {
                        data = BLTEDecoder.Decode(data);
                    }

                    return new MemoryStream(data);
                }
            }
            finally
            {
                _storageLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Opens a file by name.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <returns>A stream containing the file data.</returns>
        public Stream OpenFileByName(string fileName)
        {
            if (_context.RootHandler == null)
            {
                throw new CascException("Root handler not loaded. Cannot resolve file names.");
            }

            if (!_context.RootHandler.TryGetEntry(fileName, out var rootEntry) || rootEntry == null)
            {
                throw new CascFileNotFoundException($"File not found in root: {fileName}");
            }

            // Try to open by CKey first
            if (!rootEntry.CKey.IsEmpty)
            {
                return OpenFileByCKey(rootEntry.CKey);
            }

            // Fall back to EKey if available
            if (!rootEntry.EKey.IsEmpty)
            {
                return OpenFileByEKey(rootEntry.EKey);
            }

            throw new CascException($"Root entry for '{fileName}' has no valid keys");
        }

        /// <summary>
        /// Opens a file by file data ID.
        /// </summary>
        /// <param name="fileDataId">The file data ID.</param>
        /// <returns>A stream containing the file data.</returns>
        public Stream OpenFileByFileId(uint fileDataId)
        {
            if (_context.RootHandler == null)
            {
                throw new CascException("Root handler not loaded. Cannot resolve file data IDs.");
            }

            if (!_context.RootHandler.TryGetEntry(fileDataId, out var rootEntry) || rootEntry == null)
            {
                throw new CascFileNotFoundException($"File data ID not found in root: {fileDataId}");
            }

            // Try to open by CKey first
            if (!rootEntry.CKey.IsEmpty)
            {
                return OpenFileByCKey(rootEntry.CKey);
            }

            // Fall back to EKey if available
            if (!rootEntry.EKey.IsEmpty)
            {
                return OpenFileByEKey(rootEntry.EKey);
            }

            throw new CascException($"Root entry for file data ID {fileDataId} has no valid keys");
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
        /// Adds an encryption key from a hex string.
        /// </summary>
        /// <param name="keyName">The key name.</param>
        /// <param name="keyString">The encryption key as a hex string.</param>
        public void AddStringEncryptionKey(ulong keyName, string keyString)
        {
            if (string.IsNullOrEmpty(keyString))
            {
                throw new ArgumentException("Key string cannot be null or empty.", nameof(keyString));
            }

            // Remove any spaces or dashes from the hex string
            keyString = keyString.Replace(" ", string.Empty).Replace("-", string.Empty);

            if (keyString.Length != CascConstants.KeyLength * 2)
            {
                throw new ArgumentException($"Key string must be {CascConstants.KeyLength * 2} hex characters.", nameof(keyString));
            }

            var key = new byte[CascConstants.KeyLength];
            for (int i = 0; i < key.Length; i++)
            {
                key[i] = Convert.ToByte(keyString.Substring(i * 2, 2), 16);
            }

            AddEncryptionKey(keyName, key);
        }

        /// <summary>
        /// Imports encryption keys from a string list.
        /// </summary>
        /// <param name="keyList">The key list in format "KeyName=KeyValue" separated by newlines.</param>
        /// <returns>The number of keys imported.</returns>
        public int ImportKeysFromString(string keyList)
        {
            if (string.IsNullOrEmpty(keyList))
            {
                return 0;
            }

            int keysImported = 0;
            var lines = keyList.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();
                if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith("#") || trimmedLine.StartsWith("//"))
                {
                    continue; // Skip comments and empty lines
                }

                var parts = trimmedLine.Split('=');
                if (parts.Length != 2)
                {
                    continue; // Skip invalid lines
                }

                var keyNameStr = parts[0].Trim();
                var keyValueStr = parts[1].Trim();

                // Parse key name (can be hex or decimal)
                ulong keyName;
                if (keyNameStr.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                {
                    if (!ulong.TryParse(keyNameStr.Substring(2), System.Globalization.NumberStyles.HexNumber, null, out keyName))
                    {
                        continue;
                    }
                }
                else
                {
                    if (!ulong.TryParse(keyNameStr, out keyName))
                    {
                        continue;
                    }
                }

                try
                {
                    AddStringEncryptionKey(keyName, keyValueStr);
                    keysImported++;
                }
                catch
                {
                    // Skip invalid keys
                }
            }

            return keysImported;
        }

        /// <summary>
        /// Imports encryption keys from a file.
        /// </summary>
        /// <param name="fileName">The path to the key file.</param>
        /// <returns>The number of keys imported.</returns>
        public int ImportKeysFromFile(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException($"Key file not found: {fileName}");
            }

            var keyList = File.ReadAllText(fileName);
            return ImportKeysFromString(keyList);
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

        /// <summary>
        /// Adds a reference to the storage.
        /// </summary>
        /// <returns>The storage instance.</returns>
        public CascStorage AddRef()
        {
            Interlocked.Increment(ref _referenceCount);
            return this;
        }

        /// <summary>
        /// Releases a reference to the storage.
        /// </summary>
        /// <returns>The storage instance if still referenced; otherwise, null.</returns>
        public CascStorage? Release()
        {
            if (Interlocked.Decrement(ref _referenceCount) == 0)
            {
                Dispose();
                return null;
            }
            return this;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        /// <summary>
        /// Releases unmanaged and optionally managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _storageLock.EnterWriteLock();
                try
                {
                    // Dispose managed resources
                    _context.IndexManager?.Clear();
                    _context.EncodingFile?.Clear();
                    _context.RootHandler = null;
                    _context.BuildInfo = null;
                    _context.ActiveBuild = null;
                    _context.Product = null;
                    _context.EncryptionKeys?.Clear();
                }
                finally
                {
                    _storageLock.ExitWriteLock();
                    _storageLock.Dispose();
                }
            }

            _disposed = true;
        }
        
        /// <summary>
        /// Finalizer.
        /// </summary>
        ~CascStorage()
        {
            Dispose(false);
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

            // Initialize root handler (basic implementation for now)
            _context.RootHandler = new BasicRootHandler();

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
            // Validate size to prevent excessive memory allocation
            const uint MaxReasonableFileSize = 512 * 1024 * 1024; // 512 MB
            if (indexEntry.EncodedSize > MaxReasonableFileSize)
            {
                throw new CascException($"File size {indexEntry.EncodedSize} exceeds maximum allowed size of {MaxReasonableFileSize} bytes");
            }

            // Additional validation for suspicious sizes
            if (indexEntry.EncodedSize == 0)
            {
                throw new CascException("Invalid file size: 0 bytes");
            }

            var dataFilePath = IndexManager.GetDataFilePath(indexEntry, _context.DataPath!);

            if (!File.Exists(dataFilePath))
            {
                throw new FileNotFoundException($"Data file not found: {dataFilePath}");
            }

            using var stream = File.OpenRead(dataFilePath);
            
            // Validate that the file is large enough to contain the requested data
            if (stream.Length < (long)(indexEntry.DataFileOffset + indexEntry.EncodedSize))
            {
                throw new CascException($"Data file {dataFilePath} is too small to contain the requested data at offset {indexEntry.DataFileOffset} with size {indexEntry.EncodedSize}");
            }

            // Check for integer overflow
            if (indexEntry.DataFileOffset > (ulong)long.MaxValue)
            {
                throw new CascException($"Data file offset {indexEntry.DataFileOffset} exceeds maximum supported value");
            }

            stream.Seek((long)indexEntry.DataFileOffset, SeekOrigin.Begin);

            // Use ArrayPool for large allocations to reduce GC pressure
            byte[] data;
            if (indexEntry.EncodedSize > 81920) // 80KB threshold for ArrayPool
            {
                // For large files, consider using streaming approach instead
                data = new byte[indexEntry.EncodedSize];
            }
            else
            {
                data = new byte[indexEntry.EncodedSize];
            }

            var bytesRead = stream.Read(data, 0, data.Length);

            if (bytesRead != data.Length)
            {
                throw new CascException($"Failed to read complete data from {dataFilePath}: expected {data.Length} bytes, read {bytesRead} bytes");
            }

            return data;
        }
    }
}