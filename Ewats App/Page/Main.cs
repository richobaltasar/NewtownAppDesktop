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
using System.IO.Ports;
using System.Threading;

namespace Ewats_App.Page
{
    public partial class Main : Form
    {
        GlobalFunc f = new GlobalFunc();

        public Main()
        {
            InitializeComponent();
        }

        public void button3_Click(object sender, EventArgs e)
        {
            Panel tbx = this.Controls.Find("PagePanel", true).FirstOrDefault() as Panel;
            UserControl fc = tbx.Controls.Find("Dashboard", true).FirstOrDefault() as UserControl;
            if (fc != null)
            {
                fc.Show();
                fc.BringToFront();
            }
            else
            {
                var Page = new Page.Dashboard();
                Page.Width = this.PagePanel.Width;
                Page.Height = this.PagePanel.Height;
                PagePanel.Controls.Add(Page);
                Page.BringToFront();
            }
            f.RefreshDashboard();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Model.ConfigurationFileStatic.VFDPort != null && Model.ConfigurationFileStatic.VFDPort != "")
            {
                VFDPort.send("Registrasi Ticket", "Open Order", VFDPort.sp.PortName);
            }
            
            Panel tbx = this.Controls.Find("PagePanel", true).FirstOrDefault() as Panel;
            UserControl fc = tbx.Controls.Find("Registrasi", true).FirstOrDefault() as UserControl;
            if (fc != null)
            {
                RichTextBox TxtBacaKartu = fc.Controls.Find("TxtBacaKartu", true).FirstOrDefault() as RichTextBox;
                if (TxtBacaKartu != null)
                {
                    TxtBacaKartu.Text = "";
                }
                var Page = new Page.Registrasi();
                PagePanel.Controls.Clear();
                Page.Width = this.PagePanel.Width;
                Page.Height = this.PagePanel.Height;
                PagePanel.Controls.Add(Page);
                Page.BringToFront();
            }
            else
            {
                var Page = new Page.Registrasi();
                PagePanel.Controls.Clear();
                Page.Width = this.PagePanel.Width;
                Page.Height = this.PagePanel.Height;
                PagePanel.Controls.Add(Page);
                Page.BringToFront();
            }
            
        }

        private void lblCompanyName_Click(object sender, EventArgs e)
        {

        }

        private void BtnTopup_Click(object sender, EventArgs e)
        {
            if (Model.ConfigurationFileStatic.VFDPort != null && Model.ConfigurationFileStatic.VFDPort != "")
            {
                VFDPort.send("Topup", "Open Order", VFDPort.sp.PortName);
            }
            Panel tbx = this.Controls.Find("PagePanel", true).FirstOrDefault() as Panel;
            UserControl fc = tbx.Controls.Find("Topup", true).FirstOrDefault() as UserControl;
            if (fc != null)
            {
                PagePanel.Controls.Clear();
                var Page = new Page.Topup();
                Page.Width = this.PagePanel.Width;
                Page.Height = this.PagePanel.Height;
                PagePanel.Controls.Add(Page);
                Page.BringToFront();

                //RichTextBox TxtBacaKartu = fc.Controls.Find("TxtBacaKartu", true).FirstOrDefault() as RichTextBox;
                //if (TxtBacaKartu != null)
                //{
                //    TxtBacaKartu.Text = "";
                //}

                //fc.Show();
                //fc.BringToFront();
            }
            else
            {
                var Page = new Page.Topup();

                Page.Width = this.PagePanel.Width;
                Page.Height = this.PagePanel.Height;
                PagePanel.Controls.Add(Page);
                Page.BringToFront();
            }
        }

        private void BtnFoodCourt_Click(object sender, EventArgs e)
        {
            if (Model.ConfigurationFileStatic.VFDPort != null && Model.ConfigurationFileStatic.VFDPort != "")
            {
                VFDPort.send("F&B Transaksi", "Open Order", VFDPort.sp.PortName);
            }
            Panel tbx = this.Controls.Find("PagePanel", true).FirstOrDefault() as Panel;
            UserControl fc = tbx.Controls.Find("FoodCourt", true).FirstOrDefault() as UserControl;
            if (fc != null)
            {
                RichTextBox TxtBacaKartu = fc.Controls.Find("TxtBacaKartu", true).FirstOrDefault() as RichTextBox;
                if (TxtBacaKartu != null)
                {
                    TxtBacaKartu.Text = "";
                }

                fc.Show();
                fc.BringToFront();
            }
            else
            {
                var Page = new Page.FoodCourt();
                Page.Width = this.PagePanel.Width;
                Page.Height = this.PagePanel.Height;
                PagePanel.Controls.Add(Page);
                Page.BringToFront();
            }
            
        }

