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

namespace Ewats_App.Page
{
    public partial class OrderSewa : Form
    {
        GlobalFunc f = new GlobalFunc();
        public OrderSewa()
        {
            InitializeComponent();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void button19_Click(object sender, EventArgs e)
        {
            if (txtQty.Text != "" && txtQty.Text != "0")
            {
                Form frm = Application.OpenForms["Main"];
                if (frm != null)
                {
                    Panel tbx = frm.Controls.Find("PagePanel", true).FirstOrDefault() as Panel;
                    UserControl fc = tbx.Controls.Find("Registrasi", true).FirstOrDefault() as UserControl;
                    if (fc != null)
                    {
                        DataGridView dt = fc.Controls.Find("dt_grid2", true).FirstOrDefault() as DataGridView;
                        decimal total = f.ConvertDecimal(lblHarga.Text) * f.ConvertDecimal(txtQty.Text);
                        string[] row = new string[] { "x", lblKodeBarang.Text, lblNamaProduk.Text, lblHarga.Text, txtQty.Text, f.ConvertToRupiah(total) };
                        dt.Rows.Add(row);
                        this.Close();
                    }
                }
                Form frm2 = Application.OpenForms["Persewaan"];
                if (frm2 != null)
                {
                    frm2.Close();
                }
                this.Close();
            }
            
        }
    }
}
