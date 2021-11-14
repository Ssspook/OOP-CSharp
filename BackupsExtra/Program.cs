using System.Collections.Generic;
using System.IO;
using Backups;
using Backups.Entities;
using BackupsExtra.BackupsManagement;
using BackupsExtra.ContextSaving;
using FileInfo = Backups.FileInfo;

namespace BackupsExtra
{
    internal class Program
    {
        private static void Main()
        {
            var manager = new StorageManager("/Users/noname/Desktop/Backups", "/Users/noname/Desktop/FilesToBackup");
            var algo = new SingleStoring();
            var file1 = new FileInfo(manager.PathToFilesToBackup, "myFile3");
            var file2 = new FileInfo(manager.PathToFilesToBackup, "myFile4");
            FileStream fs = File.Create(file1.Path);

            FileStream fs2 = File.Create(file2.Path);
            fs.Close();
            fs2.Close();
            var job2 = new BackupJob(algo, "Job2");
            job2.AddFile(file1);
            job2.AddFile(file2);
            RestorePoint rp = job2.ProcessJob(manager);

            // var backupJobsList = new List<BackupJob>() { job2 };
            // var a = new ContextSaver();
            // a.SaveInfo(backupJobsList);
            // a.DownloadInfo();
            var backupsManager = new BackupsManager(manager);
            backupsManager.AddBackupJob(job2);
            backupsManager.RestoreData(rp, manager.PathToFilesToBackup);
        }
    }
}
