using Gemini.Framework;
using Gemini.Modules.Explorer.Models;
using Gemini.Modules.Explorer.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DragNDrop;
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
            TreeView.DragCommand = new RelayCommand(TreeItemDrag);
            TreeView.DropCommand = new RelayCommand(TreeItemDrop, CanExecuteTreeItemDrop);
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
            foreach (var item in e.ItemsToUnSelect)
                viewModel.SelectedItems.Remove((TreeItem)item);
            viewModel.RefreshContextMenu();
        }

        private void OnTreeItemEditing(object sender, RoutedEventArgs e)
        {
            ((ExplorerViewModel)DataContext).OnTreeItemEditing();
            e.Handled = true;
        }

        private void OnTreeItemEdited(object sender, RoutedEventArgs e)
        {
            var item = ((TreeViewExItem)sender).DataContext as TreeItem;
            ((ExplorerViewModel)DataContext).OnTreeItemEdited(item.FullPath, item.Name);
            e.Handled = true;
        }

        private void TreeItemDrag(object parameter)
        {
            var dragParameters = (DragParameters)parameter;

            var draggedItems = new List<TreeItem>();
            foreach (var controlTreeItem in dragParameters.Items)
            {
                draggedItems.Add((TreeItem)controlTreeItem.DataContext);
            }

            dragParameters.Data.SetData("DraggedItems", draggedItems);
        }

        private void TreeItemDrop(object parameter)
        {
            var dropParameters = (DropParameters)parameter;
            var dropToItem = (TreeItem)dropParameters.DropToItem.DataContext;
            var dataObject = dropParameters.Data as IDataObject;

            if (dataObject.GetDataPresent("DraggedItems"))
            {
                var draggedItems = dataObject.GetData("DraggedItems") as IEnumerable<TreeItem>;
                ((ExplorerViewModel)DataContext).OnTreeItemsMoved(dropToItem, draggedItems);
            }
        }

        private bool CanExecuteTreeItemDrop(object parameter)
        {
            var dropParameters = (DropParameters)parameter;
            var folderToDrop = dropParameters.DropToItem?.DataContext as FolderTreeItem;

            if (folderToDrop == null || dropParameters.Index != -1)
                return false;

            return true;
        }
    }
}
