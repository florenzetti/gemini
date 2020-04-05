using System.Windows;
using System.Windows.Controls;

namespace Gemini.Modules.Explorer.Controls
{
    public class TreeViewItemSelectedEventArgs : RoutedEventArgs
    {
        public TreeViewExItem SelectedItem { get; }

        internal TreeViewItemSelectedEventArgs(RoutedEvent routedEvent, object source, TreeViewExItem selectedItem)
            : base(routedEvent, source)
        {
            SelectedItem = selectedItem;
        }
    }

    public delegate void TreeViewIemEditHandler(object sender, TreeViewItemSelectedEventArgs e);
}
