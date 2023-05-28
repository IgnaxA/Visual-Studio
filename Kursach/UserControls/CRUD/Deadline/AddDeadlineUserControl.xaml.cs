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

namespace Kursach.UserControls.CRUD.Deadline
{
    /// <summary>
    /// Логика взаимодействия для AddDeadlineUserControl.xaml
    /// </summary>
    public partial class AddDeadlineUserControl : UserControl
    {
        private int teacherID = -1;
        public int teamID = -1;
        public bool isAdded = false;
        public int deadlineID = -1;
        public string date = "";
        public string commentary = "";
        private string dateToRequest = "";
        private bool isRename = false;
        public int attendanceMark = -1;
        private bool isDateCheck = true;

        public AddDeadlineUserControl()
        {
            InitializeComponent();
        }
        
        public AddDeadlineUserControl(int teacherID, int teamID)
        {
            InitializeComponent();
            this.teacherID = teacherID;
            this.teamID = teamID;
            AttendanceMatkComboBox.Visibility = Visibility.Collapsed;
            AttendanceMatkLabel.Visibility = Visibility.Collapsed;
            FillTeamComboBox();
        }
        public AddDeadlineUserControl(int teacherID, int teamID, int deadlineID,string commentary, string date)
        {
            InitializeComponent();
            this.teacherID = teacherID;
            this.teamID = teamID;
            this.commentary = commentary;
            this.date = date;
            this.deadlineID = deadlineID;
            CommentaryTextBox.Text = commentary;
            DateDatePicker.Text = date;
            isRename = true;
            isDateCheck = false;
            AttendanceMatkComboBox.Visibility = Visibility.Visible;
            AttendanceMatkLabel.Visibility = Visibility.Visible;
            FillTeamComboBox();
        }

        private void FillTeamComboBox()
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
                MessageBox.Show("Нет подключения к бд!", "Ошибка!", MessageBoxButton.OK);
            }
        }

        private bool DateCheck()
        {
            try
            {
                using (var databaseConnection = new SqlConnection(Config.GetConnect()))
                {
                    databaseConnection.Open();
                    var req = $@"SELECT De.DeadLineDate FROM Deadline De WHERE De.TeamID = {teamID}";
                    var cmd = new SqlCommand(req, databaseConnection);

                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        string currentDate = dr["DeadlineDate"].ToString().Split()[0];
                        if (date == currentDate)
                        {
                            return true;
                        }
                        
                    }
                }
                isAdded = true;
                return false;
            }
            catch
            {
                MessageBox.Show("Нет подключения к бд!", "Ошибка!", MessageBoxButton.OK);
                isAdded = false;
                return false;
            }
        }

        private int DatabaseAdd()
        {
            int primaryKey;
            try
            {
                using (var databaseConnection = new SqlConnection(Config.GetConnect()))
                {
                    databaseConnection.Open();
                    var req = $@"INSERT INTO Deadline (TeamID, DeadLineDate, Commentary, AttendanceMark)
                                  VALUES ({teamID}, '{dateToRequest}', '{commentary}', 0); SELECT SCOPE_IDENTITY()";
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

        private void DatabaseRename()
        {
            try
            {
                using (var databaseConnection = new SqlConnection(Config.GetConnect()))
                {
                    databaseConnection.Open();
                    var req = $@"UPDATE Deadline SET TeamID = {teamID}, DeadLineDate = '{dateToRequest}', Commentary = '{commentary}', AttendanceMark = {attendanceMark} WHERE ID = {deadlineID}";
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
            if (DateDatePicker.SelectedDate == null || (isDateCheck && DateDatePicker.SelectedDate < DateTime.Today))
            {
                MessageBox.Show("Выберите корректную дату!", "Ошибка!", MessageBoxButton.OK);
                return;
            }
            if (CommentaryTextBox.Text.Trim().Length == 0)
            {
                MessageBox.Show("Введите корректный комментарий!", "Ошибка!", MessageBoxButton.OK);
                return;
            }
            date = DateDatePicker.SelectedDate.ToString().Split()[0];
            dateToRequest = $"{date[6]}{date[7]}{date[8]}{date[9]}-{date[3]}{date[4]}-{date[0]}{date[1]}";
            teamID = Int32.Parse(((ComboBoxItem)TeamComboBox.SelectedValue).Tag.ToString());
            if (DateCheck())
            {
                MessageBox.Show("Дедлайн с такой датой уже есть!", "Ошибка!", MessageBoxButton.OK);
                return;
            }
            commentary = CommentaryTextBox.Text.Trim();
            attendanceMark = Int32.Parse(((ComboBoxItem)AttendanceMatkComboBox.SelectedValue).Tag.ToString());
            if (isRename)
            {
                DatabaseRename();
            }
            else
            {
                deadlineID = DatabaseAdd();
            }
            
            this.Visibility = Visibility.Collapsed;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void CommentaryTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                if (CommentaryTextBox.Text.Length == 0 || (CommentaryTextBox.Text.Length != 0 && CommentaryTextBox.Text[CommentaryTextBox.Text.Length - 1] == ' '))
                {
                    e.Handled = true;
                }
            }
        }

        private void CommentaryTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex r = new Regex("[^a-zA-Z/.:1-9А-Яа-я]+");

            e.Handled = r.IsMatch(e.Text);
            
        }
    }
}
