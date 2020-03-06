using Gemini.Framework;

namespace Gemini.Modules.Explorer
{
    public interface IExplorerTool : ITool
    {
        string FullPath { get; }
        bool IsSourceOpened { get; }
        void OpenSource();
        void CloseSource();
    }
}
