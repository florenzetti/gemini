using Gemini.Framework.Commands;
using Gemini.Framework.Menus;
using Gemini.Modules.Explorer.Models;
using Gemini.Modules.Explorer.ViewModels;
using Gemini.Modules.MainMenu;
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
        //private readonly MenuBarDefinition[] _menuBars;
        private readonly ContextMenuDefinition[] _menus;
        private readonly ContextMenuItemGroupDefinition[] _menuItemGroups;
        private readonly ContextMenuItemDefinition[] _menuItems;
        //private readonly MenuDefinition[] _excludeMenus;
        //private readonly MenuItemGroupDefinition[] _excludeMenuItemGroups;
        //private readonly MenuItemDefinition[] _excludeMenuItems;

        [ImportingConstructor]
        public ContextMenuBuilder(
            ICommandService commandService,
            //[ImportMany] MenuBarDefinition[] menuBars,
            [ImportMany] ContextMenuDefinition[] menus,
            [ImportMany] ContextMenuItemGroupDefinition[] menuItemGroups,
            [ImportMany] ContextMenuItemDefinition[] menuItems)//,
                                                               //[ImportMany] ExcludeMenuDefinition[] excludeMenus,
                                                               //[ImportMany] ExcludeMenuItemGroupDefinition[] excludeMenuItemGroups,
                                                               //[ImportMany] ExcludeMenuItemDefinition[] excludeMenuItems)
        {
            _commandService = commandService;
            //_menuBars = menuBars;
            _menus = menus;
            _menuItemGroups = menuItemGroups;
            _menuItems = menuItems;
            //_excludeMenus = excludeMenus.Select(x => x.MenuDefinitionToExclude).ToArray();
            //_excludeMenuItemGroups = excludeMenuItemGroups.Select(x => x.MenuItemGroupDefinitionToExclude).ToArray();
            //_excludeMenuItems = excludeMenuItems.Select(x => x.MenuItemDefinitionToExclude).ToArray();
        }

        public void BuildMenu(Type itemType, ContextMenuModel result)
        {
            foreach (var menu in _menus.Where(o => o.TargetTypes.Contains(itemType)))
            {
                var groups = _menuItemGroups
                    .Where(x => x.Parent == menu)
                    //.Where(x => !_excludeMenus.Contains(x))
                    .OrderBy(x => x.SortOrder);

                foreach (var group in groups)
                {
                    var menuItems = _menuItems
                    .Where(x => x.Group == group)
                    //.Where(x => !_excludeMenuItems.Contains(x))
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
            }
        }

        private void AddGroupsRecursive(MenuDefinitionBase menu, StandardMenuItem menuModel)
        {
            var groups = _menuItemGroups
                .Where(x => x.Parent == menu)
                //.Where(x => !_excludeMenuItemGroups.Contains(x))
                .OrderBy(x => x.SortOrder)
                .ToList();

            for (int i = 0; i < groups.Count; i++)
            {
                var group = groups[i];
                var menuItems = _menuItems
                    .Where(x => x.Group == group)
                    //.Where(x => !_excludeMenuItems.Contains(x))
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
