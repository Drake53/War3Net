// ------------------------------------------------------------------------------
// <copyright file="CascFileNotFoundException.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using War3Net.IO.Casc.Structures;

namespace War3Net.IO.Casc
{
    /// <summary>
    /// Exception thrown when a file is not found in CASC storage.
    /// </summary>
    public class CascFileNotFoundException : CascException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CascFileNotFoundException"/> class.
        /// </summary>
        public CascFileNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CascFileNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        public CascFileNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CascFileNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        public CascFileNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CascFileNotFoundException"/> class.
        /// </summary>
        /// <param name="fileName">The name of the file that was not found.</param>
        public CascFileNotFoundException(string fileName)
            : base($"File not found in CASC storage: {fileName}")
        {
            FileName = fileName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CascFileNotFoundException"/> class.
        /// </summary>
        /// <param name="ckey">The content key of the file that was not found.</param>
        public CascFileNotFoundException(CascKey ckey)
            : base($"File with CKey {ckey} not found in CASC storage")
        {
            CKey = ckey;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CascFileNotFoundException"/> class.
        /// </summary>
        /// <param name="ekey">The encoded key of the file that was not found.</param>
        public CascFileNotFoundException(EKey ekey)
            : base($"File with EKey {ekey} not found in CASC storage")
        {
            EKey = ekey;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CascFileNotFoundException"/> class.
        /// </summary>
        /// <param name="fileDataId">The file data ID that was not found.</param>
        public CascFileNotFoundException(uint fileDataId)
            : base($"File with FileDataId {fileDataId} not found in CASC storage")
        {
            FileDataId = fileDataId;
        }

        /// <summary>
        /// Gets the name of the file that was not found.
        /// </summary>
        public string? FileName { get; }

        /// <summary>
        /// Gets the content key of the file that was not found.
        /// </summary>
        public CascKey? CKey { get; }

        /// <summary>
        /// Gets the encoded key of the file that was not found.
        /// </summary>
        public EKey? EKey { get; }

        /// <summary>
        /// Gets the file data ID that was not found.
        /// </summary>
        public uint? FileDataId { get; }
    }
}