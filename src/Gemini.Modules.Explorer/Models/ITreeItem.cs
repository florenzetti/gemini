using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;

namespace Gemini.Modules.Explorer.Models
{
    public interface ITreeItem
    {
        string Name { get; set; }
        bool IsSelected { get; set; }
        bool IsExpanded { get; set; }
        bool IsEditing { get; set; }
        Uri IconSource { get; }
        Visibility Visibility { get; }
        IEnumerable Children { get; }
    }
}
