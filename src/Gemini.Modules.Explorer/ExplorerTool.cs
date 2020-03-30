using Gemini.Framework.Services;
using Gemini.Modules.Explorer.ContextMenu;
using Gemini.Modules.Explorer.Services;
using Gemini.Modules.Explorer.ViewModels;

namespace Gemini.Modules.Explorer
{
    public abstract class ExplorerTool<TExplorerProvider> : ExplorerViewModel
        where TExplorerProvider : IExplorerProvider
    {
        public ExplorerTool(IShell shell,
            TExplorerProvider explorerProvider,
            IEditorProvider[] editorProviders,
            ContextMenuBuilder menuBuilder)
            : base(shell, explorerProvider, editorProviders, menuBuilder)
        {
        }
    }
}
