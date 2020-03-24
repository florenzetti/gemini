using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Commands;
using Gemini.Framework.Services;
using Gemini.Framework.Threading;
using Gemini.Modules.Explorer.Commands;
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
    public class ExplorerViewModel : Tool, IExplorerTool,
        ICommandListHandler<FolderTreeItemAddListDefinition>,
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

        public void OnTreeItemEditing()
        {
            _explorerProvider.EnableRaisingEvents = false;
        }

        public void OnTreeItemEdited()
        {
            _explorerProvider.EnableRaisingEvents = true;
        }

        public void OnTreeItemsMoved(TreeItem moveToParent, IEnumerable<TreeItem> itemsMoved)
        {
            _explorerProvider.EnableRaisingEvents = false;
            foreach (var item in itemsMoved)
            {
                item.MoveTo(moveToParent);
            }
            _explorerProvider.EnableRaisingEvents = true;
        }

        private void Search(string searchTerm)
        {
            SourceTree.Search(searchTerm);
            NotifyOfPropertyChange(() => SourceTree);
        }

        void ICommandHandler<TreeItemDeleteCommandDefinition>.Update(Command command)
        {
        }

        Task ICommandHandler<TreeItemDeleteCommandDefinition>.Run(Command command)
        {
            if (MessageBox.Show("Are you sure you want to delete this item(s)?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                foreach (var selectedItem in _selectedItems)
                {
                    _explorerProvider.EnableRaisingEvents = false;
                    var parentItem = selectedItem.Parent;
                    parentItem.RemoveChild(selectedItem);
                    _explorerProvider.EnableRaisingEvents = true;
                }
                _selectedItems.Clear();
            }
            return TaskUtility.Completed;
        }

        void ICommandHandler<TreeItemRenameCommandDefinition>.Update(Command command)
        {

        }

        Task ICommandHandler<TreeItemRenameCommandDefinition>.Run(Command command)
        {
            _selectedItems[0].IsEditing = true;
            return TaskUtility.Completed;
        }

        void ICommandListHandler<FolderTreeItemAddListDefinition>.Populate(Command command, List<Command> commands)
        {
            commands.Add(new Command(command.CommandDefinition)
            {
                Text = "Add new file"
            });

        }

        Task ICommandListHandler<FolderTreeItemAddListDefinition>.Run(Command command)
        {
            _explorerProvider.EnableRaisingEvents = false;
            _explorerProvider.SourceTree.AddChild(new FileSystemFileTreeItem("new file.json", _selectedItems[0].FullPath + @"\new file.json"));
            _explorerProvider.EnableRaisingEvents = true;
            return TaskUtility.Completed;
        }
    }
}
