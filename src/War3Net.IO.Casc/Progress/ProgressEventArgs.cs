// ------------------------------------------------------------------------------
// <copyright file="ProgressEventArgs.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using War3Net.IO.Casc.Enums;

namespace War3Net.IO.Casc.Progress
{
    /// <summary>
    /// Event arguments for progress reporting.
    /// </summary>
    public class ProgressEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressEventArgs"/> class.
        /// </summary>
        /// <param name="message">The progress message.</param>
        /// <param name="objectName">The object name.</param>
        /// <param name="current">The current value.</param>
        /// <param name="total">The total value.</param>
        public ProgressEventArgs(CascProgressMessage message, string? objectName, int current, int total)
        {
            Message = message;
            ObjectName = objectName;
            Current = current;
            Total = total;
        }

        /// <summary>
        /// Gets the progress message type.
        /// </summary>
        public CascProgressMessage Message { get; }

        /// <summary>
        /// Gets the name of the object being processed.
        /// </summary>
        public string? ObjectName { get; }

        /// <summary>
        /// Gets the current progress value.
        /// </summary>
        public int Current { get; }

        /// <summary>
        /// Gets the total progress value.
        /// </summary>
        public int Total { get; }

        /// <summary>
        /// Gets the progress percentage (0-100).
        /// </summary>
        public int Percentage => Total > 0 ? (int)((Current * 100L) / Total) : 0;

        /// <summary>
        /// Gets or sets a value indicating whether the operation should be cancelled.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Gets a formatted progress message.
        /// </summary>
        /// <returns>A formatted message string.</returns>
        public string GetFormattedMessage()
        {
            var messageText = Message switch
            {
                CascProgressMessage.LoadingFile => "Loading file",
                CascProgressMessage.LoadingManifest => "Loading manifest",
                CascProgressMessage.DownloadingFile => "Downloading file",
                CascProgressMessage.LoadingIndexes => "Loading index files",
                CascProgressMessage.DownloadingArchiveIndexes => "Downloading archive indexes",
                _ => "Processing",
            };

            if (!string.IsNullOrEmpty(ObjectName))
            {
                messageText = $"{messageText}: {ObjectName}";
            }

            if (Total > 0)
            {
                messageText = $"{messageText} ({Current}/{Total} - {Percentage}%)";
            }

            return messageText;
        }
    }
}