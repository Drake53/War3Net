// ------------------------------------------------------------------------------
// <copyright file="ListFile.cs" company="Foole (fooleau@gmail.com)">
// Copyright (c) 2006 Foole (fooleau@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace War3Net.IO.Mpq
{
    /// <summary>
    /// The <see cref="ListFile"/> lists all files that are contained in the <see cref="MpqArchive"/>.
    /// </summary>
    public sealed class ListFile : IDisposable
    {
        /// <summary>
        /// The key (filename) used to open the <see cref="ListFile"/>.
        /// </summary>
        public const string Key = "(listfile)";

        private readonly Stream _baseStream;
        private bool _readOnly;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListFile"/> class.
        /// </summary>
        /// <param name="files">The collection of file paths to be included in the <see cref="ListFile"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="files"/> argument is null.</exception>
        public ListFile(IEnumerable<string> files)
        {
            _baseStream = new MemoryStream();
            _readOnly = false;

            using (var writer = GetWriter())
            {
                foreach (var fileName in files ?? throw new ArgumentNullException(nameof(files)))
                {
                    writer.WriteLine(fileName);
                }
            }

            _baseStream.Position = 0;
        }

        /// <summary>
        /// Gets the underlying stream of this <see cref="ListFile"/>.
        /// </summary>
        public Stream BaseStream => _baseStream;

        /// <summary>
        /// Appends a single file path to the <see cref="ListFile"/>.
        /// </summary>
        /// <param name="fileName">The file path to append.</param>
        public void WriteFile(string fileName)
        {
            using (var writer = GetWriter())
            {
                writer.WriteLine(fileName);
            }
        }

        /// <summary>
        /// Make the <see cref="ListFile"/> read-only.
        /// </summary>
        public void Finish()
        {
            _baseStream.Position = 0;
            _readOnly = true;
        }

        /// <summary>
        /// Releases all resources used by the <see cref="ListFile"/>.
        /// </summary>
        public void Dispose()
        {
            _baseStream.Dispose();
        }

        private StreamWriter GetWriter()
        {
            return _readOnly
                ? throw new InvalidOperationException($"Cannot obtain a {nameof(StreamWriter)} for this {nameof(ListFile)}, because it's read-only.")
                : new StreamWriter(_baseStream, new UTF8Encoding(false, true), 1024, true);
        }
    }
}