using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Backups;
using Backups.Entities;
using FileInfo = Backups.FileInfo;

namespace BackupsExtra.ContextSaving
{
    public class ContextSaver
    {
        public ContextSaver()
        {
            if (File.Exists("JobsInfo")) return;

            var xmlFile = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            xmlFile.Add(new XElement("Jobs"));
            xmlFile.Save("JobsInfo");
        }

        public void SaveInfo(List<BackupJob> backupJobs)
        {
            var backupJobsInfo = new XmlDocument();
            backupJobsInfo.Load("JobsInfo");

            XmlElement xRoot = backupJobsInfo.DocumentElement;

            backupJobs.ForEach(backupJob =>
            {
                XmlElement jobElem = backupJobsInfo.CreateElement("job");
                XmlAttribute jobNameAttr = backupJobsInfo.CreateAttribute("name");
                XmlText name = backupJobsInfo.CreateTextNode(backupJob.Name);
                jobNameAttr.AppendChild(name);

                XmlElement storingAlgorithmElem = backupJobsInfo.CreateElement("storingAlgorithm");
                XmlElement filesToBackupElem = backupJobsInfo.CreateElement("filesToBackup");
                XmlElement restorePoints = backupJobsInfo.CreateElement("restorePoints");

                jobElem.AppendChild(filesToBackupElem);
                jobElem.AppendChild(restorePoints);

                XmlNode algoNode = backupJobsInfo.CreateTextNode(GetAlgoType(backupJob.Algorithm));
                storingAlgorithmElem.AppendChild(algoNode);

                jobElem.AppendChild(storingAlgorithmElem);
                jobElem.Attributes.Append(jobNameAttr);

                foreach (FileInfo file in backupJob.FilesToBackup)
                {
                    XmlElement fileElem = backupJobsInfo.CreateElement("file");
                    XmlNode fileNode = backupJobsInfo.CreateTextNode(file.Path);

                    fileElem.AppendChild(fileNode);
                    filesToBackupElem.AppendChild(fileElem);
                }

                foreach (RestorePoint restorePoint in backupJob.RestorePoints)
                {
                    XmlElement restorePointElem = backupJobsInfo.CreateElement("restorePoint");

                    XmlAttribute restorePointNameAttr = backupJobsInfo.CreateAttribute("name");
                    XmlText restorePointName = backupJobsInfo.CreateTextNode(restorePoint.Name);
                    restorePointNameAttr.AppendChild(restorePointName);

                    restorePointElem.Attributes.Append(restorePointNameAttr);

                    foreach (string copiedFile in restorePoint.CopiesInfo)
                    {
                        XmlElement fileElem = backupJobsInfo.CreateElement("file");
                        XmlNode fileNode = backupJobsInfo.CreateTextNode(copiedFile);

                        fileElem.AppendChild(fileNode);
                        restorePointElem.AppendChild(fileElem);
                    }

                    restorePoints.AppendChild(restorePointElem);
                    jobElem.AppendChild(restorePoints);
                }

                jobElem.Attributes.Append(jobNameAttr);
                xRoot.AppendChild(jobElem);
            });
            backupJobsInfo.Save("JobsInfo");
        }

        public List<BackupJob> DownloadInfo()
        {
            var backupJobs = new List<BackupJob>();

            var backupsInfo = new XmlDocument();
            backupsInfo.Load("JobsInfo");
            XmlElement rootElem = backupsInfo.DocumentElement;
            foreach (XmlNode xnode in rootElem)
            {
                var filesToBackup = new List<FileInfo>();
                var restorePoints = new List<RestorePoint>();

                // Default value if algorithm won't be found
                IStoringAlgorithm algo = new SingleStoring();

                XmlNode jobNameAttr = xnode.Attributes.GetNamedItem("name");

                foreach (XmlNode subNode in xnode.ChildNodes)
                {
                    if (subNode.Name == "filesToBackup")
                    {
                        foreach (XmlNode file in subNode.ChildNodes)
                        {
                            string fullPath = file.InnerText;

                            string name = fullPath.Split("/").Last();
                            int startIndexToDelete = fullPath.Length - name.Length - 1;
                            string path = fullPath.Remove(startIndexToDelete, name.Length + 1);
                            filesToBackup.Add(new FileInfo(path, name));
                        }
                    }

                    if (subNode.Name == "storingAlgorithm")
                    {
                        algo = GetAlgorithm(subNode.InnerText);
                    }

                    if (subNode.Name == "restorePoints")
                    {
                        foreach (XmlNode restorePointNode in subNode.ChildNodes)
                        {
                            XmlNode restorePointNameAttr = restorePointNode.Attributes.GetNamedItem("name");
                            var copiesInfo = (from XmlNode file in restorePointNode.ChildNodes select file.InnerText).ToList();

                            var creationTime = DateTime.ParseExact(restorePointNameAttr.Value, "HH:mm:ss", null);
                            var newRestorePoint = new RestorePoint(creationTime, restorePointNameAttr.Value);
                            newRestorePoint.AddBackupedFiles(copiesInfo);

                            restorePoints.Add(newRestorePoint);
                        }
                    }
                }

                var backupJob = new BackupJob(algo, jobNameAttr.Value);
                filesToBackup.ForEach(file =>
                {
                    backupJob.AddFile(file);
                });
                backupJob.AddRestorePoints(restorePoints);

                backupJobs.Add(backupJob);
            }

            return backupJobs;
        }

        private string GetAlgoType(IStoringAlgorithm algo)
        {
            return algo switch
            {
                SingleStoring _ => "SingleStoring",
                SplitStoring _ => "SplitStoring",
                _ => null
            };
        }

        private IStoringAlgorithm GetAlgorithm(string algoName)
        {
            return algoName switch
            {
                "SingleStoring" => new SingleStoring(),
                "SplitStoring" => new SplitStoring(),
                _ => null
            };
        }
    }
}