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
        public string FullPath { get; }
        public string Name { get; }
        public ExplorerItemChangeType ChangeType { get; }
        internal ExplorerItemChangedEventArgs(string fullPath, string name, ExplorerItemChangeType changeType)
        {
            FullPath = fullPath;
            Name = name;
            ChangeType = changeType;
        }
    }

    public delegate void ExplorerItemChangedEventHandler(object sender, ExplorerItemChangedEventArgs e);
}
