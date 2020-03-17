using Gemini.Modules.Explorer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gemini.Modules.Explorer.Services
{
    public enum ExplorerItemChangeType
    {
        Created,
        Renamed,
        Deleted
    }
    public class ExplorerItemChangedEventArgs : EventArgs
    {
        public TreeItem Item { get; }
        public ExplorerItemChangeType ChangeType { get; }
        internal ExplorerItemChangedEventArgs(TreeItem item, ExplorerItemChangeType changeType)
        {
            Item = item;
            ChangeType = changeType;
        }
    }

    public delegate void ExplorerItemChangedEventHandler(object sender, ExplorerItemChangedEventArgs e);
}
