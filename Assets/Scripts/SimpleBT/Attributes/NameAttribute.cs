using System;

namespace SimpleBT.Attributes
{
    public class NameAttribute: Attribute
    {
        public NameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}