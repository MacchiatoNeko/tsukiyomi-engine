using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TsukiyomiEditor.ProjectBrowser
{
    /// <summary>
    /// Interaction logic for ProjectBrowserWindow.xaml
    /// </summary>
    public partial class ProjectBrowserWindow : Window
    {
        public ProjectBrowserWindow()
        {
            InitializeComponent();
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            if (sender == openproj)
            {
                if (newproj.IsChecked == true)
                {
                    newproj.IsChecked = false;
                    browserContent.Margin = new Thickness(0, 0, 0, 0);
                }
                openproj.IsChecked = true;
            }
            else
            {
                if (openproj.IsChecked == true)
                {
                    openproj.IsChecked = false;
                    browserContent.Margin = new Thickness(-800, 0, 0, 0);
                }
                newproj.IsChecked = true;
            }
        }
    }
}
