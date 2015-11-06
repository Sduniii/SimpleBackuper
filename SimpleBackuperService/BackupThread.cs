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
                Directory.CreateDirectory(targetDir);

                foreach (var file in Directory.GetFiles(sourceDir))
                    try {
                        File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)), true);
                    }catch(Exception exx)
                    {
                        LogHandler.writeLog(exx.Message);
                    }

                RunCopy(Directory.GetDirectories(sourceDir).ToList(), sourceDir);
            }
            m_CopyIsRun = false;
            }
        }


}
