using Gemini.Framework.Services;
using Gemini.Modules.MainMenu;
using Gemini.Modules.Explorer.ViewModels;
using Gemini.Framework;
using Gemini.Modules.Explorer.Views;
using Caliburn.Micro;

namespace Gemini.Modules.Explorer
{
    public abstract class ExplorerTool<TExplorerProvider> : Tool
        where TExplorerProvider : class, IExplorerProvider
    {
        public bool UseDefaultView { get; }
        public TExplorerProvider GetProvider()
        {
            return IoC.Get<TExplorerProvider>();
        }
    }
}
