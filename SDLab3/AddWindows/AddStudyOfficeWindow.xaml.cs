using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SDLab3.AddWindows
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class AddStudyOfficeWindow : Window
    {
        private string connect;
        public int primaryKey = -1;

        public AddStudyOfficeWindow()
        {

        }

        public AddStudyOfficeWindow(string connect)
        {
            InitializeComponent();
            this.connect = connect;
        }

        private bool CheckEmailAdress()
        {
            try
            {
                MailAddress tempAdress = new MailAddress(Email.Text);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void DatabaseAddEntry()
        {
            try
            {
                using (var databaseConnection = new SqlConnection(connect))
                {
                    databaseConnection.Open();
                    var req = $@"INSERT INTO StudyOffice (WorkerInitials, Email)
                                  VALUES (@studyOfficeWorkerInitials, @studyOfficeWorkerEmail); SELECT SCOPE_IDENTITY()";
                    var cmd = new SqlCommand(req, databaseConnection);
                    cmd.Parameters.AddWithValue("@studyOfficeWorkerInitials", WorkerInitials.Text);
                    cmd.Parameters.AddWithValue("@studyOfficeWorkerEmail", Email.Text);
                    var dr = cmd.ExecuteScalar();
                    primaryKey = Int32.Parse(dr.ToString());
                    MessageBox.Show($"Запись о {WorkerInitials.Text} успешно добавлена!", "Успешно", MessageBoxButton.OK);
                }
            }
            catch
            {
                primaryKey = -1;
                MessageBox.Show("Нет подключения к бд!", "Ошибка!", MessageBoxButton.OK);
            }
        }

        private void AddStudyOffice_Click(object sender, RoutedEventArgs e)
        {
            
            if (WorkerInitials.Text == "" || WorkerInitials.Text == null || Email.Text == "" || Email.Text == null)
            {
                MessageBox.Show("Введите непустые значения для полей!", "Ошибка!", MessageBoxButton.OK);
                return;
            }

			if (WorkerInitials.Text.Trim().Split().Length < 2)
			{
				MessageBox.Show("Введите корректные инициалы сотрудника!");
				return;
			}

			bool isAdressOk = CheckEmailAdress();

            if (!isAdressOk)
            {
                MessageBox.Show("Введите корректный email адрес!");
                return;
            }
            DatabaseAddEntry();
            this.Close();
        }
    }
}
