using System.Collections.Generic;
using System.Linq;
using IsuExtra.Tools;

namespace IsuExtra.Entities
{
    public class Student
    {
        private static uint _uniqueId;
        private readonly List<Ognp> _ognps = new List<Ognp>();
        private uint _maxOgnpAllowed = 2;
        private Group _studentsGroup;
        public Student(string name, Group group)
        {
            _uniqueId++;

            Id = _uniqueId;
            Name = name;
            GroupName = group.GroupName;
            _studentsGroup = group;
        }

        public Student(string name)
            : this(name, null)
        {
            _uniqueId++;

            Id = _uniqueId;
            Name = name;
            GroupName = null;
            _studentsGroup = null;
        }

        public uint Id { get; }

        public string Name { get; }

        public string GroupName { get; }

        public void SignToOgnp(Ognp ognp, Stream stream)
        {
            if (ognp == null)
                throw new IsuExtraException("Ognp parameter cannot be null");
            if (_ognps.Count == _maxOgnpAllowed)
                throw new IsuExtraException($"Student cannot be assigned to more than {_maxOgnpAllowed} ognps");
            if (_studentsGroup == null)
                throw new IsuExtraException($"Student {Name} isn't assigned to any group");
            if (ognp.MegaFaculty == _studentsGroup.MegaFaculty)
                throw new IsuExtraException("Student cannot be assigned to ognp of the same MegaFaculty as his");
            if (_ognps.Contains(ognp))
                throw new IsuExtraException($"Student is already assigned to ognp of {ognp.MegaFaculty}");
            List<Lesson> studentsLessons = _studentsGroup.GetLessons();

            _ognps.Add(ognp);
            stream.AddStudent(this);
        }

        public void UnsignOfOgnp(Ognp ognp)
        {
            if (ognp == null)
                throw new IsuExtraException("Ognp parameter cannot be null");

            if (!_ognps.Contains(ognp))
                throw new IsuExtraException($"Student is not assigned to {ognp.MegaFaculty}'s ognp");

            // Note: if previous exception wasn't thrown we can guarantee that student is assigned to this ognp
            _ognps.Remove(ognp);
        }

        public List<Ognp> GetOgnps()
        {
            return new List<Ognp>(_ognps);
        }
    }
}