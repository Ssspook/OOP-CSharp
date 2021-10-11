using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Backups.RestorePointServices;
using Backups.Services;

namespace Backups
{
    public class StorageManager : IRepository
    {
        private StoringAlgorithms _algorithms = new StoringAlgorithms();
        public void ProcessBackupJob(BackupJob backupJob, RestorePoint restorePoint)
        {
            if (backupJob == null)
                throw new BackupException("Backup Job cannot be null");
            if (restorePoint == null)
                throw new BackupException("Restore Point cannot be null");
            if (backupJob.StoringType == "SingleStorage")
                _algorithms.SingleStoring(backupJob, restorePoint);
            else
                _algorithms.SplitStoring(backupJob, restorePoint);
        }
    }
}