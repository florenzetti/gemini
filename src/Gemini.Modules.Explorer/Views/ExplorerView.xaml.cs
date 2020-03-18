using Gemini.Modules.Explorer.Models;
using Gemini.Modules.Explorer.ViewModels;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace Gemini.Modules.Explorer.Views
{
    /// <summary>
    /// Interaction logic for ExplorerView.xaml
    /// </summary>
    public partial class ExplorerView : UserControl
    {
        public ExplorerView()
        {
            InitializeComponent();
            TreeView.OnSelecting += OnTreeViewSelecting;
        }

        private void OnItemMouseDoubleClick(object sender, MouseButtonEventArgs args)
        {
            var item = ((TreeViewExItem)sender).DataContext as TreeItem;
            ((ExplorerViewModel)DataContext).OpenItemAsync(item);
        }

        private void OnTreeViewSelecting(object sender, SelectionChangedCancelEventArgs e)
        {
            var viewModel = ((ExplorerViewModel)DataContext);
            foreach (var item in e.ItemsToSelect)
                viewModel.SelectedItems.Add((TreeItem)item);
            foreach(var item in e.ItemsToUnSelect)
                viewModel.SelectedItems.Remove((TreeItem)item);
            viewModel.RefreshContextMenu();
        }
    }
}
