using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Services;
using Gemini.Modules.Explorer.Models;
using Gemini.Modules.Explorer.Properties;
using Gemini.Modules.Explorer.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Gemini.Modules.Explorer.ViewModels
{
    [Export(typeof(AddItemViewModel))]
    public class AddItemViewModel : WindowBase
    {
        private readonly IExplorerProvider _explorerProvider;
        private readonly IShell _shell;
        private readonly IEditorProvider[] _editorProviders;

        private readonly List<EditorFileTemplate> _itemTemplates;
        public IReadOnlyList<EditorFileTemplate> ItemTemplates => _itemTemplates.AsReadOnly();

        private EditorFileTemplate _selectedTemplateItem;
        public EditorFileTemplate SelectedTemplateItem
        {
            get => _selectedTemplateItem;
            set
            {
                _selectedTemplateItem = value;
                NotifyOfPropertyChange(() => SelectedTemplateItem);
            }
        }
        private ICommand _addCommand;
        public ICommand AddItemCommand { get => _addCommand ?? (_addCommand = new RelayCommand(o => AddItem())); }

        public string FileName { get; set; }

        private TreeItem _parentItem;
        public TreeItem ParentItem
        {
            get => _parentItem;
            set
            {
                _parentItem = value;
                FileName = ParentItem.FullPath;

            }
        }

        [ImportingConstructor]
        public AddItemViewModel(IShell shell, [ImportMany]IEditorProvider[] editorProviders, IExplorerProvider explorerProvider)
        {
            DisplayName = Resources.AddItemViewDisplayName;

            _shell = shell;
            _editorProviders = editorProviders;
            _explorerProvider = explorerProvider;
            _itemTemplates = new List<EditorFileTemplate>();
            foreach (var editorProvider in _editorProviders)
                foreach (var editorFileType in editorProvider.FileTypes)
                    _itemTemplates.Add(
                        new EditorFileTemplate()
                        {
                            Name = editorFileType.Name,
                            FileExtension = editorFileType.FileExtension,
                            Description = "Description"
                        });
            _selectedTemplateItem = _itemTemplates.FirstOrDefault();
        }

        public void SetFileExtension(string fileExtension)
        {
        }

        public async Task AddItem()
        {
            var item = _explorerProvider.CreateItem(Path.GetFileName(FileName), FileName, _selectedTemplateItem);
            await OpenItemAsync(item);
            await TryCloseAsync(false);
        }

        public Task Cancel() => TryCloseAsync(false);

        public async Task OpenItemAsync(TreeItem item)
        {
            if (!item.CanOpenDocument)
                return;

            var document = _shell.Documents.FirstOrDefault(o => o.Id == item.DocumentId);
            if (document == null)
            {
                var editor = _editorProviders.FirstOrDefault(o => o.Handles(item.FullPath));
                if (editor == null)
                    //TODO: error message
                    return;
                document = editor.Create();

                var viewAware = (IViewAware)document;
                viewAware.ViewAttached += (sender, e) =>
                {
                    var frameworkElement = (FrameworkElement)e.View;

                    RoutedEventHandler loadedHandler = null;
                    loadedHandler = async (sender2, e2) =>
                    {
                        frameworkElement.Loaded -= loadedHandler;
                        await editor.Open(document, item.FullPath);
                    };
                    frameworkElement.Loaded += loadedHandler;
                };

                item.DocumentId = document.Id;
            }
            await _shell.OpenDocumentAsync(document);
        }
    }
}
