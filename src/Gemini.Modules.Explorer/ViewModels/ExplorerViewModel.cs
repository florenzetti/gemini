using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Services;
using Gemini.Modules.Explorer.Models;
using Gemini.Modules.Explorer.Services;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Gemini.Modules.Explorer.ViewModels
{
    [Export(typeof(IExplorerTool))]
    public class ExplorerViewModel : Tool, IExplorerTool
    {
        public string FullPath => _explorerProvider.SourceName;

        private RelayCommand _openSourceCommand;
        public ICommand OpenOpenSourceCommand
        {
            get { return _openSourceCommand ?? (_openSourceCommand = new RelayCommand(o => OpenSource())); }
        }

        private readonly IShell _shell;
        private readonly IExplorerProvider _explorerProvider;
        private readonly IEditorProvider _editorProvider;
        public bool IsSourceOpened => _explorerProvider.IsOpened;

        public override PaneLocation PreferredLocation
        {
            get { return PaneLocation.Left; }
        }

        public ITreeItem SourceTree { get; private set; }

        [ImportingConstructor]
        public ExplorerViewModel(IShell shell, IExplorerProvider explorerProvider, IEditorProvider editorProvider)
        {
            _shell = shell;
            _explorerProvider = explorerProvider;
            _explorerProvider.ItemCreated += OnExplorerProviderItemCreated;
            _explorerProvider.ItemDeleted += OnExplorerProviderItemDeleted;
            _explorerProvider.ItemRenamed += OnExplorerProviderItemRenamed;
            _editorProvider = editorProvider;
            DisplayName = Properties.Resources.ExplorerViewModel_ExplorerViewModel_Explorer;
        }

        private void OnExplorerProviderItemRenamed(object sender, ExplorerItemRenamedEventArgs e)
        {
            var treeItem = SourceTree.FindChildRecursive(e.OldFullPath);
            if (treeItem != null)
            {
                treeItem.Name = e.Name;
                treeItem.FullPath = e.FullPath;
            }
        }

        private void OnExplorerProviderItemDeleted(object sender, ExplorerItemChangedEventArgs e)
        {
            SourceTree.RemoveChild(e.FullPath);
        }

        private void OnExplorerProviderItemCreated(object sender, ExplorerItemChangedEventArgs e)
        {
            SourceTree.AddChild(e.FullPath);
        }

        public void OpenSource()
        {
            SourceTree = _explorerProvider.Open();
            NotifyOfPropertyChange(() => SourceTree);
            NotifyOfPropertyChange(() => IsSourceOpened);
        }

        public void CloseSource()
        {
            SourceTree = null;
            _explorerProvider.Close();
            NotifyOfPropertyChange(() => SourceTree);
            NotifyOfPropertyChange(() => IsSourceOpened);
        }

        public async Task OpenItemAsync(ITreeItem item)
        {
            if (item.CanOpenDocument)
                return;

            var editor = _shell.Documents.FirstOrDefault(o => o.Id == item.DocumentId);
            if (editor == null)
            {
                editor = _editorProvider.Create();

                var viewAware = (IViewAware)editor;
                viewAware.ViewAttached += (sender, e) =>
                {
                    var frameworkElement = (FrameworkElement)e.View;

                    RoutedEventHandler loadedHandler = null;
                    loadedHandler = async (sender2, e2) =>
                    {
                        frameworkElement.Loaded -= loadedHandler;
                        await _editorProvider.Open(editor, item.FullPath);
                    };
                    frameworkElement.Loaded += loadedHandler;
                };

                item.DocumentId = editor.Id;
            }

            await _shell.OpenDocumentAsync(editor);
        }
    }
}
