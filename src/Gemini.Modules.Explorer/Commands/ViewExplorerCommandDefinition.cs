using Gemini.Framework.Commands;
using Gemini.Modules.Explorer.Properties;

namespace Gemini.Modules.Explorer.Commands
{
    [CommandDefinition]
    public class ViewExplorerCommandDefinition : CommandDefinition
    {
        public const string CommandName = "View.Explorer";
        public override string Name => CommandName;
        public override string Text => Resources.ExplorerText;
        public override string ToolTip => Resources.ExplorerToolTip;
    }
}
