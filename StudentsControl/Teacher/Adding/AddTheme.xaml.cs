using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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

namespace StudentsControl.Teacher.Adding
{
    /// <summary>
    /// Логика взаимодействия для AddTheme.xaml
    /// </summary>
    public partial class AddTheme : Window
    {
        private readonly Config config = new Config();
        private int teacherID = -1;
        public bool isAdded = false;
        public int primaryKey = -1;
        public string themeFormulation = "";

        public AddTheme()
        {
            InitializeComponent();
        }
        
        public AddTheme(int teacherID)
        {
            InitializeComponent();
            this.teacherID = teacherID;
        }

        #region Обработка ввода

        private void ThemeFormulation_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex r = new Regex("[^А-Яа-яA-Za-z0-9]+");

            e.Handled = r.IsMatch(e.Text);
        }

        private void ThemeFormulation_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                if (ThemeFormulation.Text.Length == 0 || (ThemeFormulation.Text.Length != 0 && ThemeFormulation.Text[ThemeFormulation.Text.Length - 1] == ' '))
                {
                    e.Handled = true;
                }
            }
        }
        #endregion

        private void DatabaseAddEntry(string themeFormulation)
        {
            try
            {
                using (var databaseConnection = new SqlConnection(config.connect))
                {
                    databaseConnection.Open();
                    var req = $@"INSERT INTO Theme (ThemeFormulation, TeacherID)
                                  VALUES ('{themeFormulation}', {teacherID}); SELECT SCOPE_IDENTITY()";
                    var cmd = new SqlCommand(req, databaseConnection);
                    var dr = cmd.ExecuteScalar();
                    
                    primaryKey = Int32.Parse(dr.ToString());
                    this.themeFormulation = themeFormulation;
                    isAdded = true;
                }
            }
            catch
            {
                isAdded = false;
                primaryKey = -1;
                themeFormulation = "";
                MessageBox.Show("Нет подключения к бд!", "Ошибка!", MessageBoxButton.OK);   
                return;
            }
        }

        private void AddEntry_Click(object sender, RoutedEventArgs e)
        {
            if (ThemeFormulation.Text.Trim().Length == 0)
            {
                MessageBox.Show("Введите корректную формулировку темы!", "Ошибка!", MessageBoxButton.OK);
                return;
            }
            DatabaseAddEntry(ThemeFormulation.Text.Trim());
            Close();
            if (isAdded) MessageBox.Show($"Тема: {themeFormulation} успешно добавлена!", "Успех!", MessageBoxButton.OK);
        }
    }
}
