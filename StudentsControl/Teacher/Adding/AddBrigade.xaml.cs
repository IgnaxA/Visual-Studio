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

namespace StudentsControl.Teacher.Adding
{
    /// <summary>
    /// Логика взаимодействия для AddBrigade.xaml
    /// </summary>
    public partial class AddBrigade : Window
    {
        private readonly Config config = new Config();
        private int teacherID = -1;
        public int primaryKey = -1;
        public string themeFormulation = "";
        private bool isOk = true;
        public bool isAdded = false;
        public int themePrimaryKey = -1;

        public AddBrigade()
        {
            InitializeComponent();
        }

        public AddBrigade(int teacherID)
        {
            InitializeComponent();
            this.teacherID = teacherID;
            FillAvailableThemes();
        }

        private void FillAvailableThemes()
        {
            try
            {
                using (var databaseConnection = new SqlConnection(config.connect))
                {
                    databaseConnection.Open();
                    var req = $@"SELECT Th.ThemeFormulation, Th.ID
                                 FROM Theme Th 
                                 WHERE NOT EXISTS (SELECT Br.ThemeID FROM Brigade Br WHERE Br.ThemeID = Th.ID) 
                                 AND Th.TeacherID = {teacherID}";
                    var cmd = new SqlCommand(req, databaseConnection);

                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        ComboBoxItem comboBoxItem = new ComboBoxItem()
                        {
                            Content = dr["ThemeFormulation"].ToString(),
                            Tag = $"{dr["ID"]}"
                        };
                        AvailableThemes.Items.Add(comboBoxItem);
                    }
                }
            }
            catch
            {
                MessageBox.Show("При загрузку БД произошла ошибка!", "Ошибка!", MessageBoxButton.OK);
            }
        }

        

        private void AddNewTheme_Click(object sender, RoutedEventArgs e)
        {
            AddTheme window = new AddTheme(teacherID);
            window.Closed += AddThemeWindow_Closed;
            window.Owner = this;
            window.Show();
        }

        private void AddThemeWindow_Closed(object sender, EventArgs e)
        {
            AddTheme window = (AddTheme) sender;

            if (!window.isAdded)
            {
                return;
            }

            ComboBoxItem comboBoxItem = new ComboBoxItem()
            {
                Content = window.themeFormulation,
                Tag = $"{window.primaryKey}"
            };

            AvailableThemes.Items.Add(comboBoxItem);
        }

        private void AddStudents_Click(object sender, RoutedEventArgs e)
        {
            AddStudent window = new AddStudent(teacherID, false);
            window.Closed += AddStudentWindow_Closed;
            window.Owner = this;
            window.Show();
        }

        private void AddStudentWindow_Closed(object sender, EventArgs e)
        {
            AddStudent window = (AddStudent) sender;
            if (!window.isAdded)
            {
                return;
            }

            foreach (TreeViewItem treeViewItem in window.StudentsList.Items)
            {
                TreeViewItem temp = new TreeViewItem()
                {
                    Header = treeViewItem.Header
                };
                StudentsList.Items.Add(temp);
            }
        }

        private int DatabaseAddBrigadeEntry()
        {
            int primaryKey;
            try
            {
                using (var databaseConnection = new SqlConnection(config.connect))
                {
                    databaseConnection.Open();
                    var req = $@"INSERT INTO Brigade (TeacherID, ThemeID)
                                  VALUES ({teacherID}, {themePrimaryKey}); SELECT SCOPE_IDENTITY()";
                    var cmd = new SqlCommand(req, databaseConnection);
                    var dr = cmd.ExecuteScalar();
                    primaryKey = Int32.Parse(dr.ToString());
                }
            }
            catch
            {
                MessageBox.Show("Нет подключения к бд!", "Ошибка!", MessageBoxButton.OK);
                isOk = false;
                return -1;
            }
            return primaryKey;
        }

        private int DatabaseAddStudentEntry(string studentInitials)
        {
            int primaryStudentKey;
            try
            {
                using (var databaseConnection = new SqlConnection(config.connect))
                {
                    databaseConnection.Open();
                    var req = $@"INSERT INTO Student (Initials, BrigadeID)
                                  VALUES ('{studentInitials}', {primaryKey}); SELECT SCOPE_IDENTITY()";
                    var cmd = new SqlCommand(req, databaseConnection);
                    var dr = cmd.ExecuteScalar();
                    primaryStudentKey = Int32.Parse(dr.ToString());
                }
            }
            catch
            {
                MessageBox.Show("Нет подключения к бд!", "Ошибка!", MessageBoxButton.OK);
                isOk = false;
                return -1;
            }
            return primaryStudentKey;
        }

        private void AddEntry_Click(object sender, RoutedEventArgs e)
        {
            if (AvailableThemes.SelectedItem == null)
            {
                MessageBox.Show("Выберите тему!", "Оишбка!", MessageBoxButton.OK);
                return;
            }

            themePrimaryKey = Int32.Parse(((ComboBoxItem)AvailableThemes.SelectedItem).Tag.ToString());
            themeFormulation = ((ComboBoxItem)AvailableThemes.SelectedItem).Content.ToString();
            primaryKey = DatabaseAddBrigadeEntry();

            if (!isOk)
            {
                MessageBox.Show("При добавлении бригады произошла ошибка!", "Ошибка!", MessageBoxButton.OK);
                return;
            }

            foreach (TreeViewItem treeViewItem in StudentsList.Items)
            {
                treeViewItem.Tag = $"{DatabaseAddStudentEntry(treeViewItem.Header.ToString())} 2";
            }

            if (!isOk)
            {
                MessageBox.Show("При добавлении студентов в бригаду произошла ошибка!", "Ошибка!", MessageBoxButton.OK);
                return;
            }
            isAdded = true;
            MessageBox.Show("Бригада успешно добавлена!", "Успех!", MessageBoxButton.OK);
            Close();
        }
    }
}