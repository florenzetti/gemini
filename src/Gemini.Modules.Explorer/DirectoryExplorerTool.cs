using Gemini.Framework.Services;
using Gemini.Modules.Explorer.ContextMenu;
using Gemini.Modules.Explorer.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

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
