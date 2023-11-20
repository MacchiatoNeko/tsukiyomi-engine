using System;
using System.Collections.Generic;
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
    }

    class NewProj : ViewModelBase
    {
        // TODO: Support install preparation script
        private readonly string _templatePath = @"..\ProjectTemplates\";
        private string _name = "New_Project";
        private string _path = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\TsukiyomiEngineProjects\";

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Path
        {
            get => _path;
            set
            {
                if (_path != value)
                {
                    _path = value;
                    OnPropertyChanged(nameof(Path));
                }
            }
        }

        public NewProj()
        {
            try
            {
                var templatesFound = Directory.GetFiles(_templatePath, "template.xml", SearchOption.AllDirectories);
                //Debug.Assert(templatesFound.Length > 0);
                Debug.Assert(templatesFound.Any());
                foreach (var templateFile in templatesFound)
                {
                    var template = new TemplateFile()
                    {
                        ProjType = "EmptyProject",
                        ProjFile = "Project.tsukiyomi",
                        ProjFolders = new List<string>() { ".tsukiyomi", "Resources", "Code" }
                    };

                    Serializer.ToFile(template, templateFile);
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
