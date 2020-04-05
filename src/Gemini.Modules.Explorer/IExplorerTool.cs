using Gemini.Framework;
using Gemini.Modules.Explorer.Models;
using System.Windows.Input;

namespace Gemini.Modules.Explorer
{
    public interface IExplorerTool : ITool
    {
        ICommand OpenSourceCommand { get; }
        string OpenSourceButtonText { get; }
        TreeItem SourceTree { get; }
        string FullPath { get; }
        bool IsSourceOpened { get; }
        void OpenSource();
        void CloseSource();
    }
}
