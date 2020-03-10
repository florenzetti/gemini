using Gemini.Framework.Commands;
using Gemini.Modules.Explorer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gemini.Modules.Explorer.Commands
{
    public abstract class MenuContextCommandHandler<TCommandDefinition> : CommandHandlerBase<TCommandDefinition>
        , IMenuContextCommandHandler<TCommandDefinition>
        where TCommandDefinition : CommandDefinition
    {
        public abstract void UpdateTreeItem(TreeItem item);
    }
}
