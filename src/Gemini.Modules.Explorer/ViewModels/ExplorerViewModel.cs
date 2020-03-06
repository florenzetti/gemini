using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Services;
using Gemini.Modules.Explorer.Models;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace Gemini.Modules.Explorer.ViewModels
{
    [Export(typeof(IExplorerTool))]
    public class ExplorerViewModel : Tool, IExplorerTool
    {
        private DirectoryInfo _directoryInfo;
        private RelayCommand _openFolderViewCommand;
        public ICommand OpenFolderViewCommand
        {
            get { return _openFolderViewCommand ?? (_openFolderViewCommand = new RelayCommand(o => OpenFolder())); }
        }

        private readonly IShell _shell;
        private readonly IEditorProvider _editorProvider;
        public bool IsFolderOpened => _directoryInfo != null;

        public override PaneLocation PreferredLocation
        {
            get { return PaneLocation.Left; }
        }

        public TreeItem FolderTree { get; private set; }

        [ImportingConstructor]
        public ExplorerViewModel(IShell shell, IEditorProvider editorProvider)
        {
            _shell = shell;
            _editorProvider = editorProvider;
            DisplayName = Properties.Resources.ExplorerViewModel_ExplorerViewModel_Explorer;
        }

        public void OpenFolder()
        {
            var folderDialog = new FolderBrowserDialog();
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                _directoryInfo = new DirectoryInfo(folderDialog.SelectedPath);
                FolderTree = TreeItem.LoadRecursive(_directoryInfo);
                NotifyOfPropertyChange(() => FolderTree);
                NotifyOfPropertyChange(() => IsFolderOpened);
            }
        }

        public void CloseFolder()
        {
            _directoryInfo = null;
            FolderTree = null;
            NotifyOfPropertyChange(() => FolderTree);
            NotifyOfPropertyChange(() => IsFolderOpened);
        }

        public async Task OpenItemAsync(TreeItem item)
        {
            if (item.IsFolder)
                return;

            var editor = _shell.Documents.FirstOrDefault(o => o.Id == item.DocumentId);
            if (editor == null)
            {
                editor = _editorProvider.Create();

                var viewAware = (IViewAware)editor;
                viewAware.ViewAttached += (sender, e) =>
                {
                    var frameworkElement = (FrameworkElement)e.View;

                    RoutedEventHandler loadedHandler = null;
                    loadedHandler = async (sender2, e2) =>
                    {
                        frameworkElement.Loaded -= loadedHandler;
                        await _editorProvider.Open(editor, item.FullPath);
                    };
                    frameworkElement.Loaded += loadedHandler;
                };

                item.DocumentId = editor.Id;
            }

            await _shell.OpenDocumentAsync(editor);
        }
    }
}
