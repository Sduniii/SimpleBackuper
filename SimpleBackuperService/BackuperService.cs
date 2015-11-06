using simpleBackuper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleBackuperService
{
    public partial class BackuperService : ServiceBase
    {
        private BackupThread bT;
        public BackuperService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try {
                bT = new BackupThread();
                Thread t = new Thread(new ThreadStart(bT.StartBackup));
                t.Start();
            }catch(Exception e)
            {
                LogHandler.writeLog(e.Message);
            }
        }

        protected override void OnStop()
        {
            bT.StopBackup();
        }

        internal void TestStartupAndStop(string[] args)
        {
            this.OnStart(args);
            Console.ReadLine();
            this.OnStop();
        }
    }
}
