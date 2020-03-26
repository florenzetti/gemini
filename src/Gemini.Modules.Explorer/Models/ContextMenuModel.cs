using Gemini.Modules.MainMenu.Models;
using System.ComponentModel.Composition;

namespace Gemini.Modules.Explorer.Models
{
    [Export(typeof(ContextMenuModel))]
    public class ContextMenuModel : MenuModel
    {
    }
}
