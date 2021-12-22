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

namespace Ewats_App.Page
{
    public partial class ChangePassword : Form
    {
        Function.GlobalFunc f = new GlobalFunc();
        public ChangePassword()
        {
            InitializeComponent();
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Text == "Show")
            {
                if (checkBox1.Checked == true)
                {
                    checkBox1.Text = "Hide";
                    txtCurrentPass.PasswordChar = (char)0;
                    txtConfirmPass.PasswordChar = (char)0;
                    txtNewPass.PasswordChar = (char)0;
                }
            }
            else if (checkBox1.Text == "Hide")
            {
                if (checkBox1.Checked == false)
                {
                    checkBox1.Text = "Show";
                    txtCurrentPass.PasswordChar = '*';
                    txtConfirmPass.PasswordChar = '*';
                    txtNewPass.PasswordChar = '*';
                }
            }
        }

        private void ChangePassword_Load(object sender, EventArgs e)
        {
            txtConfirmPass.PasswordChar = '*';
            txtCurrentPass.PasswordChar = '*';
            txtNewPass.PasswordChar = '*';
            //this.TopMost = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (txtCurrentPass.Text != "" && txtConfirmPass.Text != "" && txtNewPass.Text != "")
            {
                if (f.CheckPasswordCurrentValid(txtCurrentPass.Text, Model.General.IDUser) == true)
                {
                    if (txtConfirmPass.Text == txtNewPass.Text)
                    {
                        if (txtNewPass.Text.Length > 5)
                        {
                            var data = f.SaveUpdateChangePassword(Model.General.IDUser, txtNewPass.Text);
                            if (data.Success == true)
                            {
                                var res = f.ShowMessagebox("Password telah berhasil dirubah", "Warning", MessageBoxButtons.OK);
                                if (res == DialogResult.OK)
                                {
                                    this.Close();
                                    f.PageControl("Main");
                                }
                            }
                            else
                            {
                                var res = f.ShowMessagebox("Password gagal dirubah", "Warning", MessageBoxButtons.OK);
                            }
                        }
                        else
                        {
                            f.ShowMessagebox("New Password tidak valid, min 5 Character", "Warning", MessageBoxButtons.OK);
                            txtNewPass.Focus();
                        }
                    }
                    else
                    {
                        f.ShowMessagebox("New Password not match", "Warning", MessageBoxButtons.OK);
                        txtConfirmPass.Focus();
                    }
                }
                else
                {
                    f.ShowMessagebox("Password yang diinput tidak valid", "Warning", MessageBoxButtons.OK);
                    txtCurrentPass.Focus();
                }
            }
            else {
                f.ShowMessagebox("silahkan input semua field", "Warning", MessageBoxButtons.OK);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
