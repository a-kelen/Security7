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

namespace Security7
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            fileDialog.Filter = "Text Files|*.txt;*.doc;*.docx";

            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("filename", "Filename");
            dataGridView1.Columns.Add("filetype", "Type");
            dataGridView1.Columns[1].Width = 40;
            dataGridView1.Columns.Add("filesize", "Filesize");
            dataGridView1.Columns[2].Width = 50;
            dataGridView1.Columns.Add("result", "Result");

            cryptor = new Cryptor();
            clearFiles = new List<string>();
            encryptedFiles = new List<string>();
            allFiles = new List<FileInfo>();

        }
        Cryptor cryptor;
        public string directory = "";
        List<string> clearFiles;
        List<string> encryptedFiles;
        List<FileInfo> allFiles;

        private void openFile_Click(object sender, EventArgs e)
        {
            clearFiles.Clear();
            encryptedFiles.Clear();
            allFiles.Clear();
            dataGridView1.Rows.Clear();
            
            if (fileDialog.ShowDialog() == DialogResult.Cancel)
                return;
            Stream myStream;
            TextReader.directory = new FileInfo(fileDialog.FileNames[0]).DirectoryName;
            foreach (String file in fileDialog.FileNames)
            {
                var info = new FileInfo(file);
                allFiles.Add(info);
                if (info.Name.Contains("_encrypted"))
                    encryptedFiles.Add(file);
                else
                    clearFiles.Add(file);
                try
                {
                    dataGridView1.Rows.Add(
                        Path.GetFileNameWithoutExtension(info.Name),
                        info.Extension,
                        ConvertBytes(info.Length)
                        );
                }
                catch (Exception ex)
                {
                }
            }
        }
        static string ConvertBytes(long bytes)
        {
            if(bytes < 1024)
                return bytes.ToString() + " B";
            if (bytes < 1048576)
                return (bytes / 1024).ToString() + " KB";

            return ((bytes / 1024) / 1024).ToString() + " MB";
        }

        private void encrypt_Click(object sender, EventArgs e)
        {
            TaskCreationOptions atp = TaskCreationOptions.AttachedToParent;
            

            Task.Factory.StartNew(() =>
            {
                foreach (String file in clearFiles)
                {
                    Task.Factory.StartNew(() => { 
                        cryptor.Encrypt(file);
                        var index = allFiles.FindIndex(x => x.FullName == file);
                        dataGridView1[3, index].Value = "OK";
                    }, atp);
                }
            }).ContinueWith(cont => { 
                
            });

        }

        private void save_Click(object sender, EventArgs e)
        {
            var res = cryptor.getResult();
            foreach(var r in res)
            {
                var index = allFiles.FindIndex(x => Path.GetFileNameWithoutExtension(x.Name) == r.Name);

                var endFile = TextReader.WriteToFile(r);
                dataGridView1[3, index].Value = $"Saved as '{endFile}'";
            }
        }

        private void decrypt_Click(object sender, EventArgs e)
        {
            TaskCreationOptions atp = TaskCreationOptions.AttachedToParent;
            Task.Factory.StartNew(() =>
            {
                foreach (String file in encryptedFiles)
                {

                    Task.Factory.StartNew(() => { 
                        cryptor.Decrypt(file);
                        var index = allFiles.FindIndex(x => x.FullName == file);
                        dataGridView1[3, index].Value = "OK";
                    }, atp);
                }
            }).ContinueWith(cont => { });
        }
    }
}
