using Gemini.Framework.Commands;

namespace Gemini.Modules.Explorer.Commands
{
    [CommandDefinition]
    public class TreeItemDeleteCommandDefinition : CommandDefinition
    {
        public const string CommandName = "TreeItem.Delete";
        public override string Name => CommandName;
        public override string Text => "Delete";
        public override string ToolTip => "Delete";
    }
}
