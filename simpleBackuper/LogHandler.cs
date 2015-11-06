using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simpleBackuper
{
    public static class LogHandler
    {
        public static void writeLog(string txt)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "log.txt";

            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(txt);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(txt);
                }
            }
        }

    }
}
