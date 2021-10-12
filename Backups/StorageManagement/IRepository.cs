using Backups.Entities;

namespace Backups
{
    public interface IRepository
    {
        public void CreateBackupJobAndRestorePointDirectories(BackupJob backupJob, RestorePoint restorePoint);
        public string GetRestorePointPath(string restorePointName, string jobName);
    }
}