using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Backups.Services;

namespace Backups
{
    public class SingleStoring : IStoringAlgorithm
    {
        public string CreateZipFile(string restorePointPath, string fileName)
        {
            var zipFile = $"{restorePointPath}/{fileName}.zip";
            return zipFile;
        }

        public void Save(string restorePointPath, BackupJob backupJob)
        {
            string zipFile = CreateZipFile(restorePointPath, "archive");
            using (var archive = ZipFile.Open(zipFile, ZipArchiveMode.Create))
            {
                foreach (var file in backupJob.FilesToBackup)
                {
                    archive.CreateEntryFromFile(file.Path, Path.GetFileName(file.Path));
                }
            }
        }
    }
}