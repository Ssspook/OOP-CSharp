using System;
using System.Collections.Generic;
using System.IO;
using Backups.Entities;
using Backups.Services;

namespace Backups
{
    public class StorageManager : IRepository
    {
        private string _pathToBackUpFolder;

        private string _pathToFilesToBackup;
        public StorageManager(string pathToBackupFolder, string pathToFilesToBackup)
        {
            _pathToBackUpFolder = pathToBackupFolder;
            _pathToFilesToBackup = pathToFilesToBackup;
        }

        public string PathToFilesToBackup => _pathToFilesToBackup;
        public string PathToBackupFolder => _pathToBackUpFolder;
        public RestorePoint SaveToRepository(List<string> storages, BackupJob backupJob)
        {
            var filePaths = new List<string>();
            if (backupJob == null)
                throw new BackupException("Backup Job cannot be null");
            if (storages == null)
                throw new BackupException("Storages cannot be null");

            var newRestorePoint = new RestorePoint(DateTime.Now, DateTime.Now.ToString("HH:mm:ss"));
            Directory.CreateDirectory($"{_pathToBackUpFolder}/{backupJob.Name}/{newRestorePoint.Name}");

            foreach (var storage in storages)
            {
                File.Move(Path.GetFullPath(storage), $"{_pathToBackUpFolder}/{backupJob.Name}/{newRestorePoint.Name}/{storage}");
                string pathToFile = _pathToBackUpFolder + "/" + backupJob.Name + "/" + newRestorePoint.Name + "/" + storage;
                filePaths.Add(pathToFile);
            }

            newRestorePoint.AddBackupedFiles(filePaths);
            return newRestorePoint;
        }
    }
}