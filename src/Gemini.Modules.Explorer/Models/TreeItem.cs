using Caliburn.Micro;
using Gemini.Framework.Commands;
using Gemini.Modules.MainMenu;
using Gemini.Modules.MainMenu.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gemini.Modules.Explorer.Models
{
    public abstract class TreeItem : PropertyChangedBase//, ITreeItem
    {
        public Guid DocumentId { get; set; }
        public abstract string Name { get; set; }
        public abstract string FullPath { get; set; }
        public abstract Uri IconSource { get; }
        public abstract bool CanOpenDocument { get; }
        public abstract IList<TreeItem> Children { get; }
        //public abstract IEnumerable<CommandDefinition> Commands { get; }
        public abstract TreeItem FindChildRecursive(string fullPath);
        public abstract TreeItem AddChild(string fullPath);
        public abstract void RemoveChild(string fullPath);
    }
}
