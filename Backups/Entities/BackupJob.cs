using System.Collections.Generic;
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
        public BackupJob(IStoringAlgorithm algorithm, string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new BackupException("Name cannot be null");
            _algorithm = algorithm;
            Name = name;
        }

        public string Name { get; }
        public IReadOnlyList<RestorePoint> RestorePoints => _restorePoints.AsReadOnly();

        public IReadOnlyList<FileInfo> FilesToBackup => _filesToBackup;
        public IStoringAlgorithm Algorithm => _algorithm;
        public RestorePoint ProcessJob(IRepository storageManager)
        {
            List<string> zipFiles = _algorithm.Save(_filesToBackup);
            RestorePoint resPoint = storageManager.SaveToRepository(zipFiles, this);
            _restorePoints.Add(resPoint);
            return resPoint;
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

        public void AddRestorePoints(List<RestorePoint> restorePoints)
        {
            _restorePoints = restorePoints.ToList();
        }

        public void RemoveRestorePoint(RestorePoint restorePoint)
        {
            _restorePoints.Remove(restorePoint);
        }
    }
}