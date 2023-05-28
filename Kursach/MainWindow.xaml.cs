using Kursach.UserControls.Main;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO.Packaging;
using System.Linq;
using System.Reflection.Emit;
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

namespace Kursach
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int teacherID = 1;
        private string teacherInitials = "Викентьева Ольга Леонидовна";
        private string course = "Все";
        private string faculty = "Все";
        private int attendancyMark = -1;
        private TeamUserControl teamUserControl;
        private ThemeUserControl themeUserControl;
        private DeadLineUserControl deadlineUserControl;

        public MainWindow()
        {
            InitializeComponent();
            InitialiseUserControls();
            MainWindowElementsSettings();
            ElementsVisibilities(1);
        }

        #region SetUpping main menu

        private void InitialiseUserControls()
        {
            teamUserControl = new TeamUserControl(teacherID, "Все", "Все");
            MainGrid.Children.Add(teamUserControl);
            SetUserControlsPosition(teamUserControl);
            themeUserControl = new ThemeUserControl(teacherID);
            MainGrid.Children.Add(themeUserControl);
            SetUserControlsPosition(themeUserControl);
            deadlineUserControl = new DeadLineUserControl(teacherID);
            MainGrid.Children.Add(deadlineUserControl);
            SetUserControlsPosition(deadlineUserControl);

        }

        private void MainWindowElementsSettings()
        {
            TeacherInitialsLabel.Content = teacherInitials;
            FillCourseComboBox();
            FillFacultyComboBox();
        }

        private void ElementsVisibilities(int showLevel)
        {
            CourseSelect.Visibility = showLevel == 1 ? Visibility.Visible : Visibility.Collapsed;
            FacultySelect.Visibility = showLevel == 1 ? Visibility.Visible : Visibility.Collapsed;
            DeadlineStatusSelect.Visibility = showLevel == 2 || showLevel == 3 ? Visibility.Visible : Visibility.Collapsed;
            DeadlineSelect.Visibility = showLevel == 3 ? Visibility.Visible : Visibility.Collapsed;

            teamUserControl.Visibility = showLevel == 1 ? Visibility.Visible : Visibility.Collapsed;
            deadlineUserControl.Visibility = showLevel == 2 ? Visibility.Visible : Visibility.Collapsed;

            themeUserControl.Visibility = showLevel == 4 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void SetUserControlsPosition(UIElement element)
        {
            Grid.SetRow(element, 3);
            //Grid.SetColumn(element, 1);
            //Grid.SetRowSpan(element, 3);
        }


        #endregion

        #region Filling comboboxes

        private void FillCourseComboBox()
        {
            try
            {
                CourseComboBox.Items.Clear();
                ComboBoxItem comboBoxItem = new ComboBoxItem()
                {
                    Content = "Все",
                    IsSelected = true
                };
                CourseComboBox.Items.Add(comboBoxItem);

                using (var databaseConnection = new SqlConnection(Config.GetConnect()))
                {
                    databaseConnection.Open();
                    var req = $@"SELECT Co.Course FROM Course Co";
                    var cmd = new SqlCommand(req, databaseConnection);

                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        comboBoxItem = new ComboBoxItem()
                        {
                            Content = $"{dr["Course"]}"
                        };

                        CourseComboBox.Items.Add(comboBoxItem);
                    }
                }
            }
            catch
            {
                MessageBox.Show("При загрузку БД произошла ошибка!");
            }
        }

        private void FillFacultyComboBox()
        {
            try
            {
                List<string> facultyNames = new List<string>();

                FacultyComboBox.Items.Clear();
                ComboBoxItem comboBoxItem = new ComboBoxItem()
                {
                    Content = "Все",
                    IsSelected = true
                };
                FacultyComboBox.Items.Add(comboBoxItem);

                using (var databaseConnection = new SqlConnection(Config.GetConnect()))
                {
                    databaseConnection.Open();
                    var req = $@"SELECT Fa.FacultyName FROM Faculty Fa";
                    var cmd = new SqlCommand(req, databaseConnection);

                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        comboBoxItem = new ComboBoxItem()
                        {
                            Content = $"{dr["FacultyName"]}"
                        };

                        if (!facultyNames.Contains($"{dr["FacultyName"]}"))
                        {
                            FacultyComboBox.Items.Add(comboBoxItem);
                            facultyNames.Add($"{dr["FacultyName"]}");
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("При загрузку БД произошла ошибка!");
            }
        }

        #endregion

        #region Combobox visibility

        private void TeamButton_Click(object sender, RoutedEventArgs e)
        {
            ElementsVisibilities(1);
            teamUserControl.FillTeamTree(course, faculty);
        }

        private void DeadlineButton_Click(object sender, RoutedEventArgs e)
        {
            ElementsVisibilities(2);
            deadlineUserControl.FillDeadLineTree(attendancyMark);
            
        }

        private void ConsultationButton_Click(object sender, RoutedEventArgs e)
        {
            ElementsVisibilities(3);
        }

        private void ThemeButton_Click(object sender, RoutedEventArgs e)
        {
            ElementsVisibilities(4);
            themeUserControl.FillThemeTree();
        }

        #endregion

        #region UserControls elements visibility change

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem courseItem = (ComboBoxItem) CourseComboBox.SelectedValue;
            ComboBoxItem facultyItem = (ComboBoxItem) FacultyComboBox.SelectedValue;
            if (teamUserControl == null || courseItem == null || facultyItem == null)
            {
                return;
            }
            course = courseItem.Content.ToString();
            faculty = facultyItem.Content.ToString();
            teamUserControl.ChangeVisibility(course, faculty);
        }

        private void DeadlineStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem deadlineStatus = (ComboBoxItem)DeadlineStatusComboBox.SelectedValue;
            if (deadlineUserControl == null)
            {
                return;
            }
            attendancyMark = Int32.Parse(deadlineStatus.Tag.ToString());
            deadlineUserControl.ChangeVisibility(attendancyMark);
        }


        #endregion

        #region FontSize change

        private void SetFontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem fontSize = (ComboBoxItem)SetFontSize.SelectedItem;
            if (teamUserControl != null) SetUserControlFontSize(Int32.Parse(fontSize.Content.ToString()));
        }

        private void SetUserControlFontSize(int fontSize)
        {
            teamUserControl.TeamsView.FontSize = fontSize;
            deadlineUserControl.DeadLinesView.FontSize = fontSize;
            
            themeUserControl.ThemesView.FontSize = fontSize;
        }

        #endregion

        #region Exit from App

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion

        
    }
}
