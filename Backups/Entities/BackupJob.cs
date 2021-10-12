using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups.Entities;
using Backups.Services;

namespace Backups
{
    public class BackupJob
    {
        private List<FileInfo> _filesToBackup = new List<FileInfo>();
        private List<RestorePoint> _restorePoints = new List<RestorePoint>();
        private IStoringAlgorithm _algorithm;
        public BackupJob(IStoringAlgorithm algorithm, string name, string pathToBackup)
        {
            if (name == null)
                throw new BackupException("Name cannot be null");
            if (algorithm == null)
                throw new BackupException("Algorithm type cannot be null");
            if (pathToBackup == null)
                throw new BackupException("Path to backup cannot be null");

            PathToBackup = $"{pathToBackup}/{name}";

            _algorithm = algorithm;
            Name = name;
        }

        public string Name { get; }
        public string PathToBackup { get; }
        public IReadOnlyCollection<FileInfo> FilesToBackup => _filesToBackup.AsReadOnly();
        public IReadOnlyCollection<RestorePoint> RestorePoints => _restorePoints.AsReadOnly();

        public RestorePoint ProcessJob()
        {
            var storageManager = new StorageManager();
            string restorePointPath = storageManager.GetRestorePointPath(DateTime.Now.ToString("HH:mm:ss"), Name);
            var newRestorePoint = new RestorePoint(DateTime.Now, _filesToBackup, DateTime.Now.ToString("HH:mm:ss"), restorePointPath);
            storageManager.CreateBackupJobAndRestorePointDirectories(this, newRestorePoint);
            _restorePoints.Add(newRestorePoint);

            _algorithm.Save(newRestorePoint.Path, backupJob: this);
            return newRestorePoint;
        }

        public void RemoveFile(FileInfo file)
        {
            if (file == null)
                throw new BackupException("File cannot be null");
            _filesToBackup.Remove(file);
        }

        public void AddFile(FileInfo file)
        {
            if (file == null)
                throw new BackupException("File cannot be null");
            _filesToBackup.Add(file);
        }
    }
}