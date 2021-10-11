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
             StorageManager manager = new StorageManager();
             FileInfo file1 = new FileInfo("/Users/noname/Desktop/FilesToBackup/myFile3", "myFile3");
             FileInfo file2 = new FileInfo("/Users/noname/Desktop/FilesToBackup/myFile4", "myFile4");
             FileStream fs = File.Create(file1.Path);

             FileStream fs2 = File.Create(file2.Path);
             fs.Close();
             fs2.Close();
             BackupJob job2 = new BackupJob("SingleStorage", "Job2", "/Users/noname/Desktop/Backups");
             job2.AddFile(file1);
             job2.AddFile(file2);
             var restorePoint = job2.CreateRestorePoint("RestorePoint2", DateTime.Now);
             manager.ProcessBackupJob(job2, restorePoint);
        }
    }
}
