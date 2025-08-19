// ------------------------------------------------------------------------------
// <copyright file="CascArchive.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using War3Net.IO.Casc.Enums;
using War3Net.IO.Casc.Storage;
using War3Net.IO.Casc.Structures;

namespace War3Net.IO.Casc
{
    /// <summary>
    /// Represents a CASC (Content Addressable Storage Container) archive.
    /// </summary>
    public sealed class CascArchive : IDisposable, IEnumerable<CascEntry>
    {
        private readonly CascStorage _storage;
        private readonly ConcurrentDictionary<string, CascEntry> _entries;
        private readonly HashSet<WeakReference> _openStreams;
        private readonly object _streamLock = new object();
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="CascArchive"/> class.
        /// </summary>
        /// <param name="storagePath">The path to the CASC storage.</param>
        public CascArchive(string storagePath)
            : this(storagePath, CascLocaleFlags.All)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CascArchive"/> class.
        /// </summary>
        /// <param name="storagePath">The path to the CASC storage.</param>
        /// <param name="localeFlags">The locale flags to use.</param>
        public CascArchive(string storagePath, CascLocaleFlags localeFlags)
        {
            if (string.IsNullOrEmpty(storagePath))
            {
                throw new ArgumentException("Storage path cannot be null or empty.", nameof(storagePath));
            }

            if (!Directory.Exists(storagePath))
            {
                throw new DirectoryNotFoundException($"Storage path not found: {storagePath}");
            }

            _storage = CascStorage.OpenStorage(storagePath, localeFlags);
            _entries = new ConcurrentDictionary<string, CascEntry>(StringComparer.OrdinalIgnoreCase);
            _openStreams = new HashSet<WeakReference>();

            Initialize();
        }

        /// <summary>
        /// Gets the path to the CASC storage.
        /// </summary>
        public string StoragePath => _storage.StoragePath;

        /// <summary>
        /// Gets the features supported by this CASC storage.
        /// </summary>
        public CascFeatures Features => _storage.Features;

        /// <summary>
        /// Gets the locale flags used when opening the archive.
        /// </summary>
        public CascLocaleFlags LocaleFlags => _storage.LocaleFlags;

        /// <summary>
        /// Gets the product information for this storage.
        /// </summary>
        public CascStorageProduct? Product => _storage.Product;

        /// <summary>
        /// Gets the number of files in the archive.
        /// </summary>
        public int FileCount => _entries.Count;

        /// <summary>
        /// Opens a file from the CASC archive.
        /// </summary>
        /// <param name="fileName">The name of the file to open.</param>
        /// <returns>A stream containing the file data.</returns>
        public Stream OpenFile(string fileName)
        {
            return OpenFile(fileName, CascOpenFlags.OpenByName);
        }

