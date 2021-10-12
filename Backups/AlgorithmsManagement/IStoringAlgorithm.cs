using System.Collections.Generic;
using System.Linq;
using Backups.Services;

namespace Backups
{
    public interface IStoringAlgorithm
    {
        public string CreateZipFile(string restorePointPath, string fileName);
        public void Save(string restorePointPath, BackupJob backupJob);
    }
}