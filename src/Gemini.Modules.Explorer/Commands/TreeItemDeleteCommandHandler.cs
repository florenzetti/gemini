using Gemini.Framework.Commands;
using Gemini.Framework.Services;
using Gemini.Framework.Threading;
using Gemini.Modules.Explorer.Properties;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Gemini.Modules.Explorer.Commands
{
    [CommandHandler]
    public class TreeItemDeleteCommandHandler : ICommandHandler<TreeItemDeleteCommandDefinition>
    {
        private readonly IExplorerProvider _explorerProvider;

        [ImportingConstructor]
        public TreeItemDeleteCommandHandler(IExplorerProvider explorerProvider)
        {
            _explorerProvider = explorerProvider;
        }
        void ICommandHandler<TreeItemDeleteCommandDefinition>.Update(Command command) { }

        Task ICommandHandler<TreeItemDeleteCommandDefinition>.Run(Command command)
        {
            if (MessageBox.Show(Resources.DeleteConfirmationMessage, Resources.ConfirmationText, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                foreach (var selectedItem in _explorerProvider.SourceTree.GetAllRecursive().Where(o => o.IsSelected))
                {
                    _explorerProvider.DeleteItem(selectedItem);
                }
            }
            return TaskUtility.Completed;
        }
    }
}
