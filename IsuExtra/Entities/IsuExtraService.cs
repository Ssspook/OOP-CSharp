using System.Collections.Generic;
using System.Linq;
using IsuExtra.Services;
using IsuExtra.Tools;

namespace IsuExtra.Entities
{
    public class IsuExtraService : IIsuExtraService
    {
        private List<Group> groups = new List<Group>();
        private List<Ognp> _ognps = new List<Ognp>();
        public IsuExtraService()
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
                throw new IsuExtraException($"{group.GroupName} group doesn't exist");

            var student = new Student(name, groupToFind);
            groupToFind.AddStudent(student);
            return student;
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
            Group studentsGroup = groups.FirstOrDefault(studentsGroup => studentsGroup.GetStudents().Contains(student));
            if (CheckScheduleOverlay(studentsGroup, stream))
                throw new IsuExtraException("Your study group schedule overlays chosen ognp stream, please choose another one");
            student.SignToOgnp(ognp, stream);
            return student;
        }

        public Student RemoveStudentFromOgnp(Student student, Ognp ognp)
        {
            if (student == null)
                throw new IsuExtraException("Student cannot be null");
            if (ognp == null)
                throw new IsuExtraException("Ognp cannot be null");
            student.UnsignOfOgnp(ognp);
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
            List<Student> studentsNotAssignedToAnyOgnp = group.GetStudents().Where(student => student.GetOgnps().Count == 0).ToList();

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
            bool isOverlay = stream.GetLessons().Any(ognpLesson => Overlay(group.GetLessons(), ognpLesson));

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