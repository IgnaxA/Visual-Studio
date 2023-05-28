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

namespace Kursach.UserControls.CRUD.Team
{
    /// <summary>
    /// Логика взаимодействия для AddStudentUserControl.xaml
    /// </summary>
    public partial class AddStudentUserControl : UserControl
    {
        public bool isAdded = false;
        private int teacherID = -1;
        private int teamID = -1;

        public AddStudentUserControl()
        {
            InitializeComponent();
        }
        
        public AddStudentUserControl(int teacherID, int teamID)
        {
            InitializeComponent();
            this.teacherID = teacherID;
            this.teamID = teamID;
            SetUpInformation();
        }

        private void SetUpInformation()
        {
            FillCourse();
            FillFaculty();
            FillTeams();
            FillRoles();
        }

        private void FillCourse()
        {
            try
            {
                CourseComboBox.Items.Clear();

                using (var databaseConnection = new SqlConnection(Config.GetConnect()))
                {
                    databaseConnection.Open();
                    var req = $@"SELECT Co.Course, Co.ID FROM Course Co";
                    var cmd = new SqlCommand(req, databaseConnection);

                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        ComboBoxItem comboBoxItem = new ComboBoxItem()
                        {
                            Content = $"{dr["Course"]}",
                            Tag = $"{dr["ID"]}"
                        };

                        CourseComboBox.Items.Add(comboBoxItem);
                    }
                }
            }
            catch
            {
                isAdded = false;
                MessageBox.Show("Нет подключения к бд!", "Ошибка!", MessageBoxButton.OK);
            }
        }

