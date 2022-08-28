using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace UK_File_Mod_Manager
{
    public partial class Form1 : Form
    {
        public static void ReplaceFile(string modFile, string vanillaFile)
        {
            FileInfo fInfo = new FileInfo(modFile);
            if (File.Exists(vanillaFile))
            {
                File.Delete(vanillaFile);
            }
            fInfo.CopyTo(vanillaFile);
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if(File.Exists(System.Reflection.Assembly.GetEntryAssembly().Location.Replace(AppDomain.CurrentDomain.FriendlyName, "config.json")))
            {
                using (StreamReader r = new StreamReader(System.Reflection.Assembly.GetEntryAssembly().Location.Replace(AppDomain.CurrentDomain.FriendlyName, "config.json")))
                {
                    string json = r.ReadToEnd();
                    dynamic jsonData = JObject.Parse(json);
                    bunifuTextBox1.Text = jsonData.DP;
                    bunifuTextBox2.Text = jsonData.MP;
                }
            }
            
            if (bunifuTextBox2.Text != "") 
            {
                foreach (string folder in Directory.GetDirectories(bunifuTextBox2.Text))
                {
                    listBox1.Items.Add(folder.Split('\\')[folder.Split('\\').Length-1]);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.FromArgb(44, 47, 51), ButtonBorderStyle.Solid);
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string folder = dlg.SelectedPath;
                bunifuTextBox1.Text = folder;

                JObject data = new JObject(
                    new JProperty("DP", bunifuTextBox1.Text),
                    new JProperty("MP", bunifuTextBox2.Text));

                File.WriteAllText(System.Reflection.Assembly.GetEntryAssembly().Location.Replace(AppDomain.CurrentDomain.FriendlyName, "config.json"), data.ToString());

                // write JSON directly to a file
                using (StreamWriter file = File.CreateText(System.Reflection.Assembly.GetEntryAssembly().Location.Replace(AppDomain.CurrentDomain.FriendlyName, "config.json")))
                using (JsonTextWriter writer = new JsonTextWriter(file))
                {
                    data.WriteTo(writer);
                }
            }
            else
            {

            }
        }

        private void bunifuButton4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string folder = dlg.SelectedPath;
                bunifuTextBox2.Text = folder;

                JObject data = new JObject(
                    new JProperty("DP", bunifuTextBox1.Text),
                    new JProperty("MP", bunifuTextBox2.Text));

                File.WriteAllText(System.Reflection.Assembly.GetEntryAssembly().Location.Replace(AppDomain.CurrentDomain.FriendlyName, "config.json"), data.ToString());

                // write JSON directly to a file
                using (StreamWriter file = File.CreateText(System.Reflection.Assembly.GetEntryAssembly().Location.Replace(AppDomain.CurrentDomain.FriendlyName, "config.json")))
                using (JsonTextWriter writer = new JsonTextWriter(file))
                {
                    data.WriteTo(writer);
                }
            }
            else
            {

            }
        }

        private void bunifuButton3_Click(object sender, EventArgs e)
        {
            if (bunifuTextBox2.Text != "")
            {
                foreach (string folder in Directory.GetDirectories(bunifuTextBox2.Text))
                {
                    listBox1.Items.Add(folder.Split('\\')[folder.Split('\\').Length - 1]);
                }
            }
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex != -1)
            {
                foreach (string folder in Directory.GetDirectories(bunifuTextBox2.Text))
                {
                    if (folder.Contains(listBox1.SelectedItem.ToString()))
                    {
                        if (Directory.GetFiles(folder).Length > 0)
                        {
                            foreach (string file in Directory.GetFiles(folder))
                            {
                                ReplaceFile(file, bunifuTextBox1.Text + $"\\{file.Split('\\')[file.Split('\\').Length - 1]}");
                            }
                        }

                        if (Directory.GetDirectories(folder).Length > 0)
                        {
                            foreach (string subFolder in Directory.GetDirectories(folder))
                            {
                                if (Directory.GetFiles(subFolder).Length > 0)
                                {
                                    foreach (string subFile in Directory.GetFiles(subFolder))
                                    {
                                        ReplaceFile(subFile, bunifuTextBox1.Text + $"\\{subFile.Split('\\')[subFile.Split('\\').Length - 1]}");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void bunifuButton5_Click(object sender, EventArgs e)
        {
            Process p = new Process();
            p.StartInfo.WorkingDirectory = bunifuTextBox1.Text.Replace("ULTRAKILL_Data", "");
            p.StartInfo.FileName = "ULTRAKILL.exe";
            p.Start();
        }
    }
}
