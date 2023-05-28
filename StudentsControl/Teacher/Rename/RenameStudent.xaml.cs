using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StudentsControl.Teacher.Rename
{
    /// <summary>
    /// Логика взаимодействия для RenameStudent.xaml
    /// </summary>
    public partial class RenameStudent : Window
    {
        private readonly Config config = new Config();
        private int studentID = -1;
        public bool isChanged = false;
        public string changedInitials = "";
        private string oldInitials = "";
        private bool makeRequest = true;

        public RenameStudent()
        {
            InitializeComponent();
        }

        public RenameStudent(string initials, bool makeRequest)
        {
            InitializeComponent();
            StudentInitials.Text = initials;
            oldInitials = initials;
            this.makeRequest = makeRequest;
        }

        public RenameStudent(string initials, int studentID)
        {
            InitializeComponent();
            this.studentID = studentID;
            StudentInitials.Text = initials;
            oldInitials = initials;
        }

        private void DatabaseRenameEntry()
        {
            try
            {
                using (var databaseConnection = new SqlConnection(config.connect))
                {
                    databaseConnection.Open();
                    var req = $@"update Student set Initials = '{StudentInitials.Text}' where ID = {studentID}";
                    var cmd = new SqlCommand(req, databaseConnection);
                    var dr = cmd.ExecuteNonQuery();
                    isChanged = true;
                    changedInitials = StudentInitials.Text;
                    MessageBox.Show($"Запись о {oldInitials} успешно изменена!", "Успешно", MessageBoxButton.OK);
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
            if (StudentInitials.Text.Split().Length < 2)
            {
                MessageBox.Show("Введите корректные инициалы студента!", "Ошибка!", MessageBoxButton.OK);
                return;
            }

            if (!makeRequest)
            {
                MessageBox.Show($"Запись о {oldInitials} успешно изменена!", "Успешно", MessageBoxButton.OK);
                isChanged = true;
                changedInitials = StudentInitials.Text;
                Close();
                return;
            }

            DatabaseRenameEntry();
            if (isChanged) Close();
        }


        #region Обработчик текста и пробела
        private void StudentInitials_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex r = new Regex("[^А-Яа-я]+");
            
            e.Handled = r.IsMatch(e.Text);
        }

        private void StudentInitials_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                if (StudentInitials.Text.Length == 0 || (StudentInitials.Text.Length != 0 && StudentInitials.Text.Trim()[StudentInitials.Text.Length - 1] == ' '))
                {
                    e.Handled = true;
                }
            }

            
        }
        #endregion
    }
}
