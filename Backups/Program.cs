using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Backups
{
    internal class Program
    {
        private static void Main()
        {
            var manager = new StorageManager();
            var algo = new SingleStoring();
            var file1 = new FileInfo(manager.PathToFilesToBackup, "myFile3");
            var file2 = new FileInfo(manager.PathToFilesToBackup, "myFile4");
            FileStream fs = File.Create(file1.Path);

            FileStream fs2 = File.Create(file2.Path);
            fs.Close();
            fs2.Close();
            var job2 = new BackupJob(algo, "Job2", manager.PathToBackUpFolder);
            job2.AddFile(file1);
            job2.AddFile(file2);
            job2.ProcessJob();
        }
    }
}
