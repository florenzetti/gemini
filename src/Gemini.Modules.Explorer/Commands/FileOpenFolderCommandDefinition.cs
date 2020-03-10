using Gemini.Framework.Commands;
using Gemini.Modules.Explorer.Properties;

namespace Gemini.Modules.Explorer.Commands
{
    [CommandDefinition]
    public class FileOpenFolderCommandDefinition : CommandDefinition
    {
        public const string CommandName = "File.OpenFolder";
        public override string Name => CommandName;
        public override string Text => Resources.OpenFolderCommandText;
        public override string ToolTip => Resources.OpenFolderCommandToolTip;
    }
}
