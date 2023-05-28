using Kursach.UserControls.CRUD.Team;
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

namespace Kursach.UserControls.Main
{
    /// <summary>
    /// Логика взаимодействия для TeamUserControl.xaml
    /// </summary>
    public partial class TeamUserControl : UserControl
    {
        private bool isLoaded = false;
        private int teacherID = -1;
        private TreeViewItem renameItem;
        private TreeViewItem addingItem;
        private TreeViewItem deletedItem;

        public TeamUserControl()
        {
            InitializeComponent();
        }

        public TeamUserControl(int teacherID, string course, string faculty)
        {
            InitializeComponent();
            this.teacherID = teacherID;
            FillTeamTree(course, faculty);
        }

        #region Load Database

        public void FillTeamTree(string course, string faculty)
        {
            try
            {
                TeamsView.Items.Clear();
                TreeViewItem currentItemTeam = new TreeViewItem()
                {
                    Header = "",
                    Tag = "-1|-1"
                };
                using (var databaseConnection = new SqlConnection(Config.GetConnect()))
                {
                    databaseConnection.Open();
                    var req = $@"SELECT St.ID as StudentID, St.Initials as StudentInitials, St.Email as StudentEmail, Co.Course, Fa.FacultyName, Ro.Role, Th.ThemeFormulation, Te.ID as TeamID, St.CourseID, St.FacultyID, St.RoleID, Th.ID as ThemeID, Te.MaterialsLink
                                 FROM Student St
                                 LEFT JOIN Role Ro
                                 ON Ro.ID = St.RoleID
                                 LEFT JOIN Faculty Fa
                                 ON Fa.ID = St.FacultyID
                                 LEFT JOIN Course Co
                                 ON Co.ID = St.CourseID
                                 RIGHT JOIN Team Te
                                 ON Te.ID = St.TeamID
                                 LEFT JOIN Theme Th
                                 ON Th.ID = Te.ThemeID
                                 WHERE Th.TeacherID = {teacherID}";
                    var cmd = new SqlCommand(req, databaseConnection);

                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        bool isContainsTeam = true;
                        TreeViewItem teamEntry = new TreeViewItem()
                        {
                            Header = $"{dr["ThemeFormulation"]}",
                            Tag = $"{dr["TeamID"]}|0|{dr["ThemeID"]}|{dr["MaterialsLink"]}",
                            IsExpanded = true
                        };

                        if (!currentItemTeam.Tag.ToString().Split('|')[0].Equals(teamEntry.Tag.ToString().Split('|')[0]))
                        {
                            currentItemTeam = teamEntry;
                            isContainsTeam = false;
                        }

                        TreeViewItem studentEntry = new TreeViewItem()
                        {
                            Header = dr["StudentInitials"],
                            Tag = $"{dr["StudentID"]}|1|{dr["Course"]}|{dr["FacultyName"]}|{dr["StudentEmail"]}|{dr["CourseID"]}|{dr["FacultyID"]}|{dr["RoleID"]}"
                        };

                        if (!isContainsTeam)
                        {
                            TeamsView.Items.Add(currentItemTeam);
                        }
                        currentItemTeam.Items.Add(studentEntry);

                    }
                    isLoaded = true;
                    ChangeVisibility(course, faculty);
                }
            }
            catch
            {
                isLoaded = false;
                MessageBox.Show("При загрузку БД произошла ошибка!");
            }
        }

        #endregion

        #region Changing visibility

        public void ChangeVisibility(string course, string faculty)
        {
            foreach (TreeViewItem team in TeamsView.Items)
            {
                bool isVisible = false;
                foreach (TreeViewItem student in team.Items)
                {
                    if ((course == "Все" || (course != "Все" && student.Tag.ToString().Split('|')[2].Equals(course))) && (faculty == "Все" || (faculty != "Все" && student.Tag.ToString().Split('|')[3].Equals(faculty))))
                    {
                        isVisible = true;
                        break;
                    }
                }
                team.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            }
        }


        #endregion


        #region RMC select

        private void TeamsView_OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem treeViewItem = VisualUpwardSearch((DependencyObject)e.OriginalSource);

