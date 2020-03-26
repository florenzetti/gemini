using Gemini.Framework.Commands;
using Gemini.Modules.Explorer.Properties;

namespace Gemini.Modules.Explorer.Commands
{
    [CommandDefinition]
    public class TreeItemDeleteCommandDefinition : CommandDefinition
    {
        public const string CommandName = "TreeItem.Delete";
        public override string Name => CommandName;
        public override string Text => Resources.DeleteText;
        public override string ToolTip => Resources.DeleteToolTip;
    }
}
