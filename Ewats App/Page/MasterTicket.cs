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
    public partial class MasterTicket : Form
    {
        GlobalFunc f = new GlobalFunc();

        public MasterTicket()
        {
            InitializeComponent();
        }

        private void MasterTicket_Load(object sender, EventArgs e)
        {
            txtControl.Text = "TxtQtxTicket";
            load_datagrid("");
        }

        public void atur_grid()
        {
            dt_grid.Rows.Clear();
            dt_grid.ColumnCount = 3;
            dt_grid.Columns[0].Name = "ID Ticket";
            dt_grid.Columns[1].Name = "Nama Ticket";
            dt_grid.Columns[2].Name = "Harga";

            dt_grid.RowHeadersVisible = false;
            dt_grid.ColumnHeadersVisible = true;
            DataGridViewColumn column1 = dt_grid.Columns[0];
            DataGridViewColumn column2 = dt_grid.Columns[1];
            DataGridViewColumn column3 = dt_grid.Columns[2];

            // Initialize basic DataGridView properties.
            dt_grid.Dock = DockStyle.Fill;
            dt_grid.BackgroundColor = SystemColors.GradientInactiveCaption;
            dt_grid.BorderStyle = BorderStyle.Fixed3D;
            // Set property values appropriate for read-only display and 
            // limited interactivity. 
            dt_grid.AllowUserToAddRows = false;
            dt_grid.AllowUserToDeleteRows = false;
            dt_grid.AllowUserToOrderColumns = true;
            dt_grid.ReadOnly = true;
            dt_grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dt_grid.MultiSelect = false;
            //dt_grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            
            dt_grid.AllowUserToResizeColumns = false;
            dt_grid.AllowUserToResizeRows = false;

            //dt_grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            //dt_grid.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            // Set the selection background color for all the cells.
            dt_grid.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dt_grid.DefaultCellStyle.SelectionForeColor = Color.Black;
            // Set RowHeadersDefaultCellStyle.SelectionBackColor so that its default
            // value won't override DataGridView.DefaultCellStyle.SelectionBackColor.
            dt_grid.RowHeadersDefaultCellStyle.SelectionBackColor = Color.Empty;
            // Set the background color for all rows and for alternating rows. 
            // The value for alternating rows overrides the value for all rows. 
            dt_grid.RowsDefaultCellStyle.BackColor = Color.White;
            dt_grid.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            // Set the row and column header styles.
            dt_grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dt_grid.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
            dt_grid.RowHeadersDefaultCellStyle.BackColor = Color.Black;
            
            //dt_grid.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dt_grid.DefaultCellStyle.Font = new Font("Calibri", 12);
            //dt_grid.Columns[0].Width = 20;
            for (int i = 0; i < dt_grid.Columns.Count - 1; i++)
            {
                dt_grid.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            dt_grid.Columns[dt_grid.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            //for (int i = 0; i < dt_grid.Columns.Count; i++)
            //{
            //    int colw = dt_grid.Columns[i].Width;
            //    dt_grid.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            //    dt_grid.Columns[i].Width = colw;
            //}
            dt_grid.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dt_grid.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dt_grid.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
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

        public void load_datagrid(string search)
        {
            atur_grid();
            var data = f.GetTicket(search);
            int a = 0;
            foreach (var r in data)
            {
                a++;
                string[] row = new string[] { r.IdTicket, r.namaticket, f.ConvertToRupiah(r.harga), f.ConvertToRupiah(r.CashbackNominal),r.CashbackPersen.ToString() };
                this.dt_grid.Rows.Add(row);
                DataGridViewRow d = dt_grid.Rows[a - 1];
                d.MinimumHeight = 40;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            load_datagrid(txtSearch.Text);
        }

        private void dt_grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dt_grid.Rows[e.RowIndex];
                lblId.Text = row.Cells["ID Ticket"].Value.ToString();
                lblNama.Text = row.Cells["Nama Ticket"].Value.ToString();
                LblHarga.Text = row.Cells["Harga"].Value.ToString();
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            input_keyPad((sender as Button).Text, txtControl.Text);
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
            input_keyPad((sender as Button).Text, txtControl.Text);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (lblId.Text != "-" && TxtQtxTicket.Text != "" && TxtQtxTicket.Text != "0")
            {
                Form frm = Application.OpenForms["Main"];
                if (frm != null)
                {
                    Panel tbx = frm.Controls.Find("PagePanel", true).FirstOrDefault() as Panel;
                    UserControl fc = tbx.Controls.Find("Registrasi", true).FirstOrDefault() as UserControl;
                    if (fc != null)
                    {
                        DataGridView dt = fc.Controls.Find("dt_grid", true).FirstOrDefault() as DataGridView;
                        var data = new Model.KeranjangTicket();
                        data.IdTicket = lblId.Text;
                        data.NamaTicket = lblNama.Text;
                        data.Harga =  Convert.ToDecimal(LblHarga.Text.Replace("Rp","").Trim());
                        data.Qty = Convert.ToDecimal(TxtQtxTicket.Text);                        
                        data.Total = data.Harga * data.Qty;
                        data.IdDiskon = lblDiskonId.Text;
                        data.NamaDiskon = lblNamaDiskon.Text;
                        if (lblDiskon.Text != "" && lblDiskon.Text != "-")
                        {
                            data.Diskon = Convert.ToDecimal(lblDiskon.Text.Replace("%", "").Trim());
                        }
                        data.TotalDiskon = data.Total * data.Diskon / 100;
                        data.TotalAfterDiskon = data.Total - data.TotalDiskon;
                        string[] row = new string[] { "x",data.IdTicket,data.NamaTicket, string.Format("{0:n0}", data.Harga),data.Qty.ToString(), string.Format("{0:n0}", data.Total),data.IdDiskon,data.NamaDiskon,data.Diskon.ToString(), string.Format("{0:n0}", data.TotalDiskon), string.Format("{0:n0}",data.TotalAfterDiskon)};
                        dt.Rows.Add(row);
                        this.Close();
                    }
                }
                this.Close();
            }
            
        }

        private void button18_Click(object sender, EventArgs e)
        {
            MasterDiskon f = new MasterDiskon();
            f.Show();
            f.BringToFront();
            f.StartPosition = FormStartPosition.CenterParent;
        }

        private void dt_grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button19_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
