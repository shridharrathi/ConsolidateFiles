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

namespace ConsolidateFiles
{
    public partial class Form1 : Form
    {
        private string _sourceFolder;
        private string _destinationFolder;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                label1.Text = folderBrowserDialog1.SelectedPath;
                _sourceFolder = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog2.ShowDialog() == DialogResult.OK)
            {
                label2.Text = folderBrowserDialog2.SelectedPath;
                _destinationFolder = folderBrowserDialog2.SelectedPath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(@"C:\temp"))
            {
                Directory.CreateDirectory(@"C:\temp");
            }
            var listAllFiles = Directory.GetFiles(_sourceFolder, "*.*", SearchOption.AllDirectories);
            var count = listAllFiles.Count();
            var errorCount = 0;

            foreach (var file in listAllFiles)
            {
                FileInfo info = new FileInfo(file);
                if (info.Extension.Equals(".ini"))
                {
                    count -= 1;
                    continue;
                }
                var destinationFile = $"{_destinationFolder}\\{info.Name}";

                if (destinationFile.Equals(file))
                {
                    count -= 1;
                    continue;
                }

                if (!File.Exists(destinationFile))
                {
                    File.Move(file, destinationFile);
                }
                else
                {
                    using (StreamWriter logFile = new StreamWriter(@"C:\temp\FilesConsolidationFailLog.txt", true))
                    {
                        logFile.WriteLine($"{DateTime.Now.ToString("dd-MMM-yyyy")},{file},{destinationFile}");
                    }
                    errorCount++;
                }
            }
            if (errorCount > 0)
            {
                if (errorCount == count)
                {
                    MessageBox.Show("Failed to move any files. The log file destination is C:\\temp\\FilesConsolidationFailLog.txt");
                }
                else
                {
                    MessageBox.Show($"Successfully moved {count - errorCount}. Failed to move {errorCount}. The log file destination is C:\\temp\\FilesConsolidationFailLog.txt");
                }
            }
            else
            {
                MessageBox.Show($"Successfully moved all the files. File Count : {count}");
            }


        }
    }
}
