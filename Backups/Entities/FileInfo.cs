using Backups.Services;

namespace Backups
{
    public class FileInfo
    {
        public FileInfo(string filePath, string name)
        {
            if (filePath == null)
                throw new BackupException("File Path cannot be null");
            if (name == null)
                throw new BackupException("Name cannot be null");
            Path = $"{filePath}/{name}";
            Name = name;
        }

        public string Path { get; }
        public string Name { get; }
    }
}