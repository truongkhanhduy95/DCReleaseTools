using System;
using System.Linq;

namespace DCReleaseTools
{
    public class AndroidControl
    {
        public string Type { get; private set; }
        public string Name { get; private set; }
        public string Id { get; private set; }

        public AndroidControl(string type, string id)
        {
            Type = ParseType(type);
            Name = ParseName(id);
            Id = ParseId(id);
        }

        private string ParseType(string type)
        {
            return type.Split('.').Last();
        }

        private string ParseName(string id)
        {
            id = id.Replace("@+id/", string.Empty);
            return id.First().ToString().ToUpper() + id.Substring(1);
        }

        private string ParseId(string id)
        {
            id = id.Replace("@+id/", string.Empty);
            return string.Format("Resource.Layout." + id);
        }


        public override string ToString()
        {
            return string.Format("[AndroidControl: Type={0}, Name={1}, Id={2}]", Type, Name, Id);
        }
    }
}
