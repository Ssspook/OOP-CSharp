using System;
using System.Collections.Generic;
using System.IO;
using Backups.Entities;
using BackupsExtra.BackupsManagement;

namespace BackupsExtra.RestorePointsManagement
{
    public class RestorePointsRemover : IRemover
    {
        public void Remove(List<RestorePoint> restorePointsToRemove, BackupJobExtra backupJobExtra)
        {
            restorePointsToRemove.ForEach(restorePointToRemove =>
            {
                int index = restorePointToRemove.CopiesInfo[0].LastIndexOf("/", StringComparison.Ordinal);
                string restorePointPath = restorePointToRemove.CopiesInfo[0].Substring(0, index);

                backupJobExtra.BackupJob.RemoveRestorePoint(restorePointToRemove);
                Directory.Delete(restorePointPath, true);
            });
        }
    }
}