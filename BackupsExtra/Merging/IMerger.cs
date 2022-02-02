using Backups.Entities;

namespace BackupsExtra.Merging
{
    public interface IMerger
    {
        RestorePoint Merge(RestorePoint oldRestorePoint, RestorePoint newRestorePoint);
    }
}