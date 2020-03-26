using Gemini.Framework.Commands;
using Gemini.Framework.Threading;
using Gemini.Modules.Explorer.Services;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace Gemini.Modules.Explorer.Commands
{
    [CommandHandler]
    public class TreeItemRenameCommandHandler : ICommandHandler<TreeItemRenameCommandDefinition>
    {
        private readonly IExplorerProvider _explorerProvider;

        [ImportingConstructor]
        public TreeItemRenameCommandHandler(IExplorerProvider explorerProvider)
        {
            _explorerProvider = explorerProvider;
        }
        void ICommandHandler<TreeItemRenameCommandDefinition>.Update(Command command) { }

        Task ICommandHandler<TreeItemRenameCommandDefinition>.Run(Command command)
        {
            _explorerProvider.SourceTree.GetAllRecursive().First(o => o.IsSelected).IsEditing = true;
            return TaskUtility.Completed;
        }
    }
}
