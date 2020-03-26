using Gemini.Framework.Commands;
using Gemini.Framework.Menus;
using Gemini.Modules.Explorer.Models;
using Gemini.Modules.MainMenu.Models;
using System;
using System.ComponentModel.Composition;
using System.Linq;

namespace Gemini.Modules.Explorer.Menus
{
    [Export(typeof(ContextMenuBuilder))]
    public class ContextMenuBuilder
    {
        private readonly ICommandService _commandService;
        private readonly ContextMenuDefinition[] _menus;
        private readonly ContextMenuGroupDefinition[] _menuItemGroups;
        private readonly ContextMenuItemDefinition[] _menuItems;

        [ImportingConstructor]
        public ContextMenuBuilder(
            ICommandService commandService,
            [ImportMany] ContextMenuDefinition[] menus,
            [ImportMany] ContextMenuGroupDefinition[] menuItemGroups,
            [ImportMany] ContextMenuItemDefinition[] menuItems)
        {
            _commandService = commandService;
            _menus = menus;
            _menuItemGroups = menuItemGroups;
            _menuItems = menuItems;
        }

        public void BuildMenu(EditorFileTemplate itemType, ContextMenuModel result)
        {
            var itemContextMenus = _menus.Where(o => o.TargetTypes.Contains(itemType)).OrderBy(o => o.SortOrder).ToList();
            for (int i = 0; i < itemContextMenus.Count; i++)
            {
                var groups = _menuItemGroups
                    .Where(x => x.Parent == itemContextMenus[i])
                    .OrderBy(x => x.SortOrder);

                foreach (var group in groups)
                {
                    var menuItems = _menuItems
                    .Where(x => x.Group == group)
                    .OrderBy(x => x.SortOrder);

                    foreach (var menuItem in menuItems)
                    {
                        var menuItemModel = (menuItem.CommandDefinition != null)
                            ? new CommandMenuItem(_commandService.GetCommand(menuItem.CommandDefinition), new TextMenuItem(menuItem))
                            : (StandardMenuItem)new TextMenuItem(menuItem);
                        AddGroupsRecursive(menuItem, menuItemModel);
                        result.Add(menuItemModel);
                    }
                }

                if (i < itemContextMenus.Count - 1)
                    result.Add(new MenuItemSeparator());
            }
        }

        private void AddGroupsRecursive(MenuDefinitionBase menu, StandardMenuItem menuModel)
        {
            var groups = _menuItemGroups
                .Where(x => x.Parent == menu)
                .OrderBy(x => x.SortOrder)
                .ToList();

            for (int i = 0; i < groups.Count; i++)
            {
                var group = groups[i];
                var menuItems = _menuItems
                    .Where(x => x.Group == group)
                    .OrderBy(x => x.SortOrder);

                foreach (var menuItem in menuItems)
                {
                    var menuItemModel = (menuItem.CommandDefinition != null)
                        ? new CommandMenuItem(_commandService.GetCommand(menuItem.CommandDefinition), menuModel)
                        : (StandardMenuItem)new TextMenuItem(menuItem);
                    AddGroupsRecursive(menuItem, menuItemModel);
                    menuModel.Add(menuItemModel);
                }

                if (i < groups.Count - 1 && menuItems.Any())
                    menuModel.Add(new MenuItemSeparator());
            }
        }
    }
}
