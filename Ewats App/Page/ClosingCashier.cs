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
using System.Drawing.Printing;

namespace Ewats_App.Page
{
    public partial class ClosingCashier : Form
    {
        GlobalFunc f = new GlobalFunc();
        public Model.DashboardModel DataModel = new Model.DashboardModel();
        public ClosingCashier()
        {
            InitializeComponent();
        }

        private void ClosingCashier_Load(object sender, EventArgs e)
        {
            //this.TopMost = true;
            this.BringToFront();
            string ComputerName = f.GetComputerName();
            string NamaUser = f.GetNamaUser(General.IDUser);
            var data = f.GetDashboard(ComputerName,NamaUser);
            DataModel = data;

            TxtRegisTotal.Text = f.ConvertToRupiah(f.ConvertDecimal(data.TotalRegis));
            TxtTotalTopup.Text = f.ConvertToRupiah(f.ConvertDecimal(data.TotalTopup));
            TxtFoodCourtCash.Text = f.ConvertToRupiah(f.ConvertDecimal(data.TotalFoodcourtCash));

            txtTotalTrx.Text = f.ConvertToRupiah(f.ConvertDecimal(data.TotalTransaksi)+ f.ConvertDecimal(data.TotalFoodcourtCash));

            txtTicketPayEmoney.Text = f.ConvertToRupiah(f.ConvertDecimal(data.TotalTicketPayEmoney));
            TxtTotalRefund.Text = f.ConvertToRupiah(f.ConvertDecimal(data.TotalRefund));
            TxtFBEmoney.Text = f.ConvertToRupiah(f.ConvertDecimal(data.TotalFoodcourtEmoney));

            decimal TotalTransaksiEmoney = f.ConvertDecimal(data.TotalRefund) + f.ConvertDecimal(data.TotalFoodcourtEmoney)+ f.ConvertDecimal(data.TotalTicketPayEmoney);
            txtTotalTransaksiEmoney.Text = f.ConvertToRupiah(TotalTransaksiEmoney);

            txtDanaModal.Text = f.ConvertToRupiah(f.ConvertDecimal(data.TotalDanaModal));
            txtTotalCashin.Text = f.ConvertToRupiah((f.ConvertDecimal(data.TotalCashIn)- f.ConvertDecimal(data.TotalDanaModal)));
            txtTotalCashOut.Text = f.ConvertToRupiah(f.ConvertDecimal(data.TotalCashOut));
            if (data.TotalCashBox.Contains('-') == false)
            {
                txtTotalCashBox.Text = f.ConvertToRupiah(f.ConvertDecimal(data.TotalCashBox));
            }
            else
            {
                txtTotalCashBox.Text = "- "+f.ConvertToRupiah(f.ConvertDecimal(data.TotalCashBox.Replace("-","")));
            }

            txtEDCRegis.Text = f.ConvertToRupiah(f.ConvertDecimal(data.TotalNominalEdcRegis));
            txtEDCTopup.Text = f.ConvertToRupiah(f.ConvertDecimal(data.TotalNominalEdcTopup));

            totalTrxEdc.Text = data.TotalTrxEdc;
            txtTotalDebitNominal.Text = f.ConvertToRupiah(f.ConvertDecimal(data.TotalNominalDebit));

            txtTotalAccountTrx.Text = data.TotalTrxEmoney;
            txtDebitDeposit.Text = f.ConvertToRupiah(f.ConvertDecimal(data.TotalNominalDebitEmoney));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            input_keyPad((sender as Button).Text, txtControl.Text);
        }

