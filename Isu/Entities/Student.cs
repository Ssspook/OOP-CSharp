using Isu.Tools;

namespace Isu
{
    public class Student
    {
        private static int _uniqueId;

        public Student(string name, string group)
        {
            _uniqueId++;
            if (_uniqueId > 9999)
            {
                throw new IsuException("No more students can be added");
            }

            Id = 1000000000 + _uniqueId;
            Name = name;
            Group = group;
        }

        public Student(string name)
        {
            _uniqueId++;
            if (_uniqueId > 9999)
            {
                throw new IsuException("No more students can be added");
            }

            Id = 1000000000 + _uniqueId;
            Name = name;
            Group = null;
        }

        public int Id { get; }
        public string Name { get; }
        public string Group { get; }
    }
}
