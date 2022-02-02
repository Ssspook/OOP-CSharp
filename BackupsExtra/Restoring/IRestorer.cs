using Backups.Entities;

namespace BackupsExtra.Restoring
{
    public interface IRestorer
    {
        void RestoreData(string restoreToLocation, RestorePoint restorePoint);
    }
}