using Gemini.Framework;
using Gemini.Modules.Explorer.Models;
using Gemini.Modules.MainMenu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Gemini.Modules.Explorer.Controls
{
    public class ExplorerControl : Control
    {
        public static readonly DependencyProperty SourceTreeProperty = DependencyProperty.Register(
            "SourceTree", typeof(TreeItem), typeof(ExplorerControl));
        public TreeItem SourceTree
        {
            get { return (TreeItem)GetValue(SourceTreeProperty); }
            set { SetValue(SourceTreeProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register(
            "SelectedItems", typeof(IList<TreeItem>), typeof(ExplorerControl));
        public IList<TreeItem> SelectedItems
        {
            get { return ((TreeViewEx)Template.FindName("PART_TreeView", this)).SelectedItems.Cast<TreeItem>().ToList(); }
        }

        public static readonly DependencyProperty ContextMenuModelProperty = DependencyProperty.Register(
            "ContextMenuModel", typeof(ContextMenuModel), typeof(ExplorerControl));
        public ContextMenuModel ContextMenuModel
        {
            get { return (ContextMenuModel)GetValue(ContextMenuModelProperty); }
            set { SetValue(ContextMenuModelProperty, value); }
        }

        public static readonly DependencyProperty SearchCommandProperty = DependencyProperty.Register(
            "SearchCommand", typeof(ICommand), typeof(ExplorerControl));

        public ICommand SearchCommand
        {
            get { return (ICommand)GetValue(SearchCommandProperty); }
            set { SetValue(SearchCommandProperty, value); }
        }

        public static readonly DependencyProperty DragCommandProperty = DependencyProperty.Register(
            "DragCommand", typeof(ICommand), typeof(ExplorerControl));
        public ICommand DragCommand
        {
            get { return (ICommand)GetValue(DragCommandProperty); }
            set { SetValue(DragCommandProperty, value); }
        }

        public static readonly DependencyProperty DropCommandProperty = DependencyProperty.Register(
            "DropCommand", typeof(ICommand), typeof(ExplorerControl));
        public ICommand DropCommand
        {
            get { return (ICommand)GetValue(DropCommandProperty); }
            set { SetValue(DropCommandProperty, value); }
        }

        public static readonly RoutedEvent ItemEditedEvent = EventManager.RegisterRoutedEvent(
            "ItemEdited", RoutingStrategy.Bubble, typeof(TreeViewIemEditHandler),
            typeof(ExplorerControl));
        public event TreeViewIemEditHandler ItemEdited
        {
            add { AddHandler(ItemEditedEvent, value); }
            remove { RemoveHandler(ItemEditedEvent, value); }
        }

        public static readonly RoutedEvent ItemEditingEvent = EventManager.RegisterRoutedEvent(
            "ItemEditing", RoutingStrategy.Bubble, typeof(TreeViewIemEditHandler),
            typeof(ExplorerControl));
        public event TreeViewIemEditHandler ItemEditing
        {
            add { AddHandler(ItemEditingEvent, value); }
            remove { RemoveHandler(ItemEditingEvent, value); }
        }

        public EventHandler<SelectionChangedCancelEventArgs> OnSelecting;

        /// <summary>
        /// Static constructor to define metadata for the control (and link it to the style in Generic.xaml).
        /// </summary>
        static ExplorerControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExplorerControl), new FrameworkPropertyMetadata(typeof(ExplorerControl)));
        }

        public ExplorerControl()
        {
            Resources.Source = new Uri("pack://application:,,,/Gemini.Modules.Explorer;component/Resources/Styles.xaml");
            SetValue(SearchCommandProperty, new RelayCommand(OnSearchItem));
        }

        public override void OnApplyTemplate()
        {
            AddHandler(TreeViewExItem.OnEditingEvent, new RoutedEventHandler(Item_OnEditing));
            AddHandler(TreeViewExItem.OnEditedEvent, new RoutedEventHandler(Item_OnEdited));
            var treeView = (TreeViewEx)Template.FindName("PART_TreeView", this);
            treeView.OnSelecting += OnSelecting;
            base.OnApplyTemplate();
        }

        protected override void OnContextMenuOpening(ContextMenuEventArgs e)
        {
            var m = ContextMenuModel;
            base.OnContextMenuOpening(e);
        }

        private void OnSearchItem(object term)
        {
            SourceTree.Search(term as string);
        }

        private void Item_OnEditing(object sender, RoutedEventArgs e)
        {
            var arg = new TreeViewItemSelectedEventArgs(ItemEditingEvent, this, e.OriginalSource as TreeViewExItem);
            RaiseEvent(arg);
        }

        private void Item_OnEdited(object sender, RoutedEventArgs e)
        {
            var arg = new TreeViewItemSelectedEventArgs(ItemEditedEvent, this, e.OriginalSource as TreeViewExItem);
            RaiseEvent(arg);
        }
    }
}
