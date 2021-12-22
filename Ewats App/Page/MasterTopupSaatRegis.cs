using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ewats_App.Page
{
    public partial class MasterTopupSaatRegis : Form
    {
        public MasterTopupSaatRegis()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form frm = Application.OpenForms["Main"];
            if (frm != null)
            {
                Panel tbx = frm.Controls.Find("PagePanel", true).FirstOrDefault() as Panel;
                UserControl fc = tbx.Controls.Find("Registrasi", true).FirstOrDefault() as UserControl;
                if (fc != null)
                {
                    Button btnTopup = fc.Controls.Find("btnTopup", true).FirstOrDefault() as Button;
                    if (btnTopup != null)
                    {
                        btnTopup.Text = "Topup : Rp " + string.Format("{0:n0}", button2.Text);
                    }
                    this.Close();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form frm = Application.OpenForms["Main"];
            if (frm != null)
            {
                Panel tbx = frm.Controls.Find("PagePanel", true).FirstOrDefault() as Panel;
                UserControl fc = tbx.Controls.Find("Registrasi", true).FirstOrDefault() as UserControl;
                if (fc != null)
                {
                    Button btnTopup = fc.Controls.Find("btnTopup", true).FirstOrDefault() as Button;
                    if (btnTopup != null)
                    {
                        btnTopup.Text = "Isi Saldo Emoney";
                    }
                    this.Close();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form frm = Application.OpenForms["Main"];
            if (frm != null)
            {
                Panel tbx = frm.Controls.Find("PagePanel", true).FirstOrDefault() as Panel;
                UserControl fc = tbx.Controls.Find("Registrasi", true).FirstOrDefault() as UserControl;
                if (fc != null)
                {
                    Button btnTopup = fc.Controls.Find("btnTopup", true).FirstOrDefault() as Button;
                    if (btnTopup != null)
                    {
                        btnTopup.Text = "Topup : Rp " + string.Format("{0:n0}", button5.Text);
                    }
                    this.Close();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form frm = Application.OpenForms["Main"];
            if (frm != null)
            {
                Panel tbx = frm.Controls.Find("PagePanel", true).FirstOrDefault() as Panel;
                UserControl fc = tbx.Controls.Find("Registrasi", true).FirstOrDefault() as UserControl;
                if (fc != null)
                {
                    Button btnTopup = fc.Controls.Find("btnTopup", true).FirstOrDefault() as Button;
                    if (btnTopup != null)
                    {
                        btnTopup.Text = "Topup : Rp " + string.Format("{0:n0}", button3.Text);
                    }
                    this.Close();
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form frm = Application.OpenForms["Main"];
            if (frm != null)
            {
                Panel tbx = frm.Controls.Find("PagePanel", true).FirstOrDefault() as Panel;
                UserControl fc = tbx.Controls.Find("Registrasi", true).FirstOrDefault() as UserControl;
                if (fc != null)
                {
                    Button btnTopup = fc.Controls.Find("btnTopup", true).FirstOrDefault() as Button;
                    if (btnTopup != null)
                    {
                        btnTopup.Text = "Topup : Rp " + string.Format("{0:n0}", button6.Text);
                    }
                    this.Close();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form frm = Application.OpenForms["Main"];
            if (frm != null)
            {
                Panel tbx = frm.Controls.Find("PagePanel", true).FirstOrDefault() as Panel;
                UserControl fc = tbx.Controls.Find("Registrasi", true).FirstOrDefault() as UserControl;
                if (fc != null)
                {
                    Button btnTopup = fc.Controls.Find("btnTopup", true).FirstOrDefault() as Button;
                    if (btnTopup != null)
                    {
                        btnTopup.Text = "Topup : Rp " + string.Format("{0:n0}", button4.Text);
                    }
                    this.Close();
                }
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            Form frm = Application.OpenForms["Main"];
            if (frm != null)
            {
                Panel tbx = frm.Controls.Find("PagePanel", true).FirstOrDefault() as Panel;
                UserControl fc = tbx.Controls.Find("Registrasi", true).FirstOrDefault() as UserControl;
                if (fc != null)
                {
                    Button btnTopup = fc.Controls.Find("btnTopup", true).FirstOrDefault() as Button;
                    if (btnTopup != null)
                    {
                        btnTopup.Text = "Topup : Rp " + string.Format("{0:n0}", button23.Text);
                    }
                    this.Close();
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
        }
    }
}
