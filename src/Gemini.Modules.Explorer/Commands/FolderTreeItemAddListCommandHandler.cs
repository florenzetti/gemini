using Caliburn.Micro;
using Gemini.Framework.Commands;
using Gemini.Framework.Services;
using Gemini.Modules.Explorer.Properties;
using Gemini.Modules.Explorer.Services;
using Gemini.Modules.Explorer.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gemini.Modules.Explorer.Commands
{
    [CommandHandler]
    public class FolderTreeItemAddListCommandHandler : ICommandListHandler<FolderTreeItemAddListCommandDefinition>
    {
        private readonly IExplorerProvider _explorerProvider;
        private readonly IWindowManager _windowManager;

        public enum CommandTypes
        {
            NewItem,
            NewFolder
        }

        [ImportingConstructor]
        public FolderTreeItemAddListCommandHandler(IExplorerProvider explorerProvider, IWindowManager windowManager)
        {
            _explorerProvider = explorerProvider;
            _windowManager = windowManager;
        }

        void ICommandListHandler<FolderTreeItemAddListCommandDefinition>.Populate(Command command, List<Command> commands)
        {
            commands.Add(new Command(command.CommandDefinition) { Text = $"{Resources.NewItemText}...", Tag = CommandTypes.NewItem });
            commands.Add(new Command(command.CommandDefinition) { Text = Resources.NewFolderText, Tag = CommandTypes.NewFolder });
        }

        async Task ICommandListHandler<FolderTreeItemAddListCommandDefinition>.Run(Command command)
        {
            var selectedItem = _explorerProvider.SourceTree.GetAllRecursive().First(o => o.IsSelected);
            switch (command.Tag)
            {
                case CommandTypes.NewItem:
                    var addItemViewModel = IoC.Get<AddItemViewModel>();
                    addItemViewModel.ParentItem = selectedItem;
                    await _windowManager.ShowDialogAsync(addItemViewModel);
                    break;
                case CommandTypes.NewFolder:
                    var folderName = selectedItem.Children.GetUniqueName(Resources.NewFolderText);
                    var folderFullPath = Path.Combine(selectedItem.FullPath, folderName);
                    var newFolder = _explorerProvider.CreateFolder(folderFullPath, folderName);
                    selectedItem.AddChild(newFolder);
                    break;
            }
        }
    }
}
