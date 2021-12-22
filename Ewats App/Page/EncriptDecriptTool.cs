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
    public partial class EncriptDecriptTool : Form
    {
        public EncriptDecriptTool()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox3.Text = Encrypt.EncryptString(textBox1.Text, textBox2.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox4.Text = Encrypt.DecryptString(textBox6.Text, textBox5.Text);
        }
    }
}
