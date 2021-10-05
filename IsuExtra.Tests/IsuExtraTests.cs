using System;
using System.Collections.Generic;
using Isu;
using NUnit.Framework;
using IsuExtra.Entities;
using IsuExtra.Tools;

namespace IsuExtra.Tests
{
    [TestFixture]
    public class IsuExtraTests
    {
        private IsuExtraService _isuExtraService;
        private Ognp _ognp;
        private Group _group;
        private Stream _streamForOgnp;
        [SetUp]
        public void Setup()
        {
            _isuExtraService = new IsuExtraService();
            _ognp = _isuExtraService.AddOgnp("MegaFaculty 1");
            _group = _isuExtraService.AddGroup("R3201");
            _streamForOgnp = _isuExtraService.AddStreamToOgnp(_ognp);
        }

        [Test]
        public void AddNewOgnp_OgnpAdded()
        {
            Ognp searchingOgnp = _isuExtraService.FindOgnp(_ognp);
            Assert.AreNotEqual(searchingOgnp, null);
        }

        [Test]
        public void AddNewOgnp_OgnpAlreadyExists_ThrowException()
        {
            Assert.Catch<IsuExtraException>(() =>
            {
                Ognp ognp2 = _isuExtraService.AddOgnp("MegaFaculty 1");
            });
        }

        [Test]
        public void AddStudentToOgnp_StudentAdded()
        {
            Student student = _isuExtraService.AddStudent(_group, "Ivan Ivanov");
            
            student = _isuExtraService.AddStudentToOgnpCourse(student, _ognp, _streamForOgnp);
            Assert.True(_isuExtraService.GetStudentsOgnps(student).Contains(_ognp));
        }

        [Test]
        public void AddStudentToOgnpOfHisMegaFaculty_ThrowException()
        {
            Group invalidGroup = _isuExtraService.AddGroup("M3201");
            Student student = _isuExtraService.AddStudent(invalidGroup, "Ivan Ivanov");
            
            Assert.Catch<IsuExtraException>(() =>
            {
                student = _isuExtraService.AddStudentToOgnpCourse(student, _ognp, _streamForOgnp);
            });
        }

        [Test]
        public void AddStudentToOgnpStream_ScheduleOverlay_ThrowException()
        {
            DateTime startTimeMath = new DateTime(2021, 1, 1, 9, 0, 0);
            DateTime endTimeMath = new DateTime(2021, 1, 1, 10, 30, 0);

            DateTime startTimeOop = new DateTime(2021, 1, 1, 10, 40, 0);
            DateTime endTimeOop = new DateTime(2021, 1, 1, 12, 10, 0);

            Lesson math = new Lesson(startTimeMath, endTimeMath, "Prof 1", "320");
            Lesson oop = new Lesson(startTimeOop, endTimeOop, "Prof 2", "440");
            _isuExtraService.AddLessonToGroup(math, _group);
            _isuExtraService.AddLessonToGroup(oop, _group);

            DateTime startTimeOgnp = new DateTime(2021, 1, 1, 9, 30, 0);
            DateTime endTimeOgnp = new DateTime(2021, 1, 1, 11, 00, 0);

            Lesson ognpLesson = new Lesson(startTimeOgnp, endTimeOgnp, "Prof 3", "110");
            _streamForOgnp.AddLesson(ognpLesson);
            
            Student student = _isuExtraService.AddStudent(_group, "Ivan Ivanov");
            
            Assert.Catch<IsuExtraException>(() =>
            {
                _isuExtraService.AddStudentToOgnpCourse(student, _ognp, _streamForOgnp);
            });
        }

        [Test]
        public void AddStudentOnThirdOgnp_ThrowException()
        {
            Ognp ognp2 = _isuExtraService.AddOgnp("MegaFaculty 3");
            Stream streamForOgnp2 = _isuExtraService.AddStreamToOgnp(ognp2);
            
            Ognp ognp3 = _isuExtraService.AddOgnp("MegaFaculty 4");
            Stream streamForOgnp3 = _isuExtraService.AddStreamToOgnp(ognp3);
            
            Student student = _isuExtraService.AddStudent(_group, "Maxim Maximov");
            _isuExtraService.AddStudentToOgnpCourse(student, _ognp, _streamForOgnp);
            _isuExtraService.AddStudentToOgnpCourse(student, ognp2, streamForOgnp2);
            Assert.Catch<IsuExtraException>(() =>
            {
                _isuExtraService.AddStudentToOgnpCourse(student, ognp3, streamForOgnp3);
            });
        }
        
