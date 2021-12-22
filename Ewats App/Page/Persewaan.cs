using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ewats_App.Model;
using Ewats_App.Function;

namespace Ewats_App.Page
{
    public partial class Persewaan : Form
    {
        GlobalFunc f = new GlobalFunc();

        public Persewaan()
        {
            InitializeComponent();
        }

        private void Persewaan_Load(object sender, EventArgs e)
        {
            LoadComboTenant();
        }

        public void LoadComboTenant()
        {
            //cbTenant.Items.Clear();
            //var data = f.GetTenant();
            //foreach (var d in data)
            //{
            //    ComboboxItem item = new ComboboxItem();
            //    item.Text = d.NamaTenant;
            //    item.Value = d.Id;
            //    cbTenant.Items.Add(item);
            //}
            GetMenu("11");
        }

        private void cbTenant_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            ComboboxItem d = (ComboboxItem)cmb.SelectedItem;
            GetMenu(d.Value.ToString());

        }

        public void GetMenu(string Tenant)
        {
            GetMenu:
            var dataMenu = f.GetBarang(Tenant);
            ImageList il = new ImageList();
            int count = 0;
            ListMenu.Clear();
            foreach (var img in dataMenu)
            {
                try
                {
                    System.Net.WebRequest request = System.Net.WebRequest.Create(img.LinkPic);
                    System.Net.WebResponse resp = request.GetResponse();
                    System.IO.Stream respStream = resp.GetResponseStream();
                    Bitmap bmp = new Bitmap(respStream);
                    respStream.Dispose();

                    il.ImageSize = new Size(80, 80);
                    il.Images.Add(bmp);
                }
                catch (Exception ex)
                {
                    var res3 = MessageBox.Show("GetMenu Gagal : "+ex.Message, "Printing Fail", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                    if (res3 == DialogResult.Retry)
                    {
                        goto GetMenu;
                    }
                }
            }
            ListMenu.LargeImageList = il;
            foreach (var data in dataMenu)
            {
                ListViewItem lst = new ListViewItem();
                lst.Text = data.NamaBarang;
                lst.Name = data.IdMenu + "~" + data.NamaBarang + "~" + data.Harga + "~" + data.Stok;
                lst.ImageIndex = count++;
                ListMenu.Items.Add(lst);
            }

        }

        private void ListMenu_Click(object sender, EventArgs e)
        {
            var data = ListMenu.SelectedItems[0];
            if (data.Name != "")
            {
                OrderSewa frm = new OrderSewa();
                frm.Show();
                frm.BringToFront();
                frm.StartPosition = FormStartPosition.CenterScreen;
                Label lblKodeBarang = frm.Controls.Find("lblKodeBarang", true).FirstOrDefault() as Label;
                Label lblNamaProduk = frm.Controls.Find("lblNamaProduk", true).FirstOrDefault() as Label;
                Label lblHarga = frm.Controls.Find("lblHarga", true).FirstOrDefault() as Label;
                Label lblSisa = frm.Controls.Find("lblSisa", true).FirstOrDefault() as Label;

                if (lblKodeBarang != null)
                {
                    lblNamaProduk.Text = data.Text;
                    var param = data.Name.Split('~');
                    lblKodeBarang.Text = param[0];
                    lblNamaProduk.Text = param[1];
                    lblHarga.Text = f.ConvertToRupiah(f.ConvertDecimal(param[2]));
                    lblSisa.Text = param[3];
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
