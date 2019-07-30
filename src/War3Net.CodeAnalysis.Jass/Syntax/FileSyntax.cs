// ------------------------------------------------------------------------------
// <copyright file="FileSyntax.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class FileSyntax : SyntaxNode
    {
        private readonly LineDelimiterSyntax _eol;
        private readonly EmptyNode _empty;
        private readonly DeclarationListSyntax _declrs;
        private readonly FunctionListSyntax _functions;
        private readonly TokenNode _eof;

        public FileSyntax(LineDelimiterSyntax eolNode, DeclarationListSyntax declarationsNode, FunctionListSyntax functionsNode)
            : base(eolNode, declarationsNode, functionsNode)
        {
            _eol = eolNode ?? throw new ArgumentNullException(nameof(eolNode));
            _declrs = declarationsNode ?? throw new ArgumentNullException(nameof(declarationsNode));
            _functions = functionsNode ?? throw new ArgumentNullException(nameof(functionsNode));
        }

        public FileSyntax(EmptyNode emptyNode, DeclarationListSyntax declarationsNode, FunctionListSyntax functionsNode)
            : base(emptyNode, declarationsNode, functionsNode)
        {
            _empty = emptyNode ?? throw new ArgumentNullException(nameof(emptyNode));
            _declrs = declarationsNode ?? throw new ArgumentNullException(nameof(declarationsNode));
            _functions = functionsNode ?? throw new ArgumentNullException(nameof(functionsNode));
        }

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                if (nodes[0] is LineDelimiterSyntax eolNode)
                {
                    return new FileSyntax(eolNode, nodes[1] as DeclarationListSyntax, nodes[2] as FunctionListSyntax);
                }
                else
                {
                    return new FileSyntax(nodes[0] as EmptyNode, nodes[1] as DeclarationListSyntax, nodes[2] as FunctionListSyntax);
                }
            }

            private Parser Init()
            {
                AddParser(new OptionalParser(LineDelimiterSyntax.Parser.Get));
                AddParser(DeclarationListSyntax.Parser.Get);
                AddParser(FunctionListSyntax.Parser.Get);

                // untested
                AddParser(TokenParser.Get(SyntaxTokenType.EndOfFile));

                return this;
            }
        }
    }
}