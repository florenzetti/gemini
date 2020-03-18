using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Commands;
using Gemini.Framework.Services;
using Gemini.Framework.Threading;
using Gemini.Modules.Explorer.Commands;
using Gemini.Modules.Explorer.Menus;
//using Gemini.Modules.Explorer.Menus;
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
    public class ExplorerViewModel : Tool, IExplorerTool,
        ICommandHandler<FolderTreeItemAddCommandDefinition>,
        ICommandHandler<TreeItemDeleteCommandDefinition>,
        ICommandHandler<TreeItemRenameCommandDefinition>
    {
        private readonly IShell _shell;
        private readonly IExplorerProvider _explorerProvider;
        private readonly IEditorProvider _editorProvider;
        //private readonly ICommandRouter _commandRouter;
        private readonly ICommandService _commandService;
        private readonly IDictionary<Type, ContextMenuModel> _menuModels;

        public string FullPath => _explorerProvider.SourceName;

        private RelayCommand _openSourceCommand;
        public ICommand OpenOpenSourceCommand
        {
            get { return _openSourceCommand ?? (_openSourceCommand = new RelayCommand(o => OpenSource())); }
        }

        public bool IsSourceOpened => _explorerProvider.IsOpened;

        public override PaneLocation PreferredLocation
        {
            get { return PaneLocation.Left; }
        }

        public TreeItem SourceTree => _explorerProvider.SourceTree;

        private IList<TreeItem> _selectedItems;
        public IList<TreeItem> SelectedItems
        {
            get
            {
                if (_selectedItems == null)
                    _selectedItems = new BindableCollection<TreeItem>();
                return _selectedItems;
            }
        }

        public bool IsEditing { get; set; }

        private ContextMenuModel _contextMenuModel;
        public ContextMenuModel ContextMenuModel => _contextMenuModel;

        [ImportingConstructor]
        public ExplorerViewModel(IShell shell,
            IExplorerProvider explorerProvider,
            IEditorProvider editorProvider,
            ICommandService commandService,
            //,ICommandRouter commandRouter
            ContextMenuBuilder menuBuilder
            )
        {
            _shell = shell;
            _explorerProvider = explorerProvider;
            _explorerProvider.ItemCreated += OnExplorerProviderItemCreated;
            _explorerProvider.ItemDeleted += OnExplorerProviderItemDeleted;
            _explorerProvider.ItemRenamed += OnExplorerProviderItemRenamed;
            _editorProvider = editorProvider;
            _commandService = commandService;
            //_commandRouter = commandRouter;
            _menuModels = new Dictionary<Type, ContextMenuModel>();
            foreach (var itemType in _explorerProvider.ItemTypes)
            {
                var menuModel = new ContextMenuModel();
                menuBuilder.BuildMenu(itemType, menuModel);
                _menuModels.Add(itemType, menuModel);
            }

            DisplayName = Properties.Resources.ExplorerViewModel_ExplorerViewModel_Explorer;
        }

        private void OnExplorerProviderItemRenamed(object sender, ExplorerItemRenamedEventArgs e)
        {
            var treeItem = SourceTree.FindChildRecursive(e.Item.FullPath);
            if (treeItem != null)
            {
                treeItem.Name = e.NewName;
                treeItem.FullPath = e.NewFullPath;
            }
        }

        private void OnExplorerProviderItemDeleted(object sender, ExplorerItemChangedEventArgs e)
        {
            SourceTree.RemoveChild(e.Item);
        }

        private void OnExplorerProviderItemCreated(object sender, ExplorerItemChangedEventArgs e)
        {
            SourceTree.AddChild(e.Item);
        }

        public void RefreshContextMenu()
        {
            if (_menuModels.ContainsKey(_selectedItems[0].GetType()))
                _contextMenuModel = _menuModels[_selectedItems[0].GetType()];
            else
                _contextMenuModel = new ContextMenuModel();
            NotifyOfPropertyChange(() => ContextMenuModel);
        }

        public void OpenSource()
        {
            _explorerProvider.Open();
            NotifyOfPropertyChange(() => SourceTree);
            NotifyOfPropertyChange(() => IsSourceOpened);
        }

        public void CloseSource()
        {
            _explorerProvider.Close();
            NotifyOfPropertyChange(() => SourceTree);
            NotifyOfPropertyChange(() => IsSourceOpened);
        }

        public async Task OpenItemAsync(TreeItem item)
        {
            if (!item.CanOpenDocument)
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

        void ICommandHandler<TreeItemDeleteCommandDefinition>.Update(Command command)
        {
        }

        Task ICommandHandler<TreeItemDeleteCommandDefinition>.Run(Command command)
        {
            if (MessageBox.Show("Are you sure you want to delete this item?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _explorerProvider.EnableRaisingEvents = false;
                _explorerProvider.SourceTree.RemoveChild(_selectedItems[0]);
                _selectedItems.RemoveAt(0);
                _explorerProvider.EnableRaisingEvents = true;
            }
            return TaskUtility.Completed;
        }

        void ICommandHandler<TreeItemRenameCommandDefinition>.Update(Command command)
        {

        }

        Task ICommandHandler<TreeItemRenameCommandDefinition>.Run(Command command)
        {
            _explorerProvider.EnableRaisingEvents = false;
            _selectedItems[0].IsEditing = true;
            _explorerProvider.EnableRaisingEvents = true;
            return TaskUtility.Completed;
        }

        void ICommandHandler<FolderTreeItemAddCommandDefinition>.Update(Command command)
        {
        }

        Task ICommandHandler<FolderTreeItemAddCommandDefinition>.Run(Command command)
        {
            _explorerProvider.EnableRaisingEvents = false;
            _explorerProvider.SourceTree.AddChild(new FileSystemFileTreeItem() { Name = "new file.json", FullPath = _selectedItems[0].FullPath + @"\new file.json" });
            return TaskUtility.Completed;
        }
    }
}
