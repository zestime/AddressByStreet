using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AddressBook
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel model;
        private MainViewModelController controller;

        public MainWindow()
        {
            InitializeComponent();
            
            model = new MainWindowViewModel();
            controller = new MainViewModelController(model);

            DataContext = model;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            controller.DoLuceneSearch();
        }
    }
}
