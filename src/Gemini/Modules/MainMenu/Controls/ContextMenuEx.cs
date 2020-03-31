using Gemini.Modules.MainMenu.Controls;
using System.Windows;

namespace Gemini.Modules.MainMenu.Controls
{
    public class ContextMenuEx : System.Windows.Controls.ContextMenu
    {
        private object _currentItem;

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            _currentItem = item;
            return base.IsItemItsOwnContainerOverride(item);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return MenuItemEx.GetContainer(this, _currentItem);
        }
    }
}
