using Gemini.Framework.Menus;
using Gemini.Framework.Services;
using Gemini.Modules.Explorer.Commands;
using Gemini.Modules.Explorer.Properties;
using System.ComponentModel.Composition;

namespace Gemini.Modules.Explorer
{
    public static class MenuDefinitions
    {
        //Common tree view context menu
        [Export]
        public static ContextMenuDefinition CommonContextMenuDefinition = new ContextMenuDefinition(5);

        [Export]
        public static MenuItemGroupDefinition CommonEditMenuGroupDefinition = new MenuItemGroupDefinition(CommonContextMenuDefinition, 1);

        [Export]
        public static MenuItemDefinition CommonRenameMenuItem = new CommandMenuItemDefinition<TreeItemRenameCommandDefinition>(
            CommonEditMenuGroupDefinition, 1);

        [Export]
        public static MenuItemDefinition CommonDeleteMenuItem = new CommandMenuItemDefinition<TreeItemDeleteCommandDefinition>(
            CommonEditMenuGroupDefinition, 2);

        //Folder tree view context menu
        [Export]
        public static ContextMenuDefinition FolderContextMenuDefinition = new ContextMenuDefinition(0, DefaultFolderTemplate.DefaultTemplate);

        [Export]
        public static MenuItemGroupDefinition FolderContextMenuGroupDefinition = new MenuItemGroupDefinition(FolderContextMenuDefinition, 0);

        [Export]
        public static MenuItemDefinition FolderTextAddMenuItem = new TextMenuItemDefinition(FolderContextMenuGroupDefinition, 1, Resources.AddText);

        [Export]
        public static MenuItemGroupDefinition FolderAddCascadeGroupDefinition = new MenuItemGroupDefinition(FolderTextAddMenuItem, 0);

        [Export]
        public static MenuItemDefinition FolderAddMenuItem = new CommandMenuItemDefinition<FolderTreeItemAddListCommandDefinition>(
            FolderAddCascadeGroupDefinition, 1);

        //Main menu
        [Export]
        public static MenuItemDefinition ViewExplorerMenuItem = new CommandMenuItemDefinition<ViewExplorerCommandDefinition>(
            MainMenu.MenuDefinitions.ViewToolsMenuGroup, 1);

        [Export]
        public static MenuItemDefinition OpenFolderMenuItem = new CommandMenuItemDefinition<FileOpenFolderCommandDefinition>(
            MainMenu.MenuDefinitions.FileNewOpenMenuGroup, 4);

        [Export]
        public static MenuItemDefinition CloseFolderMenuItem = new CommandMenuItemDefinition<FileCloseFolderCommandDefinition>(
            MainMenu.MenuDefinitions.FileCloseMenuGroup, 2);
    }
}
