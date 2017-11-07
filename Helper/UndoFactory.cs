// <copyright file="UndoFactory.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System;
using System.Collections.Generic;

namespace Helper
{
    public class UndoFactory
    {
        private List<State> History
        {
            get;
            set;
        }

        public int Count
        {
            get
            {
                return History.Count;
            }
        }

        public int Index
        {
            get;
            private set;
        }

        public virtual bool CanUndo
        {
            get
            {
                return Index > 0;
            }
        }

        public virtual bool CanRedo
        {
            get
            {
                return Index < Count;
            }
        }

        public UndoFactory()
        {
            History = new List<State>();
        }

        public void Add(Action undo, Action redo)
        {
            if (undo == null)
            {
                throw new ArgumentNullException(nameof(undo));
            }

            if (redo == null)
            {
                throw new ArgumentNullException(nameof(redo));
            }

            if (Index < Count)
            {
                History.RemoveRange(Index, Count - Index);
            }

            History.Add(new State(undo, redo));
            Index++;
        }

        public void Undo()
        {
            if (CanUndo)
            {
                // We have to check this any since CanUndo can be overridden.
                if (Index <= 0)
                {
                    throw new InvalidOperationException();
                }

                History[--Index].Undo();
            }
        }

        public void Redo()
        {
            if (CanRedo)
            {
                // We have to check this any since CanRedo can be overridden.
                if (Index >= Count)
                {
                    throw new InvalidOperationException();
                }

                History[Index++].Redo();
            }
        }

        private struct State
        {
            public Action Undo
            {
                get;
                private set;
            }

            public Action Redo
            {
                get;
                private set;
            }

            public State(Action undo, Action redo)
            {
                Undo = undo;
                Redo = redo;
            }
        }
    }
}
