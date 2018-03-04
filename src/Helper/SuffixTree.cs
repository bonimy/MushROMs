// <copyright file="SuffixTree.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

/*
A suffix tree based on Ukkonen's algorithm for byte values. I followed the ideas based in this helpful article:
http://stackoverflow.com/questions/9452701/ukkonens-suffix-tree-algorithm-in-plain-english/9513423#9513423

The code written here is based on a C++ implementation given here:
http://pastie.org/5925809

A note on memory management:
Try creating as few new suffix tree classes as possible. Every time a new node is created, it has to initialize an array of child nodes of size equal to the alphabet size (257). For large sets of data, this operation becomes expensive and time-consuming. To counter the performance hit, the SuffixTree class can be reused. The class keeps a list of every used node. When the class is reset to implement a new tree structure, it simply erases all of the nodes information and reference data without letting the class itself be erased from memory. This way, the next tree creation (if another one will be made) does not lose time on allocating new memory. The performance gains are very significant.
*/

namespace Helper
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using static Helper.ThrowHelper;

    public class SuffixTree
    {
        public const int TerminationValue = AlphabetSize;
        public const int AlphabetSize = Byte.MaxValue + 1;
        private const int EndOfData = SubstringInfo.EndOfString;

        private const int FallbakNodeCollectionSize = 0x1000;

        public SuffixTree()
        {
            // Create a large node collection now to save time in the future.
            Nodes = new NodeCollection(this, FallbakNodeCollectionSize);

            Root = new RootNode(this);
        }

        public int Size
        {
            get;
            private set;
        }

        private RootNode Root
        {
            get;
            set;
        }

        private NodeCollection Nodes
        {
            get;
            set;
        }

        private int Position
        {
            get;
            set;
        }

        private Node ActiveLinkNode
        {
            get;
            set;
        }

        private int Remainder
        {
            get;
            set;
        }

        private Node ActiveNode
        {
            get;
            set;
        }

        private int ActivePosition
        {
            get;
            set;
        }

        private int ActiveLength
        {
            get;
            set;
        }

        private int[] Data
        {
            get;
            set;
        }

        public int this[int index]
        {
            get
            {
                return Data[index];
            }
        }

        public SubstringInfo GetLongestInternalSubstring(int index)
        {
            if (Size == 0)
            {
                throw new InvalidOperationException();
            }

            if (index < 0 || index >= Size - 1)
            {
                throw ValueNotInArrayBounds(
                    nameof(index),
                    index,
                    Size - 1);
            }

            var node = (Node)Root;
            var result = SubstringInfo.Empty;

            for (int i = index, length = 0; i < Size;)
            {
                // update to current node.
                var value = Data[i];
                node = node[value];

                // Check if we're at the last node.
                if (IsEndNode(node, index + length))
                {
                    return result;
                }

                // update status to current node's position.
                i += node.Length;
                length += node.Length;
                result = SubstringInfo.FromLengthAndEnd(length, node.End);
            }

            return result;
        }

        public void CreateTree(byte[] data)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            CreateTree(data, 0, data.Length);
        }

        public void CreateTree(byte[] data, int start, int size)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            unsafe
            {
                fixed (byte* ptr = data)
                {
                    CreateTree(ptr, data.Length, start, size);
                }
            }
        }

        public void CreateTree(IntPtr data, int length)
        {
            CreateTree(data, length, 0, length);
        }

        public void CreateTree(IntPtr data, int length, int start, int size)
        {
            if (data == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(data));
            }

            unsafe
            {
                CreateTree((byte*)data, length, start, size);
            }
        }

        private unsafe void CreateTree(
            byte* data,
            int length,
            int start,
            int size)
        {
            if (start < 0)
            {
                throw ValueNotGreaterThanEqualTo(
                    nameof(start),
                    start);
            }

            if (length < 0)
            {
                throw ValueNotGreaterThanEqualTo(
                    nameof(length),
                    length);
            }

            if (start + size > length)
            {
                throw new Exception();
            }

            Initialize(size);

            var src = data + start;
            for (var i = 0; i < size; i++)
            {
                Add(src[i]);
            }

            Add(TerminationValue);
            Position++;
        }

        private void Initialize(int size)
        {
            if (size < 0)
            {
                throw ValueNotGreaterThan(
                    nameof(size),
                    size);
            }

            Nodes.Clear();

            // +1 for the termination value.
            Data = new int[size + 1];
            Size = Data.Length;

            Position = -1;
            Remainder = 0;
            ActiveLength = 0;
            ActivePosition = 0;

            Root.Reset();
            ActiveNode = Root;
        }

        private void Add(int value)
        {
            // Update position and write new value.
            Data[++Position] = value;

            // [?] Why do we do this?
            ActiveLinkNode = null;

            // [?] Is there a smarter way to go through this loop?
            Remainder++;
            while (UpdatePosition(value))
            {
                continue;
            }
        }

        private bool UpdatePosition(int value)
        {
            // [?] Why do we do this?
            if (ActiveLength == 0)
            {
                ActivePosition = Position;
            }

            // Get the active child node.
            var activeValue = Data[ActivePosition];
            var stem = ActiveNode[activeValue];

            // Create a new child node if it does not currently exist
            if (stem is null)
            {
                var leaf = Nodes.Add(Position);
                ActiveNode[activeValue] = leaf;
                AddLink(ActiveNode);
            }
            else
            {
                if (IsEndNode(stem))
                {
                    return true;
                }

                if (!StillUpdating(stem, value))
                {
                    return false;
                }

                // Redefine active node as a branch. It will branch to the original substring and the new substring.
                var branch = Nodes.Add(stem.Start, stem.Start + ActiveLength);
                ActiveNode[activeValue] = branch;

                var leaf = Nodes.Add(Position, EndOfData);
                branch[value] = leaf;
                stem.Start += ActiveLength;
                branch[Data[stem.Start]] = stem;
                AddLink(branch);
            }

            UpdateActiveNode();
            return --Remainder > 0;
        }

        private bool StillUpdating(Node stem, int value)
        {
            // If the node and substring still match, update active point and terminate sequence.
            if (Data[stem.Start + ActiveLength] != value)
            {
                return true;
            }

            ActiveLength++;

            // If we are in an internal node needing a suffix link, we chain this node to those.
            AddLink(ActiveNode);
            return false;
        }

        private bool IsEndNode(Node stem)
        {
            // Determine if current substring exceeds the size of the active child node's.
            var edge = stem.Length;
            if (ActiveLength < edge)
            {
                return false;
            }

            ActivePosition += edge;
            ActiveLength -= edge;
            ActiveNode = stem;
            return true;
        }

        private void UpdateActiveNode()
        {
            if (ActiveNode != Root || ActiveLength <= 0)
            {
                ActiveNode = ActiveNode.Link ?? Root;
                return;
            }

            ActiveLength--;
            ActivePosition = Position - Remainder + 2;
        }

        private void AddLink(Node node)
        {
            if (ActiveLinkNode != null)
            {
                ActiveLinkNode.Link = node;
            }

            ActiveLinkNode = node;
        }

        private bool IsEndNode(Node node, int end)
        {
            // If no node specifies the current value, then our substring has reached max.
            if (node is null)
            {
                return true;
            }

            // if node goes to end of data, then this is the longest match.
            if (node.End > end)
            {
                return true;
            }

            // Or if this node explicitly specified the end of data.
            return node.End == EndOfData;
        }

        [DebuggerDisplay("Start = {Start}, Length = {Length}")]
        private class Node
        {
            private const int AllocationSize = AlphabetSize + 1;

            public Node(SuffixTree tree, int start, int end)
                : this(tree)
            {
                Start = start;
                End = end;
            }

            private Node(SuffixTree tree)
            {
                Debug.Assert(tree != null, "Tree cannot be null.");
                Tree = tree;

                Active = new int[AllocationSize];
                Children = new Node[AllocationSize];
            }

            public SuffixTree Tree
            {
                get;
                internal set;
            }

            public int Start
            {
                get;
                set;
            }

            public int End
            {
                get;
                set;
            }

            public int Length
            {
                get
                {
                    var last = End == EndOfData
                        ? Tree.Position
                        : End;

                    return last - Start;
                }
            }

            public SubstringInfo SubstringPointer
            {
                get
                {
                    return SubstringInfo.FromStartAndLength(Start, Length);
                }
            }

            public Node Link
            {
                get;
                set;
            }

            private Node[] Children
            {
                get;
                set;
            }

            private int[] Active
            {
                get;
                set;
            }

            private int ActiveSize
            {
                get;
                set;
            }

            public Node this[int key]
            {
                get
                {
                    return Children[key];
                }

                set
                {
                    // If we are setting a non-null node to a child index that is currently
                    // null, then we need to update the active index array to include this new index.
                    if (value != null)
                    {
                        if (this[key] is null)
                        {
                            Active[ActiveSize++] = key;
                        }
                    }

                    Children[key] = value;
                }
            }

            public Node Reset(int start, int end)
            {
                Start = start;
                End = end;
                Link = null;

                // Dereference every child node that we have set. A much smarter alternative
                // than iterating through every single child and seeing if null.
                for (var i = ActiveSize; --i >= 0;)
                {
                    this[Active[i]] = null;
                }

                ActiveSize = 0;

                return this;
            }

            public override string ToString()
            {
                return SubstringPointer.ToString();
            }
        }

        [DebuggerDisplay("Root")]
        private class RootNode : Node
        {
            public RootNode(SuffixTree tree)
                : base(tree, -1, -1)
            {
            }

            public void Reset()
            {
                Reset(-1, -1);
            }

            public override string ToString()
            {
                return "Root";
            }

            // Remove public access to this method.
            private new void Reset(int start, int end)
            {
                base.Reset(start, end);
            }
        }

        private class NodeCollection : List<Node>
        {
            public NodeCollection(SuffixTree tree, int capacity)
                : base(capacity)
            {
                Tree = tree;

                for (var i = capacity; --i >= 0;)
                {
                    Add(-1);
                }

                Clear();
            }

            public SuffixTree Tree
            {
                get;
                private set;
            }

            public new int Count
            {
                get;
                private set;
            }

            public Node Add(int position)
            {
                return Add(position, EndOfData);
            }

            public Node Add(int start, int end)
            {
                // If a node already exists in the list, then we simply reset its value.
                if (Count++ < base.Count)
                {
                    return this[Count - 1].Reset(start, end);
                }

                // Otherwise, we create a new node and add it to the list.
                var node = new Node(Tree, start, end);
                Add(node);
                return node;
            }

            public new void Clear()
            {
                // Rather than removing every existing node from the list, we set count to zero
                // and keep every node for future use.
                Count = 0;
            }
        }
    }
}
