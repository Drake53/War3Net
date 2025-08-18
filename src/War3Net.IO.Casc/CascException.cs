// ------------------------------------------------------------------------------
// <copyright file="CascException.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.IO.Casc
{
    /// <summary>
    /// Base exception for CASC-related errors.
    /// </summary>
    public class CascException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CascException"/> class.
        /// </summary>
        public CascException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CascException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        public CascException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CascException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        public CascException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}