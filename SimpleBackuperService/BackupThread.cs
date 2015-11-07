using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace simpleBackuper
{
    class BackupThread
    {
        private bool m_StopBackup, m_CopyIsRun;
        string path = AppDomain.CurrentDomain.BaseDirectory + "paths.ts";
        string pathTarget = AppDomain.CurrentDomain.BaseDirectory + "target.ts";

        public void StopBackup()
        {
            m_StopBackup = true;
            m_CopyIsRun = false;
        }

        public void StartBackup()
        {
            m_StopBackup = false;

            while (!m_StopBackup)
            {
                try
                {
                    List<String> s = new List<String>();
                    using (StreamReader sr = File.OpenText(path))
                    {
                        string st = "";
                        while ((st = sr.ReadLine()) != null)
                        {
                            s.Add(st);
                        }                       
                    }
                    String target = File.ReadAllText(pathTarget);
                    Thread dateSender = new Thread(delegate () { RunCopy(s,target); });
                    dateSender.Start();
                    while (m_CopyIsRun) { Thread.Sleep(30000); }
                }
                catch (Exception ex)
                {
                    LogHandler.writeLog(ex.Message);
                    //7 Tage
                    Thread.Sleep(604800000);
                }
                 
            }
        }

        public void RunCopy(List<String> paths, String targetDir)
        {
            m_CopyIsRun = true;
            foreach (String sourceDir in paths)
            {
                try {
                    DirectoryCopy(sourceDir, targetDir, true);
                }
                catch(Exception e)
                {
                    LogHandler.writeLog(e.StackTrace);
                }
            }
            m_CopyIsRun = false;
        }

        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}
