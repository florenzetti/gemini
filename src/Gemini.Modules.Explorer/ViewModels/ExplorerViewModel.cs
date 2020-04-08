using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Services;
using Gemini.Modules.Explorer.Models;
using Gemini.Modules.Explorer.Properties;
using Gemini.Modules.MainMenu;
using Gemini.Modules.MainMenu.Models;
using System.Collections.Generic;
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
        private readonly IShell _shell;
        private readonly IExplorerProvider _explorerProvider;
        private readonly IEditorProvider[] _editorProviders;
        private readonly Dictionary<EditorFileTemplate, ContextMenuModel> _menuModels;

        public string FullPath => _explorerProvider.SourceName;

        public string OpenSourceButtonText => $"{Resources.OpenText} {_explorerProvider.SourceName}";

        private RelayCommand _openSourceCommand;
        public ICommand OpenSourceCommand
        {
            get { return _openSourceCommand ?? (_openSourceCommand = new RelayCommand(o => OpenSource())); }
        }

        private RelayCommand _searchCommand;
        public ICommand SearchCommand
        {
            get { return _searchCommand ?? (_searchCommand = new RelayCommand(a => Search(a as string))); }
        }

        public bool IsSourceOpened => _explorerProvider.IsOpened;

        public override PaneLocation PreferredLocation
        {
            get { return PaneLocation.Left; }
        }

        public TreeItem SourceTree => _explorerProvider.SourceTree;


        public ContextMenuModel ContextMenuModel => RefreshContextMenu();

        [ImportingConstructor]
        public ExplorerViewModel(IShell shell,
            IExplorerProvider explorerProvider,
            [ImportMany]IEditorProvider[] editorProviders,
            ContextMenuBuilder menuBuilder
            )
        {
            _shell = shell;
            _explorerProvider = explorerProvider;
            _editorProviders = editorProviders;
            _menuModels = new Dictionary<EditorFileTemplate, ContextMenuModel>();
            foreach (var itemType in _explorerProvider.ItemTemplates)
            {
                var menuModel = new ContextMenuModel();
                menuBuilder.BuildMenu(itemType, menuModel);
                _menuModels.Add(itemType, menuModel);
            }

            DisplayName = Resources.ExplorerText;
        }

        public ContextMenuModel RefreshContextMenu()
        {
            if (!IsSourceOpened)
                return null;

            var item = _explorerProvider.SourceTree.AllSelectedItems.FirstOrDefault();
            return _menuModels[_explorerProvider.GetTemplate(item)];
        }

        public void OpenSource()
        {
            _explorerProvider.OpenSource();

            NotifyOfPropertyChange(() => SourceTree);
            NotifyOfPropertyChange(() => IsSourceOpened);
        }

        public void CloseSource()
        {
            _explorerProvider.CloseSource();
            NotifyOfPropertyChange(() => SourceTree);
            NotifyOfPropertyChange(() => IsSourceOpened);
        }

        public async Task OpenItemAsync(TreeItem item)
        {
            if (!item.CanOpenDocument)
                return;

            var document = _shell.Documents.FirstOrDefault(o => o.Id == item.DocumentId);
            if (document == null)
            {
                var editor = _editorProviders.FirstOrDefault(o => o.Handles(item.FullPath));
                if (editor == null)
                    //TODO: error message
                    return;
                document = editor.Create();

                var viewAware = (IViewAware)document;
                viewAware.ViewAttached += (sender, e) =>
                {
                    var frameworkElement = (FrameworkElement)e.View;

                    RoutedEventHandler loadedHandler = null;
                    loadedHandler = async (sender2, e2) =>
                    {
                        frameworkElement.Loaded -= loadedHandler;
                        await editor.Open(document, item.FullPath);
                    };
                    frameworkElement.Loaded += loadedHandler;
                };

                item.DocumentId = document.Id;
            }

            await _shell.OpenDocumentAsync(document);
        }

        public void OnTreeItemEditing()
        {
        }

        public void OnTreeItemEdited(TreeItem item, string newName)
        {
            _explorerProvider.UpdateItem(item, newName);
        }

        public void OnTreeItemsMoved(TreeItem moveToParent, IEnumerable<TreeItem> itemsMoved)
        {
            foreach (var item in itemsMoved)
            {
                _explorerProvider.MoveItem(item, moveToParent);
            }
        }

        private void Search(string searchTerm)
        {
            SourceTree.Search(searchTerm);
            NotifyOfPropertyChange(() => SourceTree);
        }
    }
}
