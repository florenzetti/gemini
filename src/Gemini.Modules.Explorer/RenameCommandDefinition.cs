using Gemini.Framework.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gemini.Modules.Explorer
{
    [CommandDefinition]
    public class RenameCommandDefinition : CommandDefinition
    {
        public const string CommandName = "Explorer.Rename";
        public override string Name => CommandName;

        public override string Text => "Rename";

        public override string ToolTip => "Rename";
    }
}
