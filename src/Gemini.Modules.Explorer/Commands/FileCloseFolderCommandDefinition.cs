using Gemini.Framework.Commands;
using Gemini.Modules.Explorer.Properties;

namespace Gemini.Modules.Explorer.Commands
{
    [CommandDefinition]
    public class FileCloseFolderCommandDefinition : CommandDefinition
    {
        public const string CommandName = "File.CloseFolder";
        public override string Name => CommandName;

        public override string Text => Resources.CloseFolderCommandText;

        public override string ToolTip => Resources.CloseFolderCommandToolTip;
    }
}
