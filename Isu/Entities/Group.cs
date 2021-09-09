using System;
using System.Collections.Generic;
using Isu.Tools;

namespace Isu
{
    public class Group
    {
        private static uint numberOfMembersAllowed = 30;
        private readonly List<Student> students = new List<Student>();
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
            {
                throw new IsuException("Invalid group name");
            }

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
            numberOfMembersAllowed--;
            if (numberOfMembersAllowed == 0)
            {
                throw new IsuException($"This group is full. You cannot add students to {GroupName}");
            }

            students.Add(student);
        }

        public Student FindStudent(string name)
        {
            foreach (Student student in students)
            {
                if (student.Name == name)
                {
                    return student;
                }
            }

            return null;
        }

        public Student FindStudent(int id)
        {
            foreach (Student student in students)
            {
                if (student.Id == id)
                {
                    return student;
                }
            }

            return null;
        }

        public void RemoveStudent(int id)
        {
            foreach (Student student in students)
            {
                if (student.Id == id)
                {
                    students.Remove(student);
                    return;
                }
            }
        }
    }
}
