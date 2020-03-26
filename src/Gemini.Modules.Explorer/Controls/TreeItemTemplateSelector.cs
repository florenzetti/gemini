using Gemini.Modules.Explorer.Models;
using System.Windows;
using System.Windows.Controls;

namespace Gemini.Modules.Explorer.Controls
{
    public class TreeItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TreeItemTemplate { get; set; }
        public DataTemplate DefaultTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is TreeItem)
                return TreeItemTemplate;
            return DefaultTemplate;
        }
    }
}
