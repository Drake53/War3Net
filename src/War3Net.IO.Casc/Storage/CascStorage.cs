// ------------------------------------------------------------------------------
// <copyright file="CascStorage.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

using War3Net.IO.Casc.Compression;
using War3Net.IO.Casc.Encoding;
using War3Net.IO.Casc.Enums;
using War3Net.IO.Casc.Index;
using War3Net.IO.Casc.Root;
using War3Net.IO.Casc.Structures;
using War3Net.IO.Casc.Utilities;

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
        /// <exception cref="ArgumentException">Thrown when the open type is invalid.</exception>
        /// <exception cref="CascFileNotFoundException">Thrown when the file is not found.</exception>
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
                    var fileId = uint.Parse(fileName, CultureInfo.InvariantCulture);
                    return OpenFileByFileId(fileId);

                default:
                    throw new ArgumentException($"Invalid open type: {openType}");
            }
        }

        /// <summary>
        /// Opens a file by content key.
        /// </summary>
        /// <param name="cKey">The content key.</param>
        /// <returns>A stream containing the file data.</returns>
        /// <exception cref="CascException">Thrown when the encoding file is not loaded or checksum validation fails.</exception>
        /// <exception cref="CascFileNotFoundException">Thrown when the file is not found.</exception>
        public Stream OpenFileByCKey(CascKey cKey)
        {
            return OpenFileByCKey(cKey, true);
        }

        /// <summary>
        /// Opens a file by content key with optional checksum validation.
        /// </summary>
        /// <param name="cKey">The content key.</param>
        /// <param name="validateChecksum">Whether to validate the checksum.</param>
        /// <returns>A stream containing the file data.</returns>
        /// <exception cref="CascException">Thrown when the encoding file is not loaded or checksum validation fails.</exception>
        /// <exception cref="CascFileNotFoundException">Thrown when the file is not found.</exception>
        public Stream OpenFileByCKey(CascKey cKey, bool validateChecksum)
        {
            // Get EKey from encoding
            if (_context.EncodingFile == null)
            {
                throw new CascException("Encoding file not loaded");
            }

            var eKey = _context.EncodingFile.GetEKey(cKey);
            if (eKey == null)
            {
                throw new CascFileNotFoundException(cKey);
            }

            var stream = OpenFileByEKey(eKey.Value);

            // Validate checksum if requested
            if (validateChecksum && stream.CanSeek)
            {
                var originalPosition = stream.Position;
                var computedHash = ChecksumValidator.ComputeMD5(StreamToArray(stream));
                stream.Position = originalPosition;

                if (!ChecksumValidator.CompareHashes(computedHash, cKey.ToArray()))
                {
                    stream.Dispose();
                    throw new CascException($"Checksum validation failed for CKey: {cKey}");
                }
            }

            return stream;
        }

        /// <summary>
        /// Converts stream to byte array.
        /// </summary>
        private static byte[] StreamToArray(Stream stream)
        {
            if (stream is MemoryStream ms)
            {
                return ms.ToArray();
            }

            using var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }

        /// <summary>
        /// Opens a file by encoded key.
        /// </summary>
        /// <param name="eKey">The encoded key.</param>
        /// <returns>A stream containing the file data.</returns>
        /// <exception cref="CascFileNotFoundException">Thrown when the file is not found.</exception>
        public Stream OpenFileByEKey(EKey eKey)
        {
            return OpenFileByEKey(eKey, false);
        }

        /// <summary>
        /// Opens a file by encoded key with streaming option.
        /// </summary>
        /// <param name="eKey">The encoded key.</param>
        /// <param name="useStreaming">If true, returns a streaming reader; if false, loads entire file to memory.</param>
        /// <returns>A stream containing the file data.</returns>
        /// <exception cref="CascFileNotFoundException">Thrown when the file is not found.</exception>
        public Stream OpenFileByEKey(EKey eKey, bool useStreaming)
        {
            _storageLock.EnterReadLock();
            try
            {
                if (_context.IndexManager == null)
                {
                    throw new CascException("Index files not loaded");
                }

                // Find entry in index
                if (!_context.IndexManager.TryFindEntry(eKey, out var indexEntry))
                {
                    throw new CascFileNotFoundException(eKey);
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
                    if (BlteDecoder.IsBlte(data))
                    {
                        data = BlteDecoder.Decode(data);
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
            // Use centralized path sanitization
            fileName = PathSanitizer.SanitizeFilePath(fileName);

            if (_context.RootHandler == null)
            {
                throw new CascException("Root handler not loaded. Cannot resolve file names.");
            }

            var rootEntry = _context.RootHandler.GetEntry(fileName);
            if (rootEntry == null)
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

            var rootEntry = _context.RootHandler.GetEntry(fileDataId);
            if (rootEntry == null)
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

            _storageLock.EnterWriteLock();
            try
            {
                _context.EncryptionKeys[keyName] = key;
            }
            finally
            {
                _storageLock.ExitWriteLock();
            }
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
            keyString = keyString.Replace(" ", string.Empty, StringComparison.Ordinal).Replace("-", string.Empty, StringComparison.Ordinal);

            if (keyString.Length != CascConstants.KeyLength * 2)
            {
                throw new ArgumentException($"Key string must be {CascConstants.KeyLength * 2} hex characters.", nameof(keyString));
            }

            var key = new byte[CascConstants.KeyLength];
            for (var i = 0; i < key.Length; i++)
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

            var keysImported = 0;
            var lines = keyList.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();
                if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith("#", StringComparison.Ordinal) || trimmedLine.StartsWith("//", StringComparison.Ordinal))
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
            // Note: ImportKeysFromFile should accept absolute paths to allow loading key files from any location
            // The path sanitization is intentionally not used here to allow flexibility
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));
            }

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
        /// Finalizes an instance of the <see cref="CascStorage"/> class.
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
            // CascLib uses 100MB as default maximum, with configurable override
            const uint DefaultMaxFileSize = 100 * 1024 * 1024; // 100 MB
            const uint AbsoluteMaxFileSize = 2147483648; // 2 GB (max array size in .NET)

            if (indexEntry.EncodedSize == 0)
            {
                throw new CascException("Invalid file size: 0 bytes");
            }

            if (indexEntry.EncodedSize >= AbsoluteMaxFileSize)
            {
                throw new CascException($"File size {indexEntry.EncodedSize} exceeds absolute maximum of {AbsoluteMaxFileSize} bytes");
            }

            // Check for potential decompression bombs (files that expand significantly)
            // Encoded size should be reasonable compared to expected decompressed size
            if (indexEntry.EncodedSize > DefaultMaxFileSize)
            {
                // Log warning but allow - some legitimate game files can be large
                System.Diagnostics.Trace.TraceWarning($"Large file size: {indexEntry.EncodedSize} bytes. Proceeding with caution.");
            }

            var dataFilePath = IndexManager.GetDataFilePath(indexEntry, _context.DataPath!);

            if (!File.Exists(dataFilePath))
            {
                throw new FileNotFoundException($"Data file not found: {dataFilePath}");
            }

            using var stream = File.OpenRead(dataFilePath);

            // Check for integer overflow when calculating end position
            // Use checked arithmetic to catch overflows
            checked
            {
                try
                {
                    // Ensure offset is valid
                    if (indexEntry.DataFileOffset > (ulong)stream.Length)
                    {
                        throw new CascException($"Data file offset {indexEntry.DataFileOffset} exceeds file size {stream.Length}");
                    }

                    // Calculate end position with overflow check
                    var endPosition = indexEntry.DataFileOffset + indexEntry.EncodedSize;

                    // Validate that the file contains the requested data
                    if (endPosition > (ulong)stream.Length)
                    {
                        throw new CascException($"Data file {dataFilePath} is too small to contain the requested data at offset {indexEntry.DataFileOffset} with size {indexEntry.EncodedSize}");
                    }

                    // Ensure we can seek to the position (long.MaxValue limit)
                    if (indexEntry.DataFileOffset > (ulong)long.MaxValue)
                    {
                        throw new CascException($"Data file offset {indexEntry.DataFileOffset} exceeds maximum seekable value");
                    }
                }
                catch (OverflowException)
                {
                    throw new CascException($"Integer overflow detected when calculating file positions for offset {indexEntry.DataFileOffset} and size {indexEntry.EncodedSize}");
                }
            }

            stream.Seek((long)indexEntry.DataFileOffset, SeekOrigin.Begin);

            // Use ArrayPool for large allocations to reduce GC pressure
            byte[]? rentedArray = null;
            byte[] data;

            try
            {
                if (indexEntry.EncodedSize > 81920) // 80KB threshold for ArrayPool
                {
                    rentedArray = System.Buffers.ArrayPool<byte>.Shared.Rent((int)indexEntry.EncodedSize);
                    data = rentedArray;
                }
                else
                {
                    data = new byte[indexEntry.EncodedSize];
                }

                var totalBytesRead = 0;
                var remaining = (int)indexEntry.EncodedSize;

                // Read in chunks to handle large files better
                while (remaining > 0)
                {
                    var bytesRead = stream.Read(data, totalBytesRead, remaining);
                    if (bytesRead == 0)
                    {
                        throw new CascException($"Unexpected end of stream reading {dataFilePath}: expected {indexEntry.EncodedSize} bytes, read {totalBytesRead} bytes");
                    }

                    totalBytesRead += bytesRead;
                    remaining -= bytesRead;
                }

                // If we used a rented array, copy to exact-sized array before returning
                if (rentedArray != null)
                {
                    var result = new byte[indexEntry.EncodedSize];
                    Buffer.BlockCopy(rentedArray, 0, result, 0, (int)indexEntry.EncodedSize);
                    return result;
                }

                return data;
            }
            finally
            {
                // Always return the rented array if it exists
                if (rentedArray != null)
                {
                    System.Buffers.ArrayPool<byte>.Shared.Return(rentedArray, clearArray: true);
                }
            }
        }
    }
}