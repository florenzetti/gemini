namespace Gemini.Framework.Menus.ContextMenu
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
