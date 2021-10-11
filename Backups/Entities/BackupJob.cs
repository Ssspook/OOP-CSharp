using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups.RestorePointServices;
using Backups.Services;

namespace Backups
{
    public class BackupJob
    {
        private List<FileInfo> _filesToBackup = new List<FileInfo>();
        private List<RestorePoint> _restorePoints = new List<RestorePoint>();

        public BackupJob(string storingType, string name, string pathToBackup)
        {
            if (name == null)
                throw new BackupException("Name cannot be null");
            if (storingType == null)
                throw new BackupException("Storing type cannot be null");
            if (pathToBackup == null)
                throw new BackupException("Path to backup cannot be null");

            PathToBackup = $"{pathToBackup}/{name}";
            Directory.CreateDirectory(PathToBackup);
            StoringType = storingType;
            Name = name;
        }

        public string StoringType { get; }
        public string Name { get; }
        public string PathToBackup { get; }
        public IReadOnlyCollection<FileInfo> FilesToBackup => _filesToBackup.AsReadOnly();
        public IReadOnlyCollection<RestorePoint> RestorePoints => _restorePoints.AsReadOnly();

        public RestorePoint CreateRestorePoint(string name, DateTime creationTime)
        {
            if (name == null)
                throw new BackupException("Name cannot be null");
            if (creationTime == null)
                throw new BackupException("Creation Time cannot be null");

            var newRestorePoint = new RestorePoint(creationTime, _filesToBackup, name);
            _restorePoints.Add(newRestorePoint);
            Directory.CreateDirectory($"{PathToBackup}/{name}");

            newRestorePoint.SetPath($"{PathToBackup}/{name}");
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