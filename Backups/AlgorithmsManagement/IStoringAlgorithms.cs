using Backups.RestorePointServices;

namespace Backups
{
    public interface IStoringAlgorithms
    {
        public void SingleStoring(BackupJob backupJob, RestorePoint restorePoint);
        public void SplitStoring(BackupJob backupJob, RestorePoint restorePoint);

        // Place for your algorithms!
    }
}