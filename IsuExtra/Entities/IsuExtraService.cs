using System.Collections.Generic;
using System.Linq;
using Isu;
using IsuExtra.Services;
using IsuExtra.Tools;

namespace IsuExtra.Entities
{
    public class IsuExtraService : IIsuExtraService
    {
        private List<Ognp> _ognps = new List<Ognp>();
        private Dictionary<Group, List<Lesson>> _groupsLessons = new Dictionary<Group, List<Lesson>>();
        private int _maxOgnpAllowed = 2;
        private IsuService _isuService = new IsuService();
        public Group AddGroup(string name)
        {
            if (name == null)
                throw new IsuExtraException("Name cannot be null");
            Group group = _isuService.AddGroup(name);
            return group;
        }

        public Student AddStudent(Group group, string name)
        {
            Group groupToFind = _isuService.GetGroups().SingleOrDefault(searchedGroup => searchedGroup.GroupName == group.GroupName);

            if (groupToFind == null)
                throw new IsuExtraException($"{group.GroupName} group doesn't exist");
            if (name == null)
                throw new IsuExtraException("Name cannot be null");
            var student = new Student(name, groupToFind.GroupName);
            group.AddStudent(student);
            return student;
        }

        public void AddLessonToGroup(Lesson lesson, Group group)
        {
            if (lesson == null)
                throw new IsuExtraException("Lesson cannot be null");
            if (group == null)
                throw new IsuExtraException("Group cannot be null");
            if (!_groupsLessons.Keys.Contains(group))
            {
                var lessons = new List<Lesson>();
                lessons.Add(lesson);
                _groupsLessons.Add(group, lessons);
                return;
            }

            _groupsLessons[group].Add(lesson);
        }

        public List<Lesson> GetGroupLessons(Group group)
        {
            if (group == null)
                throw new IsuExtraException("Group cannot be null");
            if (!_groupsLessons.Keys.Contains(group))
                throw new IsuExtraException($"There is no group with {group.GroupName} name");
            return new List<Lesson>(_groupsLessons[group]);
        }

        public Ognp AddOgnp(string megaFaculty)
        {
            if (megaFaculty == null)
                throw new IsuExtraException("MegaFaculty cannot be null");
            Ognp searchigOgnp = _ognps.FirstOrDefault(searchingOgnp => searchingOgnp.MegaFaculty == megaFaculty);
            if (searchigOgnp != null)
                throw new IsuExtraException($"The ognp of {megaFaculty} MegaFaculty is already added");

            Ognp ognp = new Ognp(megaFaculty);
            _ognps.Add(ognp);
            return ognp;
        }

        public Stream AddStreamToOgnp(Ognp ognp)
        {
            if (ognp == null)
                throw new IsuExtraException("Ognp cannot be null");
            Stream stream = new Stream();
            ognp.AddStream(stream);
            return stream;
        }

        public Student AddStudentToOgnpCourse(Student student, Ognp ognp, Stream stream)
        {
            if (student == null)
                throw new IsuExtraException("Student cannot be null");
            if (ognp == null)
                throw new IsuExtraException("Ognp cannot be null");
            if (stream == null)
                throw new IsuExtraException("Stream cannot be null");
            if (GetStudentsOgnps(student).Count == _maxOgnpAllowed)
                throw new IsuExtraException($"Student cannot be assigned to more than {_maxOgnpAllowed} ognps");

            Group studentsGroup = _isuService.GetGroups().FirstOrDefault(studentsGroup => studentsGroup.Students.Contains(student));
            if (studentsGroup == null)
                throw new IsuExtraException("Student isn't assigned to any group");

            if (studentsGroup.MegaFaculty == ognp.MegaFaculty)
                throw new IsuExtraException("Student cannot be assigned to ognp of his own megafaculty");
            if (CheckScheduleOverlay(studentsGroup, stream))
                throw new IsuExtraException("Your study group schedule overlays chosen ognp stream, please choose another one");
            stream.AddStudent(student);
            return student;
        }

        public List<Ognp> GetStudentsOgnps(Student student)
        {
            if (student == null)
                throw new IsuExtraException("Student cannot be null");
            var studentsOgnps = _ognps.Where(ognp =>
                GetStreamsList(ognp).Any(stream => stream.GetStudents().Contains(student))).ToList();

            return studentsOgnps;
        }

        public Student RemoveStudentFromOgnp(Student student, Ognp ognp)
        {
            if (student == null)
                throw new IsuExtraException("Student cannot be null");
            if (ognp == null)
                throw new IsuExtraException("Ognp parameter cannot be null");

            List<Ognp> studentsOgnps = GetStudentsOgnps(student);
            if (!studentsOgnps.Contains(ognp))
                throw new IsuExtraException($"Student is not assigned to {ognp.MegaFaculty}'s ognp");

            // Note: if previous exception wasn't thrown we can guarantee that student is assigned to this ognp
            Stream searchedStream =
                ognp.GetStreams().First(searchedStream => searchedStream.GetStudents().Contains(student));
            searchedStream.RemoveStudent(student);
            return student;
        }

        public List<Stream> GetStreamsList(Ognp ognp)
        {
            if (ognp == null)
                throw new IsuExtraException("Ognp cannot be null");
            return ognp.GetStreams();
        }

        public List<Student> GetStudentsSignedToOgnpOnStream(Stream stream)
        {
            if (stream == null)
                throw new IsuExtraException("Stream cannot be null");
            return stream.GetStudents();
        }

        public List<Student> GetStudentsWithoutOgnp(Group group)
        {
            if (group == null)
                throw new IsuExtraException("Group cannot be null");
            var studentsNotAssignedToAnyOgnp = group.Students.Where(student => GetStudentsOgnps(student).Count == 0).ToList();

            return !studentsNotAssignedToAnyOgnp.Any() ? null : studentsNotAssignedToAnyOgnp;
        }

        public Ognp FindOgnp(Ognp ognp)
        {
            if (ognp == null)
                throw new IsuExtraException("Ognp cannot be null");
            Ognp searchingOgnp = _ognps.FirstOrDefault(searchingOgnp => searchingOgnp.MegaFaculty == ognp.MegaFaculty);
            return searchingOgnp;
        }

        private bool CheckScheduleOverlay(Group group, Stream stream)
        {
            if (group == null)
                throw new IsuExtraException("Group cannot be null");
            if (stream == null)
                throw new IsuExtraException("Stream cannot be null");
            bool isOverlay = stream.GetLessons().Any(ognpLesson => Overlay(GetGroupLessons(group), ognpLesson));

            return isOverlay;
        }

        private bool Overlay(List<Lesson> groupLessons, Lesson ognpLesson)
        {
            if (groupLessons == null)
                throw new IsuExtraException("Lesson list cannot be null");
            if (ognpLesson == null)
                throw new IsuExtraException("Ognp lesson cannot be null");

            bool isOverlay = groupLessons.Any(
                groupLesson =>
                    (groupLesson.EndTime >= ognpLesson.StartTime && groupLesson.EndTime <= ognpLesson.EndTime) ||
                    (groupLesson.StartTime >= ognpLesson.StartTime && groupLesson.StartTime <= ognpLesson.EndTime));
            return isOverlay;
        }
    }
}