        private void FillFaculty()
        {
            try
            {
                FacultyComboBox.Items.Clear();

                using (var databaseConnection = new SqlConnection(Config.GetConnect()))
                {
                    databaseConnection.Open();
                    var req = $@"SELECT Fa.FacultyName, Fa.ID FROM Faculty Fa";
                    var cmd = new SqlCommand(req, databaseConnection);

                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        ComboBoxItem comboBoxItem = new ComboBoxItem()
                        {
                            Content = $"{dr["FacultyName"]}",
                            Tag = $"{dr["ID"]}"
                        };

                        FacultyComboBox.Items.Add(comboBoxItem);
                    }
                }
            }
            catch
            {
                isAdded = false;
                MessageBox.Show("Нет подключения к бд!", "Ошибка!", MessageBoxButton.OK);
            }
        }

        private void FillTeams()
        {
            try
            {
                TeamComboBox.Items.Clear();

                using (var databaseConnection = new SqlConnection(Config.GetConnect()))
                {
                    databaseConnection.Open();
                    var req = $@"SELECT Te.ID, Th.ThemeFormulation FROM Team Te
                             LEFT JOIN Theme Th 
                             ON Te.ThemeID = Th.ID
                             WHERE Th.TeacherID = {teacherID}";
                    var cmd = new SqlCommand(req, databaseConnection);

                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        ComboBoxItem comboBoxItem = new ComboBoxItem()
                        {
                            Content = $"{dr["ThemeFormulation"]}",
                            Tag = $"{dr["ID"]}"
                        };

                        if ((int)dr["ID"] == teamID) comboBoxItem.IsSelected = true;

                        TeamComboBox.Items.Add(comboBoxItem);
                    }
                }
            }
            catch
            {
                isAdded = false;
                MessageBox.Show("Нет подключения к бд!", "Ошибка!", MessageBoxButton.OK);
            }
        }

        private void FillRoles()
        {
            try
            {
                RoleComboBox.Items.Clear();
                ComboBoxItem comboBoxItem = new ComboBoxItem()
                {
                    Content = $"Нет роли",
                    Tag = null,
                    IsSelected = true
                };
                RoleComboBox.Items.Add(comboBoxItem);
                
                using (var databaseConnection = new SqlConnection(Config.GetConnect()))
                {
                    databaseConnection.Open();
                    var req = $@"SELECT Ro.ID, Ro.Role FROM Role Ro";
                    var cmd = new SqlCommand(req, databaseConnection);

                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        comboBoxItem = new ComboBoxItem()
                        {
                            Content = $"{dr["Role"]}",
                            Tag = $"{dr["ID"]}"
                        };

                        RoleComboBox.Items.Add(comboBoxItem);
                    }
                }
            }
            catch
            {
                isAdded = false;
                MessageBox.Show("Нет подключения к бд!", "Ошибка!", MessageBoxButton.OK);
            }
        }

        #region RMC select

        private void TeamsView_OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem treeViewItem = VisualUpwardSearch((DependencyObject)e.OriginalSource);

            if (treeViewItem != null)
            {
                treeViewItem.IsSelected = true;
                e.Handled = true;
            }
            else if (StudentsList.SelectedItem != null)
            {
                TreeViewItem selectedItem = (TreeViewItem)StudentsList.SelectedItem;
                selectedItem.IsSelected = false;
                e.Handled = true;
            }
        }

        static TreeViewItem VisualUpwardSearch(DependencyObject source)
        {
            while (source != null && !(source is TreeViewItem))
                source = VisualTreeHelper.GetParent(source);

            return (TreeViewItem)source;
        }

        private void MakeEnabled(bool isVisible)
        {
            DeleteNode.IsEnabled = isVisible;
        }

        private void TeamsView_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            MakeEnabled(StudentsList.SelectedItem != null);
        }

        #endregion



        private void InitialsTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex r = new Regex("[^А-Яа-я]+");

            e.Handled = r.IsMatch(e.Text);
        }

        private void InitialsTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                if (InitialsTextBox.Text.Length == 0 || (InitialsTextBox.Text.Length != 0 && InitialsTextBox.Text[InitialsTextBox.Text.Length - 1] == ' '))
                {
                    e.Handled = true;
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }


        private int DatabaseAdd(int teamID, string initials, string email, int facultyID, int courseID, string roleID)
        {
            int primaryKey;
            try
            {
                using (var databaseConnection = new SqlConnection(Config.GetConnect()))
                {
                    databaseConnection.Open();
                    string req;
                    if (roleID == "NULL")
                    {
                        req = $@"INSERT INTO Student (TeamID, Initials, Email, FacultyID, CourseID, RoleID)
                                     VALUES ({teamID}, '{initials}', '{email}', {facultyID}, {courseID}, NULL); SELECT SCOPE_IDENTITY()";
                    }
                    else
                    {
                        req = $@"INSERT INTO Student (TeamID, Initials, Email, FacultyID, CourseID, RoleID)
                                     VALUES ({teamID}, '{initials}', '{email}', {facultyID}, {courseID}, {Int32.Parse(roleID)}); SELECT SCOPE_IDENTITY()";
                    }
                    var cmd = new SqlCommand(req, databaseConnection);
                    var dr = cmd.ExecuteScalar();
                    primaryKey = Int32.Parse(dr.ToString());
                }
            }
            catch
            {
                isAdded = false;
                MessageBox.Show("Нет подключения к бд!", "Ошибка!", MessageBoxButton.OK);
                return -1;
            }
            return primaryKey;
        }

        private void AddStudentButton_Click(object sender, RoutedEventArgs e)
        {
            if (CourseComboBox.SelectedValue == null || FacultyComboBox.SelectedValue == null || TeamComboBox.SelectedValue == null)
            {
                MessageBox.Show("Заполните все поля!", "Ошибка!", MessageBoxButton.OK);
                return;
            }

            Regex r = new Regex("^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$");
            if (InitialsTextBox.Text.Trim().Split().Length < 2)
            {
                MessageBox.Show("Введите корректные инициалы студента!", "Ошибка!", MessageBoxButton.OK);
                return;
            }

            if (!r.IsMatch(EmailTextBox.Text))
            {
                MessageBox.Show("Введите корректный Email", "Ошибка!", MessageBoxButton.OK);
                return;
            }

            string courseName = ((ComboBoxItem)CourseComboBox.SelectedItem).Content.ToString();
            string facultyName = ((ComboBoxItem)FacultyComboBox.SelectedItem).Content.ToString();
            string initials = InitialsTextBox.Text;
            string email = EmailTextBox.Text.Trim();
            int courseID = Int32.Parse(((ComboBoxItem)CourseComboBox.SelectedItem).Tag.ToString());
            int facultyID = Int32.Parse(((ComboBoxItem)FacultyComboBox.SelectedItem).Tag.ToString());
            string roleID = ((ComboBoxItem)RoleComboBox.SelectedValue).Tag == null ? "NULL" : ((ComboBoxItem)RoleComboBox.SelectedValue).Tag.ToString();
            int teamID = Int32.Parse(((ComboBoxItem)TeamComboBox.SelectedValue).Tag.ToString());

            int primaryKey = DatabaseAdd(teamID, initials, email, facultyID, courseID, roleID);

            TreeViewItem studentEntry = new TreeViewItem()
            {
                Header = initials,
                Tag = $"{primaryKey}|1|{courseName}|{facultyName}|{email}|{courseID}|{facultyID}|{roleID}"
            };
            StudentsList.Items.Add(studentEntry);
        }


        private void DeleteFromDatabase(int studentID)
        {
            try
            {
                using (var databaseConnection = new SqlConnection(Config.GetConnect()))
                {
                    databaseConnection.Open();
                    var req = $@"DELETE FROM Student WHERE ID = {studentID}";
                    var cmd = new SqlCommand(req, databaseConnection);
                    var dr = cmd.ExecuteNonQuery();
                }
            }
            catch
            {
                MessageBox.Show("Нет подключения к бд!", "Ошибка!", MessageBoxButton.OK);
            }
        }

        private void DeleteNode_Click(object sender, RoutedEventArgs e)
        {
            DeleteFromDatabase(Int32.Parse(((TreeViewItem)StudentsList.SelectedItem).Tag.ToString().Split('|')[0]));
            StudentsList.Items.Remove((TreeViewItem)StudentsList.SelectedItem);
        }
    }
}
