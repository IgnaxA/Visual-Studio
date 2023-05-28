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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kursach.UserControls.CRUD.Theme
{
    /// <summary>
    /// Логика взаимодействия для AddThemeUserControl.xaml
    /// </summary>
    public partial class AddThemeUserControl : UserControl
    {
        private int teacherID = -1;
        public string themeFormulation = "";
        public int themeID = -1;
        public bool isAdded = false;
        private int changeThemeID = -1;

        public AddThemeUserControl()
        {
            InitializeComponent();
        }
        
        public AddThemeUserControl(int teacherID, string buttonName, int changeThemeID = -1, string oldThemeFormulation = "")
        {
            InitializeComponent();
            this.teacherID = teacherID;
            this.changeThemeID = changeThemeID;
            EndButton.Content = buttonName;
            if (oldThemeFormulation != "") ThemeFormulationTextBox.Text = oldThemeFormulation;
        }

        private int DatabaseAdd()
        {
            int primaryKey;
            try
            {
                using (var databaseConnection = new SqlConnection(Config.GetConnect()))
                {
                    databaseConnection.Open();
                    var req = $@"INSERT INTO Theme (TeacherID, ThemeFormulation)
                                  VALUES ({teacherID}, '{themeFormulation}'); SELECT SCOPE_IDENTITY()";
                    var cmd = new SqlCommand(req, databaseConnection);
                    var dr = cmd.ExecuteScalar();
                    primaryKey = Int32.Parse(dr.ToString());
                }
                isAdded = true;
            }
            catch
            {
                MessageBox.Show("Нет подключения к бд!", "Ошибка!", MessageBoxButton.OK);
                isAdded = false;
                return -1;
            }
            return primaryKey;
        }

        private void DatabaseChange()
        {
            try
            {
                using (var databaseConnection = new SqlConnection(Config.GetConnect()))
                {
                    databaseConnection.Open();
                    var req = $@"UPDATE Theme SET ThemeFormulation = '{themeFormulation}' WHERE ID = {changeThemeID}";
                    var cmd = new SqlCommand(req, databaseConnection);
                    var dr = cmd.ExecuteNonQuery();
                }
                isAdded = true;
            }
            catch
            {
                MessageBox.Show("Нет подключения к бд!", "Ошибка!", MessageBoxButton.OK);
                isAdded = false;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ThemeFormulationTextBox.Text.Trim().Length == 0)
            {
                MessageBox.Show("Введите корректную формулировку темы!", "Ошибка!", MessageBoxButton.OK);
                return;
            }
            themeFormulation = ThemeFormulationTextBox.Text.Trim();
            if (EndButton.Content.ToString() == "Сохранить")
            {
                themeID = DatabaseAdd();
            }
            if (EndButton.Content.ToString() == "Изменить")
            {
                DatabaseChange();
            }
            
            this.Visibility = Visibility.Collapsed;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void ThemeFormulationTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex r = new Regex("[^А-Яа-яA-Za-z0-9.]+");
            e.Handled = r.IsMatch(e.Text);
        }

        private void ThemeFormulationTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                if (ThemeFormulationTextBox.Text.Length == 0 || (ThemeFormulationTextBox.Text.Length != 0 && ThemeFormulationTextBox.Text[ThemeFormulationTextBox.Text.Length - 1] == ' '))
                {
                    e.Handled = true;
                }
            }
        }
    }
}
