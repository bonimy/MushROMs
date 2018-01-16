// <copyright file="SuffixTree.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

/*
A suffix tree based on Ukkonen's algorithm for byte values. I followed the ideas based in this helpful article:
http://stackoverflow.com/questions/9452701/ukkonens-suffix-tree-algorithm-in-plain-english/9513423#9513423

The code written here is based on a C++ implementation given here:
http://pastie.org/5925809

A note on memory management:
Try creating as few new suffix tree classes as possible. Every time a new node is created, it has to initialize an array of child nodes of size equal to the alphabet size (257). For large sets of data, this operation becomes expensive and time-consuming. To counter the performance hit, the SuffixTree class can be reused. The class keeps a list of every used node. When the class is reset to implement a new tree structure, it simply erases all of the nodes information and reference data without letting the class itself be erased from memory. This way, the next tree creation (if another one will be made) does not lose time on allocating new memory. The performance gains are very significant.
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Helper
{
    public class SuffixTree
    {
        public const int TerminationValue = AlphabetSize;
        public const int AlphabetSize = Byte.MaxValue + 1;
        private const int EndOfData = SubstringPointer.EndOfString;

        private const int FallbakNodeCollectionSize = 0x1000;

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

        public int Size
        {
            get;
            private set;
        }

        public SuffixTree()
        {
            // Create a large node collection now to save time in the future.
            Nodes = new NodeCollection(this, FallbakNodeCollectionSize);

            Root = new RootNode(this);
        }

        private void Initialize(int size)
        {
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(size),
                    SR.ErrorLowerBoundExclusive(nameof(size), size, 0));
            }

            Nodes.Clear();

            size++;                 // +1 for the termination value.
            Data = new int[size];
            Size = size;

            Position = -1;
            Remainder = 0;
            ActiveLength = 0;
            ActivePosition = 0;

            Root.Reset();
            ActiveNode = Root;
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
                    CreateTree(ptr, data.Length, start, size);
            }
        }

        public void CreateTree(IntPtr data, int length)
        {
            unsafe
            {
                CreateTree((byte*)data, length, 0, length);
            }
        }

        public void CreateTree(IntPtr data, int length, int start, int size)
        {
            unsafe
            {
                CreateTree((byte*)data, length, start, size);
            }
        }

        private unsafe void CreateTree(byte* data, int length, int start, int size)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (start < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(start),
                    SR.ErrorLowerBoundExclusive(nameof(start), start, 0));
            }

            if (start + size > length)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(size),
                    SR.ErrorArrayRange(nameof(size), size, nameof(data), length, start));
            }

            Initialize(size);

            data += start;
            for (var i = 0; i < size; i++)
            {
                Add(data[i]);
            }

            Add(TerminationValue);
            Position++;
        }

        private void Add(int value)
        {
            // Update position and write new value.
            Data[++Position] = value;

            // [?] Why do we do this?
            ActiveLinkNode = null;

            // [?] Is there a smarter way to go through this loop?
            Remainder++;
            do
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
                    // Determine if current substring exceeds the size of the active child node's.
                    var edge = stem.Length;
                    if (ActiveLength >= edge)
                    {
                        ActivePosition += edge;
                        ActiveLength -= edge;
                        ActiveNode = stem;
                        continue;
                    }

                    // If the node and substring still match, update active point and terminate sequence.
                    if (Data[stem.Start + ActiveLength] == value)
                    {
                        ActiveLength++;

                        // If we are in an internal node needing a suffix link, we chain this node to those.
                        AddLink(ActiveNode);
                        break;
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

                Remainder--;

                if (ActiveNode == Root && ActiveLength > 0)
                {
                    ActiveLength--;
                    ActivePosition = Position - Remainder + 1;
                }
                else
                {
                    ActiveNode = ActiveNode.Link ?? Root;
                }
            }
            while (Remainder > 0);
        }

        private void AddLink(Node node)
        {
            if (ActiveLinkNode != null)
            {
                ActiveLinkNode.Link = node;
            }

            ActiveLinkNode = node;
        }

        public SubstringPointer GetLongestInternalSubstring(int index)
        {
            if (index < 0 || index >= Size - 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(index),
                    SR.ErrorArrayBounds(nameof(index), index, Size - 1));
            }

            unsafe
            {
                fixed (int* ptr = Data)
                {
                    var node = (Node)Root;
                    var result = SubstringPointer.Empty;

                    for (int i = index, length = 0; i < Size;)
                    {
                        // update to current node.
                        var value = ptr[i];
                        node = node[value];

                        // If no node specifies the current value, then our substring has reached max.
                        if (node is null)
                        {
                            return result;
                        }

                        // if node goes to end of data, then this is the longest match.
                        if (node.End > index + length || node.End == -1)
                        {
                            return result;
                        }

                        // update status to current node's position.
                        i += node.Length;
                        length += node.Length;
                        result = SubstringPointer.FromLengthAndEnd(length, node.End);
                    }

                    return result;
                }
            }
        }

        [DebuggerDisplay("Start = {Start}, Length = {Length}")]
        private class Node
        {
            private const int AllocationSize = AlphabetSize + 1;

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
                    return (End == -1 ? Tree.Position : End) - Start;
                }
            }

            public SubstringPointer SubstringPointer
            {
                get
                {
                    return SubstringPointer.FromStartAndLength(Start, Length);
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

            private Node(SuffixTree tree)
            {
                Debug.Assert(tree != null, "Tree cannot be null.");
                Tree = tree;

                Active = new int[AllocationSize];
                Children = new Node[AllocationSize];
            }

            public Node(SuffixTree tree, int start, int end) : this(tree)
            {
                Start = start;
                End = end;
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
            public RootNode(SuffixTree tree) : base(tree, -1, -1)
            {
            }

            // Remove public access to this method.
            private new void Reset(int start, int end)
            {
                base.Reset(start, end);
            }

            public void Reset()
            {
                Reset(-1, -1);
            }

            public override string ToString()
            {
                return "Root";
            }
        }

        private class NodeCollection : List<Node>
        {
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

            public NodeCollection(SuffixTree tree, int capacity) : base(capacity)
            {
                Tree = tree;

                for (var i = capacity; --i >= 0;)
                {
                    Add(-1);
                }

                Clear();
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
