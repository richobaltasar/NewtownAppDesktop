using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using DataUploader.Module;
using DataUploader.Model;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Configuration;


namespace DataUploader
{
    public partial class Uploader : Form
    {
        static HttpClient client = new HttpClient();
        GlobalFunc f = new GlobalFunc();
        ReadFromFile r = new ReadFromFile();
        string Url = "";

        int count = 10;

        public Uploader()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void Uploader_Load(object sender, EventArgs e)
        {
            check_running();
            lblCount.Text = count.ToString();
            btnStart.Visible = false;
            lblStart.Visible = false;
            btnStop.Visible = true;
            lblstop.Visible = true;

            if (CheckConfig() == true)
            {
                ConfigurationFileStatic.ConnStrLogCloud = f.GetConnStringCloudDB();
                if (ConfigurationFileStatic.ConnStrLogCloud != "")
                {
                    var data = f.GetDataYangMauDiUpload();
                    if (data != null)
                    {
                        var dataID = f.FSaveUploadData(data);
                        f.FUpdateIDUploaded(dataID);
                    }
                    lblCount.Text = count.ToString();
                    timer1.Start();
                }
                else
                {
                    MessageBox.Show("ConnStrLogCloud belom disetting");
                }
            }
            else
            {
                this.Hide();
                this.Opacity = 0.0f;
                this.ShowInTaskbar = false;
                f.PageControl("FormSetting");
            }
        }

        public bool CheckConfig()
        {
            bool res = false;
            if (r.CheckFileConfig() == true)
            {
                var data = new ConfigurationFile();
                data = r.ReadFileConfig();
                if (data.ConnStrLog != null && data.ConnStrLog != ""
                        && data.IpServer != null && data.IpServer != ""
                        && data.DBServer != null && data.DBServer != ""
                        && data.UsernameServer != null && data.UsernameServer != ""
                        && data.PasswordServer != null && data.PasswordServer != ""
                        && data.PathImgWeb != null && data.PathImgWeb != ""
                )
                {

                    ConfigurationFileStatic.ConnStrLog = Encrypt.DecryptString(data.ConnStrLog, "BISMILLAH");
                    ConfigurationFileStatic.IpServer = Encrypt.DecryptString(data.IpServer, "BISMILLAH");
                    ConfigurationFileStatic.DBServer = Encrypt.DecryptString(data.DBServer, "BISMILLAH");
                    ConfigurationFileStatic.UsernameServer = Encrypt.DecryptString(data.UsernameServer, "BISMILLAH");
                    ConfigurationFileStatic.PasswordServer = Encrypt.DecryptString(data.PasswordServer, "BISMILLAH");
                    ConfigurationFileStatic.PathImgWeb = Encrypt.DecryptString(data.PathImgWeb, "BISMILLAH");
                    Url = ConfigurationFileStatic.PathImgWeb;
                    res = true;
                }
                else
                {
                    timer1.Stop();
                    DialogResult dialogResult = MessageBox.Show("Please, input Configuration file, click Yes, if you want to input", "Important Question", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        this.Hide();
                        this.Opacity = 0.0f;
                        this.ShowInTaskbar = false;
                        timer1.Stop();
                    }
                    else
                    {
                        Application.Exit();
                    }
                }

            }
            else
            {
                this.Hide();
                this.Opacity = 0.0f;
                this.ShowInTaskbar = false;
                f.PageControl("EwatsConfig");
            }
            return res;
        }
        public void check_running()
        {
            try
            {
                var processExists = Process.GetProcesses().Any(p => p.ProcessName.Contains("DataUploader"));

                if (processExists == true)
                {
                    var data = Process.GetProcessesByName("DataUploader");
                    int count = 0;
                    if (data.Count() > 1)
                    {
                        foreach (Process proc in Process.GetProcessesByName("DataUploader"))
                        {
                            if (count < data.Count() - 1)
                            {
                                proc.Kill();
                                count++;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }
        private async void LogActivityKiosk(string URI, UploadData data)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    using (var response = await client.PostAsJsonAsync(URI, data))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var productJsonString = await response.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<ResponseUploadData>(productJsonString);
                            f.FUpdateIDUploaded(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error :" + ex.Message);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            count--;
            lblCount.Text = count.ToString();
            if (count == 0)
            {
                timer1.Stop();
                
                var data = f.GetDataYangMauDiUpload();
                if (data != null)
                {
                    var dataID = f.FSaveUploadData(data);
                    f.FUpdateIDUploaded(dataID);
                }
                
                timer1.Start();
                count = 60;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            timer1.Start();
            btnStart.Visible = false;
            lblStart.Visible = false;
            btnStop.Visible = true;
            lblstop.Visible = true;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            btnStart.Visible = true;
            lblStart.Visible = true;
            btnStop.Visible = false;
            lblstop.Visible = false;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Opacity = 0.0f;
            this.ShowInTaskbar = false;
            f.PageControl("FormSetting");
        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
