using Gemini.Framework.Commands;
using Gemini.Modules.Explorer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gemini.Modules.Explorer.Commands
{
    public interface IMenuContextCommandHandler<TCommandDefinition> : ICommandHandler<TCommandDefinition>
        where TCommandDefinition : CommandDefinition
    {
        void UpdateTreeItem(TreeItem item);
    }
}
