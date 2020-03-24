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
        public static ContextMenuDefinition CommonContextMenuDefinition = new ContextMenuDefinition(1, typeof(FileSystemFileTreeItem), typeof(FileSystemFolderTreeItem));

        [Export]
        public static ContextMenuGroupDefinition CommonEditMenuGroupDefinition = new ContextMenuGroupDefinition(CommonContextMenuDefinition, 1);

        [Export]
        public static ContextMenuItemDefinition CommonRenameMenuItem = new ContextCommandMenuItemDefinition<TreeItemRenameCommandDefinition>(
            CommonEditMenuGroupDefinition, 1);

        [Export]
        public static ContextMenuItemDefinition CommonDeleteMenuItem = new ContextCommandMenuItemDefinition<TreeItemDeleteCommandDefinition>(
            CommonEditMenuGroupDefinition, 2);

        //Folder tree view context menu
        [Export]
        public static ContextMenuDefinition FolderContextMenuDefinition = new ContextMenuDefinition(0, typeof(FileSystemFolderTreeItem));

        [Export]
        public static ContextMenuGroupDefinition FolderContextMenuGroupDefinition = new ContextMenuGroupDefinition(FolderContextMenuDefinition, 0);

        [Export]
        public static ContextMenuItemDefinition FolderTextAddMenuItem = new ContextTextMenuItemDefinition(FolderContextMenuGroupDefinition, 1, "Add");

        [Export]
        public static ContextMenuGroupDefinition FolderAddCascadeGroupDefinition = new ContextMenuGroupDefinition(FolderTextAddMenuItem, 0);

        [Export]
        public static ContextMenuItemDefinition FolderAddMenuItem = new ContextCommandMenuItemDefinition<FolderTreeItemAddListDefinition>(
            FolderAddCascadeGroupDefinition, 1);

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
