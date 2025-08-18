// ------------------------------------------------------------------------------
// <copyright file="IProgressReporter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.IO.Casc.Enums;

namespace War3Net.IO.Casc.Progress
{
    /// <summary>
    /// Interface for progress reporting during CASC operations.
    /// </summary>
    public interface IProgressReporter
    {
        /// <summary>
        /// Reports progress for a CASC operation.
        /// </summary>
        /// <param name="message">The progress message type.</param>
        /// <param name="objectName">The name of the object being processed.</param>
        /// <param name="current">The current progress value.</param>
        /// <param name="total">The total progress value.</param>
        /// <returns>true to continue; false to cancel the operation.</returns>
        bool ReportProgress(CascProgressMessage message, string? objectName, int current, int total);

        /// <summary>
        /// Reports a status message.
        /// </summary>
        /// <param name="message">The status message.</param>
        void ReportStatus(string message);

        /// <summary>
        /// Reports an error.
        /// </summary>
        /// <param name="message">The error message.</param>
        void ReportError(string message);

        /// <summary>
        /// Checks if the operation should be cancelled.
        /// </summary>
        /// <returns>true if the operation should be cancelled; otherwise, false.</returns>
        bool IsCancelled();
    }
}