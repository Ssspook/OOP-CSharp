using System.Collections.Generic;
using System.IO;
using Backups.Entities;
using BackupsExtra.BackupsManagement;
using BackupsExtra.Service;

namespace BackupsExtra.CleaningAlgorithms
{
    public class SortByQuantity : ISortingAlgorithm
    {
        private int _limit;

        public SortByQuantity(int limit)
        {
            _limit = limit;
        }

        public List<RestorePoint> Clean(BackupJobExtra backupJobExtra)
        {
            var restorePointsToDelete = new List<RestorePoint>();
            int counter = 0;
            while (backupJobExtra.RestorePoints.Count - counter > _limit)
            {
                restorePointsToDelete.Add(backupJobExtra.RestorePoints[counter]);
                counter++;
            }

            if (restorePointsToDelete.Count == backupJobExtra.RestorePoints.Count)
                throw new BackupsExtraException("You cannot delete all restore points");

            return restorePointsToDelete;
        }
    }
}