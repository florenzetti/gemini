using Gemini.Framework.Commands;
using Gemini.Framework.Menus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Gemini.Modules.Explorer.Menus
{
    public abstract class ContextMenuItemDefinition : MenuDefinitionBase
    {
        public ContextMenuGroupDefinition Group { get; }

        public override int SortOrder { get; }

        public ContextMenuItemDefinition(ContextMenuGroupDefinition group, int sortOrder)
        {
            Group = group;
            SortOrder = sortOrder;
        }
    }
}
