using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using TsukiyomiEditor.Utilities;

namespace TsukiyomiEditor.ProjectBrowser
{
    [DataContract]
    public class TemplateFile
    {
        [DataMember]
        public string ProjType { get; set; }

        [DataMember]
        public string ProjFile { get; set; }

        [DataMember]
        public List<string> ProjFolders { get; set; }

        public byte[] Icon { get; set; }
        public byte[] Thumb { get; set; }

        public string IconPath { get; set; }
        public string ThumbPath { get; set; }
        public string ProjectPath { get; set; }
    }

    class NewProj : ViewModelBase
    {
        // TODO: Support install preparation script
        private readonly string _templatePath = @"..\ProjectTemplates\";
        private string _projectname = "New_Project";
        private string _projectpath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\TsukiyomiEngineProjects\";

        public string projectName
        {
            get => _projectname;
            set
            {
                if (_projectname != value)
                {
                    _projectname = value;
                    OnPropertyChanged(nameof(projectName));
                }
            }
        }

        public string projectPath
        {
            get => _projectpath;
            set
            {
                if (_projectpath != value)
                {
                    _projectpath = value;
                    OnPropertyChanged(nameof(projectPath));
                }
            }
        }

        private ObservableCollection<TemplateFile> _templateFile = new ObservableCollection<TemplateFile>();
        public ReadOnlyObservableCollection<TemplateFile> TemplateFiles { get; }

        public NewProj()
        {
            TemplateFiles = new ReadOnlyObservableCollection<TemplateFile>(_templateFile);
            try
            {
                var templatesFound = Directory.GetFiles(_templatePath, "template.xml", SearchOption.AllDirectories);
                //Debug.Assert(templatesFound.Length > 0);
                Debug.Assert(templatesFound.Any());
                foreach (var templateFile in templatesFound)
                {
                    var template = Serializer.FromFile<TemplateFile>(templateFile);
                    template.ThumbPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(templateFile), "thumbnail.png"));
                    template.IconPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(templateFile), "icon.png"));
                    template.ProjectPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(templateFile), template.ProjFile));
                    template.Icon = File.ReadAllBytes(template.IconPath);
                    template.Thumb = File.ReadAllBytes(template.ThumbPath);
                    _templateFile.Add(template);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                // TODO: Log Debug later if Logging Component will be a thing
            }
        }
    }
}
