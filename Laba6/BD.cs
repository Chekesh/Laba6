using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Laba6
{
    public class BD
    {
        static string connString = "Host=localhost;Username=postgres;Password=1234;Database=Laba6";

        NpgsqlConnection conn; //= new NpgsqlConnection(connString);

        string query;

        public BD()
        {
            try
            {
                conn = new NpgsqlConnection(connString);
                conn.Open();
                Console.WriteLine("Подключение к базе данных открыто");
            }
            catch (Exception ex)
            {
                Console.WriteLine("При подключение к базе данных ввозникла ошибка: {ex.Message}");
            }
            finally
            {
                Console.Read();
            }
        }

        public bool IsUserBD(string login, string password, string role)
        {
            query = "SELECT id FROM public.users WHERE login = @login AND pass_hash = @password AND role = @role::roles";

            using (var cmd = new NpgsqlCommand(query, conn))
            {
                try
                {
                    cmd.Parameters.AddWithValue("login", login.Trim());
                    cmd.Parameters.AddWithValue("password", password.Trim());
                    cmd.Parameters.AddWithValue("role", role.Trim());

                    var res = cmd.ExecuteScalar();
                    if (res != null)
                    {
                        // Если результат не null, то преобразуем его в long и записываем в переменную id
                        //id_user = Convert.ToInt32(res);
                        return true;
                    }
                    else
                    {
                        // Если пользователь не найден, возвращаем false
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public List<string> AllRoles()
        {
            query = "SELECT enumlabel FROM pg_enum WHERE enumtypid = 'roles'::regtype;";

            List<string> rol = new List<string>();

            using (var cmd = new NpgsqlCommand(query, conn))
            {
                try
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rol.Add(reader.GetString(0));
                        }
                    }
                    return rol;
                }
                catch (Exception ex)
                {
                    return rol;
                }
            }
        }
        public void AllType(string type, System.Windows.Forms.ComboBox box)
        {
            query = "SELECT enumlabel FROM pg_enum WHERE enumtypid = '"+type+"'::regtype;";

            List<string> rol = new List<string>();

            using (var cmd = new NpgsqlCommand(query, conn))
            {
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    rol.Add(reader.GetString(0));
                }
            }
            box.DataSource = rol;
            }
        }

        public bool AddCRM(string login,string pers_acc, string servis, string status, string type_problem, string description_problem)
        {
            query = "INSERT INTO \"CRM\"(date_creation, login_subscriber, pers_acc, service, status, type_problem, description_problem)" +
                " VALUES (@date, @login, @pers_acc, @servis::service, @status::status, @type_problem::type_problem, @description_problem)";

            using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("date", DateTime.Today);
                cmd.Parameters.AddWithValue("login", login.Trim());
                cmd.Parameters.AddWithValue("pers_acc", Convert.ToInt32(pers_acc.Trim()));
                cmd.Parameters.AddWithValue("servis", servis.Trim());
                cmd.Parameters.AddWithValue("status", status.Trim());
                cmd.Parameters.AddWithValue("type_problem", type_problem.Trim());
                cmd.Parameters.AddWithValue("description_problem", description_problem.Trim());

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public void SelectAbonents(DataGridView dt)
        {

            using (var cmd = new NpgsqlCommand("SELECT fio AS \"ФИО\", id_contract AS \"Номер договора\", pers_acc AS \"Лицевой счёт\", services AS \"Услуги\" FROM support_users", conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    dt.DataSource = dataTable;
                }
            }
        }

        public void SelectCRM(DataGridView dt)
        {

            using (var cmd = new NpgsqlCommand("SELECT id AS \"ID\", date_creation AS \"Дата создания\", login_subscriber AS \"Логин\", pers_acc AS \"Лицевой счет\", service AS \"услуга\", status AS \"Статус\", type_problem AS \"Тип проблемы\", description_problem AS \"Описание проблемы\", date_closing AS \"Дата закрытия\" FROM \"CRM\"", conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    dt.DataSource = dataTable;
                }
            }
        }

        internal void Ad_arch_Del_CRM(int id)
        {
            query = "SELECT Ad_arch_Del_CRM(@id, @date::DATE);";

            using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("date", DateTime.Today.Date);

                try
                {
                    // Выполняем запрос
                    cmd.ExecuteNonQuery();
                    //Console.WriteLine($"Строка с ID {id} успешно перенесена в архив.");
                }
                catch (NpgsqlException ex)
                {
                    MessageBox.Show("ID " + id +"  не найдена в бд");
                }
            }
        }
    }
}
