// <copyright file="MainForm.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.GenericEditor
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;
    using Helper;
    using static Helper.StringHelper;

    public sealed partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            RecentFiles = new RecentFileCollection(this);

            EventHandlerCallbacks =
                new Dictionary<ToolStripItem, Func<EventHandler>>()
            {
                { tsmNewFile, () => NewFileClick },
                { tsmOpenFile, () => OpenFileClick },
                { tsmCloseFile, () => CloseFileClick },
                { tsmSaveFile, () => SaveFileClick },
                { tsmSaveAs, () => SaveAsClick },
                { tsmSaveAll, () => SaveAllClick },
                { tsmUndo, () => UndoClick },
                { tsmRedo, () => RedoClick },
                { tsmCut, () => CutClick },
                { tsmCopy, () => CopyClick },
                { tsmPaste, () => PasteClick },
                { tsmDelete, () => DeleteClick },
                { tsmSelectAll, () => SelectAllClick },
                { tsmInvertColors, () => InvertColorsClick },
                { tsmBlend, () => BlendClick },
                { tsmColorize, () => ColorizeClick },
                { tsmGrayscale, () => GrayscaleClick },
                { tsmHorizontalGradient, () => HorizontalGradientClick },
                { tsmVerticalGradient, () => VerticalGradientClick },
                { tsmAbout, () => AboutClick },
                { tsbNewFile, () => NewFileClick },
                { tsbOpenFile, () => OpenFileClick },
                { tsbSaveFile, () => SaveFileClick },
                { tsbSaveAll, () => SaveAllClick },
                { tsbCut, () => CutClick },
                { tsbCopy, () => CopyClick },
                { tsbPaste, () => PasteClick },
                { tsbUndo, () => UndoClick },
                { tsbRedo, () => RedoClick },
                { tsbInvertColors, () => InvertColorsClick },
                { tsbBlend, () => BlendClick },
                { tsbColorize, () => ColorizeClick },
                { tsbGrayscale, () => GrayscaleClick },
                { tsbHorizontalGradient, () => HorizontalGradientClick },
                { tsbVerticalGradient, () => VerticalGradientClick },
                { tsbAbout, () => AboutClick },
            };

            foreach (var kvp in EventHandlerCallbacks)
            {
                var toolStripItem = kvp.Key;
                toolStripItem.Click += ToolStripItem_Click;
            }
        }

        public event EventHandler NewFileClick;

        public event EventHandler OpenFileClick;

        public event EventHandler<PathEventArgs> OpenRecentClick;

        public event EventHandler CloseFileClick;

        public event EventHandler SaveFileClick;

        public event EventHandler SaveAsClick;

        public event EventHandler SaveAllClick;

        public event EventHandler UndoClick;

        public event EventHandler RedoClick;

        public event EventHandler CutClick;

        public event EventHandler CopyClick;

        public event EventHandler PasteClick;

        public event EventHandler DeleteClick;

        public event EventHandler SelectAllClick;

        public event EventHandler InvertColorsClick;

        public event EventHandler BlendClick;

        public event EventHandler ColorizeClick;

        public event EventHandler GrayscaleClick;

        public event EventHandler HorizontalGradientClick;

        public event EventHandler VerticalGradientClick;

        public event EventHandler AboutClick;

        public string Status
        {
            get
            {
                return tssStatus.Text;
            }

            set
            {
                tssStatus.Text = value;
            }
        }

        public bool CanSave
        {
            get
            {
                return tsmSaveFile.Enabled;
            }

            set
            {
                tsmSaveFile.Enabled =
                tsbSaveFile.Enabled = value;
            }
        }

        public bool CanSaveAs
        {
            get
            {
                return tsmSaveAs.Enabled;
            }

            set
            {
                tsmSaveAs.Enabled = value;
            }
        }

        public bool CanSaveAll
        {
            get
            {
                return tsmSaveAll.Enabled;
            }

            set
            {
                tsmSaveAll.Enabled =
                tsbSaveAll.Enabled = value;
            }
        }

        public bool CanUndo
        {
            get
            {
                return tsmUndo.Enabled;
            }

            set
            {
                tsmUndo.Enabled =
                tsbUndo.Enabled = value;
            }
        }

        public bool CanRedo
        {
            get
            {
                return tsmRedo.Enabled;
            }

            set
            {
                tsmRedo.Enabled =
                tsbRedo.Enabled = value;
            }
        }

        public bool CanCut
        {
            get
            {
                return tsmCut.Enabled;
            }

            set
            {
                tsmCut.Enabled =
                tsbCut.Enabled = value;
            }
        }

        public bool CanCopy
        {
            get
            {
                return tsmCopy.Enabled;
            }

            set
            {
                tsmCopy.Enabled =
                tsbCopy.Enabled = value;
            }
        }

        public bool CanPaste
        {
            get
            {
                return tsmPaste.Enabled;
            }

            set
            {
                tsmPaste.Enabled =
                tsbPaste.Enabled = value;
            }
        }

        public bool CanDelete
        {
            get
            {
                return tsmDelete.Enabled;
            }

            set
            {
                tsmDelete.Enabled = value;
            }
        }

        public bool CanSelectAll
        {
            get
            {
                return tsmSelectAll.Enabled;
            }

            set
            {
                tsmSelectAll.Enabled = value;
            }
        }

        public IList<string> RecentFiles
        {
            get;
        }

        private IReadOnlyDictionary<ToolStripItem, Func<EventHandler>>
            EventHandlerCallbacks
        {
            get;
        }

        private void UpdateRecentFiles()
        {
            tsmOpenRecent.Enabled =
            tsbOpenRecent.Enabled = RecentFiles.Count > 0;

            tsmOpenRecent.DropDownItems.Clear();
            AddItems(tsmOpenRecent.DropDownItems.Add);

            cmsRecentFiles.Items.Clear();
            AddItems(cmsRecentFiles.Items.Add);

            void AddItems(Func<ToolStripItem, int> add)
            {
                for (var i = 0; i < RecentFiles.Count; i++)
                {
                    var file = RecentFiles[i];
                    var text = GetString("{0}. {1}", i + 1, file);

                    var tsi = new ToolStripMenuItem(text)
                    {
                        Tag = file
                    };

                    tsi.Click += OpenRecent_Click;

                    add(tsi);
                }
            }
        }

        private void OpenRecent_Click(object sender, EventArgs e)
        {
            var tsi = sender as ToolStripItem;
            var path = tsi.Tag as string;

            var recentFileArgs = new PathEventArgs(path);
            OpenRecentClick?.Invoke(this, recentFileArgs);
        }

        private void OpenRecent_ToolStripButtonClick(
            object sender,
            EventArgs e)
        {
            var control = toolStrip;
            var bounds = tsbOpenRecent.Bounds;
            var location = new Point(bounds.Right, bounds.Bottom);
            cmsRecentFiles.Show(control, location);
        }

        private void ToolStripItem_Click(object sender, EventArgs e)
        {
            var toolStripItem = sender as ToolStripItem;
            var eventHandler = EventHandlerCallbacks[toolStripItem]();

            eventHandler?.Invoke(this, EventArgs.Empty);
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private sealed class RecentFileCollection : IList<string>
        {
            private const int MaxRecentFiles = 10;

            public RecentFileCollection(MainForm mainForm)
            {
                MainForm = mainForm ??
                    throw new ArgumentNullException(nameof(mainForm));

                RecentFiles = new List<string>(MaxRecentFiles);
            }

            public int Count
            {
                get
                {
                    return RecentFiles.Count;
                }
            }

            bool ICollection<string>.IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            private MainForm MainForm
            {
                get;
            }

            private IList<string> RecentFiles
            {
                get;
            }

            public string this[int index]
            {
                get
                {
                    return RecentFiles[index];
                }

                set
                {
                    RecentFiles[index] = value;
                    MainForm.UpdateRecentFiles();
                }
            }

            public int IndexOf(string item)
            {
                var comparer = PathComparer.Default;
                for (var i = 0; i < Count; i++)
                {
                    if (comparer.Equals(this[i], item))
                    {
                        return i;
                    }
                }

                return -1;
            }

            public bool Contains(string item)
            {
                return IndexOf(item) != -1;
            }

            public void RemoveAt(int index)
            {
                RecentFiles.RemoveAt(index);
                MainForm.UpdateRecentFiles();
            }

            public void Add(string item)
            {
                if (item is null)
                {
                    throw new ArgumentNullException(nameof(item));
                }

                RemoveInternal(item);
                RecentFiles.Insert(0, item);
                RestrictSize();

                MainForm.UpdateRecentFiles();
            }

            public void Insert(int index, string item)
            {
                if (index >= MaxRecentFiles)
                {
                    index = MaxRecentFiles - 1;
                }

                RemoveInternal(item);
                RecentFiles.Insert(index, item);
                RestrictSize();

                MainForm.UpdateRecentFiles();
            }

            public void Clear()
            {
                RecentFiles.Clear();
                MainForm.UpdateRecentFiles();
            }

            public bool Remove(string item)
            {
                if (RemoveInternal(item))
                {
                    return false;
                }

                MainForm.UpdateRecentFiles();
                return true;
            }

            public void CopyTo(string[] array, int arrayIndex)
            {
                RecentFiles.CopyTo(array, arrayIndex);
            }

            public IEnumerator<string> GetEnumerator()
            {
                return RecentFiles.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            private bool RemoveInternal(string item)
            {
                // We need to use the path comparer.
                var index = IndexOf(item);
                if (index == -1)
                {
                    return false;
                }

                RecentFiles.RemoveAt(index);
                return true;
            }

            private void RestrictSize()
            {
                while (Count > MaxRecentFiles)
                {
                    RecentFiles.RemoveAt(Count - 1);
                }
            }
        }
    }
}
