// <copyright file="IEditor.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs
{
    using System;

    public interface IEditor : IEquatable<IEditor>
    {
        string Path
        {
            get;
            set;
        }

        bool CanUndo
        {
            get;
        }

        bool CanRedo
        {
            get;
        }

        bool CanCut
        {
            get;
        }

        bool CanCopy
        {
            get;
        }

        bool CanPaste
        {
            get;
        }

        bool CanDelete
        {
            get;
        }

        bool CanSelectAll
        {
            get;
        }
    }
}
