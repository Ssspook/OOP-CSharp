using Backups.RestorePointServices;

namespace Backups
{
    public interface IRepository
    {
        public void ProcessBackupJob(BackupJob backupJob, RestorePoint restorePoint);
    }
}