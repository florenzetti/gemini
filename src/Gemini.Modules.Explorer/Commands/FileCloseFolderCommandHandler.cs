using Gemini.Framework.Commands;
using Gemini.Framework.Threading;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace Gemini.Modules.Explorer.Commands
{
    [CommandHandler]
    public class FileCloseFolderCommandHandler : CommandHandlerBase<FileCloseFolderCommandDefinition>
    {
        private readonly IExplorerTool _explorer;

        [ImportingConstructor]
        public FileCloseFolderCommandHandler(IExplorerTool explorer)
        {
            _explorer = explorer;
        }

        public override void Update(Command command)
        {
            if (!_explorer.IsSourceOpened)
                command.Enabled = false;
            else
                command.Enabled = true;
            base.Update(command);
        }

        public override Task Run(Command command)
        {
            _explorer.CloseSource();
            return TaskUtility.Completed;
        }
    }
}
