using Gemini.Framework;

namespace Gemini.Modules.Explorer
{
    public interface IExplorerTool : ITool
    {
        bool IsFolderOpened { get; }
        void OpenFolder();
        void CloseFolder();
    }
}
