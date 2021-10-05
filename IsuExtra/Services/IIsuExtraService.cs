using System.Collections.Generic;
using Isu;
using IsuExtra.Entities;

namespace IsuExtra.Services
{
    public interface IIsuExtraService
    {
        Group AddGroup(string name);
        Student AddStudent(Group group, string name);
        Ognp AddOgnp(string megaFaculty);
        Student AddStudentToOgnpCourse(Student student, Ognp ognp, Stream stream);
        Student RemoveStudentFromOgnp(Student student, Ognp ognp);
        List<Stream> GetStreamsList(Ognp ognp);
        List<Student> GetStudentsSignedToOgnpOnStream(Stream stream);
        List<Student> GetStudentsWithoutOgnp(Group group);
    }
}