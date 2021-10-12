using System;
using System.IO;
using Backups.Entities;
using Backups.Services;

namespace Backups
{
    public class StorageManager : IRepository
    {
        public string PathToBackUpFolder => "/Users/noname/Desktop/Backups";

        public string PathToFilesToBackup => "/Users/noname/Desktop/FilesToBackup";
        public void CreateBackupJobAndRestorePointDirectories(BackupJob backupJob, RestorePoint restorePoint)
        {
            if (backupJob == null)
                throw new BackupException("Backup Job cannot be null");

            Directory.CreateDirectory($"{PathToBackUpFolder}/{backupJob.Name}");
            Directory.CreateDirectory($"{PathToBackUpFolder}/{backupJob.Name}/{restorePoint.Name}");
        }

        public string GetRestorePointPath(string restorePointName, string jobName)
        {
            return $"{PathToBackUpFolder}/{jobName}/{restorePointName}";
        }
    }
}