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
    public partial class MasterCashback : Form
    {
        GlobalFunc f = new GlobalFunc();

        public MasterCashback()
        {
            InitializeComponent();
        }

        private void MasterCashback_Load(object sender, EventArgs e)
        {
            load_datagrid("");
        }

        public void atur_grid()
        {
            dt_grid.Rows.Clear();
            dt_grid.ColumnCount = 3;
            dt_grid.Columns[0].Name = "Id Cashback";
            dt_grid.Columns[1].Name = "Nama Cashback";
            dt_grid.Columns[2].Name = "Nominal";

            dt_grid.RowHeadersVisible = false;
            dt_grid.ColumnHeadersVisible = true;
            DataGridViewColumn column1 = dt_grid.Columns[0];
            DataGridViewColumn column2 = dt_grid.Columns[1];
            DataGridViewColumn column3 = dt_grid.Columns[2];

            // Initialize basic DataGridView properties.
            dt_grid.Dock = DockStyle.None;
            dt_grid.BackgroundColor = SystemColors.GradientInactiveCaption;
            dt_grid.BorderStyle = BorderStyle.Fixed3D;
            // Set property values appropriate for read-only display and 
            // limited interactivity. 
            dt_grid.AllowUserToAddRows = false;
            dt_grid.AllowUserToDeleteRows = false;
            dt_grid.AllowUserToOrderColumns = true;
            dt_grid.ReadOnly = true;
            dt_grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dt_grid.MultiSelect = true;
            dt_grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dt_grid.AllowUserToResizeColumns = true;
            dt_grid.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dt_grid.AllowUserToResizeRows = false;
            dt_grid.RowHeadersWidthSizeMode =
                DataGridViewRowHeadersWidthSizeMode.DisableResizing;
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
            dt_grid.DefaultCellStyle.Font = new Font("Tahoma", 12);
            dt_grid.Columns[0].Width = 40;

        }

        public void load_datagrid(string search)
        {
            atur_grid();
            var data = f.GetCashbacks(search);
            int a = 0;
            foreach (var r in data)
            {
                a++;
                string[] row = new string[] { r.id,r.NamaCashback, string.Format("{0:n0}", r.Nominal)};
                this.dt_grid.Rows.Add(row);
                DataGridViewRow d = dt_grid.Rows[a - 1];
                d.MinimumHeight = 40;
            }
        }

        private void dt_grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dt_grid.Rows[e.RowIndex];
                Form frm = Application.OpenForms["MasterTicket"];
                if (frm != null)
                {
                    Label lblDiskonId = frm.Controls.Find("lblDiskonId", true).FirstOrDefault() as Label;
                    if (lblDiskonId != null)
                    {
                        lblDiskonId.Text = row.Cells["ID Promo"].Value.ToString();
                    }
                    Label lblNamaDiskon = frm.Controls.Find("lblNamaDiskon", true).FirstOrDefault() as Label;
                    if (lblNamaDiskon != null)
                    {
                        lblNamaDiskon.Text = row.Cells["Nama Event"].Value.ToString();
                    }

                    Label lblDiskon = frm.Controls.Find("lblDiskon", true).FirstOrDefault() as Label;
                    if (lblDiskon != null)
                    {
                        lblDiskon.Text = row.Cells["Diskon"].Value.ToString();
                    }

                    this.Close();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            load_datagrid(txtSearch.Text);
        }

        private void dt_grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dt_grid_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var data = new Model.DataCashback();
                DataGridViewRow row = this.dt_grid.Rows[e.RowIndex];
                data.id = row.Cells["Id Cashback"].Value.ToString();
                data.NamaCashback = row.Cells["Nama Cashback"].Value.ToString();
                string nom = row.Cells["Nominal"].Value.ToString().Replace("Rp", "").Replace(".", "").Replace(",", "");
                if (nom != "")
                {
                    data.Nominal = Convert.ToDecimal(row.Cells["Nominal"].Value.ToString());
                    if (data.Nominal != 0)
                    {
                        Form frm = Application.OpenForms["Main"];
                        if (frm != null)
                        {
                            Panel tbx = frm.Controls.Find("PagePanel", true).FirstOrDefault() as Panel;
                            UserControl fc = tbx.Controls.Find("Registrasi", true).FirstOrDefault() as UserControl;
                            if (fc != null)
                            {
                                Button btnCashback = fc.Controls.Find("btnCashback", true).FirstOrDefault() as Button;
                                if (btnCashback != null)
                                {
                                    btnCashback.Text = "Cashback : Rp " + string.Format("{0:n0}", data.Nominal);
                                    Model.RegisCashPayment.Cashback = data.Nominal;
                                }
                                this.Close();
                            }
                        }
                    }
                }
            }
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
                    Button btnCashback = fc.Controls.Find("btnCashback", true).FirstOrDefault() as Button;
                    if (btnCashback != null)
                    {
                        btnCashback.Text = "Give Cashback";
                    }
                    this.Close();
                }
            }
        }
    }
}
