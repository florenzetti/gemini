using Gemini.Framework.Menus;
using Gemini.Modules.Explorer.Commands;
using Gemini.Modules.Explorer.Menus;
using System.ComponentModel.Composition;

namespace Gemini.Modules.Explorer
{
    public static class MenuDefinitions
    {
        //Tree view context menu
        [Export]
        public static ContextMenuDefinition ContextMenuDefinition = new ContextMenuDefinition();

        [Export]
        public static ContextMenuItemGroupDefinition EditMenuGroupItemDefinition = new ContextMenuItemGroupDefinition(ContextMenuDefinition, 1);

        [Export]
        public static ContextMenuItemDefinition RenameMenuItem = new ContextCommandMenuItemDefinition<TreeItemRenameCommandDefinition>(
            EditMenuGroupItemDefinition, 1);

        [Export]
        public static ContextMenuItemDefinition DeleteMenuItem = new ContextCommandMenuItemDefinition<TreeItemDeleteCommandDefinition>(
            EditMenuGroupItemDefinition, 2);

        //Main menu
        [Export]
        public static MenuItemDefinition ViewInspectorMenuItem = new CommandMenuItemDefinition<ViewExplorerCommandDefinition>(
            MainMenu.MenuDefinitions.ViewToolsMenuGroup, 1);

        [Export]
        public static MenuItemDefinition OpenFolderMenuItem = new CommandMenuItemDefinition<FileOpenFolderCommandDefinition>(
            MainMenu.MenuDefinitions.FileNewOpenMenuGroup, 4);

        [Export]
        public static MenuItemDefinition CloseFolderMenuItem = new CommandMenuItemDefinition<FileCloseFolderCommandDefinition>(
            MainMenu.MenuDefinitions.FileCloseMenuGroup, 2);
    }
}
