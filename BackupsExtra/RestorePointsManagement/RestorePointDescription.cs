using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BackupsExtra.RestorePointsManagement
{
    public class RestorePointDescription
    {
        public RestorePointDescription(string pathToRestorePoint, ReadOnlyCollection<string> copiesInfo)
        {
            PathToRestorePoint = pathToRestorePoint;
            CopiesInfo = copiesInfo;
        }

        public string PathToRestorePoint { get; }
        public ReadOnlyCollection<string> CopiesInfo { get; }
    }
}