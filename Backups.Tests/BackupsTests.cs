
using System.IO;
using System.Threading;
using NUnit.Framework;

namespace Backups.Tests
{
    [TestFixture]
    public class BackupsTests
    {
        [Test]
        [Ignore("Not proper filesystem usage")]
        public void TestCase1()
         {
             int storages = 0;
             var manager = new StorageManager("/Users/noname/Desktop/Backups", "/Users/noname/Desktop/FilesToBackup");
             var algo = new SplitStoring();
             
             var file1 = new FileInfo(manager.PathToFilesToBackup, "myFile");
             var file2 = new FileInfo(manager.PathToFilesToBackup, "myFile1");
             FileStream fs = File.Create(file1.Path);

             FileStream fs2 = File.Create(file2.Path);
             fs.Close();
             fs2.Close();
             
             var job = new BackupJob(algo, "Job1");
             job.AddFile(file1);
             job.AddFile(file2);

             var restorePoint1 = job.ProcessJob(manager);
             storages += restorePoint1.CopiesInfo.Count;
             
             job.RemoveFile(file1);
             Thread.Sleep(2000);
             var restorePoint2 = job.ProcessJob(manager);
             storages += restorePoint2.CopiesInfo.Count;
             
             int restorePoints = job.RestorePoints.Count;
             Assert.AreEqual(3, storages);
             Assert.AreEqual(2, restorePoints);
         }
    }
}