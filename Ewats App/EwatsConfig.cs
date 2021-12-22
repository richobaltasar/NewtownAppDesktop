using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ewats_App.Model;
using Ewats_App.Function;

namespace Ewats_App
{
    public partial class EwatsConfig : Form
    {
        GlobalFunc f = new GlobalFunc();
        ReadFromFile r = new ReadFromFile();

        public EwatsConfig()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (CheckConfig() == true)
            {
                var data = new ConfigurationFile();
                data.ConnStrLog = General.ConnStringLog;
                data.IpServer = General.IpLocalServer;
                data.DBServer = General.DBServer;
                data.UsernameServer = General.UsernameServer;
                data.PasswordServer = General.PasswordServer;
                data.PathImgWeb = General.PathImgWeb;
                data.VFDPort = General.VFDPort;
                if (r.CheckFileConfig() == true)
                {
                    if (r.CreateFileConfig(data) == true)
                    {
                        this.Hide();
                        f.PageControl("InitPage");
                    }
                    else
                    {
                        MessageBox.Show("Saving Configuration file failed", "Warning");
                    }
                }
                else
                {
                    if (r.CreateFileConfig(data) == true)
                    {
                        this.Hide();
                        f.PageControl("InitPage");
                    }
                    else
                    {
                        MessageBox.Show("Saving Configuration file failed", "Warning");
                    }
                }
            }
            else
            {
                MessageBox.Show("Configuration failed", "Warning");
            }
        }

        public bool CheckConfig()
        {
            bool res = false;
            if (txtIpServer.Text != "" && TxtDBName.Text != ""
                && txtUsername.Text != "" && txtPassword.Text != "" && txtWebServer.Text != "")
            {
                string conn = f.CheckDBLocal(txtIpServer.Text.Trim(), "master", txtUsername.Text.Trim(), txtPassword.Text.Trim());
                General.ConnStringLog = "";
                if (conn.Contains("error") == false)
                {
                    if (f.CheckDbAlreadyExists(conn, TxtDBName.Text.Trim()) == true)
                    {
                        string checkDb = f.CheckDBLocal(txtIpServer.Text.Trim(), TxtDBName.Text.Trim(), txtUsername.Text.Trim(), txtPassword.Text.Trim());
                        if (checkDb.Contains("error") == false)
                        {
                            string constr = "Data Source = " + txtIpServer.Text.Trim() + "; " +
                            "Initial Catalog = " + TxtDBName.Text.Trim() + "; " +
                            "User ID = " + txtUsername.Text.Trim() + "; " +
                            "Password = " + txtPassword.Text.Trim() + "";
                            General.ConnStringLog = constr;
                            General.IpLocalServer = txtIpServer.Text.Trim();
                            General.DBServer = TxtDBName.Text.Trim();
                            General.UsernameServer = txtUsername.Text.Trim();
                            General.PasswordServer = txtPassword.Text.Trim();
                            General.PathImgWeb = txtWebServer.Text.Trim();
                            General.VFDPort = TxtVFDPort.Text.Trim();
                            res = true;
                        }
                    }
                    else
                    {
                        if (f.CreateDB(conn, TxtDBName.Text.Trim()) == true)
                        {
                            string constr = "Data Source = " + txtIpServer.Text.Trim() + "; " +
                            "Initial Catalog = " + TxtDBName.Text.Trim() + "; " +
                            "User ID = " + txtUsername.Text.Trim() + "; " +
                            "Password = " + txtPassword.Text.Trim() + "";
                            General.ConnStringLog = constr;

                            General.IpLocalServer = txtIpServer.Text.Trim();
                            General.DBServer = TxtDBName.Text.Trim();
                            General.UsernameServer = txtUsername.Text.Trim();
                            General.PasswordServer = txtPassword.Text.Trim();
                            General.PathImgWeb = txtWebServer.Text.Trim();
                            General.VFDPort = TxtVFDPort.Text.Trim();
                            res = true;
                        }
                        else
                        {
                            General.ConnStringLog = "";
                        }
                    }
                }
            }
            return res;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            f.PageControl("InitPage");
        }

        private void EwatsConfig_Load(object sender, EventArgs e)
        {
            ReadConfig();
        }

        public bool ReadConfig()
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
                )
                {
                    txtIpServer.Text = Encrypt.DecryptString(data.IpServer, "BISMILLAH");
                    TxtDBName.Text = Encrypt.DecryptString(data.DBServer, "BISMILLAH");
                    txtUsername.Text = Encrypt.DecryptString(data.UsernameServer, "BISMILLAH");
                    txtPassword.Text = Encrypt.DecryptString(data.PasswordServer, "BISMILLAH");
                    txtWebServer.Text = Encrypt.DecryptString(data.PathImgWeb, "BISMILLAH");
                    if (data.VFDPort != null)
                    {
                        TxtVFDPort.Text = Encrypt.DecryptString(data.VFDPort, "BISMILLAH");
                    }
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

                //this.Hide();
                //this.Opacity = 0.0f;
                //this.ShowInTaskbar = false;
                //f.PageControl("EwatsConfig");
            }
            return res;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            f.PageControl("VFDConfig");
        }

        private void TxtDBName_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtIpServer_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void txtWebServer_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void LblNamaCompany_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
