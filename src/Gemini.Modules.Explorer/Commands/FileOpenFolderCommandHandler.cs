using Gemini.Framework.Commands;
using Gemini.Framework.Threading;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace Gemini.Modules.Explorer.Commands
{
    [CommandHandler]
    public class FileOpenFolderCommandHandler : CommandHandlerBase<FileOpenFolderCommandDefinition>
    {
        private readonly IExplorerTool _explorer;

        [ImportingConstructor]
        public FileOpenFolderCommandHandler(IExplorerTool explorer)
        {
            _explorer = explorer;
        }

        public override Task Run(Command command)
        {
            _explorer.OpenFolder();
            return TaskUtility.Completed;
        }
    }
}
