using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Compression;
using System.Linq;
using Backups.Services;

namespace Backups.Entities
{
    public class RestorePoint
    {
        private List<string> _copiesInfo;
        public RestorePoint(DateTime creationTime, string name)
        {
            if (name == null)
                throw new BackupException("Name cannot be null");
            Name = name;
        }

        public string Name { get; }
        public ReadOnlyCollection<string> CopiesInfo => _copiesInfo.AsReadOnly();

        public void AddBackupedFiles(List<string> backupedFiles)
        {
            _copiesInfo = backupedFiles.ToList();
        }
    }
}