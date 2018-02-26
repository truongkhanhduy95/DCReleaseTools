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
        private static ProjectFile ParentFile;
        private static List<AndroidControl> ControlList;

        public static void CreateControlWrapperClass(ProjectFile parentFile, List<AndroidControl> controlList)
        {
            //TODO: Change parent File
            ParentFile = parentFile;
            Namespace = GetClassNameSpace(parentFile.FilePath);
            ControlList = controlList;

            //TODO: Add child file
            CreateUIFile();


            //TODO: Nest child to parent
            foreach (var x in controlList)
            {
                System.Diagnostics.Debug.WriteLine(x);
            }
        }

        private static string GetClassNameSpace(string filePath)
        {
            return "test namespace";
        }

        private static void CreateUIFile()
        {
            var fileName = ParentFile.Name.Split('.')[0] + ".ui.cs";
            var filePath = Path.Combine(ParentFile.FilePath.ParentDirectory.FullPath.ToString(), fileName);
            if (!File.Exists(filePath))
            {
                //Create file
                File.Create(filePath);
                AppendTemplateContent(filePath);

                //Add file to project
                IdeApp.ProjectOperations.CurrentSelectedProject.AddFile(fileName);
            }
        }

        private static void AppendTemplateContent(string filePath)
        {
            using (TextWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("using System.Collections.Generic;\nusing Android.Widget;"); //using
                writer.WriteLine();
                writer.WriteLine("namespace " + Namespace);
                writer.WriteLine("{");
                writer.WriteLine("\tpublic partial class "+ ParentFile.Name);
                writer.WriteLine("\t{");
                WriteControls(writer);
                writer.WriteLine("\t}");
                writer.WriteLine("}");
            }
        }

        private static void WriteControls(TextWriter writer)
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
            }
        }
    }
}
