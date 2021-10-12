using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Backups
{
    public class SplitStoring : IStoringAlgorithm
    {
        public string CreateZipFile(string restorePointPath, string fileName)
        {
            string zipFile = $"{restorePointPath}/{fileName}.zip";
            return zipFile;
        }

        public void Save(string restorePointPath, BackupJob backupJob)
        {
            foreach (var file in backupJob.FilesToBackup)
            {
                    string zipFile = CreateZipFile(restorePointPath, file.Name);

                    using (var archive = ZipFile.Open(zipFile, ZipArchiveMode.Create))
                    {
                        archive.CreateEntryFromFile(file.Path, Path.GetFileName(file.Path));
                    }
            }
        }
    }
}