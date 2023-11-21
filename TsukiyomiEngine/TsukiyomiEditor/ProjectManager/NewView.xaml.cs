using System;
using System.Windows;
using System.Windows.Controls;

namespace TsukiyomiEditor.ProjectManager
{
    /// <summary>
    /// Interaction logic for NewView.xaml
    /// </summary>
    public partial class NewView : UserControl
    {
        public NewView()
        {
            InitializeComponent();
        }
        private void OnClick(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as NewProject;
            var projectPath = vm.CreateProject(TemplateList.SelectedItem as TemplateFile);
            bool dialogResult = false;
            var window = Window.GetWindow(this);
            if (!string.IsNullOrEmpty(projectPath))
            {
                dialogResult = true;
            }
            window.DialogResult = dialogResult;
            window.Close();
        }
    }
}
