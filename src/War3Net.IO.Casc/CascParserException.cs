// ------------------------------------------------------------------------------
// <copyright file="CascParserException.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.IO.Casc
{
    /// <summary>
    /// Exception thrown when parsing CASC data fails.
    /// </summary>
    public class CascParserException : CascException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CascParserException"/> class.
        /// </summary>
        public CascParserException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CascParserException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        public CascParserException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CascParserException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        public CascParserException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CascParserException"/> class.
        /// </summary>
        /// <param name="fileName">The name of the file that failed to parse.</param>
        /// <param name="reason">The reason for the failure.</param>
        public CascParserException(string fileName, string reason)
            : base($"Failed to parse '{fileName}': {reason}")
        {
            FileName = fileName;
            Reason = reason;
        }

        /// <summary>
        /// Gets the name of the file that failed to parse.
        /// </summary>
        public string? FileName { get; }

        /// <summary>
        /// Gets the reason for the parse failure.
        /// </summary>
        public string? Reason { get; }
    }
}