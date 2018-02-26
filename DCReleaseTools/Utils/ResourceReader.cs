using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace DCReleaseTools.Utils
{
    public class ResourceReader
    {
        public List<AndroidControl> Controls { get; private set; }

        public ResourceReader()
        {
            Controls = new List<AndroidControl>();
        }

        public ResourceReader(string path)
        {
            Controls = new List<AndroidControl>();
            LoadFromResource(path);
        }

        public void LoadFromResource(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            Console.WriteLine("Path: " + path);

            IterateNode(doc.ChildNodes);
        }

        private void IterateNode(XmlNodeList listNode)
        {
            foreach (var x in listNode)
            {
                var node = x as XmlNode;
                var control = GetMeaningControl(node);
                if (control != null)
                    Controls.Add(control);

                if (node.HasChildNodes)
                {
                    IterateNode(node.ChildNodes);
                }
            }
        }

        private AndroidControl GetMeaningControl(XmlNode node)
        {
            if (node.Attributes != null)
            {
                var id = node.Attributes["android:id"]?.Value;
                if (!string.IsNullOrEmpty(id))
                {
                    return new AndroidControl(node.Name, id);
                }
            }
            return null;
        }
    }
}
