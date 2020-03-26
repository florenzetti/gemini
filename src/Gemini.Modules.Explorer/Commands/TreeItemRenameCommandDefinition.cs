using Gemini.Framework.Commands;
using Gemini.Modules.Explorer.Properties;

namespace Gemini.Modules.Explorer.Commands
{
    [CommandDefinition]
    public class TreeItemRenameCommandDefinition : CommandDefinition
    {
        public const string CommandName = "TreeItem.Rename";
        public override string Name => CommandName;
        public override string Text => Resources.RenameText;
        public override string ToolTip => Resources.RenameToolTip;
    }
}