            if (treeViewItem != null)
            {
                treeViewItem.IsSelected = true;
                e.Handled = true;
            }
            else if (TeamsView.SelectedItem != null)
            {
                TreeViewItem selectedItem = (TreeViewItem)TeamsView.SelectedItem;
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
            AddNode.IsEnabled = isLoaded && !(TeamsView.SelectedItem != null && ((TreeViewItem)TeamsView.SelectedItem).Tag.ToString().Split('|')[1] == "1");
            DeleteNode.IsEnabled = isVisible;
            RenameNode.IsEnabled = isVisible;
        }

        private void TeamsView_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            MakeEnabled(TeamsView.SelectedItem != null);
        }

        #endregion

        #region Adding

        private void ShowAddStudentsWindow()
        {
            addingItem = (TreeViewItem)TeamsView.SelectedItem;
            AddStudentUserControl addStudentUserControl = new AddStudentUserControl(teacherID, Int32.Parse(((TreeViewItem)TeamsView.SelectedItem).Tag.ToString().Split('|')[0]));
            TeamUserControlGrid.Children.Add(addStudentUserControl);
            TeamsView.Visibility = Visibility.Collapsed;
            addStudentUserControl.Visibility = Visibility.Visible;
            addStudentUserControl.IsVisibleChanged += AddStudentUserControl_IsVisibleChanged;
        }

        private void ShowAddBrigadeWindow()
        {
            AddTeamUserControl addTeamUserControl = new AddTeamUserControl(teacherID);
            TeamUserControlGrid.Children.Add(addTeamUserControl);
            TeamsView.Visibility = Visibility.Collapsed;
            addTeamUserControl.Visibility = Visibility.Visible;
            addTeamUserControl.IsVisibleChanged += AddTeamUserControl_IsVisibleChanged;
        }

        private void AddTeamUserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            TeamsView.Visibility = Visibility.Visible;
            AddTeamUserControl userControl = (AddTeamUserControl)sender;
            if (!userControl.isAdded)
            {
                return;
            }

            TreeViewItem newTeam = new TreeViewItem()
            {
                Header = userControl.themeFormulation,
                Tag = $"{userControl.teamID}|0|{userControl.themeID}|{userControl.materialsLink}",
                IsSelected = true,
                IsExpanded = true
            };
            TeamsView.Items.Add(newTeam);
            TeamUserControlGrid.Children.Remove(userControl);
            ShowAddStudentsWindow();
        }

        private void AddNode_Click(object sender, RoutedEventArgs e)
        {

            if (TeamsView.SelectedItem != null)
            {
                ShowAddStudentsWindow();
            }
            else
            {
                ShowAddBrigadeWindow();
            }
        }

        private void AddStudentUserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            TeamsView.Visibility = Visibility.Visible;
            AddStudentUserControl userControl = (AddStudentUserControl) sender;
            foreach (TreeViewItem treeViewItem in userControl.StudentsList.Items)
            {
                TreeViewItem newStudent = new TreeViewItem() 
                {
                    Header = treeViewItem.Header,
                    Tag = treeViewItem.Tag
                };
                addingItem.Items.Add(newStudent);
            }
            TeamUserControlGrid.Children.Remove(userControl);
        }

        #endregion

        #region Deleting

        private void DatabaseDelete(string req)
        {
            try
            {
                using (var databaseConnection = new SqlConnection(Config.GetConnect()))
                {
                    databaseConnection.Open();
                    var cmd = new SqlCommand(req, databaseConnection);
                    var dr = cmd.ExecuteNonQuery();
                    MessageBox.Show($"Запись успешно удалена!", "Успешно", MessageBoxButton.OK);
                }
            }
            catch
            {
                MessageBox.Show("Не получилось удалить запись!", "Ошибка!", MessageBoxButton.OK);
                return;
            }
        }


        private void DeleteNode_Click(object sender, RoutedEventArgs e)
        {
            deletedItem = (TreeViewItem) TeamsView.SelectedItem;
            string req;
            if (deletedItem.Tag.ToString().Split('|')[1] == "0")
            {
                req = $"DELETE FROM Team WHERE ID = {Int32.Parse(deletedItem.Tag.ToString().Split('|')[0])}";
                TeamsView.Items.Remove(deletedItem);
            }
            else
            {
                req = $"DELETE FROM Student WHERE ID = {Int32.Parse(deletedItem.Tag.ToString().Split('|')[0])}";
                ((TreeViewItem)deletedItem.Parent).Items.Remove(deletedItem);
            }

            DatabaseDelete(req);
        }

        #endregion

        #region Renaming

        private void RenameNode_Click(object sender, RoutedEventArgs e)
        {
            renameItem = (TreeViewItem) TeamsView.SelectedItem;
            
            string[] studentInfo = renameItem.Tag.ToString().Split('|');
            if (studentInfo[1] == "0")
            {
                string themeFormulation = renameItem.Header.ToString();
                int teamID = Int32.Parse(renameItem.Tag.ToString().Split('|')[0]);
                int themeID = Int32.Parse(renameItem.Tag.ToString().Split('|')[2]);
                string materialsLink = renameItem.Tag.ToString().Split('|')[3];
                RenameTeamTeamUserControl renameTeamTeamUserControl = new RenameTeamTeamUserControl(teamID, themeID, teacherID, materialsLink, themeFormulation);
                TeamUserControlGrid.Children.Add(renameTeamTeamUserControl);
                TeamsView.Visibility = Visibility.Collapsed;
                renameTeamTeamUserControl.Visibility = Visibility.Visible;
                renameTeamTeamUserControl.IsVisibleChanged += RenameTeamTeamUserControl_IsVisibleChanged;
            }
            if (studentInfo[1] == "1")
            {
                int studentID = Int32.Parse(studentInfo[0]);
                int teamID = Int32.Parse(((TreeViewItem)((TreeViewItem)TeamsView.SelectedItem).Parent).Tag.ToString().Split('|')[0]);
                int courseID = Int32.Parse(studentInfo[5]);
                int facultyID = Int32.Parse(studentInfo[6]);
                RenameStudentTeamUserControl renameStudentTeamUserControl = new RenameStudentTeamUserControl(studentID, renameItem.Header.ToString(), studentInfo[4], courseID, facultyID, teamID, studentInfo[7], teacherID);
                TeamUserControlGrid.Children.Add(renameStudentTeamUserControl);
                TeamsView.Visibility = Visibility.Collapsed;
                renameStudentTeamUserControl.Visibility = Visibility.Visible;
                renameStudentTeamUserControl.IsVisibleChanged += RenameStudentTeamUserControl_IsVisibleChanged;
            }
        }

        private void RenameTeamTeamUserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            TeamsView.Visibility = Visibility.Visible;
            RenameTeamTeamUserControl userControl = (RenameTeamTeamUserControl) sender;

            if (!userControl.isChanged)
            {
                TeamUserControlGrid.Children.Remove(userControl);
                return;
            }

            renameItem.Header = $"{userControl.themeFormulation}";
            renameItem.Tag = $"{userControl.teamID}|0|{userControl.themeID}|{userControl.materialsLink}";

            TeamUserControlGrid.Children.Remove(userControl);
            MessageBox.Show("Запись успешно изменена!", "Успех!", MessageBoxButton.OK);
        }

        private void RenameStudentTeamUserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            TeamsView.Visibility = Visibility.Visible;
            RenameStudentTeamUserControl userControl = (RenameStudentTeamUserControl) sender;
            
            if (!userControl.isChanged)
            {
                TeamUserControlGrid.Children.Remove(userControl);
                return;
            }

            TreeViewItem updatedStudentEntry = new TreeViewItem()
            {
                Header = $"{userControl.initials}",
                Tag = $"{userControl.studentID}|1|{userControl.course}|{userControl.faculty}|{userControl.email}|{userControl.courseID}|{userControl.facultyID}|{userControl.roleID}"
            };

            ((TreeViewItem)renameItem.Parent).Items.Remove(renameItem);
            
            foreach (TreeViewItem treeViewItem in TeamsView.Items)
            {
                if (treeViewItem.Tag.ToString().Split('|')[0] == userControl.teamID.ToString())
                {
                    treeViewItem.Items.Add(updatedStudentEntry);
                }
            }

            TeamUserControlGrid.Children.Remove(userControl);
            MessageBox.Show("Запись успешно изменена!", "Успех!", MessageBoxButton.OK);
        }

        #endregion
    }
}
