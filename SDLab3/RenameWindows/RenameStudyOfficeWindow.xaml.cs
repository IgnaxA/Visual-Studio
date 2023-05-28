using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
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

namespace SDLab3.RenameWindows
{
    /// <summary>
    /// Логика взаимодействия для RenameStudyOfficeWindow.xaml
    /// </summary>
    public partial class RenameStudyOfficeWindow : Window
    {
        private string connect;
        private int id;
        public bool isChanged = false;

        public RenameStudyOfficeWindow()
        {
            InitializeComponent();
        }

        public RenameStudyOfficeWindow(string connect, int id, string WorkerInitials, string Email)
        {
            InitializeComponent();
            this.connect = connect;
            this.id = id;
            this.WorkerInitials.Text = WorkerInitials;
            this.Email.Text = Email;
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

        private void DatabaseRenameEntry()
        {
            try
            {
                using (var databaseConnection = new SqlConnection(connect))
                {
                    databaseConnection.Open();
                    var req = $@"update StudyOffice set WorkerInitials = @studyOfficeWorkerInitials, Email = @studyOfficeWorkerEmail where ID = {id}";
                    var cmd = new SqlCommand(req, databaseConnection);
                    cmd.Parameters.AddWithValue("@studyOfficeWorkerInitials", WorkerInitials.Text);
                    cmd.Parameters.AddWithValue("@studyOfficeWorkerEmail", Email.Text);
                    var dr = cmd.ExecuteNonQuery();
                    isChanged = true;
                    MessageBox.Show($"Запись о {WorkerInitials.Text} успешно изменена!", "Успешно", MessageBoxButton.OK);
                }
            }
            catch
            {
                isChanged = false;
                MessageBox.Show("Нет подключения к бд!", "Ошибка!", MessageBoxButton.OK);
            }
        }



        private void RenameEntry_Click(object sender, RoutedEventArgs e)
        {
            if (WorkerInitials.Text == "" || WorkerInitials.Text == null || Email.Text == "" || Email.Text == null)
            {
                MessageBox.Show("Введите непустые значения для полей!", "Ошибка!", MessageBoxButton.OK);
                return;
            }
            bool isAdressOk = CheckEmailAdress();

            if (!isAdressOk)
            {
                MessageBox.Show("Введите корректный email адрес!");
                return;
            }

            if (WorkerInitials.Text.Split().Length < 2)
            {
                MessageBox.Show("Введите корректные инициалы сотрудника!");
                return;
            }
            DatabaseRenameEntry();
            this.Close();
        }
    }
}
