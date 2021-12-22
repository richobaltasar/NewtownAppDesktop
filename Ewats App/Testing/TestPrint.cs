using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;


namespace Ewats_App.Testing
{
    public partial class TestPrint : Form
    {
        public TestPrint()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            print();
        }

        private void TestPrint_Load(object sender, EventArgs e)
        {

        }
        public void print()
        {
            string now = DateTime.Now.ToString("dd/mm/yyyy - HH':'mm':'ss");
            string s = "Now\t\t: " + now + Environment.NewLine;
            s += "ID Transaction\t: 1000" + Environment.NewLine;
            s += "----------------------------------------------------------------------" + Environment.NewLine;
            for (int a = 0; a <= 5; a++)
            {

            }

            s += Environment.NewLine;
            s += "***************Terima kasih*****************" + Environment.NewLine;
            s += "Merchant ID :\t cho" + Environment.NewLine;
            s += "Nama Petugas:\t cho" + Environment.NewLine;
            s += "*******************************************" + Environment.NewLine;
            s += "----------------------------------------------------------------------" + Environment.NewLine;
            s += "Detail Transaksi Tiket" + Environment.NewLine;
            s += "1. Riko\t Tiket Rombongan\t 1\t 10.000,-" + Environment.NewLine;
            s += "1. Riko\t Tiket Rombongan\t 1\t 10.000,-" + Environment.NewLine;
            s += "Total : " + Environment.NewLine;

            s += "^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^" + Environment.NewLine;
            s += "Struk ini Untuk Penjual" + Environment.NewLine;
            PrintDocument p = new PrintDocument();
            p.PrintPage += delegate (object sender1, PrintPageEventArgs e1)
            {
                int HeadreX = 25;
                int startY = 0;
                int Offset = 10;
                e1.Graphics.DrawString("Welcome to Water Park", new Font("verdana", 7, FontStyle.Bold), new SolidBrush(Color.Black), HeadreX, startY + Offset);
                Offset = Offset + 15;
                e1.Graphics.DrawString("      Cimanggis Vilage", new Font("verdana", 7, FontStyle.Bold), new SolidBrush(Color.Black), HeadreX, startY + Offset);
                Offset = Offset + 15;
                string underLine = "======================================";
                e1.Graphics.DrawString(underLine, new Font("verdana", 5), new SolidBrush(Color.Black), 0, startY + Offset);
                Offset = Offset + 10;
                e1.Graphics.DrawString(s, new Font("calibri", 6), new SolidBrush(Color.Black), new RectangleF(0, startY + Offset, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));
            };
            try
            {
                p.Print();

            }
            catch (Exception ex)
            {
                throw new Exception("Exception Occured While Printing", ex);
            }
        }
    }
}
