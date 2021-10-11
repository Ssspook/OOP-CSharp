using System.IO;
using System.IO.Compression;
using System.Linq;
using Backups.RestorePointServices;
using Backups.Services;

namespace Backups
{
    public class StoringAlgorithms : IStoringAlgorithms
    {
        public void SingleStoring(BackupJob backupJob, RestorePoint restorePoint)
        {
            if (backupJob == null)
                throw new BackupException("Backup Job cannot be null");
            if (restorePoint == null)
                throw new BackupException("Restore Point cannot be null");

            if (!backupJob.RestorePoints.Contains(restorePoint))
                throw new BackupException($"{restorePoint.Name} restore point is not a part of {backupJob.Name}");

            string zipFile = $"{restorePoint.Path}/archive.zip";

            using (var archive = ZipFile.Open(zipFile, ZipArchiveMode.Create))
            {
                foreach (var file in backupJob.FilesToBackup)
                {
                    archive.CreateEntryFromFile(file.Path, Path.GetFileName(file.Path));
                }
            }
        }

        public void SplitStoring(BackupJob backupJob, RestorePoint restorePoint)
        {
            if (backupJob == null)
                throw new BackupException("Backup Job cannot be null");
            if (restorePoint == null)
                throw new BackupException("Restore Point cannot be null");

            if (!backupJob.RestorePoints.Contains(restorePoint))
                throw new BackupException($"{restorePoint.Name} restore point is not a part of {backupJob.Name}");

            foreach (var file in backupJob.FilesToBackup)
            {
                string zipFile = $"{restorePoint.Path}/{file.Name}.zip";

                using (var archive = ZipFile.Open(zipFile, ZipArchiveMode.Create))
                {
                    archive.CreateEntryFromFile(file.Path, Path.GetFileName(file.Path));
                }
            }
        }
    }
}