using Isu.Services;
using Isu.Tools;
using NUnit.Framework;

namespace Isu.Tests
{
    [TestFixture]
    public class Tests
    {
        private IIsuService _isuService;

        [SetUp]
        public void Setup()
        {
            //TODO: implement        
            _isuService = new IsuService();
        }

        [Test]
        public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
        {
            Group group = _isuService.AddGroup("M3201");
            Student student = _isuService.AddStudent(group, "Ivan Ivanov");
            

            if (_isuService.GetStudent(student.Id) == null && student.Group != group.GroupName)
            {
                Assert.Fail("Group doesn't contain student and student isn't assigned to a group");
            }
            else if (_isuService.GetStudent(student.Id) != null && student.Group != group.GroupName)
            {
                Assert.Fail("Student isn't assigned to a group");
            }
            else if (_isuService.GetStudent(student.Id) == null && student.Group == group.GroupName)
            {
                Assert.Fail("Group doesn't contain student");
            }

        }

        [Test]
        public void ReachMaxStudentPerGroup_ThrowException()
        {

            Assert.Catch<IsuException>(() =>
            {
                Group group = _isuService.AddGroup("M3201");

                for (int i = 0; i < 31; i++)
                {
                    _isuService.AddStudent(group, "Maxim Maximov");
                }
            });
        }

        [Test]
        public void CreateGroupWithInvalidName_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                var group = new Group("M310599");
            });
        }

        [Test]
        public void TransferStudentToAnotherGroup_GroupChanged()
        {
            var initialGroup = new Group("M3201");
            var newGroup = new Group("M3202");

            _isuService.AddGroup("M3201");
            _isuService.AddGroup("M3202");

            var student = new Student("Artem Artemovich");

            _isuService.AddStudent(initialGroup, "Artem Artemovich");
            _isuService.ChangeStudentGroup(student, newGroup);

            if (student.Group == initialGroup.GroupName)
            {
                Assert.Fail("Group hasn't changed!");
            }
        }
    }
}