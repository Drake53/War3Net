// ------------------------------------------------------------------------------
// <copyright file="JassRenamer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenamer
    {
        private readonly Dictionary<string, string> _functionDeclarationRenames;
        private readonly Dictionary<string, string> _globalVariableRenames;
        private readonly HashSet<string> _localVariableNames;

        public JassRenamer(
            Dictionary<string, string> functionDeclarationRenames,
            Dictionary<string, string> globalVariableRenames)
        {
            _functionDeclarationRenames = functionDeclarationRenames;
            _globalVariableRenames = globalVariableRenames;
            _localVariableNames = new(StringComparer.Ordinal);
        }

        public bool RenameExecuteFuncArgument { get; set; }

        private bool TryRenameDummy<TSyntax>(TSyntax? syntax, out TSyntax? renamed)
            where TSyntax : class
        {
            renamed = null;
            return false;
        }
    }
}