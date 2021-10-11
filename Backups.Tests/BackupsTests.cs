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
            int restorePoints = 0;
            
            StorageManager _storageManager = new StorageManager();
            FileInfo file1 = new FileInfo("/Users/noname/Desktop/FilesToBackup/myFile", "myFile");
            FileInfo file2 = new FileInfo("/Users/noname/Desktop/FilesToBackup/myFile1", "myFile1");
            
            FileStream fs = File.Create(file1.Path);
            FileStream fs2 = File.Create(file2.Path);
            fs.Close();
            fs2.Close();

            BackupJob job = new BackupJob("SplitStorage", "Job1", "/Users/noname/Desktop/Backups");
            job.AddFile(file1);
            job.AddFile(file2);
            var restorePoint1 = job.CreateRestorePoint("RestorePoint1", DateTime.Now);
            storages += restorePoint1.CopiesInfo.Count;
            
            _storageManager.ProcessBackupJob(job, restorePoint1);
            job.RemoveFile(file1);
            
            var restorePoint2 = job.CreateRestorePoint("RestorePoint2", DateTime.Now);
            storages += restorePoint2.CopiesInfo.Count;
            
            _storageManager.ProcessBackupJob(job, restorePoint2);
            restorePoints = job.RestorePoints.Count;
            Assert.AreEqual(3, storages);
            Assert.AreEqual(2, restorePoints);
        }
    }
}