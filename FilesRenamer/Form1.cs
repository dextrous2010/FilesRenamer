using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace FilesRenamer
{
    public partial class Form1 : Form
    {

        static String dirPath, fileType;
        static DirectoryInfo dirInfo;
        FileInfo[] files;
        String newFilePath;

        

        public Form1()
        {
            InitializeComponent();
        }


        public void DisplayFolderFiles()
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();

            foreach (FileInfo fileName in files)
            {
                listBox1.Items.Add(fileName);
            }
        }

        public void DisplayFolderFiles(String fileType)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();

            foreach (FileInfo fileName in files)
            {
                String fileExtension = Path.GetExtension(fileName.Name);

                if (fileExtension == fileType)
                {
                    listBox1.Items.Add(fileName);
                }
            }
        }


        public void PopulateCBWithFilesExetensions()
        {
            comboBox1.Items.Clear();

            foreach (FileInfo fileName in files)
            {
                String fileExtension = Path.GetExtension(fileName.Name);
                if (!comboBox1.Items.Contains(fileExtension))
                {
                    comboBox1.Items.Add(fileExtension);
                }
            }
        }


        public void RenameFiles() 
        {
            Random random = new Random();

            string folderName = "Renamed";
            Directory.CreateDirectory(dirPath + @"\" + folderName);

            progressBar1.Minimum = 1;
            progressBar1.Maximum = files.Length;
            progressBar1.Step = 1;
            progressBar1.Value = 1;

            foreach (FileInfo fileName in files)
            {

                if (Path.GetExtension(fileName.Name) == (String)comboBox1.SelectedItem || (String)comboBox1.SelectedItem == null)
                {
                    String oldFileName = dirPath + @"\" + fileName.Name;
                    String newFileName = random.Next(1, 199) + "_" + fileName.Name;
                    newFilePath = dirPath + @"\" + folderName;
                    String newFilePathAndName = dirPath + @"\" + folderName + @"\" + newFileName;

                    File.Copy(oldFileName, newFilePathAndName, true);

                    listBox2.Items.Add(newFileName);
                }

                progressBar1.PerformStep();
            }


            string message = "Файли успішно перейменовано! Натисніть ОК, щоб відкрити папку з ними.";
            string caption = "Виконано!";
            MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
            DialogResult result;

            result = MessageBox.Show(message, caption, buttons);

            if (result == System.Windows.Forms.DialogResult.OK)
            {

                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                string _path = newFilePath;
                startInfo.Arguments = string.Format("/C start {0}", _path);
                process.StartInfo = startInfo;
                process.Start();

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();

            if(fbd.SelectedPath != "") 
            {
                textBox1.Text = fbd.SelectedPath;

                dirPath = fbd.SelectedPath;
                dirInfo = new DirectoryInfo(dirPath);
                files = dirInfo.GetFiles();

                PopulateCBWithFilesExetensions();

                if (comboBox1.SelectedItem != null)
                {
                    DisplayFolderFiles((String)comboBox1.SelectedItem);
                }
                else
                {
                    DisplayFolderFiles();
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedItem != "") 
            {
                DisplayFolderFiles((String)comboBox1.SelectedItem);
            }

        }

        private void buttonRename_Click(object sender, EventArgs e)
        {
            if (dirPath != null)
            {
                RenameFiles();
            }
            else 
            {
                MessageBox.Show("Оберіть шлях до папки!");
            }
            
        }
    }

}
