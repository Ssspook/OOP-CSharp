using System.Collections.Generic;
using System.IO.Compression;
using Backups.Entities;

namespace Backups
{
    public interface IRepository
    {
        public RestorePoint SaveToRepository(List<string> zipFiles, BackupJob backupJob);
    }
}