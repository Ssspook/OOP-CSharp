using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Tools;

namespace Isu
{
    public class Group
    {
        private readonly List<Student> students = new List<Student>();
        private uint numberOfMembersAllowed = 30;
        private string _groupNumber;

        public Group(string name)
        {
            var allowedCourses = new List<string>() { "1", "2", "3", "4", "5", "6" };

            var allowedGroups = new List<string>()
            {
              "01", "02", "03", "04", "05", "06",
              "07", "08", "09", "10", "11", "12",
              "13", "14", "15",
            };

            string header, course, groupNum;
            header = name.Substring(0, 2);
            course = name.Substring(2, 1);
            groupNum = name.Substring(3, 2);

            if (name.Length != 5 || header != "M3" || !allowedCourses.Contains(course) || !allowedGroups.Contains(groupNum))
                throw new IsuException("Invalid group name");

            string groupName = name;
            Course = Convert.ToUInt32(groupName.Substring(2, 1));
            _groupNumber = groupName.Substring(3, 2);

            GroupName = $"M3{Course}{_groupNumber}";
        }

        public string GroupName { get; }

        public uint Course { get; }

        public List<Student> Students { get; }

        public void AddStudent(Student student)
        {
            if (students.Count == numberOfMembersAllowed)
                throw new IsuException($"This group is full. You cannot add students to {GroupName}");

            students.Add(student);
        }

        public Student FindStudent(string name)
        {
            Student student = students.SingleOrDefault(student => student.Name == name);

            return student;
        }

        public Student FindStudent(uint id)
        {
            Student student = students.SingleOrDefault(student => student.Id == id);

            return student;
        }

        public void RemoveStudent(uint id)
        {
            Student student = students.SingleOrDefault(student => student.Id == id);
            students.Remove(student);
        }
    }
}