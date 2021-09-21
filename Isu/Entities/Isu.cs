using System.Collections.Generic;
using System.Linq;
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
            Group groupToFind = groups.SingleOrDefault(searchedGroup => searchedGroup.GroupName == group.GroupName);

            if (groupToFind == null)
                throw new IsuException($"{group.GroupName} group doesn't exist");

            var student = new Student(name, groupToFind.GroupName);
            groupToFind.AddStudent(student);
            return student;
        }

        public Student GetStudent(uint id)
        {
            Group group = groups.SingleOrDefault(group => group.FindStudent(id) != null);

            if (group == null)
                throw new IsuException("There is no such student");

            return group.FindStudent(id);
        }

        public Student FindStudent(string name)
        {
            Group group = groups.SingleOrDefault(group => group.FindStudent(name) != null);

            if (group == null)
                throw new IsuException("There is no such student");

            return group.FindStudent(name);
        }

        public List<Student> FindStudents(string groupName)
        {
            Group group = groups.SingleOrDefault(group => group.GroupName == groupName);
            if (group == null)
                throw new IsuException($"{group.GroupName} group doesn't exist");

            return group.Students;
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            Group group = groups.SingleOrDefault(group => group.Course == courseNumber.Number);
            if (group == null)
                throw new IsuException($"{courseNumber.Number} course doesn't exist");

            return group.Students;
        }

        public Group FindGroup(string groupName)
        {
            Group group = groups.SingleOrDefault(group => group.GroupName == groupName);
            if (group == null)
                throw new IsuException($"{groupName} group doesn't exist");

            return group;
        }

        public List<Group> FindGroups(CourseNumber course)
        {
            var groupsToFind = groups.Where(group => group.Course == course.Number).ToList();

            if (!groupsToFind.Any())
                throw new IsuException($"No groups of {course.Number} course found");

            return groupsToFind;
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            Group groupToRemoveStudentFrom = groups.SingleOrDefault(group => group.GroupName == newGroup.GroupName);

            newGroup.AddStudent(student);
            groupToRemoveStudentFrom.RemoveStudent(student.Id);
        }
    }
}
