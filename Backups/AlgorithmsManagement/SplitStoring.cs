using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Backups
{
    public class SplitStoring : IStoringAlgorithm
    {
        public List<string> Save(List<FileInfo> filesToBackup)
        {
            var storages = new List<string>();

            foreach (var file in filesToBackup)
            {
                    string zipFile = $"{file.Name}.zip";

                    using (var archive = ZipFile.Open(zipFile, ZipArchiveMode.Create))
                    {
                        archive.CreateEntryFromFile(file.Path, Path.GetFileName(file.Path));
                    }

                    storages.Add(zipFile);
            }

            return storages;
        }
    }
}