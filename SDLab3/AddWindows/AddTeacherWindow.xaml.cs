using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace SDLab3.AddWindows
{
    /// <summary>
    /// Логика взаимодействия для AddTeacherWindow.xaml
    /// </summary>
    public partial class AddTeacherWindow : Window
    {
        private string connect;
        private int studyOfficeID;
        public int primaryKey = -1;

        public AddTeacherWindow()
        {

        }

        public AddTeacherWindow(string connect, int studyOfficeID)
        {
            InitializeComponent();
            this.connect = connect;
            this.studyOfficeID = studyOfficeID;
        }

        private void DatabaseAddEntry()
        {
            try
            {
                using (var databaseConnection = new SqlConnection(connect))
                {
                    databaseConnection.Open();
                    var req = $@"INSERT INTO Teacher (TeacherInitials, StudyOfficeID)
                                  VALUES (@teacherInitials, @studyOfficeID); SELECT SCOPE_IDENTITY()";
                    var cmd = new SqlCommand(req, databaseConnection);
                    cmd.Parameters.AddWithValue("@teacherInitials", WorkerInitials.Text);
                    cmd.Parameters.AddWithValue("@studyOfficeID", studyOfficeID);
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

        private void AddTeacher_Click(object sender, RoutedEventArgs e)
        {
            if (WorkerInitials.Text == null || WorkerInitials.Text == "")
            {
                MessageBox.Show("Введите непустое значение для поля!", "Ошибка!", MessageBoxButton.OK);
                return;
            }
            DatabaseAddEntry();
            this.Close();
        }
    }
}
