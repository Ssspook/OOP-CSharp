using System;
using System.IO;
using System.Linq;
using System.Threading;
using Backups;
using Backups.Entities;
using BackupsExtra.BackupsManagement;
using BackupsExtra.CleaningAlgorithms;
using BackupsExtra.Loggers;
using BackupsExtra.Merging;
using BackupsExtra.RestorePointsManagement;
using BackupsExtra.Restoring;
using BackupsExtra.Service;
using BackupsExtra.SortingAlgorithms;
using NUnit.Framework;
using FileInfo = Backups.FileInfo;

namespace BackupsExtra.Tests
{
    [TestFixture]
    public class BackupsExtraTests
    {
        private StorageManager _storageManager;
        private IMerger _merger;
        private IRemover _remover;
        private BackupsManager _backupsManager;
        [SetUp]
        public void Setup()
        {
            _storageManager = new StorageManager("/Users/noname/Desktop/Backups", "/Users/noname/Desktop/FilesToBackup");
            _merger = new FileSystemMerger();
            _remover = new RestorePointsRemover();
            _backupsManager = new BackupsManager(_remover, new Restore(), _merger);
        }

        [Ignore("Not proper filesystem usage")]
        [Test]
        public void SortByDate_RestorePointsRemoved()
        {
            IStoringAlgorithm storingAlgorithm = new SplitStoring();
            ILogger logger = new ConsoleLogger(false);
            BackupJobExtra job1 = _backupsManager.CreateBackupJob(storingAlgorithm, "job1", logger);

            var file1 = new FileInfo(_storageManager.PathToFilesToBackup, "myFile");
            var file2 = new FileInfo(_storageManager.PathToFilesToBackup, "myFile1");
            FileStream fs = File.Create(file1.Path);

            FileStream fs2 = File.Create(file2.Path);
            fs.Close();
            fs2.Close();
            job1.AddFile(file1);
            job1.AddFile(file2);

            var oldestDate = new DateTime(2021, 8, 1, 12, 0, 0);
            ISortingAlgorithm sortingAlgorithm = new SortByDate(oldestDate);
            RestorePoint restorePointToDelete1 = _backupsManager.ProcessJob(job1, _storageManager, new DateTime(2021, 8, 1, 11, 0, 0));
            Thread.Sleep(1000);
            RestorePoint restorePointToDelete2 = _backupsManager.ProcessJob(job1, _storageManager, new DateTime(2021, 8, 1, 11, 23, 0));

            Thread.Sleep(1000);
            RestorePoint restorePointToKeep = _backupsManager.ProcessJob(job1, _storageManager, new DateTime(2021, 8, 1, 13, 0, 0));
            Thread.Sleep(1000);
            RestorePoint restorePointToKeep1 = _backupsManager.ProcessJob(job1, _storageManager, new DateTime(2021, 8, 1, 14, 0, 0));
            Thread.Sleep(1000);
            _backupsManager.CleanBackup(job1, sortingAlgorithm);
            Assert.True(!job1.RestorePoints.Contains(restorePointToDelete1) &&
                        !job1.RestorePoints.Contains(restorePointToDelete2) &&
                        job1.RestorePoints.Contains(restorePointToKeep) &&
                        job1.RestorePoints.Contains(restorePointToKeep1));
        }

        [Ignore("Not proper filesystem usage")]
        [Test]
        public void TryToRemoveAllRestorePoint_ThrowException()
        {
            IStoringAlgorithm storingAlgorithm = new SplitStoring();
            ILogger logger = new ConsoleLogger(false);
            BackupJobExtra job1 = _backupsManager.CreateBackupJob(storingAlgorithm, "job1", logger);

            var file1 = new FileInfo(_storageManager.PathToFilesToBackup, "myFile");
            var file2 = new FileInfo(_storageManager.PathToFilesToBackup, "myFile1");
            FileStream fs = File.Create(file1.Path);

            FileStream fs2 = File.Create(file2.Path);
            fs.Close();
            fs2.Close();
            job1.AddFile(file1);
            job1.AddFile(file2);
            ISortingAlgorithm algorithm = new SortByObjectsInRestorePoint(0);
            _backupsManager.ProcessJob(job1, _storageManager, new DateTime(2021, 8, 1, 11, 0, 0));
            Assert.Catch<BackupsExtraException>(() =>
            {
                _backupsManager.CleanBackup(job1, algorithm);
            });
        }

        [Ignore("Not proper filesystem usage")]
        [Test]
        public void MergingTwoRestorePoints_RestorePointsMerged()
        {
            IStoringAlgorithm storingAlgorithm = new SplitStoring();
            ILogger logger = new ConsoleLogger(false);
            BackupJobExtra job1 = _backupsManager.CreateBackupJob(storingAlgorithm, "job2", logger);

            var file1 = new FileInfo(_storageManager.PathToFilesToBackup, "myFile");
            var file2 = new FileInfo(_storageManager.PathToFilesToBackup, "myFile1");
            var file3 = new FileInfo(_storageManager.PathToFilesToBackup, "myFile3");
            var file4 = new FileInfo(_storageManager.PathToFilesToBackup, "myFile4");
            FileStream fs = File.Create(file1.Path);

            FileStream fs2 = File.Create(file2.Path);
            FileStream fs3 = File.Create(file3.Path);

            FileStream fs4 = File.Create(file4.Path);
            fs.Close();
            fs2.Close();
            fs3.Close();
            fs4.Close();
            job1.AddFile(file1);
            job1.AddFile(file2);
            job1.AddFile(file3);
            job1.AddFile(file4);
            ISortingAlgorithm algorithm = new SortByQuantity(2);
            RestorePoint restorePoint1 = _backupsManager.ProcessJob(job1, _storageManager, new DateTime(2021, 8, 1, 11, 0, 0));
            Thread.Sleep(1000);
            job1.RemoveFile(file3);
            job1.RemoveFile(file4);
            job1.RemoveFile(file1);
            job1.RemoveFile(file2);
            RestorePoint restorePoint2 = _backupsManager.ProcessJob(job1, _storageManager, new DateTime(2021, 8, 1, 11, 0, 0));
            Thread.Sleep(1000);
            job1.AddFile(file3);
            job1.AddFile(file4);

            RestorePoint restorePoint3 = _backupsManager.ProcessJob(job1, _storageManager, new DateTime(2021, 8, 1, 11, 0, 0));

            Console.WriteLine(_backupsManager.CleanBackup(job1, algorithm));
            Assert.True(!job1.RestorePoints.Contains(restorePoint1));
            Assert.True(restorePoint3.CopiesInfo.Contains($"{_storageManager.PathToBackupFolder}/{job1.Name}/{restorePoint3.Name}/{file1.Name}") &&
                        restorePoint3.CopiesInfo.Contains($"{_storageManager.PathToBackupFolder}/{job1.Name}/{restorePoint3.Name}/{file2.Name}") &&
                        restorePoint3.CopiesInfo.Contains($"{_storageManager.PathToBackupFolder}/{job1.Name}/{restorePoint3.Name}/{file3.Name}") &&
                        restorePoint3.CopiesInfo.Contains($"{_storageManager.PathToBackupFolder}/{job1.Name}/{restorePoint3.Name}/{file4.Name}"));

        }
    }
}