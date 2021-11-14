using Backups;
using Backups.Entities;
using BackupsExtra.Loggers;

namespace BackupsExtra.BackupsManagement
{
    public class BackupJobExtra
    {
        private ILogger _logger;
        private BackupJob _backupJob;

        public BackupJobExtra(IStoringAlgorithm algorithm, string name, ILogger logger)
        {
            _logger = logger;
            _backupJob = new BackupJob(algorithm, name);
        }

        public RestorePoint ProcessJob(IRepository storageManager)
        {
            RestorePoint restorePoint = _backupJob.ProcessJob(storageManager);
            string backupJobLogLine = _backupJob.CreateLogLine();
            string restorePointLogLine = restorePoint.CreateLogLine();

            _logger.Log(backupJobLogLine);
            _logger.Log(restorePointLogLine);
            return restorePoint;
        }
    }
}