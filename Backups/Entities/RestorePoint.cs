using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Backups.Services;

namespace Backups.RestorePointServices
{
    public class RestorePoint
    {
        private List<FileInfo> _copiesInfo = new List<FileInfo>();
        private string _restorePointPath;
        public RestorePoint(DateTime creationTime, List<FileInfo> backupedFiles, string name)
        {
            if (backupedFiles == null)
                throw new BackupException("Backuped files cannot be null");
            if (name == null)
                throw new BackupException("Name cannot be null");

            CreationTime = creationTime;
            Name = name;
            _copiesInfo = backupedFiles.ToList();
        }

        public DateTime CreationTime { get; }
        public string Name { get; }
        public ReadOnlyCollection<FileInfo> CopiesInfo => _copiesInfo.AsReadOnly();
        public string Path => _restorePointPath;

        public void SetPath(string path)
        {
            if (path == null)
                throw new BackupException("Path cannot be null");
            _restorePointPath = path;
        }
    }
}