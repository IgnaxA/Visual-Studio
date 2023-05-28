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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kursach.UserControls.CRUD.Team
{
    /// <summary>
    /// Логика взаимодействия для AddTeamUserControl.xaml
    /// </summary>
    public partial class AddTeamUserControl : UserControl
    {
        public bool isAdded = false;
        private int teacherID = -1;
        public int themeID = -1;
        public string themeFormulation = "";
        public string materialsLink = "";
        public int teamID = -1;


        public AddTeamUserControl(int teacherID)
        {
            InitializeComponent();
            this.teacherID = teacherID;
            SetUpInformation();
        }

        private void SetUpInformation()
        {
            FillThemes();
        }

        private void FillThemes()
        {
            try
            {
                ThemeComboBox.Items.Clear();

                using (var databaseConnection = new SqlConnection(Config.GetConnect()))
                {
                    databaseConnection.Open();
                    var req = $@"SELECT Th.ThemeFormulation, Th.ID
                             FROM Theme Th 
                             WHERE NOT EXISTS (SELECT Te.ThemeID FROM Team Te WHERE Te.ThemeID = Th.ID) 
                             AND Th.TeacherID = {teacherID}";
                    var cmd = new SqlCommand(req, databaseConnection);

                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        ComboBoxItem comboBoxItem = new ComboBoxItem()
                        {
                            Content = $"{dr["ThemeFormulation"]}",
                            Tag = $"{dr["ID"]}"
                        };

                        ThemeComboBox.Items.Add(comboBoxItem);
                    }
                }
            }
            catch
            {
                isAdded = false;
                MessageBox.Show("Нет подключения к бд!", "Ошибка!", MessageBoxButton.OK);
            }
        }

        private void MaterialsTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                if (MaterialsTextBox.Text.Length == 0 || (MaterialsTextBox.Text.Length != 0 && MaterialsTextBox.Text[MaterialsTextBox.Text.Length - 1] == ' '))
                {
                    e.Handled = true;
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private int DatabaseAdd()
        {
            int primaryKey;
            try
            {
                using (var databaseConnection = new SqlConnection(Config.GetConnect()))
                {
                    databaseConnection.Open();
                    var req = $@"INSERT INTO Team (ThemeID, MaterialsLink)
                                  VALUES ({themeID}, '{materialsLink}'); SELECT SCOPE_IDENTITY()";
                    var cmd = new SqlCommand(req, databaseConnection);
                    var dr = cmd.ExecuteScalar();
                    primaryKey = Int32.Parse(dr.ToString());
                }
                isAdded= true;
            }
            catch
            {
                MessageBox.Show("Нет подключения к бд!", "Ошибка!", MessageBoxButton.OK);
                isAdded = false;
                return -1;
            }
            return primaryKey;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ThemeComboBox.SelectedValue == null)
            {
                MessageBox.Show("Заполните все поля!", "Ошибка!", MessageBoxButton.OK);
                return;
            }
            themeID = Int32.Parse(((ComboBoxItem)ThemeComboBox.SelectedValue).Tag.ToString());
            themeFormulation = ((ComboBoxItem)ThemeComboBox.SelectedValue).Content.ToString();
            materialsLink = MaterialsTextBox.Text;
            teamID = DatabaseAdd();
            Visibility = Visibility.Collapsed;
        }

    }
}
