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

namespace StudentsControl.Teacher.Rename
{
    /// <summary>
    /// Логика взаимодействия для RenameTheme.xaml
    /// </summary>
    public partial class RenameTheme : Window
    {
        private readonly Config config = new Config();
        private int themeID = -1;
        public bool isChanged = false;
        public string changedThemeFormulation = "";
        private string oldThemeFormulation = "";

        public RenameTheme()
        {
            InitializeComponent();
        }

        public RenameTheme(string themeFormulation, int themeID)
        {
            InitializeComponent();
            this.themeID = themeID;
            ThemeFormulation.Text = themeFormulation;
            oldThemeFormulation = themeFormulation;
        }

        private void DatabaseRenameEntry()
        {
            try
            {
                using (var databaseConnection = new SqlConnection(config.connect))
                {
                    databaseConnection.Open();
                    var req = $@"update Theme set ThemeFormulation = '{ThemeFormulation.Text}' where ID = {themeID}";
                    var cmd = new SqlCommand(req, databaseConnection);
                    var dr = cmd.ExecuteNonQuery();
                    isChanged = true;
                    changedThemeFormulation = ThemeFormulation.Text;
                    MessageBox.Show($"Запись о {oldThemeFormulation} успешно изменена!", "Успешно", MessageBoxButton.OK);
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
            if (ThemeFormulation.Text.Trim().Split().Length == 0 || ThemeFormulation.Text == "")
            {
                MessageBox.Show("Введите корректную формулировку темы!", "Ошибка!", MessageBoxButton.OK);
                return;
            }

            DatabaseRenameEntry();
            if (isChanged) Close();
        }


        #region Обработчик текста и пробела
        private void WorkerInitials_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex r = new Regex("[^0-9А-Яа-яa-zA-Z]+");

            e.Handled = r.IsMatch(e.Text);
        }

        private void WorkerInitials_PreviewKeyDown(object sender, KeyEventArgs e)
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
    }
}
