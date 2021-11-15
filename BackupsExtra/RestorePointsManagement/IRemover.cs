using System.Collections.Generic;
using Backups.Entities;
using BackupsExtra.BackupsManagement;

namespace BackupsExtra.RestorePointsManagement
{
    public interface IRemover
    {
        void Remove(List<RestorePoint> restorePointsToRemove, BackupJobExtra backupJobExtra);
    }
}