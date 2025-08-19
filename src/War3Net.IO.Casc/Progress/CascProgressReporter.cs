// ------------------------------------------------------------------------------
// <copyright file="CascProgressReporter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using War3Net.IO.Casc.Enums;

namespace War3Net.IO.Casc.Progress
{
    /// <summary>
    /// Default implementation of progress reporter using events.
    /// </summary>
    public class CascProgressReporter : IProgressReporter
    {
        private bool _cancelled;

        /// <summary>
        /// Occurs when progress is reported.
        /// </summary>
        public event EventHandler<ProgressEventArgs>? ProgressChanged;

        /// <summary>
        /// Occurs when a status message is reported.
        /// </summary>
        public event EventHandler<string>? StatusChanged;

        /// <summary>
        /// Occurs when an error is reported.
        /// </summary>
        public event EventHandler<string>? ErrorOccurred;

        /// <inheritdoc/>
        public bool ReportProgress(CascProgressMessage message, string? objectName, int current, int total)
        {
            var args = new ProgressEventArgs(message, objectName, current, total);
            ProgressChanged?.Invoke(this, args);

            if (args.Cancel)
            {
                _cancelled = true;
            }

            return !_cancelled;
        }

        /// <inheritdoc/>
        public void ReportStatus(string message)
        {
            StatusChanged?.Invoke(this, message);
        }

        /// <inheritdoc/>
        public void ReportError(string message)
        {
            ErrorOccurred?.Invoke(this, message);
        }

        /// <inheritdoc/>
        public bool IsCancelled()
        {
            return _cancelled;
        }

        /// <summary>
        /// Cancels the operation.
        /// </summary>
        public void Cancel()
        {
            _cancelled = true;
        }

        /// <summary>
        /// Resets the cancellation state.
        /// </summary>
        public void Reset()
        {
            _cancelled = false;
        }
    }
}