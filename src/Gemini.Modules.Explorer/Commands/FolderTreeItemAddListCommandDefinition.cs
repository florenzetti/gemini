using Gemini.Framework.Commands;

namespace Gemini.Modules.Explorer.Commands
{
    [CommandDefinition]
    public class FolderTreeItemAddListCommandDefinition : CommandListDefinition
    {
        public const string CommandName = "FolderTreeItem.Add";
        public override string Name => CommandName;
    }
}
