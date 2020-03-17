using Gemini.Modules.Explorer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gemini.Modules.Explorer.Services
{
    public class ExplorerItemRenamedEventArgs : ExplorerItemChangedEventArgs
    {
        public TreeItem OldItem { get; }
        internal ExplorerItemRenamedEventArgs(TreeItem item, TreeItem oldItem)
            : base(item, ExplorerItemChangeType.Renamed)
        {
            OldItem = oldItem;
        }
    }

    public delegate void ExplorerItemRenamedEventHandler(object sender, ExplorerItemRenamedEventArgs e);
}
