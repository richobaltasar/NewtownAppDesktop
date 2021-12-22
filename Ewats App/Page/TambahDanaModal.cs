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
    public partial class TambahDanaModal : Form
    {
        GlobalFunc f = new GlobalFunc();

        public TambahDanaModal()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
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

        private void button13_Click(object sender, EventArgs e)
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

        private void TambahDanaModal_Load(object sender, EventArgs e)
        {
            load_datagrid();
        }

        public void load_datagrid()
        {
            atur_grid();
            var data = f.GetDanaModalLog(f.GetComputerName(),f.GetNamaUser(General.IDUser));
            int a = 0;
            foreach (var r in data)
            {
                a++;
                string[] row = new string[] { r.Id, r.Datetime, "Rp "+string.Format("{0:n0}", r.Nominal),r.NamaUser };
                this.dt_grid2.Rows.Add(row);
                DataGridViewRow d = dt_grid2.Rows[a - 1];
                d.MinimumHeight = 40;
            }
        }
        public void atur_grid()
        {
            dt_grid2.Rows.Clear();
            dt_grid2.ColumnCount = 4;
            dt_grid2.Columns[0].Name = "Id Trx";
            dt_grid2.Columns[1].Name = "Date time";
            dt_grid2.Columns[2].Name = "Nominal";
            dt_grid2.Columns[3].Name = "Nama User";

            dt_grid2.RowHeadersVisible = false;
            dt_grid2.ColumnHeadersVisible = true;
            DataGridViewColumn column1 = dt_grid2.Columns[0];
            DataGridViewColumn column2 = dt_grid2.Columns[1];
            DataGridViewColumn column3 = dt_grid2.Columns[2];
            DataGridViewColumn column4 = dt_grid2.Columns[3];

            // Initialize basic DataGridView properties.
            dt_grid2.Dock = DockStyle.None;
            dt_grid2.BackgroundColor = SystemColors.GradientInactiveCaption;
            dt_grid2.BorderStyle = BorderStyle.Fixed3D;
            // Set property values appropriate for read-only display and 
            // limited interactivity. 
            dt_grid2.AllowUserToAddRows = false;
            dt_grid2.AllowUserToDeleteRows = false;
            dt_grid2.AllowUserToOrderColumns = true;
            dt_grid2.ReadOnly = true;
            dt_grid2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dt_grid2.MultiSelect = true;
            dt_grid2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dt_grid2.AllowUserToResizeColumns = true;
            dt_grid2.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dt_grid2.AllowUserToResizeRows = false;
            dt_grid2.RowHeadersWidthSizeMode =
                DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            // Set the selection background color for all the cells.
            dt_grid2.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dt_grid2.DefaultCellStyle.SelectionForeColor = Color.Black;
            // Set RowHeadersDefaultCellStyle.SelectionBackColor so that its default
            // value won't override DataGridView.DefaultCellStyle.SelectionBackColor.
            dt_grid2.RowHeadersDefaultCellStyle.SelectionBackColor = Color.Empty;
            // Set the background color for all rows and for alternating rows. 
            // The value for alternating rows overrides the value for all rows. 
            dt_grid2.RowsDefaultCellStyle.BackColor = Color.White;
            dt_grid2.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            // Set the row and column header styles.
            dt_grid2.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dt_grid2.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
            dt_grid2.RowHeadersDefaultCellStyle.BackColor = Color.Black;
            dt_grid2.DefaultCellStyle.Font = new Font("Tahoma", 11);
            dt_grid2.Columns[0].Width = 40;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            var res = MessageBox.Show("Apakah Anda yakin untuk melakukan Penambahan Modal Cash Box sebesar : "+ f.ConvertToRupiah(f.ConvertDecimal(txtCashbox.Text))+" ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (res == DialogResult.Yes)
            {
                decimal Nominal = f.ConvertDecimal(txtCashbox.Text);
                if (Nominal > 0)
                {
                    var data = new TambahModalCashbox();
                    data.ComputerName = f.GetComputerName();
                    data.NamaUser = f.GetNamaUser(General.IDUser);
                    data.Nominal = Nominal;
                    var save = f.SaveDataTambahModal(data);
                    if (save.Success == true)
                    {
                        Form frm = Application.OpenForms["Main"];
                        if (frm != null)
                        {
                            Panel tbx = frm.Controls.Find("PagePanel", true).FirstOrDefault() as Panel;
                            UserControl fc = tbx.Controls.Find("Dashboard", true).FirstOrDefault() as UserControl;
                            Button btnRegis = frm.Controls.Find("btnRegistrasi", true).FirstOrDefault() as Button;
                            Button BtnTopup = frm.Controls.Find("BtnTopup", true).FirstOrDefault() as Button;
                            Button btnRefund = frm.Controls.Find("btnRefund", true).FirstOrDefault() as Button;
                            Button BtnFoodCourt = frm.Controls.Find("BtnFoodCourt", true).FirstOrDefault() as Button;
                            if (fc != null)
                            {
                                fc.Show();
                                fc.BringToFront();
                            }
                            else
                            {
                                var Page = new Page.Dashboard();
                                Page.Width = tbx.Width;
                                Page.Height = tbx.Height;
                                tbx.Controls.Add(Page);
                                Page.BringToFront();
                            }
                            if (btnRegis != null)
                            {
                                btnRegis.Enabled = true;
                            }
                            if (BtnTopup != null)
                            {
                                BtnTopup.Enabled = true;
                            }
                            if (btnRefund != null)
                            {
                                btnRefund.Enabled = true;
                            }
                            if (BtnFoodCourt != null)
                            {
                                BtnFoodCourt.Enabled = true;
                            }
                        }
                        f.RefreshDashboard();
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Nominal yang diinput masih : " + f.ConvertToRupiah(f.ConvertDecimal(txtCashbox.Text)) + ",silahkan Isi", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
