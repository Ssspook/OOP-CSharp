namespace Isu
{
    public class Student
    {
        private static uint _uniqueId;

        public Student(string name, string group)
        {
            _uniqueId++;

            Id = _uniqueId;
            Name = name;
            Group = group;
        }

        public Student(string name)
            : this(name, null)
        {
            _uniqueId++;

            Id = _uniqueId;
            Name = name;
            Group = null;
        }

        public uint Id { get; }
        public string Name { get; }
        public string Group { get; }
    }
}