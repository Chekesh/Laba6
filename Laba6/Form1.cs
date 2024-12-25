using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laba6
{
    public partial class Form1 : Form
    {
        BD connection = new BD();
        public Form1()
        {
            InitializeComponent();
            connection.AllType("roles", comboBox1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (connection.IsUserBD(textBox1.Text, hashPass(textBox2.Text), comboBox1.Text))
            {
                Form2 form2 = new Form2(this, connection);
                form2.Show();
            }
            else
            {
                MessageBox.Show("Данные не существуют");
            }

        }
        private string hashPass(string pass)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Преобразуем строку пароля в массив байтов
                byte[] passwordBytes = Encoding.UTF8.GetBytes(pass);

                // Вычисляем хеш
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);


                // Преобразуем хеш в строку Base64
                return Convert.ToBase64String(hashBytes);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                textBox2.UseSystemPasswordChar = false;
            }
            else 
            {
                textBox2.UseSystemPasswordChar = true;
            }
        }
    }
}
