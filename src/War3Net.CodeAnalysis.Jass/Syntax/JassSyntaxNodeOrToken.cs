// ------------------------------------------------------------------------------
// <copyright file="JassSyntaxNodeOrToken.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    // Based on https://github.com/mcintyre321/OneOf/blob/master/OneOf/OneOfT1.generated.cs
    public readonly struct JassSyntaxNodeOrToken
    {
        private readonly JassSyntaxNode? _node;
        private readonly JassSyntaxToken? _token;
        private readonly bool _isNode;

        private JassSyntaxNodeOrToken(JassSyntaxNode node)
        {
            if (node is null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            _node = node;
            _token = null;
            _isNode = true;
        }

        private JassSyntaxNodeOrToken(JassSyntaxToken token)
        {
            if (token is null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            _node = null;
            _token = token;
            _isNode = false;
        }

        public object Value => _isNode ? _node! : _token!;

        public bool IsNode => _isNode;

        public bool IsToken => !_isNode;

        public JassSyntaxNode AsNode => _node ?? throw new InvalidOperationException($"Cannot return as node, because result is token.");

        public JassSyntaxToken AsToken => _token ?? throw new InvalidOperationException($"Cannot return as token, because result is node.");

        public static implicit operator JassSyntaxNodeOrToken(JassSyntaxNode node) => new(node);

        public static implicit operator JassSyntaxNodeOrToken(JassSyntaxToken token) => new(token);

        public static JassSyntaxNodeOrToken FromJassSyntaxNode(JassSyntaxNode node) => node;

        public static JassSyntaxNodeOrToken FromJassSyntaxToken(JassSyntaxToken token) => token;

        public void Switch(Action<JassSyntaxNode>? nodeFunc, Action<JassSyntaxToken>? tokenFunc)
        {
            if (_isNode)
            {
                if (nodeFunc is null)
                {
                    throw new ArgumentNullException(nameof(nodeFunc));
                }

                nodeFunc.Invoke(_node!);
            }
            else
            {
                if (tokenFunc is null)
                {
                    throw new ArgumentNullException(nameof(tokenFunc));
                }

                tokenFunc.Invoke(_token!);
            }
        }

        public TResult Match<TResult>(Func<JassSyntaxNode, TResult>? nodeFunc, Func<JassSyntaxToken, TResult>? tokenFunc)
        {
            if (_isNode)
            {
                if (nodeFunc is null)
                {
                    throw new ArgumentNullException(nameof(nodeFunc));
                }

                return nodeFunc.Invoke(_node!);
            }
            else
            {
                if (tokenFunc is null)
                {
                    throw new ArgumentNullException(nameof(tokenFunc));
                }

                return tokenFunc.Invoke(_token!);
            }
        }

        public bool TryGetNode([NotNullWhen(true)] out JassSyntaxNode? node)
        {
            node = _node;
            return _isNode;
        }

        public bool TryGetToken([NotNullWhen(true)] out JassSyntaxToken? token)
        {
            token = _token;
            return !_isNode;
        }

        public bool TryPickNode([NotNullWhen(true)] out JassSyntaxNode? node, [NotNullWhen(false)] out JassSyntaxToken? token)
        {
            node = _node;
            token = _token;
            return _isNode;
        }

        public bool TryPickToken([NotNullWhen(true)] out JassSyntaxToken? token, [NotNullWhen(false)] out JassSyntaxNode? node)
        {
            node = _node;
            token = _token;
            return !_isNode;
        }

        public override string ToString()
        {
            return _isNode
                ? $"Node: {_node}"
                : $"Token: {_token}";
        }
    }
}