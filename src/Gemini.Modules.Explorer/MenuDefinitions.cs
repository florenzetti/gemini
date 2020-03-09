using Gemini.Framework.Menus;
using Gemini.Modules.Explorer.Commands;
//using Gemini.Modules.Explorer.Menus;
using Gemini.Modules.Explorer.Models;
using System.ComponentModel.Composition;

namespace Gemini.Modules.Explorer
{
    public static class MenuDefinitions
    {
        [Export]
        public static MenuBarDefinition MenuContextBar = new MenuBarDefinition();

        [Export]
        public static MenuDefinition ContextMenuDefinition = new MenuDefinition(MenuContextBar, 1, "Context menu");

        [Export]
        public static MenuItemGroupDefinition EditMenuGroupItemDefinition = new MenuItemGroupDefinition(ContextMenuDefinition, 1);

        [Export]
        public static MenuItemDefinition renameMenuItem = new CommandMenuItemDefinition<RenameCommandDefinition>(
            EditMenuGroupItemDefinition, 1);

        [Export]
        public static MenuItemDefinition ViewInspectorMenuItem = new CommandMenuItemDefinition<ViewExplorerCommandDefinition>(
            MainMenu.MenuDefinitions.ViewToolsMenuGroup, 1);

        [Export]
        public static MenuItemDefinition OpenFolderMenuItem = new CommandMenuItemDefinition<FileOpenFolderCommandDefinition>(
            MainMenu.MenuDefinitions.FileNewOpenMenuGroup, 4);

        [Export]
        public static MenuItemDefinition CloseFolderMenuItem = new CommandMenuItemDefinition<FileCloseFolderCommandDefinition>(
            MainMenu.MenuDefinitions.FileCloseMenuGroup, 2);

        //[Export]
        //public static ExplorerMenuItemDefinition<FileSystemTreeItem> Rename = new ExplorerCommandMenuItemDefinition<FileSystemTreeItem, RenameCommandDefinition>(EditMenuGroupItem, 1);

        //[Export]
        //public static ExplorerMenuItemGroupDefinition EditMenuGroupItem = new ExplorerMenuItemGroupDefinition(treeItemMenu, 0);

        //[Export]
        //public static ExplorerMenuDefinition treeItemMenu = new ExplorerMenuDefinition();
    }
}
