using Gemini.Framework.Commands;
using Gemini.Framework.Menus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Gemini.Modules.Explorer.Menus
{
    public class ContextMenuGroupDefinition
    {
        public MenuDefinitionBase Parent { get; }
        public int SortOrder { get; }

        public ContextMenuGroupDefinition(MenuDefinitionBase parent, int sortOrder)
        {
            Parent = parent;
            SortOrder = sortOrder;
        }
    }
}
