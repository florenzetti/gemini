using Gemini.Framework.Menus;
using Gemini.Modules.Explorer.Commands;
using Gemini.Modules.Explorer.Menus;
using Gemini.Modules.Explorer.Models;
using System.ComponentModel.Composition;

namespace Gemini.Modules.Explorer
{
    public static class MenuDefinitions
    {
        //Common tree view context menu
        [Export]
        public static ContextMenuDefinition CommonContextMenuDefinition = new ContextMenuDefinition(1, typeof(FileSystemFileTreeItem), typeof(DirectoryTreeItem));

        [Export]
        public static ContextMenuItemGroupDefinition CommonEditMenuGroupItemDefinition = new ContextMenuItemGroupDefinition(CommonContextMenuDefinition, 1);

        [Export]
        public static ContextMenuItemDefinition CommonRenameMenuItem = new ContextCommandMenuItemDefinition<TreeItemRenameCommandDefinition>(
            CommonEditMenuGroupItemDefinition, 1);

        [Export]
        public static ContextMenuItemDefinition CommonDeleteMenuItem = new ContextCommandMenuItemDefinition<TreeItemDeleteCommandDefinition>(
            CommonEditMenuGroupItemDefinition, 2);

        //Folder tree view context menu
        [Export]
        public static ContextMenuDefinition FolderContextMenuDefinition = new ContextMenuDefinition(2, typeof(DirectoryTreeItem));

        [Export]
        public static ContextMenuItemGroupDefinition FolderEditMenuGroupItemDefinition = new ContextMenuItemGroupDefinition(FolderContextMenuDefinition, 0);

        [Export]
        public static ContextMenuItemDefinition FolderAddMenuItem = new ContextCommandMenuItemDefinition<FolderTreeItemAddCommandDefinition>(
            FolderEditMenuGroupItemDefinition, 1);

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
