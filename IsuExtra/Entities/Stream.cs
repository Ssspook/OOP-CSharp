using System.Collections.Generic;
using System.Linq;
using Isu;
using IsuExtra.Tools;

namespace IsuExtra.Entities
{
    public class Stream
    {
        private static uint _id = 0;
        private static uint _maxStudentsQuantity = 24;
        private List<Lesson> _lessons = new List<Lesson>();
        private List<Student> _students = new List<Student>();

        public Stream()
        {
            _id++;
            Id = _id;
        }

        public uint Id { get; }

        public List<Student> GetStudents()
        {
            return new List<Student>(_students);
        }

        public void AddLesson(Lesson lesson)
        {
            _lessons.Add(lesson);
        }

        public void AddStudent(Student student)
        {
            if (student == null)
                throw new IsuExtraException("Student cannot be null");
            if (_students.Count == _maxStudentsQuantity)
                throw new IsuExtraException("This stream is full, please choose another one");

            _students.Add(student);
        }

        public List<Lesson> GetLessons()
        {
            return new List<Lesson>(_lessons);
        }

        public void RemoveStudent(Student student)
        {
            if (student == null)
                throw new IsuExtraException("Student cannot be null");
            _students.Remove(student);
        }
    }
}