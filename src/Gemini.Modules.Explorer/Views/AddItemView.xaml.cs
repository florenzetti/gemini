using Gemini.Modules.Explorer.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Gemini.Modules.Explorer.Views
{
    /// <summary>
    /// Interaction logic for AddItemView.xaml
    /// </summary>
    public partial class AddItemView : Window
    {
        public AddItemView()
        {
            InitializeComponent();
        }

        private void OnListItemSelected(object sender, RoutedEventArgs e)
        {
            var viewModel = (AddItemViewModel)DataContext;
        }
    }
}
