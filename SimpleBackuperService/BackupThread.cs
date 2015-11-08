using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
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
            if (System.Environment.Is64BitOperatingSystem)
            {
                ExtractResource("HoboCopy64", "HoboCopy.exe");
                ExtractResource("HoboCopy641", "HoboCopy.pdb");
            }
            else
            {
                ExtractResource("HoboCopy", "HoboCopy.exe");
                ExtractResource("HoboCopy1", "HoboCopy.pdb");
            }
            foreach (String sourceDir in paths)
            {
                var process = System.Diagnostics.Process.Start("HoboCopy.exe /statefile=state /incremental /recursive "+ sourceDir +" "+targetDir);
                process.WaitForExit();
            }
            m_CopyIsRun = false;
        }

       private void ExtractResource(string resName, string fName)
        {
      object ob = SimpleBackuperService.Properties.Resources.ResourceManager.GetObject(resName);
      byte[] myResBytes = (byte[])ob;
      using (FileStream fsDst = new FileStream(fName, FileMode.CreateNew, FileAccess.Write))
      {
         byte[] bytes = myResBytes;
         fsDst.Write(bytes, 0, bytes.Length);
         fsDst.Close();
         fsDst.Dispose();
      }
}
    }
}