        [Test]
        public void UnsignOfOgnp_StudentUnsigned()
        {
            Student student = _isuExtraService.AddStudent(_group, "Ivan Ivanov");
            
            student = _isuExtraService.AddStudentToOgnpCourse(student, _ognp, _streamForOgnp);
            _isuExtraService.RemoveStudentFromOgnp(student, _ognp);
            Assert.True(!_isuExtraService.GetStudentsOgnps(student).Contains(_ognp));
        }

        [Test]
        public void UnsignOfOgnp_StudentIsntSignedToChosenOgnp_ThrowException()
        {
            Student student = _isuExtraService.AddStudent(_group, "Ivan Ivanov");

            Assert.Catch<IsuExtraException>(() =>
            {
                _isuExtraService.RemoveStudentFromOgnp(student, _ognp);
            });
        }

        [Test]
        public void GetStreamsList_ListAcquired()
        {
            Stream stream2 = _isuExtraService.AddStreamToOgnp(_ognp);
            List<Stream> streamsList = _isuExtraService.GetStreamsList(_ognp);
            Assert.True(streamsList.Contains(_streamForOgnp) && streamsList.Contains(stream2));
        }

        [Test]
        public void GetStreamsList_ListIsEmpty_ReturnNull()
        {
            Ognp newOgnp = _isuExtraService.AddOgnp("MegaFaculty 2");
            List<Stream> streamsList = _isuExtraService.GetStreamsList(newOgnp);
            Assert.AreEqual(streamsList, null);
        }
        
        [Test]
        public void GetStudentsListOnStream_ListAcquired()
        {
            Student student1 = _isuExtraService.AddStudent(_group, "Name1");

            Group groupForStudent2 = _isuExtraService.AddGroup("U3201");
            Student student2 = _isuExtraService.AddStudent(groupForStudent2, "Name2");
            
            _isuExtraService.AddStudentToOgnpCourse(student1, _ognp, _streamForOgnp);
            _isuExtraService.AddStudentToOgnpCourse(student2, _ognp, _streamForOgnp);
            
            List<Student> studentsList = _isuExtraService.GetStudentsSignedToOgnpOnStream(_streamForOgnp);
            Assert.True(studentsList.Contains(student1) && studentsList.Contains(student2));
        }
        
        [Test]
        public void GetStudentsListNotAssignedToAnyOgnp_ListAcquired()
        {
            Student student1 = _isuExtraService.AddStudent(_group, "Name1");
            Student student2 = _isuExtraService.AddStudent(_group, "Name2");
            List<Student> notAssignedToOgnpStudents = _isuExtraService.GetStudentsWithoutOgnp(_group);
            Assert.True(notAssignedToOgnpStudents.Contains(student1) && notAssignedToOgnpStudents.Contains(student2));
        }
        
        [Test]
        public void GetStudentsListNotAssignedToAnyOgnp_EverybodyAssigned_ReturnNull()
        {
            Ognp ognp2 = _isuExtraService.AddOgnp("MegaFaculty 3");
            Stream streamForOgnp2 = _isuExtraService.AddStreamToOgnp(ognp2);
            
            Student student1 = _isuExtraService.AddStudent(_group, "Name1");
            Student student2 = _isuExtraService.AddStudent(_group, "Name2");
            
            _isuExtraService.AddStudentToOgnpCourse(student1, _ognp, _streamForOgnp);
            _isuExtraService.AddStudentToOgnpCourse(student2, ognp2, streamForOgnp2);
            
            List<Student> notAssignedToOgnpStudents = _isuExtraService.GetStudentsWithoutOgnp(_group);
            Assert.AreEqual(null, notAssignedToOgnpStudents);
        }
    }
}