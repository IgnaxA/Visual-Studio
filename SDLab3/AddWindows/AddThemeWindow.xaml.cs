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

namespace SDLab3.AddWindows
{
    /// <summary>
    /// Логика взаимодействия для AddThemeWindow.xaml
    /// </summary>
    public partial class AddThemeWindow : Window
    {
        private string connect;
        private int teacherID;
        public int primaryKey = -1;

        public AddThemeWindow()
        {
            InitializeComponent();
        }
        
        public AddThemeWindow(string connect, int teacherID)
        {
            InitializeComponent();
            this.connect = connect;
            this.teacherID = teacherID;
        }

        private void DatabaseAddEntry()
        {
            try
            {
                using (var databaseConnection = new SqlConnection(connect))
                {
                    databaseConnection.Open();
                    var req = $@"INSERT INTO Theme (ThemeFormulation, TeacherID)
                                  VALUES (@themeFormulation, @teacherID); SELECT SCOPE_IDENTITY()";
                    var cmd = new SqlCommand(req, databaseConnection);
                    cmd.Parameters.AddWithValue("@themeFormulation", ThemeFormulation.Text);
                    cmd.Parameters.AddWithValue("@teacherID", teacherID);
                    var dr = cmd.ExecuteScalar();
                    primaryKey = Int32.Parse(dr.ToString());
                    MessageBox.Show($"Запись о теме: '{ThemeFormulation.Text}' успешно добавлена!", "Успешно", MessageBoxButton.OK);
                }
            }
            catch
            {
                primaryKey = -1;
                MessageBox.Show("Нет подключения к бд!", "Ошибка!", MessageBoxButton.OK);
            }
        }

        private void AddTheme_Click(object sender, RoutedEventArgs e)
        {
            if (ThemeFormulation.Text == null || ThemeFormulation.Text == "")
            {
                MessageBox.Show("Введите непустое значение для поля!", "Ошибка!", MessageBoxButton.OK);
                return;
            }
            DatabaseAddEntry();
            this.Close();
        }
    }
}
