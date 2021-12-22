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
    public partial class MasterBank : Form
    {
        GlobalFunc f = new GlobalFunc();

        public MasterBank()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MasterBank_Load(object sender, EventArgs e)
        {
            load_datagrid("");
        }

        public void atur_grid()
        {
            dt_grid.Rows.Clear();
            dt_grid.ColumnCount = 5;
            dt_grid.Columns[0].Name = "Id Log";
            dt_grid.Columns[1].Name = "Kode Bank";
            dt_grid.Columns[2].Name = "Nama Bank";
            dt_grid.Columns[3].Name = "Diskon Bank";
            dt_grid.Columns[4].Name = "Admin Charges";

            dt_grid.RowHeadersVisible = false;
            dt_grid.ColumnHeadersVisible = true;
            DataGridViewColumn column1 = dt_grid.Columns[0];
            DataGridViewColumn column2 = dt_grid.Columns[1];
            DataGridViewColumn column3 = dt_grid.Columns[2];
            DataGridViewColumn column4 = dt_grid.Columns[3];
            DataGridViewColumn column5 = dt_grid.Columns[4];

            // Initialize basic DataGridView properties.
            dt_grid.Dock = DockStyle.None;            
            dt_grid.BorderStyle = BorderStyle.None;
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
            var data = f.GetDataBank(search);
            int a = 0;
            foreach (var r in data)
            {
                a++;
                string[] row = new string[] { r.idLog, r.KodeBank,r.NamaBank,r.DiskonBank,r.AdminCharges};
                this.dt_grid.Rows.Add(row);
                DataGridViewRow d = dt_grid.Rows[a - 1];
                d.MinimumHeight = 40;
            }
        }

        private void dt_grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var data = new Model.DataBank();
                DataGridViewRow row = this.dt_grid.Rows[e.RowIndex];

                data.idLog = row.Cells["Id Log"].Value.ToString();
                data.KodeBank = row.Cells["Kode Bank"].Value.ToString();
                data.NamaBank = row.Cells["Nama Bank"].Value.ToString();
                data.DiskonBank = row.Cells["Diskon Bank"].Value.ToString();
                data.AdminCharges = row.Cells["Admin Charges"].Value.ToString();                

                if (data != null)
                {
                    Form frm = Application.OpenForms[LblPage.Text];
                    if (frm != null)
                    {
                        if (LblPage.Text == "DebitCard")
                        {
                            TextBox txtTotalBelanja = frm.Controls.Find("txtTotalBelanja", true).FirstOrDefault() as TextBox;
                            TextBox txtBankCode = frm.Controls.Find("txtBankCode", true).FirstOrDefault() as TextBox;
                            TextBox txtNamaBank = frm.Controls.Find("txtNamaBank", true).FirstOrDefault() as TextBox;
                            TextBox txtDiskon = frm.Controls.Find("txtDiskon", true).FirstOrDefault() as TextBox;
                            TextBox TxtNominalDiskon = frm.Controls.Find("TxtNominalDiskon", true).FirstOrDefault() as TextBox;
                            TextBox txtAdminCharges = frm.Controls.Find("txtAdminCharges", true).FirstOrDefault() as TextBox;
                            TextBox txtTotalDebit = frm.Controls.Find("txtTotalDebit", true).FirstOrDefault() as TextBox;
                            decimal TotalBelanja = 0;
                            decimal DiskonBankNominal = 0;
                            decimal AdminCharges = 0;
                            if (txtTotalBelanja != null)
                            {
                                TotalBelanja = f.ConvertDecimal(txtTotalBelanja.Text);
                            }
                            if (txtBankCode != null)
                            {
                                txtBankCode.Text = data.KodeBank;
                            }
                            if (txtNamaBank != null)
                            {
                                txtNamaBank.Text = data.NamaBank;
                            }
                            if (txtDiskon != null)
                            {
                                txtDiskon.Text = data.DiskonBank;
                            }
                            if (TxtNominalDiskon != null)
                            {
                                TxtNominalDiskon.Text = f.ConvertToRupiah(TotalBelanja * f.ConvertDecimal(txtDiskon.Text)/100);
                                DiskonBankNominal = TotalBelanja * f.ConvertDecimal(txtDiskon.Text)/100;
                            }
                            if (txtAdminCharges != null)
                            {
                                txtAdminCharges.Text = f.ConvertToRupiah(f.ConvertDecimal(data.AdminCharges));
                                AdminCharges = f.ConvertDecimal(data.AdminCharges);
                            }
                            if (txtTotalDebit != null)
                            {
                                txtTotalDebit.Text = f.ConvertToRupiah((f.ConvertDecimal(txtTotalBelanja.Text) - DiskonBankNominal) + AdminCharges);
                                txtTotalDebit.Focus();
                            }
                            this.Close();
                        }
                    }
                }
            }
        }
    }
}
