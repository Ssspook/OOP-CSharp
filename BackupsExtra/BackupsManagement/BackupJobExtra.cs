using System;
using System.Collections.Generic;
using System.Linq;
using Backups;
using Backups.Entities;
using BackupsExtra.CleaningAlgorithms;
using BackupsExtra.Loggers;
using BackupsExtra.RestorePointsManagement;

namespace BackupsExtra.BackupsManagement
{
    public class BackupJobExtra
    {
        private ILogger _logger;
        private BackupJob _backupJob;
        public BackupJobExtra(IStoringAlgorithm algorithm, string name, ILogger logger)
        {
            _logger = logger;
            _backupJob = new BackupJob(algorithm, name);
        }

        public IReadOnlyList<RestorePoint> RestorePoints => BackupJob.RestorePoints;
        public IReadOnlyList<FileInfo> FilesToBackup => _backupJob.FilesToBackup;
        public IStoringAlgorithm Algorithm => _backupJob.Algorithm;
        public string Name => _backupJob.Name;
        public BackupJob BackupJob => _backupJob;
        public RestorePoint ProcessJob(IRepository storageManager, DateTime creationTime)
        {
            RestorePoint restorePoint = _backupJob.ProcessJob(storageManager);
            restorePoint.SetCreationTime(creationTime);

            string backupJobLogLine = CreateLogLine();
            string restorePointLogLine = restorePoint.CreateLogLine();

            _logger.Log(backupJobLogLine);
            _logger.Log(restorePointLogLine);
            return restorePoint;
        }

        public void AddFile(FileInfo file)
        {
            BackupJob.AddFile(file);
        }

        public void RemoveFile(FileInfo file)
        {
            BackupJob.RemoveFile(file);
        }

        private string CreateLogLine()
        {
            string restorePoints = RestorePoints.Aggregate(string.Empty, (current, restorePoint) => current + (restorePoint.Name + " "));

            return $"Backup job {Name} was created with restore points: " + restorePoints;
        }
    }
}