using System;
using System.Collections.Generic;
using System.Text;

namespace Gemini.Modules.Explorer.Models
{
    public interface ITreeItem
    {
        Guid DocumentId { get; set; }
        string Name { get; set; }
        string FullPath { get; set; }
        Uri IconSource { get; }
        bool CanOpenDocument { get; }
        IList<ITreeItem> Children { get; }
        ITreeItem FindChildRecursive(string fullPath);
        ITreeItem AddChild(string fullPath);
        void RemoveChild(string fullPath);
    }
}
