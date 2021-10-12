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
        [Ignore("Not proper filesystem usage")]
        public void TestCase1()
         {
             int storages = 0;
             var manager = new StorageManager();
             var algo = new SplitStoring();
             
             var file1 = new FileInfo(manager.PathToFilesToBackup, "myFile");
             var file2 = new FileInfo(manager.PathToFilesToBackup, "myFile1");
        
             BackupJob job = new BackupJob(algo, "Job1", "/Users/noname/Desktop/Backups");
             job.AddFile(file1);
             job.AddFile(file2);

             var restorePoint1 = job.ProcessJob();
             storages += restorePoint1.CopiesInfo.Count;
             
             job.RemoveFile(file1);
             
             var restorePoint2 = job.ProcessJob();
             storages += restorePoint2.CopiesInfo.Count;
             
             int restorePoints = job.RestorePoints.Count;
             Assert.AreEqual(3, storages);
             Assert.AreEqual(2, restorePoints);
         }
    }
}