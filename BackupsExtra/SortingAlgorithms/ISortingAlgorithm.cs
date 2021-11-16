using System.Collections.Generic;
using Backups.Entities;
using BackupsExtra.BackupsManagement;

namespace BackupsExtra.CleaningAlgorithms
{
    public interface ISortingAlgorithm
    {
        List<RestorePoint> Clean(BackupJobExtra backupJob);
    }
}