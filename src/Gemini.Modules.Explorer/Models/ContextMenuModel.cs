using Gemini.Modules.Explorer.Menus;
using Gemini.Modules.Explorer.Models;
using Gemini.Modules.MainMenu;
using Gemini.Modules.MainMenu.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

namespace Gemini.Modules.Explorer.Models
{
    [Export(typeof(ContextMenuModel))]
    public class ContextMenuModel : MenuModel
    {
    }
}