        private void btnRefund_Click(object sender, EventArgs e)
        {
            if (Model.ConfigurationFileStatic.VFDPort != null && Model.ConfigurationFileStatic.VFDPort != "")
            {
                VFDPort.send("Refund", "Open Order", VFDPort.sp.PortName);
            }
            Panel tbx = this.Controls.Find("PagePanel", true).FirstOrDefault() as Panel;
            UserControl fc = tbx.Controls.Find("Refund", true).FirstOrDefault() as UserControl;
            if (fc != null)
            {
                PagePanel.Controls.Clear();
                var Page = new Page.Refund();
                Page.Width = this.PagePanel.Width;
                Page.Height = this.PagePanel.Height;
                PagePanel.Controls.Add(Page);
                Page.BringToFront();

                //RichTextBox TxtBacaKartu = fc.Controls.Find("TxtBacaKartu", true).FirstOrDefault() as RichTextBox;
                //if (TxtBacaKartu != null)
                //{
                //    TxtBacaKartu.Text = "";
                //}

                //fc.Show();
                //fc.BringToFront();
            }
            else
            {
                var Page = new Page.Refund();
                Page.Width = this.PagePanel.Width;
                Page.Height = this.PagePanel.Height;
                PagePanel.Controls.Add(Page);
                Page.BringToFront();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void Main_Load(object sender, EventArgs e)
        {
            var data = f.CheckOpenCashier();
            if (data.Success == true)
            {
                btnRegistrasi.Enabled = true;
                BtnTopup.Enabled = true;
                btnRefund.Enabled = true;
                BtnFoodCourt.Enabled = true;

                Panel tbx = this.Controls.Find("PagePanel", true).FirstOrDefault() as Panel;
                UserControl fc = tbx.Controls.Find("Dashboard", true).FirstOrDefault() as UserControl;

                if (fc != null)
                {
                    fc.Show();
                    fc.BringToFront();
                }
                else
                {
                    var Page = new Page.Dashboard();
                    Page.Width = this.PagePanel.Width;
                    Page.Height = this.PagePanel.Height;
                    PagePanel.Controls.Add(Page);
                    Page.BringToFront();
                }

                f.RefreshDashboard();
                if (Model.ConfigurationFileStatic.VFDPort != null && Model.ConfigurationFileStatic.VFDPort != "")
                {
                    VFDPort.send("Selamat Datang", "di KUMPAY WATERPARK", Model.ConfigurationFileStatic.VFDPort);
                }
            }
            else
            {
                btnRegistrasi.Enabled = false;
                BtnTopup.Enabled = false;
                btnRefund.Enabled = false;
                BtnFoodCourt.Enabled = false;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }
        
        private void button1_Click_2(object sender, EventArgs e)
        {
            this.Close();
            Login f = new Login();
            f.Show();
            f.BringToFront();
            f.StartPosition = FormStartPosition.CenterScreen;
        }

        private void PagePanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click_3(object sender, EventArgs e)
        {
            this.Close();
            Login f = new Login();
            f.Show();
            f.BringToFront();
            f.StartPosition = FormStartPosition.CenterScreen;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Panel tbx = this.Controls.Find("PagePanel", true).FirstOrDefault() as Panel;
            UserControl fc = tbx.Controls.Find("AccountLog", true).FirstOrDefault() as UserControl;
            if (fc != null)
            {
                //RichTextBox TxtBacaKartu = fc.Controls.Find("TxtBacaKartu", true).FirstOrDefault() as RichTextBox;
                //if (TxtBacaKartu != null)
                //{
                //    TxtBacaKartu.Text = "";
                //}

                fc.Show();
                fc.BringToFront();
                //var Page = new Page.HistoryAccount();
                //PagePanel.Controls.Clear();
                //Page.Width = this.PagePanel.Width;
                //Page.Height = this.PagePanel.Height;
                //PagePanel.Controls.Add(Page);
                //Page.BringToFront();
            }
            else
            {
                var Page = new Page.AccountLog();
                Page.Width = this.PagePanel.Width;
                Page.Height = this.PagePanel.Height;
                PagePanel.Controls.Add(Page);
                Page.BringToFront();
            }
        }

        private void Main_Activated(object sender, EventArgs e)
        {
            //Function.VFDPort.send("Selamat Datang", "di KUMPAY WATERPARK", Function.VFDPort.sp.PortName);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            f.PageControl("ChangePassword");
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

