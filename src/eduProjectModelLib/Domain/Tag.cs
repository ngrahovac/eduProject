﻿namespace eduProjectModel.Domain
{
    public class Tag : IValueObject
    {
        public string Name { get; private set; }

        public string Description { get; private set; }
        public Tag()
        {

        }
        public Tag(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
