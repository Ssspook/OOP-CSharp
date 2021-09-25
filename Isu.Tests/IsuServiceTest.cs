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
            _isuService = null;
        }

        [Test]
        public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
        {
        }

        [Test]
        public void ReachMaxStudentPerGroup_ThrowException()
        {
        }

        [Test]
        public void CreateGroupWithInvalidName_ThrowException()
        {
        }

        [Test]
        public void TransferStudentToAnotherGroup_GroupChanged()
        {
        }
    }
}