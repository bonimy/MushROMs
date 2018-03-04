// <copyright file="UndoFactory.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Helper
{
    using System;
    using System.Collections.Generic;

    public class UndoFactory : IUndoFactory
    {
        public UndoFactory()
        {
            History = new List<State>();
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

        public bool CanUndo
        {
            get
            {
                return Index > 0;
            }
        }

        public bool CanRedo
        {
            get
            {
                return Index < Count;
            }
        }

        private List<State> History
        {
            get;
        }

        public void Add(Action undo, Action redo)
        {
            if (undo is null)
            {
                throw new ArgumentNullException(nameof(undo));
            }

            if (redo is null)
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
                return;
            }

            History[--Index].Undo();
        }

        public void Redo()
        {
            if (!CanRedo)
            {
                return;
            }

            History[Index++].Redo();
        }

        private struct State
        {
            public State(Action undo, Action redo)
            {
                Undo = undo;
                Redo = redo;
            }

            public Action Undo
            {
                get;
            }

            public Action Redo
            {
                get;
            }
        }
    }
}
