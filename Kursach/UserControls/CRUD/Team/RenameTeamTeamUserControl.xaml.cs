using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Логика взаимодействия для RenameTeamTeamUserControl.xaml
    /// </summary>
    public partial class RenameTeamTeamUserControl : UserControl
    {
        public int teamID = -1;
        public int themeID = -1;
        private int teacherID = -1;
        public string materialsLink = "";
        public string themeFormulation = "";
        public bool isChanged = false;


        public RenameTeamTeamUserControl()
        {
            InitializeComponent();
        }
        
        public RenameTeamTeamUserControl(int teamID, int themeID, int teacherID, string materialsLink, string themeFormulation)
        {
            InitializeComponent();
            this.teamID = teamID;
            this.themeID = themeID;
            this.teacherID = teacherID;
            this.materialsLink = materialsLink;
            this.themeFormulation = themeFormulation;
            SetUpInformation();
        }

        private void SetUpInformation()
        {
            MaterialsTextBox.Text = materialsLink;

            FillThemes();
        }

        private void FillThemes()
        {
            ThemeComboBox.Items.Clear();
            ComboBoxItem comboBoxItem = new ComboBoxItem()
            {
                Content = $"{themeFormulation}",
                Tag = $"{themeID}",
                IsSelected = true
            };
            ThemeComboBox.Items.Add(comboBoxItem);
            
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
                    comboBoxItem = new ComboBoxItem()
                    {
                        Content = $"{dr["ThemeFormulation"]}",
                        Tag = $"{dr["ID"]}"
                    };

                    ThemeComboBox.Items.Add(comboBoxItem);
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void UpdateDatabse()
        {
            try
            {
                using (var databaseConnection = new SqlConnection(Config.GetConnect()))
                {
                    databaseConnection.Open();
                    var req = $@"UPDATE Team SET ThemeID = {themeID}, MaterialsLink = '{materialsLink}' WHERE ID = {teamID}";

                    var cmd = new SqlCommand(req, databaseConnection);

                    var dr = cmd.ExecuteNonQuery();

                    isChanged = true;
                }
            }
            catch
            {
                isChanged = false;
                MessageBox.Show("Нет подключения к бд!", "Ошибка!", MessageBoxButton.OK);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (MaterialsTextBox.Text.Trim().Length == 0)
            {
                MessageBox.Show("Введите корректную ссылку!", "Ошибка!", MessageBoxButton.OK);
                return;
            }
            materialsLink = MaterialsTextBox.Text;
            themeFormulation = ((ComboBoxItem)ThemeComboBox.SelectedValue).Content.ToString();
            themeID = Int32.Parse(((ComboBoxItem)ThemeComboBox.SelectedValue).Tag.ToString());
            UpdateDatabse();
            this.Visibility = Visibility.Collapsed;
        }

        private void MaterialsTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex r = new Regex("[^a-zA-Z/.:1-9А-Яа-я]+");

            e.Handled = r.IsMatch(e.Text);
        }

        private void MaterialsTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
    }
}
