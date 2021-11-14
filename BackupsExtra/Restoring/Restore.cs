using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using BackupsExtra.RestorePointsManagement;
using FileInfo = System.IO.FileInfo;

namespace BackupsExtra.Restoring
{
    public class Restore
    {
        public static void RestoreData(string restoreToLocation, RestorePointDescription restorePointDescriptor)
        {
            string pathToRestorePoint = restorePointDescriptor.PathToRestorePoint;
            var filesInZip = new List<string>();

            foreach (string zipFile in restorePointDescriptor.CopiesInfo)
            {
                ZipArchive zip = ZipFile.OpenRead(pathToRestorePoint + "/" + zipFile);
                filesInZip = zip.Entries.Select(file => file.Name).ToList();
            }

            var directoryInfo = new DirectoryInfo(restoreToLocation);
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                if (filesInZip.Contains(file.Name))
                    File.Delete(file.FullName);
            }

            foreach (string fileToMove in restorePointDescriptor.CopiesInfo)
            {
                ZipFile.ExtractToDirectory(pathToRestorePoint + "/" + fileToMove, restoreToLocation);
            }
        }
    }
}