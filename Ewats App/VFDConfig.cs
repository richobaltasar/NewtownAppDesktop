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
using System.IO.Ports;
using System.Threading;
using Microsoft.PointOfService;

namespace Ewats_App
{
    public partial class VFDConfig : Form
    {
        PosExplorer lineDisplayDevice = new PosExplorer();
        GlobalFunc f = new GlobalFunc();
        SerialPort sp = new SerialPort();
        public VFDConfig()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (VFDPort.sp.IsOpen)
            {
                sp.Close();
                sp.Dispose();
                sp = null;
            }
            this.Hide();
            f.PageControl("EwatsConfig");
        }

        private void VFDConfig_Load(object sender, EventArgs e)
        {
            var serialList = SerialPort.GetPortNames();
            foreach (string l in serialList)
            {
                cmbPort.Items.Add(l);
            }
        }
        public void send(string Line1, string Line2,string Port)
        {
            string title = Line1;
            
            byte[] bytesToSend = new byte[1] { 0x0C };
            sp.Write(bytesToSend, 0, 1);
            int d = title.Length;
            if (title.Length < 20)
            {
                int sisa = 20 - d;
                for (int a = 0; a < sisa; a++)
                {
                    title = title + " ";
                }
            }
            byte[] asciiBytes = Encoding.ASCII.GetBytes(title);
            sp.Write(asciiBytes, 0, asciiBytes.Length);
            byte[] enter = new byte[2] { 0x1F, 0x42 };
            sp.WriteLine(Line2);
            sp.Write(enter, 0, enter.Length);
        }

        private void cmbPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbPort.SelectedItem.ToString() != "")
                {
                    
                    if (VFDPort.HasOpenPort(cmbPort.SelectedItem.ToString()) == false)
                    {
                        VFDPort.sp.PortName = cmbPort.SelectedItem.ToString();
                        VFDPort.sp.BaudRate = 9600;
                        VFDPort.sp.Parity = Parity.None;
                        VFDPort.sp.DataBits = 8;
                        VFDPort.sp.StopBits = StopBits.One;
                        VFDPort.sp.Open();
                        VFDPort.send("Koneksi VFD Sukses", "", cmbPort.SelectedItem.ToString());
                        Thread.Sleep(1000);
                        VFDPort.send("Selamat Datang", "Kumpay Waterpark", cmbPort.SelectedItem.ToString());
                    }
                    
                }
                else
                {
                    string message = "Port Serial tidak ditemukan";
                    string title = "Warning";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result = MessageBox.Show(message, title, buttons);
                }
            }
            catch (Exception ex)
            {
                string message = "Error :"+ex.Message;
                string title = "Exception ERROR";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, title, buttons);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (cmbPort.SelectedItem != null)
            {
                if (cmbPort.SelectedItem.ToString() != "")
                {
                    VFDPort.send(TxtDBName.Text, "", cmbPort.SelectedItem.ToString());
                }
                else
                {
                    VFDPort.send(TxtDBName.Text, "", VFDPort.sp.PortName);
                }
            }
            else
            {
                f.ShowMessagebox("Silahkan Pilih Port Serial yang terkonek ke VFD", "", MessageBoxButtons.OK);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (cmbPort.SelectedIndex.ToString() != "")
            {
                if (VFDPort.HasOpenPort(VFDPort.sp.PortName) == true)
                {
                    sp.Close();
                    sp.Dispose();
                    sp = null;
                }
            }
            
            
            Form frm = Application.OpenForms["EwatsConfig"];
            if (frm != null)
            {
                TextBox VFDPort = frm.Controls.Find("txtVFDPort", true).FirstOrDefault() as TextBox;

                if (VFDPort != null)
                {
                    if (cmbPort.SelectedItem != null && cmbPort.SelectedItem.ToString() != "")
                    {
                        VFDPort.Text = cmbPort.SelectedItem.ToString();
                    }
                    else
                    {
                        VFDPort.Text = "";
                    }
                }
                this.Close();
            }
            else
            {
                this.Hide();
                f.PageControl("InitPage");
            }
        }
    }
}