        public void input_keyPad(string key, string Object)
        {
            TextBox txt = this.Controls.Find(Object, true).FirstOrDefault() as TextBox;
            if (txt != null)
            {
                if (key == "<-")
                {
                    if (txt.Text.Length > 0)
                    {
                        txt.Text = txt.Text.Remove(txt.Text.Length - 1, 1);
                        string data = txt.Text.Replace(".", "").Replace(",", "");
                        if (data != "")
                        {
                            decimal t = Convert.ToDecimal(data);
                            txt.Text = string.Format("{0:n0}", t);
                        }
                        else
                        {
                            txt.Text = "0";
                        }
                    }
                }
                else if (key == "Reset")
                {
                    txt.Text = "0";
                }
                else if (key == "Enter")
                {
                }
                else
                {
                    string data = (txt.Text + key).Replace(".", "").Replace(",", "");
                    if (data != "")
                    {
                        decimal t = Convert.ToDecimal(data);
                        txt.Text = string.Format("{0:n0}", t);
                    }
                    else
                    {
                        txt.Text = txt.Text + key;
                    }

                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            input_keyPad((sender as Button).Text, txtControl.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            input_keyPad((sender as Button).Text, txtControl.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            input_keyPad((sender as Button).Text, txtControl.Text);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            input_keyPad((sender as Button).Text, txtControl.Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            input_keyPad((sender as Button).Text, txtControl.Text);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            input_keyPad((sender as Button).Text, txtControl.Text);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            input_keyPad((sender as Button).Text, txtControl.Text);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            input_keyPad((sender as Button).Text, txtControl.Text);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            input_keyPad((sender as Button).Text, txtControl.Text);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            input_keyPad((sender as Button).Text, txtControl.Text);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            input_keyPad((sender as Button).Text, txtControl.Text);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            input_keyPad((sender as Button).Text, txtControl.Text);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            input_keyPad((sender as Button).Text, txtControl.Text);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Ulang: 
            
            decimal input = f.ConvertDecimal(txtUangCashBox.Text);
            if(input != 0)
            { 
            if (input != f.ConvertDecimal(DataModel.TotalCashBox))
            {
                var res = MessageBox.Show("Uang yang anda masukkan tidak sesuai dengan jumlah Uang pada Cashbox, " +
                    "Tekan Yes untuk tetap melakukan tutup buku, dan tekan No untuk membatalkan permintaan",
                    "Reader not connected", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res == DialogResult.Yes)
                {
                    if (DataModel != null)
                    {
                        var save = f.SaveClosing(DataModel, f.GetComputerName(), f.GetNamaUser(General.IDUser), input);
                        if (save.Success == true)
                        {
                            PrintLagi1:
                            var print = ClosingPrint(DataModel,input);
                            if (print.Success == true)
                            {
                                this.Close();
                                Form fc2 = Application.OpenForms["Main"];
                                if (fc2 != null)
                                {
                                    fc2.Close();
                                }
                                f.PageControl("Login");
                            }
                            else
                            {
                                var res3 = MessageBox.Show("Print Gagal", "Printing Fail", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                                if (res3 == DialogResult.Retry)
                                {
                                    goto PrintLagi1;
                                }
                            }
                        }
                        else
                        {
                            var res3 = MessageBox.Show("Save Closing Gagal", "SaveClosing Fail", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                            if (res3 == DialogResult.Retry)
                            {
                                goto Ulang;
                            }
                        }
                    }
                    else
                    {
                        var res3 = MessageBox.Show("Data Closing Kosong", "SaveClosing Fail", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.Close();
                    }
                }
            }
            else
            {
                if (DataModel != null)
                {
                    
                    var save = f.SaveClosing(DataModel, f.GetComputerName(), f.GetNamaUser(General.IDUser), input);
                    if (save.Success == true)
                    {
                        PrintLagi2:
                        var print = ClosingPrint(DataModel,input);
                        if(print.Success == true)
                        {
                            this.Close();
                            Form fc2 = Application.OpenForms["Main"];
                            if (fc2 != null)
                            {
                                fc2.Close();
                            }

                            f.PageControl("Login");
                        }
                        else
                        {
                            var res3 = MessageBox.Show("Print Gagal", "Printing Fail", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                            if (res3 == DialogResult.Retry)
                            {
                                goto PrintLagi2;
                            }
                        }
                    }
                    else
                    {
                        var res3 = MessageBox.Show("Save Closing Gagal", "SaveClosing Fail", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                        if (res3 == DialogResult.Retry)
                        {
                            goto Ulang;
                        }
                    }
                }
            }
            }
            else
            {
                var res3 = MessageBox.Show("Nominal yang diinput masih kosong", "Submit Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        public ReturnResult ClosingPrint(DashboardModel data,decimal Input)
        {
            var res = new ReturnResult();
            try
            {
                string now = f.GetDatetime();
                string s = "Datetime \t: " + now + Environment.NewLine;
                s += "Merchant ID \t: " + f.GetComputerName() + Environment.NewLine;
                s += "Nama Petugas \t: " + f.GetNamaUser(General.IDUser) + Environment.NewLine;
                s += "-------------------------------------------------------" + Environment.NewLine;
                s += "Total Registrasi \t\t: " + f.ConvertToRupiah(f.ConvertDecimal(data.TotalRegis))+ Environment.NewLine;
                s += "Total Topup \t\t: " + f.ConvertToRupiah(f.ConvertDecimal(data.TotalTopup)) + Environment.NewLine;
                s += "Total FoodCourt Cash \t: " + f.ConvertToRupiah(f.ConvertDecimal(data.TotalFoodcourtCash)) + Environment.NewLine;
                s += "-------------------------------------------------------" + Environment.NewLine;
                s += "Total CashIn \t\t: " + f.ConvertToRupiah(f.ConvertDecimal(data.TotalRegis)+ f.ConvertDecimal(data.TotalTopup)+f.ConvertDecimal(data.TotalFoodcourtCash)) + Environment.NewLine;
                s += "-------------------------------------------------------" + Environment.NewLine;
                s += "Total EDC Regis \t\t: " + f.ConvertToRupiah(f.ConvertDecimal(data.TotalNominalEdcRegis)) + Environment.NewLine;
                s += "Total EDC Topup \t\t: " + f.ConvertToRupiah(f.ConvertDecimal(data.TotalNominalEdcTopup)) + Environment.NewLine;
                s += "-------------------------------------------------------" + Environment.NewLine;
                s += "Total EDC \t\t: " + f.ConvertToRupiah(f.ConvertDecimal(data.TotalNominalDebit))+ Environment.NewLine;
                s += "-------------------------------------------------------" + Environment.NewLine;
                s += "Total Ticket Emoney \t: " + f.ConvertToRupiah(f.ConvertDecimal(data.TotalTicketPayEmoney)) + Environment.NewLine;
                s += "Total FoodCourt Emoney \t: " + f.ConvertToRupiah(f.ConvertDecimal(data.TotalFoodcourtEmoney)) + Environment.NewLine;
                s += "Total Refund \t\t: " + f.ConvertToRupiah(f.ConvertDecimal(data.TotalRefund)) + Environment.NewLine;
                s += "-------------------------------------------------------" + Environment.NewLine;
                s += "Total Emoney Trasaksi \t: " + f.ConvertToRupiah(f.ConvertDecimal(data.TotalFoodcourtEmoney) + f.ConvertDecimal(data.TotalRefund) + f.ConvertDecimal(data.TotalTicketPayEmoney)) + Environment.NewLine;
                s += "-------------------------------------------------------" + Environment.NewLine;
                s += "Total Dana Modal \t: " + f.ConvertToRupiah(f.ConvertDecimal(data.TotalDanaModal)) + Environment.NewLine;
                s += "Total Cash in \t\t: " + f.ConvertToRupiah((f.ConvertDecimal(data.TotalCashIn) - f.ConvertDecimal(data.TotalDanaModal))) + Environment.NewLine;
                s += "Total Cashout \t\t: " + f.ConvertToRupiah(f.ConvertDecimal(data.TotalCashOut)) + Environment.NewLine;
                s += "-------------------------------------------------------" + Environment.NewLine;
                decimal TotalCasbox = ((f.ConvertDecimal(data.TotalDanaModal) + (f.ConvertDecimal(data.TotalCashIn) - f.ConvertDecimal(data.TotalDanaModal))) - f.ConvertDecimal(data.TotalCashOut));
                s += "Total Cashbox \t\t: " + f.ConvertToRupiah(TotalCasbox) + Environment.NewLine;
                s += "Cash Input by Kasir \t: " + f.ConvertToRupiah(Input) + Environment.NewLine;
                s += "-------------------------------------------------------" + Environment.NewLine;
                if (Input == TotalCasbox)
                {
                    s += "Closing Status \t\t: Matching" + Environment.NewLine;
                }
                else if (Input < TotalCasbox)
                {
                    s += "Closing Status \t: Kasir Minus Uang Cashbox" + Environment.NewLine;
                }
                else if (Input > TotalCasbox)
                {
                    s += "Closing Status \t: Kasir Kelebihan Uang Cashbox" + Environment.NewLine;
                }

                s += "-------------------------------------------------------" + Environment.NewLine;
                foreach (string pfoot in f.GetFooterPrint())
                {
                    s += pfoot + Environment.NewLine;
                }

                PrintDocument p = new PrintDocument();
                p.PrintPage += delegate (object sender1, PrintPageEventArgs e1)
                {
                    int HeadreX = 0;
                    int startY = 0;
                    int Offset = 0;
                    string underLine1 = "======================================";
                    e1.Graphics.DrawString(underLine1, new Font("calibri", 7), new SolidBrush(Color.Black), 0, startY + Offset);
                    Offset = Offset + 10;
                    e1.Graphics.DrawString(f.GetFooterPrint(1), new Font("Arial", 8, FontStyle.Bold), new SolidBrush(Color.Black), HeadreX, startY + Offset);
                    Offset = Offset + 15;
                    //e1.Graphics.DrawString(f.GetFooterPrint(2), new Font("Arial", 10, FontStyle.Bold), new SolidBrush(Color.Black), HeadreX, startY + Offset);
                    e1.Graphics.DrawString(f.GetFooterPrint(2), new Font("Arial", 7, FontStyle.Bold), new SolidBrush(Color.Black), new RectangleF(0, startY + Offset, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));
                    Offset = Offset + 15;
                    e1.Graphics.DrawString(f.GetFooterPrint(3), new Font("Arial", 5, FontStyle.Italic), new SolidBrush(Color.Black), new RectangleF(0, startY + Offset, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));
                    Offset = Offset + 15;
                    string underLine = "======================================";
                    e1.Graphics.DrawString(underLine, new Font("Arial", 6), new SolidBrush(Color.Black), 0, startY + Offset);
                    Offset = Offset + 10;
                    e1.Graphics.DrawString(s, new Font("Arial", 7), new SolidBrush(Color.Black), new RectangleF(0, startY + Offset, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));
                };
                p.Print();
                res.Success = true;
                res.Message = "Print Success";
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = "Exception Occured While Printing " + ex.Message;
            }
            return res;
        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
