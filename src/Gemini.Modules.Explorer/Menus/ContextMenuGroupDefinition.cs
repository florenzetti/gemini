using Gemini.Framework.Menus;

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
