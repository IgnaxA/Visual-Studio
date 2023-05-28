using StudentsControl.Teacher.Adding;
using StudentsControl.Teacher.Rename;
using StudentsControl.Teacher.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
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

namespace StudentsControl.Teacher
{
    /// <summary>
    /// Логика взаимодействия для TeacherWindow.xaml
    /// </summary>
    public partial class TeacherWindow : Window
    {
        private int teacherID = -1;
        private int elementToShow = 1;
        private StudentDatabaseControl studentDatabaseControl;
        private ThemeDatabaseControl themeDatabaseControl;
        private CalendarDatabaseControl calendarDatabaseControl;

        public TeacherWindow()
        {
            InitializeComponent();
        }

        public TeacherWindow(string initials, int teacherID)
        {
            InitializeComponent();
            LabelInitials.Content = initials;
            this.teacherID = teacherID;
            InitialisdeUserControls();
            SetGridUserControls();
            SetUserControlsVisibility();
        }

        private void InitialisdeUserControls()
        {
            studentDatabaseControl = new StudentDatabaseControl(teacherID);
            themeDatabaseControl = new ThemeDatabaseControl(teacherID);
            calendarDatabaseControl = new CalendarDatabaseControl(teacherID);
        }

        private void SetGridUserControls()
        {
            TeacherWindowGrid.Children.Add(studentDatabaseControl);
            SetUserControlsPosition(studentDatabaseControl);
            TeacherWindowGrid.Children.Add(themeDatabaseControl);
            SetUserControlsPosition(themeDatabaseControl);
            TeacherWindowGrid.Children.Add(calendarDatabaseControl);
            SetUserControlsPosition(calendarDatabaseControl);
        }

        private void SetUserControlsPosition(UIElement element)
        {
            Grid.SetRow(element, 1);
            Grid.SetColumn(element, 1);
            Grid.SetRowSpan(element, 3);
        }

        private void SetUserControlsVisibility()
        {
            studentDatabaseControl.Visibility = elementToShow == 1 ? Visibility.Visible : Visibility.Collapsed;
            themeDatabaseControl.Visibility = elementToShow == 2 ? Visibility.Visible : Visibility.Collapsed;
            calendarDatabaseControl.Visibility = elementToShow == 3 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void SetBrigadeView_Click(object sender, RoutedEventArgs e)
        {
            elementToShow = 1;
            SetUserControlsVisibility();
        }

        private void SetThemeView_Click(object sender, RoutedEventArgs e)
        {
            elementToShow = 2;
            themeDatabaseControl.FillThemeTree();
            SetUserControlsVisibility();
        }

        private void SetDeadlineView_Click(object sender, RoutedEventArgs e)
        {
            elementToShow = 3;
            SetUserControlsVisibility();
        }

        private void SetUserControlFontSize(int fontSize)
        {
            studentDatabaseControl.BrigadesView.FontSize = fontSize;
            themeDatabaseControl.ThemeView.FontSize = fontSize;
        }

        private void SetFontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem comboBoxItem = (ComboBoxItem)SetFontSize.SelectedItem;

            if (comboBoxItem.Content != null) SetUserControlFontSize(Int32.Parse(comboBoxItem.Content.ToString()));
        }
    }
}
