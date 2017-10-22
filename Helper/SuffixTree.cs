// <copyright file="SuffixTree.cs>
//     Copyright (c) 2017 Nelson Garcia
// </copyright>

/* A suffix tree based on Ukkonen's algorithm for byte values. I followed the ideas based in this helpful article:
* http://stackoverflow.com/questions/9452701/ukkonens-suffix-tree-algorithm-in-plain-english/9513423#9513423
*
* The code written here is based on a C++ implementation given here:
* http://pastie.org/5925809
*
* A note on memory management:
* Try creating as few new suffix tree classes as possible. Every time a new node is created, it has to initialize an array of child nodes of size equal to the alphabet size (257). For large sets of data, this operation becomes expensive and time-consuming. To counter the performance hit, the SuffixTree class can be reused. The class keeps a list of every used node. When the class is reset to implement a new tree structure, it simply erases all of the nodes information and reference data without letting the class itself be erased from memory. This way, the next tree creation (if another one will be made) does not lose time on allocating new memory. The performance gains are very significant.
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Helper
{
    /// <summary>
    /// Specifies a generalized suffix tree for <see cref="Byte"/> arrays.
    /// </summary>
    /// <remarks>
    /// The suffix tree is generated using Ukkonen's algorithm.
    /// </remarks>
    /// <seealso cref="SubstringPointer"/>
    public class SuffixTree
    {
        /// <summary>
        /// A value that is outside of the standard <see cref="Byte"/> alphabet that signifies
        /// the end of a suffix branch.
        /// This field is constant.
        /// </summary>
        public const int TerminationValue = AlphabetSize;

        /// <summary>
        /// The total size of the alphabet, including <see cref="TerminationValue"/>.
        /// This field is constant.
        /// </summary>
        public const int AlphabetSize = Byte.MaxValue + 1;

        /// <summary>
        /// Specifies an index that extends to <see cref="Position"/>.
        /// This field is constant.
        /// </summary>
        private const int EndOfData = SubstringPointer.EndOfString;

        /// <summary>
        /// The default size of <see cref="Nodes"/>.
        /// </summary>
        private const int FallbakNodeCollectionSize = 0x1000;

        /// <summary>
        /// Gets or sets the <see cref="Node"/> containing the beginning of each suffix string.
        /// </summary>
        private RootNode Root
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a container for every <see cref="Node"/> created in <see cref="SuffixTree"/>.
        /// Used for recycling purposes.
        /// </summary>
        private NodeCollection Nodes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the current position in <see cref="Data"/>.
        /// </summary>
        private int Position
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="Node"/> that <see cref="ActiveNode"/> links to.
        /// </summary>
        private Node ActiveLinkNode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the number of <see cref="Node"/>s left to add.
        /// </summary>
        private int Remainder
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets <see cref="Root"/> if we are doing a direct insertion.
        /// Otherwise, it is the branch of a child node where a matching substring
        /// pattern exists.
        /// </summary>
        private Node ActiveNode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the data position of <see cref="ActiveNode"/>.
        /// </summary>
        private int ActivePosition
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a substring length at <see cref="ActivePosition"/>.
        /// </summary>
        private int ActiveLength
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a copy of all the data of the tree.
        /// </summary>
        private int[] Data
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the value of the passed tree data at <paramref name="index"/>.
        /// </summary>
        /// <param name="index">
        /// The zero-based index to read from.
        /// </param>
        /// <returns>
        /// The value of <see cref="Data"/> at <paramref name="index"/>.
        /// </returns>
        public int this[int index] => Data[index];

        /// <summary>
        /// Gets the size of the data passed to this <see cref="SuffixTree"/>, including
        /// <see cref="TerminationValue"/>.
        /// </summary>
        public int Size
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SuffixTree"/> class.
        /// </summary>
        public SuffixTree()
        {
            // Create a large node collection now to save time in the future.
            Nodes = new NodeCollection(this, FallbakNodeCollectionSize);

            Root = new RootNode(this);
        }

        /// <summary>
        /// Initializes the parameters of this <see cref="SuffixTree"/> with a given
        /// data size.
        /// </summary>
        /// <param name="size">
        /// The size of <see cref="Data"/> (excluding the termination value).
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="size"/> is less than zero.
        /// </exception>
        private void Initialize(int size)
        {
            if (size < 0)
                throw new ArgumentOutOfRangeException(nameof(size),
                    SR.ErrorLowerBoundExclusive(nameof(size), size, 0));

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

        /// <summary>
        /// Creates a suffix tree of <paramref name="data"/>.
        /// </summary>
        /// <param name="data">
        /// An array of <see cref="Byte"/>s to create a suffix tree of.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="data"/> is null.
        /// </exception>
        public void CreateTree(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            CreateTree(data, 0, data.Length);
        }

        /// <summary>
        /// Creates a suffix tree of <paramref name="data"/> at <paramref name="start"/>,
        /// with range specified by <paramref name="size"/>.
        /// </summary>
        /// <param name="data">
        /// An array of <see cref="Byte"/>s to create a suffix tree of.
        /// </param>
        /// <param name="start">
        /// The starting index to read data from.
        /// </param>
        /// <param name="size">
        /// The number of bytes to read from <paramref name="data"/> after <paramref name="start"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="data"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="start"/> is less than zero. -or-
        /// <paramref name="start"/> + <paramref name="size"/> is greater than length of
        /// <paramref name="data"/>
        /// </exception>
        public void CreateTree(byte[] data, int start, int size)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            unsafe
            {
                fixed (byte* ptr = data)
                    CreateTree(ptr, data.Length, start, size);
            }
        }

        /// <summary>
        /// Creates a suffix tree of <paramref name="data"/>.
        /// </summary>
        /// <param name="data">
        /// An array of <see cref="Byte"/>s to create a suffix tree of.
        /// </param>
        /// <param name="length">
        /// The size, in bytes, of <paramref name="data"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="data"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="length"/> is less than zero.
        /// </exception>
        public void CreateTree(IntPtr data, int length)
        {
            unsafe
            {
                CreateTree((byte*)data, length, 0, length);
            }
        }

        /// <summary>
        /// Creates a suffix tree of <paramref name="data"/> at <paramref name="start"/>,
        /// with range specified by <paramref name="size"/>.
        /// </summary>
        /// <param name="data">
        /// An array of <see cref="Byte"/>s to create a suffix tree of.
        /// </param>
        /// <param name="length">
        /// The size, in bytes, of <paramref name="data"/>.
        /// </param>
        /// <param name="start">
        /// The starting index to read data from.
        /// </param>
        /// <param name="size">
        /// The number of bytes to read from <paramref name="data"/> after <paramref name="start"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="data"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="start"/> is less than zero. -or-
        /// <paramref name="start"/> + <paramref name="size"/> is greater than <paramref name="length"/>.
        /// </exception>
        public void CreateTree(IntPtr data, int length, int start, int size)
        {
            unsafe
            {
                CreateTree((byte*)data, length, start, size);
            }
        }

        /// <summary>
        /// Creates a suffix tree of <paramref name="data"/> at <paramref name="start"/>,
        /// with range specified by <paramref name="size"/>.
        /// </summary>
        /// <param name="data">
        /// An array of <see cref="Byte"/>s to create a suffix tree of.
        /// </param>
        /// <param name="length">
        /// The size, in bytes, of <paramref name="data"/>.
        /// </param>
        /// <param name="start">
        /// The starting index to read data from.
        /// </param>
        /// <param name="size">
        /// The number of bytes to read from <paramref name="data"/> after <paramref name="start"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="data"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="start"/> is less than zero. -or-
        /// <paramref name="start"/> + <paramref name="size"/> is greater than <paramref name="length"/>.
        /// </exception>
        private unsafe void CreateTree(byte* data, int length, int start, int size)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (start < 0)
                throw new ArgumentOutOfRangeException(nameof(start),
                    SR.ErrorLowerBoundExclusive(nameof(start), start, 0));
            if (start + size > length)
                throw new ArgumentOutOfRangeException(nameof(size),
                    SR.ErrorArrayRange(nameof(size), size, nameof(data), length, start));

            Initialize(size);

            data += start;
            for (var i = 0; i < size; i++)
                Add(data[i]);
            Add(TerminationValue);
            Position++;
        }

        /// <summary>
        /// Adds <paramref name="value"/> to <see cref="Data"/> and updates the tree.
        /// </summary>
        /// <param name="value">
        /// The value to add to <see cref="Data"/>.
        /// </param>
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
                    ActivePosition = Position;

                // Get the active child node.
                var activeValue = Data[ActivePosition];
                var stem = ActiveNode[activeValue];

                // Create a new child node if it does not currently exist
                if (stem == null)
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
                    ActiveNode = ActiveNode.Link ?? Root;
            } while (Remainder > 0);
        }

        /// <summary>
        /// Add <paramref name="node"/> to the active link chain.
        /// </summary>
        /// <param name="node">
        /// The <see cref="Node"/> to link to.
        /// </param>
        private void AddLink(Node node)
        {
            if (ActiveLinkNode != null)
                ActiveLinkNode.Link = node;

            ActiveLinkNode = node;
        }

        /// <summary>
        /// Gets a <see cref="SubstringPointer"/> specifying the index of length of the longest
        /// substring before <paramref name="index"/> that matches the substring starting at
        /// <paramref name="index"/>.
        /// </summary>
        /// <param name="index">
        /// The index within <see cref="Data"/> to look at.
        /// </param>
        /// <returns>
        /// A <see cref="SubstringPointer"/> whose start position is less than
        /// <paramref name="index"/> and also specifies a substring that matches
        /// the substring starting at <paramref name="index"/> within <see cref="Data"/>.
        /// It's length is specified by the returning <see cref="SubstringPointer"/>'s length and
        /// is the longest such substring if more than one match was found. If no matching substring
        /// was found, <see cref="SubstringPointer.Empty"/> is returned.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than 0. -or-
        /// <paramref name="index"/> is greater than or equal to <see cref="Size"/> - 1.
        /// </exception>
        public SubstringPointer GetLongestInternalSubstring(int index)
        {
            if (index < 0 || index >= Size - 1)
                throw new ArgumentOutOfRangeException(nameof(index),
                    SR.ErrorArrayBounds(nameof(index), index, Size - 1));

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
                        if (node == null)
                            return result;

                        // if node goes to end of data, then this is the longest match.
                        if (node.End > index + length || node.End == -1)
                            return result;

                        // update status to current node's position.
                        i += node.Length;
                        length += node.Length;
                        result = SubstringPointer.FromLengthAndEnd(length, node.End);
                    }

                    return result;
                }
            }
        }

        /// <summary>
        /// A class that contains the substring information of each node in a <see cref="SuffixTree"/>.
        /// </summary>
        [DebuggerDisplay("Start = {Start}, Length = {Length}")]
        private class Node
        {
            /// <summary>
            /// The size of <see cref="Children"/>.
            /// </summary>
            private const int AllocationSize = AlphabetSize + 1;

            /// <summary>
            /// Gets the <see cref="SuffixTree"/> that owns this <see cref="Node"/>.
            /// </summary>
            public SuffixTree Tree
            {
                get;
                internal set;
            }

            /// <summary>
            /// Gets or sets the start index of the substring that describes this <see cref="Node"/>.
            /// </summary>
            public int Start
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the end index of the substring that describes this <see cref="Node"/>.
            /// </summary>
            public int End
            {
                get;
                set;
            }

            /// <summary>
            /// Gets the length of the substring that describes this <see cref="Node"/>.
            /// </summary>
            public int Length => (End == -1 ? Tree.Position : End) - Start;             /// <summary>

            /// Gets the <see cref="SubstringPointer"/> that describes this <see cref="Node"/>.
            /// </summary>
            public SubstringPointer SubstringPointer => SubstringPointer.FromStartAndLength(Start, Length);

            /// <summary>
            /// Gets or sets the <see cref="Node"/> that links to this <see cref="Node"/>
            /// </summary>
            /// <remarks>
            /// Link nodes are necessary when we split off of a non-root node to help
            /// us chain back to the root node.
            /// </remarks>
            public Node Link
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the children of this <see cref="Node"/>.
            /// </summary>
            /// <remarks>
            /// This array has a constant size of <see cref="AllocationSize"/>, and is indexed by the
            /// alphabet values. It is better to think of this array as a node dictionary.
            /// </remarks>
            private Node[] Children
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets a collection of indexes of every active child node.
            /// </summary>
            /// <remarks>
            /// When resetting the node, we remove the reference to every child
            /// whose index is in this array. This saves a substantial number of
            /// cycles as every node in <see cref="Children"/> is typically not
            /// set.
            /// </remarks>
            private int[] Active
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the number of index values in <see cref="Active"/>.
            /// </summary>
            private int ActiveSize
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the <see cref="Node"/> whose substring described by this
            /// <see cref="Node"/> has <paramref name="key"/> as its next value.
            /// </summary>
            /// <param name="key">
            /// A value contained by the suffix tree alphabet.
            /// </param>
            /// <returns>
            /// The <see cref="Node"/> whose substring described by this <see cref="Node"/> has
            /// <paramref name="key"/> as its next value.
            /// </returns>
            public Node this[int key]
            {
                get => Children[key];
                set
                {
                    // If we are setting a non-null node to a child index that is currently
                    // null, then we need to update the active index array to include this new index.
                    if (value != null)
                    {
                        if (this[key] == null)
                            Active[ActiveSize++] = key;
                    }
                    Children[key] = value;
                }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Node"/> class with
            /// <paramref name="tree"/> as its owner.
            /// </summary>
            /// <param name="tree">
            /// The <see cref="SuffixTree"/> of the <see cref="Node"/>.
            /// </param>
            private Node(SuffixTree tree)
            {
                Debug.Assert(tree != null);
                Tree = tree;

                Active = new int[AllocationSize];
                Children = new Node[AllocationSize];
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Node"/> class with
            /// <paramref name="tree"/> as its owner and with the specified
            /// <paramref name="start"/> and <paramref name="end"/> values.
            /// </summary>
            /// <param name="tree">
            /// The <see cref="SuffixTree"/> of the <see cref="Node"/>.
            /// </param>
            /// <param name="start">
            /// The start index of the substring describing this <see cref="Node"/>.
            /// </param>
            /// <param name="end">
            /// The end index of the substring describing this <see cref="Node"/>.
            /// </param>
            public Node(SuffixTree tree, int start, int end) : this(tree)
            {
                Start = start;
                End = end;
            }

            /// <summary>
            /// Resets the <see cref="Node"/> information with the given
            /// <paramref name="start"/> and <paramref name="end"/> values.
            /// </summary>
            /// <param name="start">
            /// The new value of <see cref="Start"/>.
            /// </param>
            /// <param name="end">
            /// The new value of <see cref="End"/>.
            /// </param>
            /// <returns>
            /// This <see cref="Node"/>.
            /// </returns>
            /// <remarks>
            /// <see cref="Link"/> and every child in <see cref="Children"/> are
            /// dereferenced.
            /// </remarks>
            public Node Reset(int start, int end)
            {
                Start = start;
                End = end;
                Link = null;

                // Dereference every child node that we have set. A much smarter alternative
                // than iterating through every single child and seeing if null.
                for (var i = ActiveSize; --i >= 0;)
                    this[Active[i]] = null;
                ActiveSize = 0;

                return this;
            }

            /// <summary>
            /// Converts this <see cref="Node"/> to a human-readable <see cref="String"/>.
            /// </summary>
            /// <returns>
            /// A <see cref="String"/> the represent this <see cref="Node"/>.
            /// </returns>
            public override string ToString() => SubstringPointer.ToString();
        }

        /// <summary>
        /// Specifies the root <see cref="Node"/> of a <see cref="SuffixTree"/>.
        /// </summary>
        [DebuggerDisplay("Root")]
        private class RootNode : Node
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="RootNode"/> class with
            /// <paramref name="tree"/> as its owner.
            /// </summary>
            /// <param name="tree">
            /// The <see cref="SuffixTree"/> of the <see cref="RootNode"/>.
            /// </param>
            public RootNode(SuffixTree tree) : base(tree, -1, -1)
            {
            }

            // Remove public access to this method.
            private new void Reset(int start, int end) =>
                base.Reset(start, end);

            /// <summary>
            /// Dereferences <see cref="Node.Link"/> and children in <see cref="Node.Children"/>.
            /// </summary>
            public void Reset() => Reset(-1, -1);

            /// <summary>
            /// Converts this <see cref="RootNode"/> to a human-readable <see cref="String"/>.
            /// </summary>
            /// <returns>
            /// A <see cref="String"/> the represent this <see cref="RootNode"/>.
            /// </returns>
            public override string ToString() => "Root";
        }

        /// <summary>
        /// Represents a collection of <see cref="Node"/>s that doesn't dispose of items
        /// in its collection.
        /// </summary>
        /// <remarks>
        /// <see cref="Node"/>s added to <see cref="NodeCollection"/> are not removed when calling
        /// <see cref="Clear"/>(). This way, if <see cref="Add(Int32)"/> or <see cref="Add(Int32, Int32)"/>
        /// is called, a previously "cleared" <see cref="Node"/> is reset with the specified parameters.
        /// This saves time against instantiating a new <see cref="Node"/> due to its large alphabet size.
        /// </remarks>
        private class NodeCollection : List<Node>
        {
            /// <summary>
            /// Gets the <see cref="SuffixTree"/> that this <see cref="NodeCollection"/> belongs to.
            /// </summary>
            public SuffixTree Tree
            {
                get;
                private set;
            }

            /// <summary>
            /// Gets the number of elements that are accessible in <see cref="NodeCollection"/>.
            /// </summary>
            public new int Count
            {
                get;
                private set;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="NodeCollection"/> with the given
            /// <see cref="SuffixTree"/> as its owner and has the specified initial capacity.
            /// </summary>
            /// <param name="tree">
            /// The <see cref="SuffixTree"/> that will own this <see cref="NodeCollection"/>.
            /// </param>
            /// <param name="capacity">
            /// The number of new elements that <see cref="NodeCollection"/> can initially store.
            /// </param>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="tree"/> is null.
            /// </exception>
            /// <remarks>
            /// When instantiated, <see cref="NodeCollection"/> will not be empty. Each element
            /// up to <paramref name="capacity"/> will be initialized. This implies there is a heavy
            /// initial cost, but with increased performance during use.
            /// </remarks>
            public NodeCollection(SuffixTree tree, int capacity) : base(capacity)
            {
                Tree = tree;

                for (var i = capacity; --i >= 0;)
                    Add(-1);
                Clear();
            }

            /// <summary>
            /// Adds a <see cref="Node"/> with <see cref="Node.Start"/> set to
            /// <paramref name="position"/> at the end of the <see cref="NodeCollection"/>.
            /// </summary>
            /// <param name="position">
            /// The value <see cref="Node.Start"/> will have.
            /// </param>
            /// <returns>
            /// The resulting <see cref="Node"/> in <see cref="NodeCollection"/>.
            /// </returns>
            /// <exception cref="ArgumentOutOfRangeException">
            /// <paramref name="position"/> is less than 0.
            /// </exception>
            /// <remarks>
            /// <see cref="Node.End"/> is set to <see cref="EndOfData"/>.
            /// </remarks>
            public Node Add(int position) => Add(position, EndOfData);

            /// <summary>
            /// Adds a <see cref="Node"/> with <see cref="Node.Start"/> set to
            /// <paramref name="start"/> and <see cref="Node.End"/> set to <paramref name="end"/>
            /// at the end of the <see cref="NodeCollection"/>.
            /// </summary>
            /// <param name="start">
            /// The value <see cref="Node.Start"/> will have.
            /// </param>
            /// <param name="end">
            /// The values <see cref="Node.End"/> will have.
            /// </param>
            /// <returns>
            /// The resulting <see cref="Node"/> in <see cref="NodeCollection"/>.
            /// </returns>
            /// <exception cref="ArgumentOutOfRangeException">
            /// <paramref name="start"/> is less than 0.
            /// </exception>
            public Node Add(int start, int end)
            {
                // If a node already exists in the list, then we simply reset its value.
                if (Count++ < base.Count)
                    return this[Count - 1].Reset(start, end);

                // Otherwise, we create a new node and add it to the list.
                var node = new Node(Tree, start, end);
                Add(node);
                return node;
            }

            /// <summary>
            /// Updates <see cref="Count"/> to 0.
            /// </summary>
            /// <remarks>
            /// The <see cref="Node"/>s are not actually removed from <see cref="NodeCollection"/>.
            /// </remarks>
            public new void Clear()
            {
                // Rather than removing every existing node from the list, we set count to zero
                // and keep every node for future use.
                Count = 0;
            }
        }
    }
}
