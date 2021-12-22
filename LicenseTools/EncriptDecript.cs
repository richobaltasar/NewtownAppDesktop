using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

namespace LicenseTools
{
    public partial class EncriptDecript : Form
    {
        public EncriptDecript()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox7.Text != "" && textBox2.Text != "")
            {
                string data = textBox1.Text + textBox7.Text.Replace("/", "");
                textBox3.Text = Encrypt.EncryptString(data, textBox2.Text);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox4.Text = Encrypt.DecryptString(textBox6.Text, textBox5.Text);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void EncriptDecript_Load(object sender, EventArgs e)
        {
            monthCalendar1.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            monthCalendar1.Visible = true;
            monthCalendar1.BringToFront();
        }

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            monthCalendar1.Visible = false;
            textBox7.Text = monthCalendar1.SelectionRange.Start.ToString("yyyy/MM/dd");
        }
    }

    public static class Encrypt
    {
        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        private const string initVector = "pemgail9uzpgzl88";
        // This constant is used to determine the keysize of the encryption algorithm
        private const int keysize = 256;
        //Encrypt
        public static string EncryptString(string plainText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(cipherTextBytes);
        }
        //Decrypt
        public static string DecryptString(string cipherText, string passPhrase)
        {
            string res = "";
            try
            {
                byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
                byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
                PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
                byte[] keyBytes = password.GetBytes(keysize / 8);
                RijndaelManaged symmetricKey = new RijndaelManaged();
                symmetricKey.Mode = CipherMode.CBC;
                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
                MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
                CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                memoryStream.Close();
                cryptoStream.Close();
                res = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            }
            catch (Exception ex)
            {
                res = "Error";
            }
            return res;
        }
    }
}
