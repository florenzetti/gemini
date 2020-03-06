using System;
using System.Collections.Generic;
using System.Text;

namespace Gemini.Modules.Explorer.Services
{
    public class ExplorerItemRenamedEventArgs : ExplorerItemChangedEventArgs
    {
        public string OldFullPath { get; }
        public string OldName { get; }
        internal ExplorerItemRenamedEventArgs(string fullPath, string name, string oldFullPath, string oldName)
            : base(fullPath, name, ExplorerItemChangeType.Renamed)
        {
            OldFullPath = oldFullPath;
            OldName = oldName;
        }
    }

    public delegate void ExplorerItemRenamedEventHandler(object sender, ExplorerItemRenamedEventArgs e);
}
