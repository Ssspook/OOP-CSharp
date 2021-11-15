using System.Collections.Generic;
using System.Linq;
using Backups.Entities;
using BackupsExtra.BackupsManagement;
using BackupsExtra.Service;

namespace BackupsExtra.CleaningAlgorithms
{
    public class HybridAlgorithm : ISortingAlgorithm
    {
        private List<ISortingAlgorithm> _cleaningAlgorithms;
        private HybridMode _mode;

        public HybridAlgorithm(List<ISortingAlgorithm> cleaningAlgorithms, HybridMode mode)
        {
            _cleaningAlgorithms = cleaningAlgorithms;
            _mode = mode;
        }

        public List<RestorePoint> Clean(BackupJobExtra backupJobExtra)
        {
            var restorePointsToDelete = new List<RestorePoint>();
            switch (_mode)
            {
                case HybridMode.Harsh:
                    _cleaningAlgorithms.ForEach(algo =>
                    {
                        restorePointsToDelete = algo.Clean(backupJobExtra).Union(restorePointsToDelete).ToList();
                    });
                    break;
                case HybridMode.Soft:
                    _cleaningAlgorithms.ForEach(algo =>
                    {
                        restorePointsToDelete = algo.Clean(backupJobExtra).Intersect(restorePointsToDelete).ToList();
                    });
                    break;
            }

            if (restorePointsToDelete.Count == backupJobExtra.RestorePoints.Count)
                throw new BackupsExtraException("You can't delete all restore points");

            return restorePointsToDelete;
        }
    }
}