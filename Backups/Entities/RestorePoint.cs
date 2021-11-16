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

        public DateTime CreationTime { get; private set; }
        public ReadOnlyCollection<string> CopiesInfo => _copiesInfo.AsReadOnly();

        public void AddBackupedFiles(List<string> backupedFiles)
        {
            _copiesInfo = backupedFiles.ToList();
        }

        public void RemoveFile(string file)
        {
            _copiesInfo.Remove(file);
        }

        public string CreateLogLine()
        {
            string files = string.Empty;
            _copiesInfo.ForEach(file =>
            {
                files += file;
            });
            return $"Restore point {Name} was created with archives paths: " + files;
        }

        public void SetCreationTime(DateTime creationdate)
        {
            CreationTime = creationdate;
        }
    }
}