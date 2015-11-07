using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace simpleBackuper
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        string path = AppDomain.CurrentDomain.BaseDirectory + "paths.ts";
        String patht = AppDomain.CurrentDomain.BaseDirectory + "target.ts";
        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (ServiceInstaller.ServiceIsInstalled("SimpleBackuperService"))
                {
                    button2.Enabled = false;
                    button3.Enabled = true;
                }
                else
                {
                    button3.Enabled = false;
                }
                
                using (StreamReader sr = File.OpenText(path))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        listBox1.Items.Add(s);
                    }
                }
                using (StreamReader sr = File.OpenText(patht))
                {
                    textBox1.Text = sr.ReadLine();
                }
            } catch (Exception ex)
            {
                MessageBox.Show("Noch keine Daten vorhanden.\nBitte Ordner hinzufügen!", "Fehler",MessageBoxButtons.OK,MessageBoxIcon.Information);
                LogHandler.writeLog(ex.Message);
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(folderBrowserDialog1.ShowDialog() == DialogResult.OK && Directory.Exists(folderBrowserDialog1.SelectedPath))
            {
                listBox1.Items.Add(folderBrowserDialog1.SelectedPath);
                if (!File.Exists(path))
                {
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine(folderBrowserDialog1.SelectedPath);
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine(folderBrowserDialog1.SelectedPath);
                    }
                }
                if (!ServiceInstaller.ServiceIsInstalled("SimpleBackuperService"))
                {
                    button2.Enabled = true;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button3.Enabled = true;
            ServiceInstaller.InstallAndStart("SimpleBackuperService", "SimpleBackuper", AppDomain.CurrentDomain.BaseDirectory + "SimpleBackuperService.exe");
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ServiceInstaller.Uninstall("SimpleBackuperService");
            button3.Enabled = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(folderBrowserDialog1.ShowDialog() == DialogResult.OK && Directory.Exists(folderBrowserDialog1.SelectedPath))
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;

                File.WriteAllText(patht,folderBrowserDialog1.SelectedPath);
            }
        }
    }
}
