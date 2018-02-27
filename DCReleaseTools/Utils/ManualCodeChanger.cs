using System;
using System.Collections.Generic;
using System.IO;
using MonoDevelop.Ide;
using MonoDevelop.Projects;

namespace DCReleaseTools.Utils
{
    public class ManualCodeChanger
    {
        private static string Namespace;
        private static string ParentName;

        private static ProjectFile ParentFile;
        private static List<AndroidControl> ControlList;

        public static void CreateControlWrapperClass(ProjectFile parentFile, List<AndroidControl> controlList)
        {
            //Change parent File
            ParentFile = parentFile;
            ControlList = controlList;
            ParentName = Path.GetFileName(ParentFile.Name.Remove(ParentFile.Name.LastIndexOf('.')));
            Namespace = GetClassNameSpace(parentFile.FilePath);

            //Create UI file
            var newFile = CreateUIFile();

            //Nest child to parent
            Nester.Nest(newFile, parentFile);
        }

        private static string GetClassNameSpace(string filePath)
        {
            //TODO: Get name space from .cs file
            return "Some.Text.Here";
        }

        private static ProjectFile CreateUIFile()
        {
            ProjectFile newFile = null;
            var fileName =  ParentName + ".ui.cs";
            var filePath = Path.Combine(ParentFile.FilePath.ParentDirectory.FullPath.ToString(), fileName);
            if (!File.Exists(filePath))
            {
                //Create file
                File.Create(filePath).Dispose();
                AppendTemplateContent(filePath);

                //Add file to project
                newFile = IdeApp.ProjectOperations.CurrentSelectedProject.AddFile(filePath);
            }
            return newFile;
        }

        private static void AppendTemplateContent(string filePath)
        {
            using (TextWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("using System.Collections.Generic;\nusing Android.Widget;"); //using
                writer.WriteLine();
                writer.WriteLine("namespace " + Namespace);
                writer.WriteLine("{");
                writer.WriteLine("\tpublic partial class "+ ParentName);
                writer.WriteLine("\t{");
                WriteControls(writer);
                writer.WriteLine("\t}");
                writer.WriteLine("}");
            }
        }

        private static void WriteControls(TextWriter writer)
        {
            if (ControlList?.Count != 0)
            {
                foreach (var control in ControlList)
                {
                    writer.WriteLine($"\t\tprivate {control.Type} {control.PrivateName};");
                    writer.WriteLine($"\t\tpublic {control.Type} {control.Name}");
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tget");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine($"\t\t\t\treturn {control.PrivateName} ??");
                    writer.WriteLine($"\t\t\t\t\t({control.PrivateName} = RootView.FindViewById<{control.Type}>({control.Id}));");
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();
                }
            }
        }
    }
}
