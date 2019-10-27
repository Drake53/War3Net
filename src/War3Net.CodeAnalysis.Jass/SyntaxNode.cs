// ------------------------------------------------------------------------------
// <copyright file="SyntaxNode.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass
{
    // TODO: implement IEnumerable to enumerate over all child nodes
    public abstract class SyntaxNode //: IEnumerable<SyntaxNode>
    {
        //private readonly JassSyntaxTree _tree;
        private readonly List<SyntaxNode> _children;
        private readonly int _start;
        private readonly int _length;

        private SyntaxNode _parent;

        public SyntaxNode(int position, bool empty)
        {
            _children = new List<SyntaxNode>();

            _start = position;
            _length = empty ? 0 : 1;
        }

        public SyntaxNode(params SyntaxNode[] children)
        {
            _children = new List<SyntaxNode>(children);
            if (_children.Count == 0)
            {
                throw new ArgumentException("This constructor requires there to be at least one child node.", nameof(children));
            }

            _start = _children[0].Span.Start;
            foreach (var child in _children)
            {
                child._parent = this;
                _length += child.Span.Length;
            }
        }

        public SyntaxNode(SyntaxNode child1, params SyntaxNode[] tail)
        {
            _children = new List<SyntaxNode>();
            _children.Add(child1);
            child1._parent = this;

            _start = child1.Span.Start;
            _length = child1.Span.Length;

            foreach (var child in tail)
            {
                child._parent = this;
                _children.Add(child);
                _length += child.Span.Length;
            }
        }

       // public JassSyntaxTree SyntaxTree => _tree;

        public SyntaxNode Parent => _parent;

        // note: span is currently measured in tokens, not characters in source file
        public (int Start, int Length) Span => (_start, _length);

        // public abstract SyntaxKind Kind { get; }
        // TODO: override in all subclasses
        //public virtual SyntaxKind Kind => (SyntaxKind)(-1);

        /*public bool IsKind(SyntaxKind kind)
        {
            return kind == Kind;
        }*/

        public SyntaxNode[] GetChildren()
        {
            return _children.ToArray();
        }

        public override string ToString()
        {
            // Should return a string with same length as Span.Length (where Span is measured in source file, NOT in tokens)
            return base.ToString();
        }
    }
}