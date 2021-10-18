using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Backups
{
    public class SingleStoring : IStoringAlgorithm
    {
        public List<string> Save(List<FileInfo> filesToBackup)
        {
            var storages = new List<string>();
            string zipFile = "archive.zip";

            using (var archive = ZipFile.Open(zipFile, ZipArchiveMode.Create))
            {
                foreach (var file in filesToBackup)
                {
                    archive.CreateEntryFromFile(file.Path, Path.GetFileName(file.Path));
                }

                storages.Add(zipFile);
            }

            return storages;
        }
    }
}