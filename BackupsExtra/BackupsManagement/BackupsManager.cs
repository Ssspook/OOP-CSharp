using System.Collections.Generic;
using System.Linq;
using Backups;
using Backups.Entities;
using BackupsExtra.RestorePointsManagement;
using BackupsExtra.Restoring;
using BackupsExtra.Service;

namespace BackupsExtra.BackupsManagement
{
    public class BackupsManager
    {
        private List<BackupJob> _backupJobs;
        private StorageManager _storageManager;

        public BackupsManager(StorageManager storageManager)
        {
            _backupJobs = new List<BackupJob>();
            _storageManager = storageManager;
        }

        public void RestoreData(RestorePoint restorePoint, string restoreToLocation)
        {
            BackupJob backupJob = _backupJobs.FirstOrDefault(backupJob => backupJob.RestorePoints.Contains(restorePoint));
            if (backupJob is null)
                throw new BackupsExtraException("Such restore point cannot be found");
            string restorePointPath = _storageManager.PathToBackupFolder + "/" + backupJob.Name + "/" + restorePoint.Name;
            var restorePointDescriptor = new RestorePointDescription(restorePointPath, restorePoint.CopiesInfo);

            Restore.RestoreData(restoreToLocation, restorePointDescriptor);
        }

        public void AddBackupJob(BackupJob backupJob)
        {
            _backupJobs.Add(backupJob);
        }
    }
}