        /// <summary>
        /// Opens a file from the CASC archive.
        /// </summary>
        /// <param name="fileName">The name of the file to open.</param>
        /// <param name="openFlags">Flags for opening the file.</param>
        /// <returns>A stream containing the file data.</returns>
        public Stream OpenFile(string fileName, CascOpenFlags openFlags)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));
            }

            if (!TryGetEntry(fileName, out var entry))
            {
                throw new FileNotFoundException($"File not found in CASC archive: {fileName}");
            }

            return OpenFileInternal(entry, openFlags);
        }

        /// <summary>
        /// Opens a file from the CASC archive using a content key.
        /// </summary>
        /// <param name="cKey">The content key of the file.</param>
        /// <returns>A stream containing the file data.</returns>
        public Stream OpenFile(CascKey cKey)
        {
            return OpenFile(cKey.ToString(), CascOpenFlags.OpenByCKey);
        }

        /// <summary>
        /// Opens a file from the CASC archive using an encoded key.
        /// </summary>
        /// <param name="eKey">The encoded key of the file.</param>
        /// <returns>A stream containing the file data.</returns>
        public Stream OpenFile(EKey eKey)
        {
            return OpenFile(eKey.ToString(), CascOpenFlags.OpenByEKey);
        }

        /// <summary>
        /// Opens a file from the CASC archive using a file data ID.
        /// </summary>
        /// <param name="fileDataId">The file data ID.</param>
        /// <returns>A stream containing the file data.</returns>
        public Stream OpenFile(uint fileDataId)
        {
            var fileName = string.Format(CascConstants.FileIdFormat, fileDataId);
            return OpenFile(fileName, CascOpenFlags.OpenByFileId);
        }

        /// <summary>
        /// Attempts to open a file from the CASC archive.
        /// </summary>
        /// <param name="fileName">The name of the file to open.</param>
        /// <param name="stream">The stream containing the file data.</param>
        /// <returns>true if the file was opened successfully; otherwise, false.</returns>
        public bool TryOpenFile(string fileName, out Stream? stream)
        {
            return TryOpenFile(fileName, CascOpenFlags.OpenByName, out stream);
        }

        /// <summary>
        /// Attempts to open a file from the CASC archive.
        /// </summary>
        /// <param name="fileName">The name of the file to open.</param>
        /// <param name="openFlags">Flags for opening the file.</param>
        /// <param name="stream">The stream containing the file data.</param>
        /// <returns>true if the file was opened successfully; otherwise, false.</returns>
        public bool TryOpenFile(string fileName, CascOpenFlags openFlags, out Stream? stream)
        {
            try
            {
                stream = OpenFile(fileName, openFlags);
                return true;
            }
            catch (FileNotFoundException ex)
            {
                System.Diagnostics.Trace.TraceWarning($"File not found: {fileName}. Details: {ex.Message}");
                stream = null;
                return false;
            }
            catch (CascFileNotFoundException ex)
            {
                System.Diagnostics.Trace.TraceWarning($"CASC file not found: {fileName}. Details: {ex.Message}");
                stream = null;
                return false;
            }
            catch (ArgumentException ex)
            {
                System.Diagnostics.Trace.TraceWarning($"Invalid argument for file: {fileName}. Details: {ex.Message}");
                stream = null;
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError($"Unexpected error opening file: {fileName}. Exception: {ex}");
                stream = null;
                return false;
            }
        }

        /// <summary>
        /// Checks if a file exists in the CASC archive.
        /// </summary>
        /// <param name="fileName">The name of the file to check.</param>
        /// <returns>true if the file exists; otherwise, false.</returns>
        public bool FileExists(string fileName)
        {
            return !string.IsNullOrEmpty(fileName) && _entries.ContainsKey(fileName);
        }

        /// <summary>
        /// Gets an entry from the CASC archive.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <returns>The CASC entry.</returns>
        public CascEntry GetEntry(string fileName)
        {
            if (!TryGetEntry(fileName, out var entry))
            {
                throw new FileNotFoundException($"File not found in CASC archive: {fileName}");
            }

            return entry;
        }

        /// <summary>
        /// Attempts to get an entry from the CASC archive.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="entry">The CASC entry.</param>
        /// <returns>true if the entry was found; otherwise, false.</returns>
        public bool TryGetEntry(string fileName, out CascEntry? entry)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                entry = null;
                return false;
            }

            return _entries.TryGetValue(fileName, out entry);
        }

        /// <summary>
        /// Finds files matching a pattern.
        /// </summary>
        /// <param name="pattern">The search pattern (supports wildcards).</param>
        /// <returns>An enumerable of matching entries.</returns>
        public IEnumerable<CascEntry> FindFiles(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                return Enumerable.Empty<CascEntry>();
            }

            // Simple wildcard matching (can be enhanced later)
            if (pattern == "*")
            {
                return _entries.Values;
            }

            pattern = pattern.Replace("*", ".*").Replace("?", ".");
            var regex = new System.Text.RegularExpressions.Regex(pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            return _entries.Values.Where(e => regex.IsMatch(e.FileName));
        }

        /// <summary>
        /// Adds an encryption key to the storage.
        /// </summary>
        /// <param name="keyName">The key name.</param>
        /// <param name="key">The encryption key.</param>
        public void AddEncryptionKey(ulong keyName, byte[] key)
        {
            _storage.AddEncryptionKey(keyName, key);
        }

        /// <summary>
        /// Adds an encryption key from a hex string.
        /// </summary>
        /// <param name="keyName">The key name.</param>
        /// <param name="keyString">The encryption key as a hex string.</param>
        public void AddStringEncryptionKey(ulong keyName, string keyString)
        {
            _storage.AddStringEncryptionKey(keyName, keyString);
        }

        /// <summary>
        /// Imports encryption keys from a string list.
        /// </summary>
        /// <param name="keyList">The key list in format "KeyName=KeyValue" separated by newlines.</param>
        /// <returns>The number of keys imported.</returns>
        public int ImportKeysFromString(string keyList)
        {
            return _storage.ImportKeysFromString(keyList);
        }

        /// <summary>
        /// Imports encryption keys from a file.
        /// </summary>
        /// <param name="fileName">The path to the key file.</param>
        /// <returns>The number of keys imported.</returns>
        public int ImportKeysFromFile(string fileName)
        {
            return _storage.ImportKeysFromFile(fileName);
        }

        /// <summary>
        /// Finds an encryption key by name.
        /// </summary>
        /// <param name="keyName">The key name.</param>
        /// <returns>The encryption key, or null if not found.</returns>
        public byte[]? FindEncryptionKey(ulong keyName)
        {
            return _storage.GetEncryptionKey(keyName);
        }

        /// <inheritdoc/>
        public IEnumerator<CascEntry> GetEnumerator()
        {
            return _entries.Values.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            // Dispose all open streams first
            lock (_streamLock)
            {
                // Clean up dead references and dispose live streams
                var liveStreams = new List<Stream>();
                foreach (var weakRef in _openStreams)
                {
                    if (weakRef.IsAlive && weakRef.Target is Stream stream)
                    {
                        liveStreams.Add(stream);
                    }
                }

                foreach (var stream in liveStreams)
                {
                    try
                    {
                        stream.Dispose();
                    }
                    catch
                    {
                        // Ignore disposal errors for individual streams
                    }
                }

                _openStreams.Clear();
            }

            // Then dispose the storage
            _storage?.Dispose();
            _entries.Clear();

            _disposed = true;
        }

        private void Initialize()
        {
            if (_storage == null)
            {
                throw new InvalidOperationException("Storage is not initialized");
            }

            // Build file list from root handler if available
            var rootHandler = _storage.RootHandler;
            if (rootHandler != null)
            {
                foreach (var rootEntry in rootHandler.GetEntries())
                {
                    var entry = new CascEntry(rootEntry.FileName)
                    {
                        CKey = rootEntry.CKey,
                        EKey = rootEntry.EKey,
                        FileSize = rootEntry.FileSize,
                        LocaleFlags = rootEntry.LocaleFlags,
                        ContentFlags = rootEntry.ContentFlags,
                        FileDataId = rootEntry.FileDataId,
                    };
                    
                    _entries.TryAdd(entry.FileName, entry);
                }
            }
        }

        private Stream OpenFileInternal(CascEntry entry, CascOpenFlags openFlags)
        {
            ThrowIfDisposed();

            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            Stream stream;
            
            // Use the storage to open the file
            if (!entry.CKey.IsEmpty)
            {
                stream = _storage.OpenFileByCKey(entry.CKey);
            }
            else if (!entry.EKey.IsEmpty)
            {
                stream = _storage.OpenFileByEKey(entry.EKey);
            }
            else if (entry.FileDataId != CascConstants.InvalidId)
            {
                stream = _storage.OpenFileByFileId(entry.FileDataId);
            }
            else if (!string.IsNullOrEmpty(entry.FileName))
            {
                stream = _storage.OpenFile(entry.FileName, openFlags);
            }
            else
            {
                throw new CascException($"Cannot open file: entry has no valid identifier (CKey, EKey, FileDataId, or FileName)");
            }

            // Track the stream for proper disposal using weak reference
            // This prevents memory leaks if streams are not explicitly disposed
            var weakRef = new WeakReference(stream);
            lock (_streamLock)
            {
                // Clean up dead references while we're here
                CleanupDeadReferences();
                _openStreams.Add(weakRef);
            }

            // Wrap the stream to remove it from tracking when disposed
            // Use a separate disposal lock to avoid potential deadlocks
            return new TrackedStream(stream, weakRef, () =>
            {
                lock (_streamLock)
                {
                    _openStreams.Remove(weakRef);
                }
            });
        }

        private void CleanupDeadReferences()
        {
            // Remove weak references that no longer point to live objects
            // This prevents the collection from growing indefinitely
            // Note: RemoveWhere is thread-safe on HashSet, but we still need the lock
            // since we're modifying the collection that other methods are reading
            _openStreams.RemoveWhere(wr => !wr.IsAlive);
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(CascArchive));
            }
        }

        /// <summary>
        /// Wrapper stream that notifies when disposed.
        /// </summary>
        private sealed class TrackedStream : Stream
        {
            private readonly Stream _innerStream;
            private readonly WeakReference _weakReference;
            private readonly Action _onDispose;
            private bool _disposed;

            /// <summary>
            /// Initializes a new instance of the <see cref="TrackedStream"/> class.
            /// </summary>
            /// <param name="innerStream">The inner stream to wrap.</param>
            /// <param name="weakReference">The weak reference tracking this stream.</param>
            /// <param name="onDispose">Action to invoke when the stream is disposed.</param>
            public TrackedStream(Stream innerStream, WeakReference weakReference, Action onDispose)
            {
                _innerStream = innerStream ?? throw new ArgumentNullException(nameof(innerStream));
                _weakReference = weakReference ?? throw new ArgumentNullException(nameof(weakReference));
                _onDispose = onDispose ?? throw new ArgumentNullException(nameof(onDispose));
            }

            /// <inheritdoc/>
            public override bool CanRead => _innerStream.CanRead;
            
            /// <inheritdoc/>
            public override bool CanSeek => _innerStream.CanSeek;
            
            /// <inheritdoc/>
            public override bool CanWrite => _innerStream.CanWrite;
            
            /// <inheritdoc/>
            public override long Length => _innerStream.Length;
            
            /// <inheritdoc/>
            public override long Position
            {
                get => _innerStream.Position;
                set => _innerStream.Position = value;
            }

            /// <inheritdoc/>
            public override void Flush() => _innerStream.Flush();
            
            /// <inheritdoc/>
            public override int Read(byte[] buffer, int offset, int count) => _innerStream.Read(buffer, offset, count);
            
            /// <inheritdoc/>
            public override long Seek(long offset, SeekOrigin origin) => _innerStream.Seek(offset, origin);
            
            /// <inheritdoc/>
            public override void SetLength(long value) => _innerStream.SetLength(value);
            
            /// <inheritdoc/>
            public override void Write(byte[] buffer, int offset, int count) => _innerStream.Write(buffer, offset, count);

            /// <inheritdoc/>
            protected override void Dispose(bool disposing)
            {
                if (!_disposed)
                {
                    if (disposing)
                    {
                        _innerStream.Dispose();
                        _onDispose();
                    }
                    _disposed = true;
                }
                base.Dispose(disposing);
            }
        }
    }
}