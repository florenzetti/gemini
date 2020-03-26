using Gemini.Framework.Menus;

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
