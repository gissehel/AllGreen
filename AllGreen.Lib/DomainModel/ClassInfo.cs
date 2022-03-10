using System;

namespace AllGreen.Lib.DomainModel
{
    public abstract class ClassInfo
    {
        protected abstract string SuffixToRemove { get; }

        public ClassInfo(Type t)
        {
            Type = t;
        }

        public ClassInfo(string name)
        {
            _name = name;
        }

        public Type Type { get; private set; }

        public string ClassName => Type != null ? Type.Name : SlugName + "_" + SuffixToRemove.ToLower();

        public string SlugName => Name.Replace(" ", "_");

        private string _name = null;

        public string Namespace => Type?.Namespace;

        public string Name
        {
            get
            {
                if (_name != null)
                {
                    return _name;
                }
                var name = Type.Name;
                if (name.ToLower().EndsWith(SuffixToRemove.ToLower()))
                {
                    name = name.Substring(0, name.Length - SuffixToRemove.Length);
                }
                name = name.Trim('_').Replace("_", " ");
                return name;
            }
        }
    }
}