using Gemini.Framework.Commands;

namespace Gemini.Modules.Explorer.Commands
{
    [CommandDefinition]
    public class TreeItemRenameCommandDefinition : CommandDefinition
    {
        public const string CommandName = "TreeItem.Rename";
        public override string Name => CommandName;
        public override string Text => "Rename";
        public override string ToolTip => "Rename";
    }
}
