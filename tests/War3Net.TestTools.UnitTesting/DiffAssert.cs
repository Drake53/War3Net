// ------------------------------------------------------------------------------
// <copyright file="DiffAssert.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace War3Net.TestTools.UnitTesting
{
    public static class DiffAssert
    {
        /// <summary>
        /// Asserts that two strings are equal, providing a detailed diff output if they are not.
        /// </summary>
        /// <param name="expected">The expected string.</param>
        /// <param name="actual">The actual string.</param>
        public static void AreEqual(string expected, string actual)
        {
            if (string.Equals(expected, actual, StringComparison.Ordinal))
            {
                return;
            }

            var diffMessage = BuildDiffMessage(expected, actual);
            Assert.Fail(diffMessage);
        }

        /// <summary>
        /// Asserts that two strings are equal, providing a detailed diff output if they are not.
        /// </summary>
        /// <param name="expected">The expected string.</param>
        /// <param name="actual">The actual string.</param>
        /// <param name="message">Additional message to include in the assertion failure.</param>
        public static void AreEqual(string expected, string actual, string message)
        {
            if (string.Equals(expected, actual, StringComparison.Ordinal))
            {
                return;
            }

            var diffMessage = BuildDiffMessage(expected, actual);
            Assert.Fail($"{message}{Environment.NewLine}{diffMessage}");
        }

        /// <summary>
        /// Asserts that two strings are equal, providing a detailed diff output if they are not.
        /// Ignores differences in line ending styles (CRLF vs LF).
        /// </summary>
        /// <param name="expected">The expected string.</param>
        /// <param name="actual">The actual string.</param>
        public static void AreEqualIgnoreLineEndings(string expected, string actual)
        {
            var normalizedExpected = NormalizeLineEndings(expected);
            var normalizedActual = NormalizeLineEndings(actual);

            if (string.Equals(normalizedExpected, normalizedActual, StringComparison.Ordinal))
            {
                return;
            }

            var diffMessage = BuildDiffMessage(normalizedExpected, normalizedActual);
            Assert.Fail(diffMessage);
        }

        /// <summary>
        /// Asserts that two strings are equal, providing a detailed diff output if they are not.
        /// Ignores differences in line ending styles (CRLF vs LF).
        /// </summary>
        /// <param name="expected">The expected string.</param>
        /// <param name="actual">The actual string.</param>
        /// <param name="message">Additional message to include in the assertion failure.</param>
        public static void AreEqualIgnoreLineEndings(string expected, string actual, string message)
        {
            var normalizedExpected = NormalizeLineEndings(expected);
            var normalizedActual = NormalizeLineEndings(actual);

            if (string.Equals(normalizedExpected, normalizedActual, StringComparison.Ordinal))
            {
                return;
            }

            var diffMessage = BuildDiffMessage(normalizedExpected, normalizedActual);
            Assert.Fail($"{message}{Environment.NewLine}{diffMessage}");
        }

        private static string BuildDiffMessage(string expected, string actual)
        {
            var diffBuilder = new InlineDiffBuilder(new Differ());
            var diff = diffBuilder.BuildDiffModel(expected, actual, ignoreWhitespace: false);

            var messageBuilder = new StringBuilder();
            messageBuilder.AppendLine();
            messageBuilder.AppendLine("Strings are not equal. Diff:");
            messageBuilder.AppendLine();

            var addedLines = 0;
            var deletedLines = 0;
            var modifiedLines = 0;

            foreach (var line in diff.Lines)
            {
                switch (line.Type)
                {
                    case ChangeType.Inserted:
                        messageBuilder.AppendLine($"+ {line.Text}");
                        addedLines++;
                        break;

                    case ChangeType.Deleted:
                        messageBuilder.AppendLine($"- {line.Text}");
                        deletedLines++;
                        break;

                    case ChangeType.Modified:
                        messageBuilder.AppendLine($"~ {line.Text}");
                        modifiedLines++;
                        break;

                    case ChangeType.Imaginary:
                        // Imaginary lines are used for alignment in side-by-side diffs
                        // We can skip them in inline diff output
                        break;

                    case ChangeType.Unchanged:
                        messageBuilder.AppendLine($"  {line.Text}");
                        break;
                }
            }

            messageBuilder.AppendLine();
            messageBuilder.AppendLine("Summary:");
            messageBuilder.AppendLine($"  Lines added: {addedLines}");
            messageBuilder.AppendLine($"  Lines deleted: {deletedLines}");
            messageBuilder.AppendLine($"  Lines modified: {modifiedLines}");

            if (addedLines == 0 && deletedLines == 0 && modifiedLines == 0)
            {
                return BuildDiffMessage(
                    StringHelper.ShowNewLineCharacters(expected),
                    StringHelper.ShowNewLineCharacters(actual));
            }

            return messageBuilder.ToString();
        }

        [return: NotNullIfNotNull(nameof(text))]
        private static string? NormalizeLineEndings(string? text)
        {
            if (text is null)
            {
                return null;
            }

            return text
                .Replace("\r\n", "\n", StringComparison.Ordinal)
                .Replace("\r", "\n", StringComparison.Ordinal);
        }
    }
}