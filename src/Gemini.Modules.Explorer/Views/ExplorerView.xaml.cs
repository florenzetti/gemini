using Gemini.Modules.Explorer.Models;
using Gemini.Modules.Explorer.ViewModels;
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
        }

        private void OnItemMouseDoubleClick(object sender, MouseButtonEventArgs args)
        {
            var item = ((TreeViewExItem)sender).DataContext as ITreeItem;
            ((ExplorerViewModel)DataContext).OpenItemAsync(item);
        }
    }
}
