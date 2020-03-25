using Caliburn.Micro;
using Gemini.Framework.Commands;
using Gemini.Framework.Threading;
using Gemini.Modules.Explorer.Commands;
using Gemini.Modules.Explorer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Gemini.Modules.Explorer.ViewModels
{
    public partial class ExplorerViewModel :
        ICommandListHandler<FolderTreeItemAddListDefinition>,
        ICommandHandler<TreeItemDeleteCommandDefinition>,
        ICommandHandler<TreeItemRenameCommandDefinition>
    {
        void ICommandHandler<TreeItemDeleteCommandDefinition>.Update(Command command) { }

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

        void ICommandHandler<TreeItemRenameCommandDefinition>.Update(Command command) { }

        Task ICommandHandler<TreeItemRenameCommandDefinition>.Run(Command command)
        {
            _selectedItems[0].IsEditing = true;
            return TaskUtility.Completed;
        }

        void ICommandListHandler<FolderTreeItemAddListDefinition>.Populate(Command command, List<Command> commands)
        {
            commands.Add(new Command(command.CommandDefinition)
            {
                Text = "New item..."
            });

        }

        async Task ICommandListHandler<FolderTreeItemAddListDefinition>.Run(Command command)
        {
            var addItemViewModel = IoC.Get<AddItemViewModel>();
            addItemViewModel.ParentItem = _selectedItems[0];
            await _windowManager.ShowDialogAsync(addItemViewModel);
        }
    }
}
