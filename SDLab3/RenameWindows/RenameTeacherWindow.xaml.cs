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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace SDLab3.RenameWindows
{
    /// <summary>
    /// Логика взаимодействия для RenameTeacherWindow.xaml
    /// </summary>
    public partial class RenameTeacherWindow : Window
    {
        private string connect;
        private int id = -1;
        private int studyOfficeID;
        public bool isChanged = false;
        private Dictionary<int, string> studyOfficeEntry = new Dictionary<int, string>();

        public RenameTeacherWindow()
        {
            InitializeComponent();
        }

        public RenameTeacherWindow(string connect, int id, string WorkerInitials, int studyOfficeID)
        {
            InitializeComponent();
            this.connect = connect;
            this.id = id;
            this.WorkerInitials.Text = WorkerInitials;
            this.studyOfficeID = studyOfficeID;   
        }

        /*private void GetAllStudyOffices()
        {
            try
            {
                using (var databaseConnection = new SqlConnection(connect))
                {
                    databaseConnection.Open();
                    var req = $@"select SO.WorkerInitials, SO.ID from StudyOffice SO";
                    var cmd = new SqlCommand(req, databaseConnection);
                    cmd.Parameters.AddWithValue("@teacherInitials", WorkerInitials.Text);
                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {

                        studyOfficeEntry[(int)dr["ID"]] = dr["WorkerInitials"].ToString();
                    }
                    FillCombobox();
                }
            }
            catch
            {
                MessageBox.Show("Нет подключения к бд!", "Ошибка!", MessageBoxButton.OK);
            }

        }

        private void FillCombobox()
        {
            foreach (var elem in studyOfficeEntry)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem()
                {
                    Content = elem.Value,
                    Tag = elem.Key
                };
                StudyOffice.Items.Add(comboBoxItem);
                if (elem.Key == studyOfficeID)
                {
                    StudyOffice.SelectedValue = comboBoxItem;
                }
            }
        }*/

        private void DatabaseRenameEntry()
        {
            try
            {
                using (var databaseConnection = new SqlConnection(connect))
                {
                    databaseConnection.Open();
                    var req = $@"update Teacher set TeacherInitials = @teacherInitials where ID = {id}";
                    var cmd = new SqlCommand(req, databaseConnection);
                    cmd.Parameters.AddWithValue("@teacherInitials", WorkerInitials.Text);
                    var dr = cmd.ExecuteNonQuery();
                    isChanged = true;
                    MessageBox.Show($"Запись о {WorkerInitials.Text} успешно изменена!", "Успешно", MessageBoxButton.OK);
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
            if (WorkerInitials.Text.Split().Length < 2)
            {
                MessageBox.Show("Введите корректные инициалы сотрудника!");
                return;
            }
            DatabaseRenameEntry();
            this.Close();
        }
    }
}
