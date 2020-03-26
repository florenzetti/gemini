using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Services;
using Gemini.Modules.Explorer.Menus;
using Gemini.Modules.Explorer.Models;
using Gemini.Modules.Explorer.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Gemini.Modules.Explorer.ViewModels
{
    [Export(typeof(IExplorerTool))]
    public partial class ExplorerViewModel : Tool, IExplorerTool
    {
        private readonly IShell _shell;
        private readonly IExplorerProvider _explorerProvider;
        private readonly IEditorProvider[] _editorProviders;
        private readonly IDictionary<EditorFileTemplate, ContextMenuModel> _menuModels;

        public string FullPath => _explorerProvider.SourceName;

        private RelayCommand _openSourceCommand;
        public ICommand OpenOpenSourceCommand
        {
            get { return _openSourceCommand ?? (_openSourceCommand = new RelayCommand(o => OpenSource())); }
        }

        private RelayCommand _searchCommand;
        public ICommand SearchCommand
        {
            get { return _searchCommand == null ? _searchCommand = new RelayCommand(a => Search(a as string)) : _searchCommand; }
        }

        public bool IsSourceOpened => _explorerProvider.IsOpened;

        public override PaneLocation PreferredLocation
        {
            get { return PaneLocation.Left; }
        }

        public TreeItem SourceTree => _explorerProvider.SourceTree;

        //private IList<TreeItem> _selectedItems;
        public IList<TreeItem> SelectedItems
        {
            get
            {
                return _explorerProvider?.SourceTree?.GetAllRecursive().Where(o => o.IsSelected).ToList();
            }
        }

        private ContextMenuModel _contextMenuModel;
        public ContextMenuModel ContextMenuModel => _contextMenuModel;

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
            foreach (var itemType in _explorerProvider.ItemTypes)
            {
                var menuModel = new ContextMenuModel();
                menuBuilder.BuildMenu(itemType, menuModel);
                _menuModels.Add(itemType, menuModel);
            }

            DisplayName = Properties.Resources.ExplorerText;
        }

        public void RefreshContextMenu()
        {
            if (_menuModels.ContainsKey(SelectedItems[0].Template))
                _contextMenuModel = _menuModels[SelectedItems[0].Template];
            else
                _contextMenuModel = new ContextMenuModel();
            NotifyOfPropertyChange(() => ContextMenuModel);
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

        public void OnTreeItemEdited(string fullPath, string newName)
        {
            _explorerProvider.UpdateItem(fullPath, newName);
        }

        public void OnTreeItemsMoved(TreeItem moveToParent, IEnumerable<TreeItem> itemsMoved)
        {
            foreach (var item in itemsMoved)
            {
                string oldFullPath = item.FullPath;
                item.MoveTo(moveToParent);
                _explorerProvider.MoveItem(oldFullPath, item.FullPath);
            }
        }

        private void Search(string searchTerm)
        {
            SourceTree.Search(searchTerm);
            NotifyOfPropertyChange(() => SourceTree);
        }
    }
}
