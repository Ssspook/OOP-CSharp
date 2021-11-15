using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Backups.Entities;
using BackupsExtra.RestorePointsManagement;
using FileInfo = System.IO.FileInfo;

namespace BackupsExtra.Restoring
{
    public class Restore : IRestorer
    {
        public void RestoreData(string restoreToLocation, RestorePoint restorePoint)
        {
            var filesInZip = new List<string>();

            foreach (string file in restorePoint.CopiesInfo)
            {
                ZipArchive zip = ZipFile.OpenRead(file);
                filesInZip = zip.Entries.Select(fileInZip => fileInZip.Name).ToList();
            }

            var directoryInfo = new DirectoryInfo(restoreToLocation);
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                if (filesInZip.Contains(file.Name))
                    File.Delete(file.FullName);
            }

            foreach (string fileToMove in restorePoint.CopiesInfo)
            {
                ZipFile.ExtractToDirectory(fileToMove, restoreToLocation);
            }
        }
    }
}