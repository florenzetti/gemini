using Gemini.Framework.Commands;
using Gemini.Modules.Explorer.Properties;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace Gemini.Modules.Explorer.Commands
{
    [CommandDefinition]
    public class TreeItemRenameCommandDefinition : CommandDefinition
    {
        public const string CommandName = "TreeItem.Rename";
        public override string Name => CommandName;
        public override string Text => Resources.RenameText;
        public override string ToolTip => Resources.RenameToolTip;

        [Export]
        public static CommandKeyboardShortcut KeyGesture = new CommandKeyboardShortcut<TreeItemRenameCommandDefinition>(new KeyGesture(Key.F2));
    }
}
