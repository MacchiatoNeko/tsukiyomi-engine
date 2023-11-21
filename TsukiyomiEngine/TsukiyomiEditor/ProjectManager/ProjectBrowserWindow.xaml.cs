using System.Windows;

namespace TsukiyomiEditor.ProjectManager
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
