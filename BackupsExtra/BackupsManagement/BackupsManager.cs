using System;
using System.Collections.Generic;
using System.Linq;
using Backups;
using Backups.Entities;
using BackupsExtra.CleaningAlgorithms;
using BackupsExtra.Loggers;
using BackupsExtra.Merging;
using BackupsExtra.RestorePointsManagement;
using BackupsExtra.Restoring;
using BackupsExtra.Service;

namespace BackupsExtra.BackupsManagement
{
    public class BackupsManager
    {
        private List<BackupJobExtra> _backupJobsExtra;
        private IRemover _restorePointsRemover;
        private IRestorer _restorer;
        private IMerger _merger;
        public BackupsManager(IRemover remover, IRestorer restorer, IMerger merger)
        {
            _backupJobsExtra = new List<BackupJobExtra>();
            _restorePointsRemover = remover;
            _restorer = restorer;
            _merger = merger;
        }

        public IReadOnlyList<BackupJobExtra> BackupJobs => _backupJobsExtra;
        public void RestoreData(RestorePoint restorePoint, string restoreToLocation)
        {
            if (restorePoint is null)
                throw new BackupsExtraException("Restore point cannot be null");

            if (restoreToLocation is null)
                throw new BackupsExtraException("Restore location cannot be null");

            BackupJobExtra backupJob = _backupJobsExtra.FirstOrDefault(backupJob => backupJob.RestorePoints.Contains(restorePoint));
            if (backupJob is null)
                throw new BackupsExtraException("Such restore point cannot be found");

            _restorer.RestoreData(restoreToLocation, restorePoint);
        }

        public BackupJobExtra CreateBackupJob(IStoringAlgorithm algorithm, string name, ILogger logger)
        {
            var backupJob = new BackupJobExtra(algorithm, name, logger);
            _backupJobsExtra.Add(backupJob);

            return backupJob;
        }

        public RestorePoint ProcessJob(BackupJobExtra backupJobExtra, IRepository storageManager, DateTime creationTime)
        {
            if (!_backupJobsExtra.Contains(backupJobExtra))
                throw new BackupsExtraException("There is no such job registered");
            RestorePoint restorePoint = backupJobExtra.ProcessJob(storageManager, creationTime);

            return restorePoint;
        }

        public int CleanBackup(BackupJobExtra backupJobExtra, ISortingAlgorithm sortingAlgorithm)
        {
            List<RestorePoint> restorePointsToRemove = sortingAlgorithm.Clean(backupJobExtra);
            if (restorePointsToRemove.Count == 1)
            {
                if (backupJobExtra.Algorithm is SingleStoring)
                {
                    _restorePointsRemover.Remove(new List<RestorePoint>() { restorePointsToRemove[0] }, backupJobExtra);
                    backupJobExtra.BackupJob.RemoveRestorePoint(restorePointsToRemove[0]);
                }

                _merger.Merge(restorePointsToRemove[0], backupJobExtra.RestorePoints[^1]);
                backupJobExtra.BackupJob.RemoveRestorePoint(restorePointsToRemove[0]);
            }
            else
            {
                _restorePointsRemover.Remove(restorePointsToRemove, backupJobExtra);
                restorePointsToRemove.ForEach(backupJobExtra.BackupJob.RemoveRestorePoint);
            }

            return restorePointsToRemove.Count;
        }
    }
}