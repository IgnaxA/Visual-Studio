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
    /// Логика взаимодействия для RenameStudentTeamUserControl.xaml
    /// </summary>
    public partial class RenameStudentTeamUserControl : UserControl
    {
        private int teacherID = -1;
        public int studentID = -1;
        public string initials = "";
        public string email = "";
        public int courseID = -1;
        public int facultyID = -1;
        public int teamID = -1;
        public string roleID = "";
        public string course = "";
        public string faculty = "";
        public bool isChanged = false;

        public RenameStudentTeamUserControl()
        {
            InitializeComponent();
        }
        
        public RenameStudentTeamUserControl(int studentID, string initials, string email, int courseID, int facultyID, int teamID, string roleID, int teacherID)
        {
            InitializeComponent();
            this.studentID = studentID;
            this.initials = initials;
            this.email = email;
            this.courseID = courseID;
            this.facultyID = facultyID;
            this.teamID = teamID;
            this.roleID = roleID;
            this.teacherID = teacherID;
            SetUpInformation();
        }

        private void SetUpInformation()
        {
            InitialsTextBox.Text = initials;
            EmailTextBox.Text = email;

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
                        if ((int)dr["ID"] == courseID) comboBoxItem.IsSelected = true;
                    }
                }
            }
            catch
            {
                isChanged = false;
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
                        if ((int)dr["ID"] == facultyID) comboBoxItem.IsSelected = true;
                    }
                }
            }
            catch
            {
                isChanged = false;
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

                        TeamComboBox.Items.Add(comboBoxItem);
                        if ((int)dr["ID"] == teamID) comboBoxItem.IsSelected = true;
                    }
                }
            }
            catch
            {
                isChanged = false;
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
                    Tag = null
                };
                RoleComboBox.Items.Add(comboBoxItem);
                if (roleID == "" || roleID == "NULL") comboBoxItem.IsSelected = true;
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
                        if (roleID != "" && roleID != "NULL" && Int32.Parse(roleID) == (int)dr["ID"]) comboBoxItem.IsSelected = true;
                    }
                }
            }
            catch
            {
                isChanged = false;
                MessageBox.Show("Нет подключения к бд!", "Ошибка!", MessageBoxButton.OK);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

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

        private void UpdateDatabase()
        {
            try
            {
                using (var databaseConnection = new SqlConnection(Config.GetConnect()))
                {
                    databaseConnection.Open();
                    string req;
                    if (roleID == "NULL")
                    {
                        req = $@"UPDATE Student SET TeamID = {teamID}, Initials = '{initials}', Email = '{email}', FacultyID = {facultyID}, CourseID = {courseID}, RoleID = NULL WHERE ID = {studentID}";
                    }
                    else
                    {
                        req = $@"UPDATE Student SET TeamID = {teamID}, Initials = '{initials}', Email = '{email}', FacultyID = {facultyID}, CourseID = {courseID}, RoleID = {Int32.Parse(roleID)} WHERE ID = {studentID}";
                    }
                     
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

            initials = InitialsTextBox.Text.Trim();
            email = EmailTextBox.Text.Trim();
            courseID = Int32.Parse(((ComboBoxItem)CourseComboBox.SelectedValue).Tag.ToString());
            facultyID = Int32.Parse(((ComboBoxItem)FacultyComboBox.SelectedValue).Tag.ToString());
            teamID = Int32.Parse(((ComboBoxItem)TeamComboBox.SelectedValue).Tag.ToString());
            course = ((ComboBoxItem)CourseComboBox.SelectedValue).Content.ToString();
            faculty = ((ComboBoxItem)FacultyComboBox.SelectedValue).Content.ToString();
            if (((ComboBoxItem)RoleComboBox.SelectedValue).Tag == null) roleID = "NULL";
            else roleID = ((ComboBoxItem)RoleComboBox.SelectedValue).Tag.ToString();

            UpdateDatabase();
            Visibility = Visibility.Collapsed;
        }
    }
}