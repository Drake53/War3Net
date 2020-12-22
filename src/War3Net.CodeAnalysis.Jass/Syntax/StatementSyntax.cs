// ------------------------------------------------------------------------------
// <copyright file="StatementSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class StatementSyntax : SyntaxNode
    {
        private readonly SetStatementSyntax? _set;
        private readonly CallStatementSyntax? _call;
        private readonly IfStatementSyntax? _if;
        private readonly LoopStatementSyntax? _loop;
        private readonly ExitStatementSyntax? _exit;
        private readonly ReturnStatementSyntax? _return;
        private readonly DebugStatementSyntax? _debug;

        public StatementSyntax(SetStatementSyntax setStatementNode)
            : base(setStatementNode)
        {
            _set = setStatementNode ?? throw new ArgumentNullException(nameof(setStatementNode));
        }

        public StatementSyntax(CallStatementSyntax callStatementNode)
            : base(callStatementNode)
        {
            _call = callStatementNode ?? throw new ArgumentNullException(nameof(callStatementNode));
        }

        public StatementSyntax(IfStatementSyntax ifStatementNode)
            : base(ifStatementNode)
        {
            _if = ifStatementNode ?? throw new ArgumentNullException(nameof(ifStatementNode));
        }

        public StatementSyntax(LoopStatementSyntax loopStatementNode)
            : base(loopStatementNode)
        {
            _loop = loopStatementNode ?? throw new ArgumentNullException(nameof(loopStatementNode));
        }

        public StatementSyntax(ExitStatementSyntax exitStatementNode)
            : base(exitStatementNode)
        {
            _exit = exitStatementNode ?? throw new ArgumentNullException(nameof(exitStatementNode));
        }

        public StatementSyntax(ReturnStatementSyntax returnStatementNode)
            : base(returnStatementNode)
        {
            _return = returnStatementNode ?? throw new ArgumentNullException(nameof(returnStatementNode));
        }

        public StatementSyntax(DebugStatementSyntax debugStatementNode)
            : base(debugStatementNode)
        {
            _debug = debugStatementNode ?? throw new ArgumentNullException(nameof(debugStatementNode));
        }

        public SetStatementSyntax? SetStatementNode => _set;

        public CallStatementSyntax? CallStatementNode => _call;

        public IfStatementSyntax? IfStatementNode => _if;

        public LoopStatementSyntax? LoopStatementNode => _loop;

        public ExitStatementSyntax? ExitStatementNode => _exit;

        public ReturnStatementSyntax? ReturnStatementNode => _return;

        public DebugStatementSyntax? DebugStatementNode => _debug;

        internal sealed class Parser : AlternativeParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(SyntaxNode node)
            {
                if (node is SetStatementSyntax setStatementNode)
                {
                    return new StatementSyntax(setStatementNode);
                }

                if (node is CallStatementSyntax callStatementNode)
                {
                    return new StatementSyntax(callStatementNode);
                }

                if (node is IfStatementSyntax ifStatementNode)
                {
                    return new StatementSyntax(ifStatementNode);
                }

                if (node is LoopStatementSyntax loopStatementNode)
                {
                    return new StatementSyntax(loopStatementNode);
                }

                if (node is ExitStatementSyntax exitStatementNode)
                {
                    return new StatementSyntax(exitStatementNode);
                }

                if (node is ReturnStatementSyntax returnStatementNode)
                {
                    return new StatementSyntax(returnStatementNode);
                }

                if (node is DebugStatementSyntax debugStatementNode)
                {
                    return new StatementSyntax(debugStatementNode);
                }

                throw new Exception();
            }

            private Parser Init()
            {
                AddParser(SetStatementSyntax.Parser.Get);
                AddParser(CallStatementSyntax.Parser.Get);
                AddParser(IfStatementSyntax.Parser.Get);
                AddParser(LoopStatementSyntax.Parser.Get);
                AddParser(ExitStatementSyntax.Parser.Get);
                AddParser(ReturnStatementSyntax.Parser.Get);
                AddParser(DebugStatementSyntax.Parser.Get);

                return this;
            }
        }
    }
}