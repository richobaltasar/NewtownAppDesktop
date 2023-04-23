using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ewats_App.Function;
using Ewats_App.Model;
using System.Diagnostics;

namespace Ewats_App
{
    public partial class Login : Form
    {
        GlobalFunc f = new GlobalFunc();
        ReadFromFile r = new ReadFromFile();
        public Login()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            
        }
        private void Login_Load(object sender, EventArgs e)
        {
            if (Model.ConfigurationFileStatic.VFDPort != null && Model.ConfigurationFileStatic.VFDPort !="")
            {
                VFDPort.send("Login Kasir", "Please wait ....", Model.ConfigurationFileStatic.VFDPort);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            f.ShowOnScreenKeyboard();
        }

        private void txtPassword_MouseClick(object sender, MouseEventArgs e)
        {
            f.ShowOnScreenKeyboard();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            LoginProc();
            f.HideOnScreenKeyboard();
        }

        private void LoginProc()
        {
            if (txtUsername.Text != "" && txtPassword.Text != "")
            {
                var data = f.LoginProc(txtUsername.Text, txtPassword.Text);
                if (data.ID != null)
                {
                    if (data.HakAkses == "Kasir")
                    {
                        General.IDUser = data.ID;
                        var close = f.CheckClosingCashier(f.GetNamaUser(General.IDUser));
                        if (close.Success == false)
                        {
                            f.PageControl("Main");
                            this.Hide();
                        }
                        else
                        {
                            var msgbox = MessageBox.Show(close.Message + ", Silahkan melakukan Approval oleh bagian Keuangan",
                                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        lblAlert.Text = "Anda tidak diizinkan mengakses area Kasir";
                        lblAlert.Visible = true;
                    }
                }
                else
                {
                    lblAlert.Text = "Username / Password tidak valid";
                    lblAlert.Visible = true;
                }
            }
            else
            {
                lblAlert.Text = "Silahkan isi field yang masih kosong";
                lblAlert.Visible = true;
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (Model.ConfigurationFileStatic.VFDPort != null && Model.ConfigurationFileStatic.VFDPort != "")
            {
                VFDPort.send("", "", VFDPort.sp.PortName);
            }
                
            if (VFDPort.sp.IsOpen)
            {
                VFDPort.sp.Close();
                VFDPort.sp.Dispose();
                VFDPort.sp = null;
            }
            Application.Exit();
        }

        private void LblNamaCompany_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)

            {
                f.HideOnScreenKeyboard();
                LoginProc();
            }
        }

        private void txtUsername_MouseClick(object sender, MouseEventArgs e)
        {
            f.ShowOnScreenKeyboard();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Hide();
            f.PageControl("EwatsConfig");
        }

        private void Login_Activated(object sender, EventArgs e)
        {
            if (Model.ConfigurationFileStatic.VFDPort != null && Model.ConfigurationFileStatic.VFDPort !="")
            {
                VFDPort.send("Login Kasir", "Please wait ....", Model.ConfigurationFileStatic.VFDPort);
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            panel2.Location = new Point(
    this.ClientSize.Width / 2 - panel2.Size.Width / 2,
    this.ClientSize.Height / 2 - panel2.Size.Height / 2);
            panel2.Anchor = AnchorStyles.None;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            f.PageControl("EwatsConfig");
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (Model.ConfigurationFileStatic.VFDPort != null && Model.ConfigurationFileStatic.VFDPort != "")
            {
                VFDPort.send("", "", VFDPort.sp.PortName);
            }

            if (VFDPort.sp.IsOpen)
            {
                VFDPort.sp.Close();
                VFDPort.sp.Dispose();
                VFDPort.sp = null;
            }
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoginProc();
            f.HideOnScreenKeyboard();
        }

        private void lblAlert_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtUsername_MouseClick_1(object sender, MouseEventArgs e)
        {
            f.ShowOnScreenKeyboard();
        }

        private void txtUsername_Click(object sender, EventArgs e)
        {
            f.ShowOnScreenKeyboard();
        }

        private void txtUsername_DoubleClick(object sender, EventArgs e)
        {
            f.ShowOnScreenKeyboard();
        }

        private void txtPassword_Click(object sender, EventArgs e)
        {
            f.ShowOnScreenKeyboard();
        }

        private void txtPassword_DoubleClick(object sender, EventArgs e)
        {
            f.ShowOnScreenKeyboard();
        }

        private void txtPassword_MouseClick_1(object sender, MouseEventArgs e)
        {
            f.ShowOnScreenKeyboard();
        }
    }
}
