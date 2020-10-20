// ------------------------------------------------------------------------------
// <copyright file="Dialog.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;

using Veldrid;

using War3Net.Runtime.Core;

namespace War3Net.Runtime.UI
{
    public sealed class Dialog : Agent, IEnumerable<DialogButton>
    {
        public static readonly Dictionary<Player, Dialog> _shownDialogs = new Dictionary<Player, Dialog>();

        private readonly List<DialogButton> _buttons;

        public Dialog()
        {
            _buttons = new List<DialogButton>();
        }

        public string? Message { get; set; }

        public DialogButton AddButton(string? buttonText, Key hotkey)
        {
            var button = new DialogButton(buttonText, hotkey);
            _buttons.Add(button);
            return button;
        }

        public void Display(Player player, bool flag)
        {
            if (_shownDialogs.TryGetValue(player, out var shownDialog))
            {
                if (flag)
                {
                    _shownDialogs[player] = this;
                }
                else if (shownDialog == this)
                {
                    _shownDialogs.Remove(player);
                }
            }
            else if (flag)
            {
                _shownDialogs.Add(player, this);
            }
        }

        public void Clear()
        {
            // TODO
        }

        public static void DisposeAllDialogs()
        {
            // TODO

            _shownDialogs.Clear();
        }

        public override void Dispose()
        {
            // TODO
        }

        public IEnumerator<DialogButton> GetEnumerator()
        {
            return ((IEnumerable<DialogButton>)_buttons).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_buttons).GetEnumerator();
        }
    }
}