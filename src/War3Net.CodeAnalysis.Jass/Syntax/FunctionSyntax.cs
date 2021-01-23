// ------------------------------------------------------------------------------
// <copyright file="FunctionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class FunctionSyntax : SyntaxNode
    {
        private readonly TokenNode? _constant;
        private readonly EmptyNode? _empty;
        private readonly TokenNode _function;
        private readonly FunctionDeclarationSyntax _declr;
        private readonly LineDelimiterSyntax _eol1;
        private readonly LocalVariableListSyntax _locals;
        private readonly StatementListSyntax _statements;
        private readonly TokenNode _endfunction;
        private readonly LineDelimiterSyntax _eol2;

        public FunctionSyntax(TokenNode constantNode, TokenNode functionNode, FunctionDeclarationSyntax declarationNode, LineDelimiterSyntax eolNode1, LocalVariableListSyntax localVariableListNode, StatementListSyntax statementListNode, TokenNode endfunctionNode, LineDelimiterSyntax eolNode2)
            : base(constantNode, functionNode, declarationNode, eolNode1, localVariableListNode, statementListNode, endfunctionNode, eolNode2)
        {
            _constant = constantNode ?? throw new ArgumentNullException(nameof(constantNode));
            _function = functionNode ?? throw new ArgumentNullException(nameof(functionNode));
            _declr = declarationNode ?? throw new ArgumentNullException(nameof(declarationNode));
            _eol1 = eolNode1 ?? throw new ArgumentNullException(nameof(eolNode1));
            _locals = localVariableListNode ?? throw new ArgumentNullException(nameof(localVariableListNode));
            _statements = statementListNode ?? throw new ArgumentNullException(nameof(statementListNode));
            _endfunction = endfunctionNode ?? throw new ArgumentNullException(nameof(endfunctionNode));
            _eol2 = eolNode2 ?? throw new ArgumentNullException(nameof(eolNode2));
        }

        public FunctionSyntax(EmptyNode emptyNode, TokenNode functionNode, FunctionDeclarationSyntax declarationNode, LineDelimiterSyntax eolNode1, LocalVariableListSyntax localVariableListNode, StatementListSyntax statementListNode, TokenNode endfunctionNode, LineDelimiterSyntax eolNode2)
            : base(emptyNode, functionNode, declarationNode, eolNode1, localVariableListNode, statementListNode, endfunctionNode, eolNode2)
        {
            _empty = emptyNode ?? throw new ArgumentNullException(nameof(emptyNode));
            _function = functionNode ?? throw new ArgumentNullException(nameof(functionNode));
            _declr = declarationNode ?? throw new ArgumentNullException(nameof(declarationNode));
            _eol1 = eolNode1 ?? throw new ArgumentNullException(nameof(eolNode1));
            _locals = localVariableListNode ?? throw new ArgumentNullException(nameof(localVariableListNode));
            _statements = statementListNode ?? throw new ArgumentNullException(nameof(statementListNode));
            _endfunction = endfunctionNode ?? throw new ArgumentNullException(nameof(endfunctionNode));
            _eol2 = eolNode2 ?? throw new ArgumentNullException(nameof(eolNode2));
        }

        public TokenNode? ConstantKeywordToken => _constant;

        public TokenNode FunctionKeywordToken => _function;

        public FunctionDeclarationSyntax FunctionDeclarationNode => _declr;

        public LineDelimiterSyntax DeclarationLineDelimiterNode => _eol1;

        public LocalVariableListSyntax LocalVariableListNode => _locals;

        public StatementListSyntax StatementListNode => _statements;

        public TokenNode EndfunctionKeywordToken => _endfunction;

        public LineDelimiterSyntax LastLineDelimiterNode => _eol2;

        [Obsolete]
        public NativeFunctionDeclarationSyntax AsNativeFunction()
        {
            throw new NotSupportedException();
        }

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                if (nodes[0] is TokenNode constantTokenNode)
                {
                    return new FunctionSyntax(constantTokenNode, nodes[1] as TokenNode, nodes[2] as FunctionDeclarationSyntax, nodes[3] as LineDelimiterSyntax, nodes[4] as LocalVariableListSyntax, nodes[5] as StatementListSyntax, nodes[6] as TokenNode, nodes[7] as LineDelimiterSyntax);
                }
                else
                {
                    return new FunctionSyntax(nodes[0] as EmptyNode, nodes[1] as TokenNode, nodes[2] as FunctionDeclarationSyntax, nodes[3] as LineDelimiterSyntax, nodes[4] as LocalVariableListSyntax, nodes[5] as StatementListSyntax, nodes[6] as TokenNode, nodes[7] as LineDelimiterSyntax);
                }
            }

            private Parser Init()
            {
                AddParser(new OptionalParser(TokenParser.Get(SyntaxTokenType.ConstantKeyword)));
                AddParser(TokenParser.Get(SyntaxTokenType.FunctionKeyword));
                AddParser(FunctionDeclarationSyntax.Parser.Get);
                AddParser(LineDelimiterSyntax.Parser.Get);
                AddParser(LocalVariableListSyntax.Parser.Get);
                AddParser(StatementListSyntax.Parser.Get);
                AddParser(TokenParser.Get(SyntaxTokenType.EndfunctionKeyword));
                AddParser(LineDelimiterSyntax.Parser.Get);

                return this;
            }
        }
    }
}