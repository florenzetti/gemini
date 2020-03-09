using Gemini.Framework.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gemini.Modules.Explorer.Models
{
    public interface ITreeItem
    {
        string Name { get; set; }
        string FullPath { get; set; }
        Uri IconSource { get; }
        bool CanOpenDocument { get; }
        IList<TreeItem> Children { get; }
        IEnumerable<CommandDefinition> Commands { get; }
    }
}
