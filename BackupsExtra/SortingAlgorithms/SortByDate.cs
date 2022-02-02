using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Entities;
using BackupsExtra.BackupsManagement;
using BackupsExtra.CleaningAlgorithms;
using BackupsExtra.Service;

namespace BackupsExtra.SortingAlgorithms
{
    public class SortByDate : ISortingAlgorithm
    {
        private DateTime _oldestDate;

        public SortByDate(DateTime oldestDate)
        {
            _oldestDate = oldestDate;
        }

        public List<RestorePoint> Clean(BackupJobExtra backupJobExtra)
        {
            var restorePointsToDelete = backupJobExtra.RestorePoints.Where(restorePoint =>
                restorePoint.CreationTime < _oldestDate).ToList();

            if (restorePointsToDelete.Count == backupJobExtra.RestorePoints.Count)
                throw new BackupsExtraException("You cannot delete all restore points");

            return restorePointsToDelete;
        }
    }
}