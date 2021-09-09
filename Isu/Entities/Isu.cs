using System.Collections.Generic;
using Isu.Services;
using Isu.Tools;

namespace Isu
{
    public class Isu : IIsuService
    {
        private List<Group> groups = new List<Group>();

        public Isu()
        {
        }

        public Group AddGroup(string name)
        {
            var gr = new Group(name);
            groups.Add(gr);
            return gr;
        }

        public Student AddStudent(Group group, string name)
        {
            foreach (Group searchedGroup in groups)
            {
                if (searchedGroup.GroupName == group.GroupName)
                {
                    var student = new Student(name, searchedGroup.GroupName);
                    searchedGroup.AddStudent(student);
                    return student;
                }
            }

            throw new IsuException($"{group.GroupName} group doesn't exist");
        }

        public Student GetStudent(int id)
        {
            foreach (Group group in groups)
            {
                Student student = group.FindStudent(id);
                if (student != null)
                {
                    return student;
                }
            }

            throw new IsuException("There is no such student");
        }

        public Student FindStudent(string name)
        {
            foreach (Group group in groups)
            {
                Student student = group.FindStudent(name);
                if (student != null)
                {
                    return student;
                }
            }

            throw new IsuException("There is no such student");
        }

        public List<Student> FindStudents(string groupName)
        {
            foreach (Group group in groups)
            {
                if (group.GroupName == groupName)
                {
                    return group.Students;
                }
            }

            throw new IsuException($"{groupName} group doesn't exist");
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            foreach (Group group in groups)
            {
                if (group.Course == courseNumber.Number)
                {
                    return group.Students;
                }
            }

            throw new IsuException($"Nobody is on {courseNumber.Number} course");
        }

        public Group FindGroup(string groupName)
        {
            foreach (Group group in groups)
            {
                if (group.GroupName == groupName)
                {
                    return group;
                }
            }

            throw new IsuException($"{groupName} group doesn't exist");
        }

        public List<Group> FindGroups(CourseNumber course)
        {
            var groupsWithFindingCourse = new List<Group>();

            foreach (Group group in groups)
            {
                if (group.Course == course.Number)
                {
                    groupsWithFindingCourse.Add(group);
                }
            }

            if (groupsWithFindingCourse.Count == 0)
            {
                throw new IsuException($"No groups of {course.Number} course found");
            }
            else
            {
                return groupsWithFindingCourse;
            }
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            foreach (Group group in groups)
            {
                Student foundStudent = group.FindStudent(student.Name);
                if (foundStudent != null)
                {
                    group.RemoveStudent(student.Id);
                    newGroup.AddStudent(student);
                    return;
                }
            }
        }
    }
}
