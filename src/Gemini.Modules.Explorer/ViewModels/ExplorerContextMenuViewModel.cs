using Gemini.Modules.Explorer.Models;
using Gemini.Modules.MainMenu;
using Gemini.Modules.MainMenu.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

namespace Gemini.Modules.Explorer.ViewModels
{
    [Export(typeof(ExplorerContextMenuViewModel))]
    public class ExplorerContextMenuViewModel : MenuModel//, IPartImportsSatisfiedNotification
    {
        private readonly IMenuBuilder _menuBuilder;

        [ImportingConstructor]
        public ExplorerContextMenuViewModel(IMenuBuilder menuBuilder)
        {
            _menuBuilder = menuBuilder;
            //_autoHide = Properties.Settings.Default.AutoHideMainMenu;
            //_settingsEventManager.AddListener(s => s.AutoHideMainMenu, value => { AutoHide = value; });
        }

        //void IPartImportsSatisfiedNotification.OnImportsSatisfied()
        //{
        //    _menuBuilder.BuildMenuBar(MenuDefinitions.MenuContextBar, this);
        //}

        public MenuModel BuildMenu()
        {
            _menuBuilder.BuildMenuBar(MenuDefinitions.MenuContextBar, this);
            return this;
        }
    }
}
