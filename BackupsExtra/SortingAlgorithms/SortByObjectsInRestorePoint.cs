using System.Collections.Generic;
using System.Linq;
using Backups.Entities;
using BackupsExtra.BackupsManagement;
using BackupsExtra.Service;

namespace BackupsExtra.CleaningAlgorithms
{
    public class SortByObjectsInRestorePoint : ISortingAlgorithm
    {
        private int _maxObjectsInRestorePoint;

        public SortByObjectsInRestorePoint(int maxObjectsInRestorePoint)
        {
            _maxObjectsInRestorePoint = maxObjectsInRestorePoint;
        }

        public List<RestorePoint> Clean(BackupJobExtra backupJobExtra)
        {
            var restorePointsToDelete = backupJobExtra.RestorePoints.Where(restorePoint => restorePoint.CopiesInfo.Count > _maxObjectsInRestorePoint).ToList();

            if (restorePointsToDelete.Count == backupJobExtra.RestorePoints.Count)
                throw new BackupsExtraException("You cannot delete all restore points");

            return restorePointsToDelete;
        }
    }
}