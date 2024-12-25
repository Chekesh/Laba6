using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laba6
{
    public partial class Form2 : Form
    {
        BD connection;
        public Form2(Form1 form, BD connection)
        {
            InitializeComponent();
            form.Hide();
            this.connection = connection;
            //connection.SelectAbonents()\
            connection.SelectAbonents(dataGridView2);
            connection.SelectCRM(dataGridView1);
            connection.AllType("service", comboBox1);
            connection.AllType("status", comboBox2);
            connection.AllType("type_problem", comboBox3);
        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                connection.AddCRM(textBox1.Text, textBox2.Text, comboBox1.Text, comboBox2.Text, comboBox3.Text, textBox6.Text);
            }
            catch {
                MessageBox.Show("Обнаруденны недопустипые значения");
            }
            connection.SelectCRM(dataGridView1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int id = 0;
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                // Получаем данные из ячеек выбранной строки
                id = Convert.ToInt32(row.Cells["id"].Value); // Получить значение из столбца "id"

                // Выводим данные в консоль для проверки
                Console.WriteLine($"ID: {id}");
            }

            connection.Ad_arch_Del_CRM(id);
            connection.SelectCRM(dataGridView1);
        }
    }
}
