// ------------------------------------------------------------------------------
// <copyright file="DialogButton.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Veldrid;

using War3Net.Runtime.Core;

namespace War3Net.Runtime.UI
{
    public sealed class DialogButton : Agent
    {
        private readonly string? _text;
        private readonly Key _hotkey;

        public DialogButton(string? text, Key hotkey)
        {
            _text = text;
            _hotkey = hotkey;
        }

        public string? Text => _text;

        public Key Hotkey => _hotkey;

        public override void Dispose()
        {
            // TODO
        }
    }
}