// ------------------------------------------------------------------------------
// <copyright file="NewDebugStatementSyntax.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class NewDebugStatementSyntax : SyntaxNode
    {
        private readonly SetStatementSyntax _set;
        private readonly CallStatementSyntax _call;
        private readonly IfStatementSyntax _if;
        private readonly LoopStatementSyntax _loop;

        public NewDebugStatementSyntax(SetStatementSyntax setStatementNode)
            : base(setStatementNode)
        {
            _set = setStatementNode ?? throw new ArgumentNullException(nameof(setStatementNode));
        }

        public NewDebugStatementSyntax(CallStatementSyntax callStatementNode)
            : base(callStatementNode)
        {
            _call = callStatementNode ?? throw new ArgumentNullException(nameof(callStatementNode));
        }

        public NewDebugStatementSyntax(IfStatementSyntax ifStatementNode)
            : base(ifStatementNode)
        {
            _if = ifStatementNode ?? throw new ArgumentNullException(nameof(ifStatementNode));
        }

        public NewDebugStatementSyntax(LoopStatementSyntax loopStatementNode)
            : base(loopStatementNode)
        {
            _loop = loopStatementNode ?? throw new ArgumentNullException(nameof(loopStatementNode));
        }

        internal sealed class Parser : AlternativeParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(SyntaxNode node)
            {
                if (node is SetStatementSyntax set)
                {
                    return new NewDebugStatementSyntax(set);
                }

                if (node is CallStatementSyntax call)
                {
                    return new NewDebugStatementSyntax(call);
                }

                if (node is IfStatementSyntax @if)
                {
                    return new NewDebugStatementSyntax(@if);
                }

                return new NewDebugStatementSyntax(node as LoopStatementSyntax);
            }

            private Parser Init()
            {
                AddParser(SetStatementSyntax.Parser.Get);
                AddParser(CallStatementSyntax.Parser.Get);
                AddParser(IfStatementSyntax.Parser.Get);
                AddParser(LoopStatementSyntax.Parser.Get);

                return this;
            }
        }
    }
}