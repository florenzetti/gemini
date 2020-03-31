using Gemini.Framework.Services;
using Gemini.Modules.MainMenu;
using Gemini.Modules.Explorer.Services;
using System.ComponentModel.Composition;

namespace Gemini.Modules.Explorer
{
    [Export(typeof(IExplorerTool))]
    public class DirectoryExplorerTool : ExplorerTool<DirectoryExplorerProvider>
    {
        [ImportingConstructor]
        public DirectoryExplorerTool(IShell shell,
            DirectoryExplorerProvider explorerProvider,
            [ImportMany]IEditorProvider[] editorProviders,
            ContextMenuBuilder menuBuilder) : base(shell, explorerProvider, editorProviders, menuBuilder)
        {
        }
    }
}
