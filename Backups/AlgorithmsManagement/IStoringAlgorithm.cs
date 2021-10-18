using System.Collections.Generic;

namespace Backups
{
    public interface IStoringAlgorithm
    {
        public List<string> Save(List<FileInfo> filesToBackup);
    }
}