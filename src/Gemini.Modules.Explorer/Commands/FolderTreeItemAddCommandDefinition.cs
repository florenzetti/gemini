using Gemini.Framework.Commands;

namespace Gemini.Modules.Explorer.Commands
{
    [CommandDefinition]
    public class FolderTreeItemAddCommandDefinition : CommandDefinition
    {
        public const string CommandName = "FolderTreeItem.Add";
        public override string Name => CommandName;
        public override string Text => "Add..";
        public override string ToolTip => "Add..";
    }
}
