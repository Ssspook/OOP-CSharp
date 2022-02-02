using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups.Entities;

namespace BackupsExtra.Merging
{
    public class FileSystemMerger : IMerger
    {
        public RestorePoint Merge(RestorePoint oldRestorePoint, RestorePoint newRestorePoint)
        {
            var newFiles = new List<string>();

            int indexOfLastSlashInOldRestorePoint = oldRestorePoint.CopiesInfo[0].LastIndexOf("/", StringComparison.Ordinal);
            string oldRestorePointPath = oldRestorePoint.CopiesInfo[0].Substring(0, indexOfLastSlashInOldRestorePoint);

            int indexOfLastSlashInNewRestorePoint = newRestorePoint.CopiesInfo[0].LastIndexOf("/", StringComparison.Ordinal);
            string newRestorePointPath = newRestorePoint.CopiesInfo[0].Substring(0, indexOfLastSlashInNewRestorePoint);

            oldRestorePoint.CopiesInfo.ToList().ForEach(file =>
            {
                int index = file.LastIndexOf("/", StringComparison.Ordinal);
                int oldFileNameLength = file.Length - index;
                string oldFileName = file.Substring(indexOfLastSlashInNewRestorePoint, oldFileNameLength);

                newRestorePoint.CopiesInfo.ToList().ForEach(newRestorePointFile =>
                {
                    int fileNameLength = newRestorePointFile.Length - indexOfLastSlashInNewRestorePoint;
                    string fileName = newRestorePointFile.Substring(indexOfLastSlashInNewRestorePoint, fileNameLength);

                    if (fileName != oldFileName && !newFiles.Contains($"{newRestorePointPath}/{oldFileName}"))
                    {
                        string newFilePath = $"{newRestorePointPath}/{oldFileName}";
                        try
                        {
                            File.Move(file, newFilePath); // if we catch moving error, than file already exists
                        }
                        catch (System.IO.IOException)
                        {
                        }

                        newFiles.Add(newFilePath);
                    }
                });
            });
            newRestorePoint.AddBackupedFiles(newFiles);

            Directory.Delete(oldRestorePointPath, true);

            return newRestorePoint;
        }
    }
}