using StudentsControl.Teacher;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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

namespace StudentsControl
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Config config = new Config();
        private int administrationLevel = -1;
        private int userID = -1;
        private string initials = "";
        private int workerID = -1;
        
        public MainWindow()
        {
            InitializeComponent();
            SetTextBoxsToDefault();
        }

        private void SetTextBoxsToDefault()
        {
            LoginTextBox.Text = "Введите логин:";
            LoginTextBox.Foreground = Brushes.Gray;
            PasswordTextBox.Password = "";
        }

        private void SetFielsToDefault()
        {
            administrationLevel = -1;
            userID = -1;
            workerID= -1;
            initials = "";
        }

        private void Login_GotFocus(object sender, RoutedEventArgs e)
        {
            if (LoginTextBox.Text == "Введите логин:")
            {
                LoginTextBox.Text = "";
                LoginTextBox.Foreground = Brushes.Black;
            }
        }

        private void Login_LostFocus(object sender, RoutedEventArgs e)
        {
            if (LoginTextBox.Text == "")
            {
                LoginTextBox.Text = "Введите логин:";
                LoginTextBox.Foreground = Brushes.Gray;
            }
        }

        private void Login_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex r = new Regex("[^0-9A-Za-z]+");

            e.Handled = r.IsMatch(e.Text);
        }

        private void Login_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) e.Handled = true;
        }

        private void Password_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) e.Handled = true;
        }

        private string GetRequest()
        {
            string req = "";
            if (administrationLevel == 1)
            {
                req = $@"SELECT SO.Initials, SO.ID
                         FROM StudyOffice SO
                         WHERE SO.UserID = {userID}";
            }
            if (administrationLevel == 2)
            {
                req = $@"SELECT Te.Initials, Te.ID
                         FROM Teacher Te
                         WHERE Te.UserID = {userID}";
            }

            return req;
        }

        private void GetUserAdministrationLevelAndTeacherInitials(string userLogin, string userPassword)
        {
            try
            {
                using (var databaseConnection = new SqlConnection(config.connect))
                {
                    databaseConnection.Open();
                    string req = $@"SELECT U.AdministrationLevel, U.ID
                                    FROM AppUser U
                                    WHERE U.Login = '{userLogin}' AND U.Password = '{userPassword}'";

                    var cmd = new SqlCommand(req, databaseConnection);
                    var dr = cmd.ExecuteReader();
                    bool isReadable = dr.Read();
                    if (!isReadable)
                    {
                        SetFielsToDefault();
                        return;
                    }
                    while (isReadable)
                    {
                        administrationLevel = (int) dr["AdministrationLevel"];
                        userID = (int) dr["ID"];
                        isReadable = dr.Read();
                    }
                    dr.Close();

                    req = GetRequest();
                    var newCmd = new SqlCommand(req, databaseConnection);
                    var newDr = newCmd.ExecuteReader();
                    while (newDr.Read())
                    {
                        workerID = (int) newDr["ID"];
                        initials = newDr["Initials"].ToString();
                    }

                }
            }
            catch
            {
                SetFielsToDefault();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (LoginTextBox.Text == "Введите логин:")
            {
                MessageBox.Show("Введите Ваш логин!", "Ошибка!", MessageBoxButton.OK);
                return;
            }
            if (PasswordTextBox.Password.Length == 0)
            {
                MessageBox.Show("Введите Ваш пароль!", "Ошибка!", MessageBoxButton.OK);
                return;
            }

            GetUserAdministrationLevelAndTeacherInitials(LoginTextBox.Text, PasswordTextBox.Password);

            if (administrationLevel == -1)
            {
                MessageBox.Show("Ошибка! Такого пользователя не существует!", "Ошибка!", MessageBoxButton.OK);
                return;
            }

            if (administrationLevel == 1)
            {
                //MessageBox.Show("Юзер - учебка");
            }

            if (administrationLevel == 2)
            {
                //MessageBox.Show("Юзер - преподаватель");
                TeacherWindow teacherWindow = new TeacherWindow(initials, workerID);
                teacherWindow.Show();
                this.Close();
            }
        }
    }
}
