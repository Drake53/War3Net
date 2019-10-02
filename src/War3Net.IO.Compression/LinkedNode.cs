using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace War3Net.IO.Compression
{
    // A node which is both hierachcical (parent/child) and doubly linked (next/prev)
    internal class LinkedNode
    {
        public int DecompressedValue;
        public int Weight;
        public LinkedNode Parent;
        public LinkedNode Child0;

        public LinkedNode Child1 => Child0.Prev;

        public LinkedNode Next;
        public LinkedNode Prev;

        public LinkedNode(int decompVal, int weight)
        {
            DecompressedValue = decompVal;
            Weight = weight;
        }

        // TODO: This would be more efficient as a member of the other class
        // ie avoid the recursion
        public LinkedNode Insert(LinkedNode other)
        {
            // 'Next' should have a lower weight
            // we should return the lower weight
            if (other.Weight <= Weight)
            {
                // insert before
                if (Next != null)
                {
                    Next.Prev = other;
                    other.Next = Next;
                }

                Next = other;
                other.Prev = this;
                return other;
            }
            else
            {
                if (Prev == null)
                {
                    // Insert after
                    other.Prev = null;
                    Prev = other;
                    other.Next = this;
                }
                else
                {
                    Prev.Insert(other);
                }
            }

            return this;
        }
    }
}