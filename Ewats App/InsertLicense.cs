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

namespace Ewats_App
{
    public partial class InsertLicense : Form
    {
        GlobalFunc f = new GlobalFunc();
        ReadFromFile r = new ReadFromFile();

        public InsertLicense()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string checkValid = Encrypt.DecryptString(txtKey.Text, "BISMILLAH");
            if (checkValid.Length == 16)
            {
                if (checkValid.Contains("TENTAKEL") == true)
                {
                    string dateExp = checkValid.Replace("TENTAKEL", "");
                    decimal now = f.ConvertDecimal(DateTime.Now.ToString("yyyyMMdd"));
                    if (f.ConvertDecimal(dateExp) >= now)
                    {
                        if (r.CreateFileKey(txtKey.Text) == true)
                        {
                            MessageBox.Show("lincense key is Valid, Run again", "Important Question", MessageBoxButtons.OK);
                            f.PageControl("InitPage");
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Please, input valid lincense key!", "Important Question", MessageBoxButtons.OK);
                        }

                    }
                    else
                    {
                        MessageBox.Show("Please, input new lincense key, your key had been expired", "Important Question", MessageBoxButtons.OK);
                    }
                }
                else
                {
                    MessageBox.Show("Please, input valid lincense key!", "Important Question", MessageBoxButtons.OK);
                }
            }
            else
            {
                MessageBox.Show("Please, input valid lincense key!", "Important Question", MessageBoxButtons.OK);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
