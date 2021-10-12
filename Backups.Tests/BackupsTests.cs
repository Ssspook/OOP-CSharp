using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace Backups.Tests
{
    [TestFixture]
    public class BackupsTests
    {
        [Test]
        public void TestCase1()
        {
            int storages = 0;
            FileInfo file1 = new FileInfo("/Users/noname/Desktop/FilesToBackup/myFile", "myFile");
            FileInfo file2 = new FileInfo("/Users/noname/Desktop/FilesToBackup/myFile1", "myFile1");
            

            BackupJob job = new BackupJob("SplitStorage", "Job1", "/Users/noname/Desktop/Backups");
            job.AddFile(file1);
            job.AddFile(file2);
            
            var restorePoint1 = job.CreateRestorePoint("RestorePoint1", DateTime.Now);
            storages += restorePoint1.CopiesInfo.Count;
            
            job.RemoveFile(file1);
            
            var restorePoint2 = job.CreateRestorePoint("RestorePoint2", DateTime.Now);
            storages += restorePoint2.CopiesInfo.Count;
            
            int restorePoints = job.RestorePoints.Count;
            Assert.AreEqual(3, storages);
            Assert.AreEqual(2, restorePoints);
        }
    }
}