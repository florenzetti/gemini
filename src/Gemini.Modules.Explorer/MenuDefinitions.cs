using Gemini.Framework.Menus;
using Gemini.Modules.Explorer.Commands;
using System.ComponentModel.Composition;

namespace Gemini.Modules.Explorer
{
    public static class MenuDefinitions
    {
        [Export]
        public static MenuItemDefinition ViewInspectorMenuItem = new CommandMenuItemDefinition<ViewExplorerCommandDefinition>(
            MainMenu.MenuDefinitions.ViewToolsMenuGroup, 1);

        [Export]
        public static MenuItemDefinition OpenFolderMenuItem = new CommandMenuItemDefinition<FileOpenFolderCommandDefinition>(
            MainMenu.MenuDefinitions.FileNewOpenMenuGroup,4);

        [Export]
        public static MenuItemDefinition CloseFolderMenuItem = new CommandMenuItemDefinition<FileCloseFolderCommandDefinition>(
            MainMenu.MenuDefinitions.FileCloseMenuGroup, 2);
    }
}
