using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Tools;
using IsuExtra.Tools;

namespace IsuExtra.Entities
{
    public class Group
    {
        private readonly List<Student> _students = new List<Student>();
        private List<Lesson> _lessons = new List<Lesson>();
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
            /*
             As long as our goal is not to study university's group naming, I decided to simplify Mega Faculty initialization,
             by applying the following scheme:
             First sign(letter) of group name is Mega faculty
             M -- MegaFaculty 1
             R -- MegaFaculty 2
             U -- MegaFaculty 3
             N -- MegaFaculty 4
             */
            var allowedMegaFaculties = new List<string>()
            {
                "M", "R", "U", "N",
            };

            string megaFacultyLetter, course, groupNum, studyType;
            megaFacultyLetter = name.Substring(0, 1);
            studyType = name.Substring(1, 1);
            course = name.Substring(2, 1);
            groupNum = name.Substring(3, 2);

            if (name.Length != 5 || !allowedMegaFaculties.Contains(megaFacultyLetter) || !allowedCourses.Contains(course) || !allowedGroups.Contains(groupNum) || studyType != "3")
                throw new IsuException("Invalid group name");

            string groupName = name;
            Course = Convert.ToUInt32(groupName.Substring(2, 1));
            _groupNumber = groupName.Substring(3, 2);

            GroupName = $"{megaFacultyLetter}3{Course}{_groupNumber}";
            switch (megaFacultyLetter)
            {
                case "M":
                    MegaFaculty = "MegaFaculty 1";
                    break;
                case "R":
                    MegaFaculty = "MegaFaculty 2";
                    break;
                case "U":
                    MegaFaculty = "MegaFaculty 3";
                    break;
                case "N":
                    MegaFaculty = "MegaFaculty 4";
                    break;
            }
        }

        public string GroupName { get; }

        public uint Course { get; }

        public string MegaFaculty { get; }
        public void AddStudent(Student student)
        {
            if (_students.Count == numberOfMembersAllowed)
                throw new IsuException($"This group is full. You cannot add students to {GroupName}");

            _students.Add(student);
        }

        public Student FindStudent(string name)
        {
            Student student = _students.SingleOrDefault(student => student.Name == name);

            return student;
        }

        public Student FindStudent(uint id)
        {
            Student student = _students.SingleOrDefault(student => student.Id == id);

            return student;
        }

        public void RemoveStudent(uint id)
        {
            Student student = _students.SingleOrDefault(student => student.Id == id);
            _students.Remove(student);
        }

        public void AddLesson(Lesson lesson)
        {
            if (lesson == null)
                throw new IsuExtraException("Lesson cannot be null");
            _lessons.Add(lesson);
        }

        public List<Lesson> GetLessons()
        {
            return new List<Lesson>(_lessons);
        }

        public List<Student> GetStudents()
        {
            return new List<Student>(_students);
        }
    }
}