using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using TsukiyomiEditor.Utilities;

namespace TsukiyomiEditor.ProjectManager
{
    [DataContract]
    public class TemplateFile
    {
        [DataMember]
        public string ProjectType { get; set; }

        [DataMember]
        public string ProjectFile { get; set; }

        [DataMember]
        public List<string> ProjectFolders { get; set; }

        public byte[] Icon { get; set; }
        public byte[] Thumb { get; set; }

        public string IconPath { get; set; }
        public string ThumbPath { get; set; }
        public string ProjectPath { get; set; }
    }

    class NewProject : ViewModelBase
    {
        // TODO: Support install preparation script
        private readonly string _templatePath = @"..\ProjectTemplates\";
        private string _projectname = "NewProject";
        private string _projectpath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\TsukiyomiEngineProjects\";

        public string ProjectName
        {
            get => _projectname;
            set
            {
                if (_projectname != value)
                {
                    _projectname = value;
                    ValidateProjectPath();
                    OnPropertyChanged(nameof(ProjectName));
                }
            }
        }

        public string ProjectPath
        {
            get => _projectpath;
            set
            {
                if (_projectpath != value)
                {
                    _projectpath = value;
                    ValidateProjectPath();
                    OnPropertyChanged(nameof(ProjectPath));
                }
            }
        }

        private bool ValidateProjectPath()
        {
            var path = Path.Combine(ProjectPath?.Trim(), ProjectName?.Trim());
            ErrorMessage = string.Empty;

            if (string.IsNullOrEmpty(path))
            {
                ErrorMessage = "Your project name or path is empty.";
                IsValid = false;
                return IsValid;
            }

            if (path.IndexOfAny(Path.GetInvalidPathChars()) != -1 || ProjectName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
            {
                ErrorMessage = "Your project name or path is invalid.";
                IsValid = false;
                return IsValid;
            }

            if (Directory.Exists(path) && Directory.EnumerateFileSystemEntries(path).Any())
            {
                ErrorMessage = "Selected project folder already exists and is not empty.";
                IsValid = false;
                return IsValid;
            }
            IsValid = true;
            return IsValid;
        }

        private bool _isValid;
        public bool IsValid
        {
            get => _isValid;
            set
            {
                if(_isValid != value)
                {
                    _isValid = value;
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    OnPropertyChanged(nameof(ErrorMessage));
                }
            }
        }

        public void GenerateExampleTemplateXML()
        {
            TemplateFile template = new TemplateFile
            {
                ProjectType = "Empty Project",
                ProjectFile = "project.tsukiyomi",
                ProjectFolders = new List<string> { ".tsukiyomi", "Content", "Code" },
            };
            var here = @$"{_templatePath}\EmptyProject\";
            Serializer.ToFile(template, Path.Combine(here, "template.xml"));
        }

        public string CreateProject(TemplateFile template)
        {
            ValidateProjectPath();
            if (!IsValid)
            {
                return string.Empty;
            }
            Path.Combine(ProjectPath?.Trim(), ProjectName?.Trim());
            var path = $@"{ProjectPath}{ProjectName}\";

            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                foreach (var folder in template.ProjectFolders)
                {
                    Directory.CreateDirectory(Path.GetFullPath(Path.Combine(Path.GetDirectoryName(path), folder)));
                }
                var dirInfo = new DirectoryInfo(path + @".tsukiyomi\");
                File.Copy(template.IconPath, Path.GetFullPath(Path.Combine(dirInfo.FullName, "icon.png")));
                File.Copy(template.IconPath, Path.GetFullPath(Path.Combine(dirInfo.FullName, "thumbnail.png")));

                var projectXml = File.ReadAllText(template.ProjectPath);
                projectXml = String.Format(projectXml, ProjectName, ProjectPath);
                var projectPath = Path.GetFullPath(Path.Combine(path, $"{ProjectName}{Project.Extension}"));
                File.WriteAllText(projectPath, projectXml);
                return path;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                // TODO: Log Debug later if Logging Component will be a thing
                return string.Empty;
            }
        }

        private ObservableCollection<TemplateFile> _templateFile = new ObservableCollection<TemplateFile>();
        public ReadOnlyObservableCollection<TemplateFile> TemplateFiles { get; }

        public NewProject()
        {
            TemplateFiles = new ReadOnlyObservableCollection<TemplateFile>(_templateFile);
            try
            {
                var templatesFound = Directory.GetFiles(_templatePath, "template.xml", SearchOption.AllDirectories);
                //Debug.Assert(templatesFound.Length > 0);
                Debug.Assert(templatesFound.Any());
                foreach (var file in templatesFound)
                {
                    var template = Serializer.FromFile<TemplateFile>(file);
                    template.ThumbPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file), "thumbnail.png"));
                    template.IconPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file), "icon.png"));
                    template.ProjectPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file), template.ProjectFile));
                    template.Icon = File.ReadAllBytes(template.IconPath);
                    template.Thumb = File.ReadAllBytes(template.ThumbPath);
                    _templateFile.Add(template);
                }
                //ValidateProjectPath();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                // TODO: Log Debug later if Logging Component will be a thing
            }
        }
    }
}
