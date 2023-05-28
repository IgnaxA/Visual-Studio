using StudentsControl.Teacher.Adding;
using StudentsControl.Teacher.Rename;
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

namespace StudentsControl.Teacher.UserControls
{
    /// <summary>
    /// Логика взаимодействия для StudentDatabaseControl.xaml
    /// </summary>
    public partial class StudentDatabaseControl : UserControl
    {
        private readonly Config config = new Config();
        private int teacherID = -1;
        private bool isLoaded = false;
        private TreeViewItem currentRenamingItem;
        private TreeViewItem currentAddingItem;
        private TreeViewItem currentDeleteItem;
        private int count = 1;

        public StudentDatabaseControl()
        {
            InitializeComponent();
        }

        public StudentDatabaseControl(int teacherID)
        {
            InitializeComponent();
            this.teacherID = teacherID;
            FillBrigadeTree();
        }

        #region Загрузка бд

        private void FillBrigadeTree()
        {
            try
            {
                BrigadesView.Items.Clear();
                TreeViewItem currentItemBrigade = new TreeViewItem()
                {
                    Header = "",
                    Tag = "-1 -1"
                };
                TreeViewItem currentItemRoster = new TreeViewItem()
                {
                    Header = "Состав бригады:",
                    Tag = "0 1"
                };
                count = 1;
                using (var databaseConnection = new SqlConnection(config.connect))
                {
                    databaseConnection.Open();
                    var req = $@"SELECT St.BrigadeID as BrigadeID, St.ID as StudentID, St.Initials as StudentInitials, Th.ID as ThemeID,Th.ThemeFormulation as ThemeFormulation
                                FROM Student St 
                                RIGHT JOIN Brigade Br 
                                ON St.BrigadeID = Br.ID 
                                LEFT JOIN Theme Th 
                                ON Br.ThemeID = Th.ID
                                WHERE Br.TeacherID = {teacherID}";
                    var cmd = new SqlCommand(req, databaseConnection);

                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        bool isContainsBrigade = true;
                        TreeViewItem brigadeEntry = new TreeViewItem()
                        {
                            Header = $"{count} бригада:",
                            Tag = $"{dr["BrigadeID"]} 0"
                        };


                        if (!currentItemBrigade.Tag.ToString().Split()[0].Equals(brigadeEntry.Tag.ToString().Split()[0]))
                        {
                            currentItemBrigade = brigadeEntry;
                            currentItemRoster = new TreeViewItem()
                            {
                                Header = "Состав бригады:",
                                Tag = "0 1"
                            };
                            isContainsBrigade = false;
                        }

                        TreeViewItem studentEntry = new TreeViewItem()
                        {
                            Header = dr["StudentInitials"],
                            Tag = $"{dr["StudentID"]} 2"
                        };

                        TreeViewItem themeEntry = new TreeViewItem()
                        {
                            Header = dr["ThemeFormulation"],
                            Tag = $"{dr["ThemeID"]} 3"
                        };

                        if (!isContainsBrigade)
                        {
                            BrigadesView.Items.Add(currentItemBrigade);
                            currentItemBrigade.Items.Add(themeEntry);
                            currentItemBrigade.Items.Add(currentItemRoster);
                            ++count;
                        }
                        currentItemRoster.Items.Add(studentEntry);

                    }
                    isLoaded = true;
                }
            }
            catch
            {
                isLoaded = false;
                MessageBox.Show("При загрузку БД произошла ошибка!");
            }
        }

        #endregion

        #region Выбор элемента ПКМ

        private void BrigadesView_OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem treeViewItem = VisualUpwardSearch((DependencyObject)e.OriginalSource);

            if (treeViewItem != null)
            {
                treeViewItem.IsSelected = true;
                e.Handled = true;
            }
            else if (BrigadesView.SelectedItem != null)
            {
                TreeViewItem selectedItem = (TreeViewItem)BrigadesView.SelectedItem;
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
            AddNode.IsEnabled = isLoaded && (BrigadesView.SelectedItem == null || (BrigadesView.SelectedItem != null && ((TreeViewItem)BrigadesView.SelectedItem).Tag.ToString().Split()[1] == "1"));
            DeleteNode.IsEnabled = isVisible && ((TreeViewItem)BrigadesView.SelectedItem).Tag.ToString().Split()[1] != "3";
            RenameNode.IsEnabled = IsLoaded && BrigadesView.SelectedItem != null && (((TreeViewItem)BrigadesView.SelectedItem).Tag.ToString().Split()[1] == "2" || ((TreeViewItem)BrigadesView.SelectedItem).Tag.ToString().Split()[1] == "3");
        }

        private void ContextMenu_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            MakeEnabled(BrigadesView.SelectedItem != null);
        }

        #endregion

        #region Добавление

        private void AddNode_Click(object sender, RoutedEventArgs e)
        {
            if (BrigadesView.SelectedItem != null) currentAddingItem = (TreeViewItem)BrigadesView.SelectedItem;
            if (BrigadesView.SelectedItem == null)
            {
                AddBrigade window = new AddBrigade(teacherID);
                window.Closed += AddBrigadeWindow_Closed;
                window.Owner = Window.GetWindow(this);
                window.Show();
            }
            if (BrigadesView.SelectedItem != null && currentAddingItem.Tag.ToString().Split()[1] == "1")
            {
                AddStudent window = new AddStudent(Int32.Parse(((TreeViewItem)((TreeViewItem)BrigadesView.SelectedItem).Parent).Tag.ToString().Split()[0]));
                window.Closed += AddStudentsWindow_Closed;
                window.Owner = Window.GetWindow(this);
                window.Show();
            }
        }

        private void AddBrigadeWindow_Closed(object sender, EventArgs e)
        {
            AddBrigade window = (AddBrigade) sender;
            if (!window.isAdded)
            {
                return;
            }

            TreeViewItem brigade = new TreeViewItem()
            {
                Header = $"{count++} бригада:",
                Tag = $"{window.primaryKey} 0"
            };

            TreeViewItem theme = new TreeViewItem()
            {
                Header = $"{window.themeFormulation}",
                Tag = $"{window.themePrimaryKey} 3"
            };
            brigade.Items.Add(theme);

            TreeViewItem brigadeRoster = new TreeViewItem()
            {
                Header = "Состав бригады:",
                Tag = $"0 1"
            };

            foreach (TreeViewItem treeViewItem in window.StudentsList.Items)
            {
                TreeViewItem student = new TreeViewItem()
                {
                    Header = treeViewItem.Header.ToString(),
                    Tag = treeViewItem.Tag.ToString(),
                };
                brigadeRoster.Items.Add(student);
            }
            brigade.Items.Add(brigadeRoster);
            BrigadesView.Items.Add(brigade);
        }

        private void AddStudentsWindow_Closed(object sender, EventArgs e)
        {
            AddStudent window = (AddStudent)sender;
            if (window.StudentsList.Items.Count == 0 || !window.isAdded) return;

            foreach (TreeViewItem treeViewItem in window.StudentsList.Items)
            {
                TreeViewItem temp = new TreeViewItem()
                {
                    Header = treeViewItem.Header,
                    Tag = treeViewItem.Tag,
                };
                currentAddingItem.Items.Add(temp);
            }
        }

        #endregion

        #region Удаление

        private bool DeleteTable(string req, bool showMessage = true)
        {
            try
            {
                using (var databaseConnection = new SqlConnection(config.connect))
                {
                    databaseConnection.Open();
                    var cmd = new SqlCommand(req, databaseConnection);
                    var dr = cmd.ExecuteNonQuery();
                    if (showMessage) MessageBox.Show($"Запись успешно удалена!", "Успешно", MessageBoxButton.OK);
                    return true;
                }
            }
            catch
            {
                MessageBox.Show("Нет подключения к бд!", "Ошибка!", MessageBoxButton.OK);
                return false;
            }
        }

        private void DeleteStudentsFromBrigade()
        {
            bool isDeleted = true;
            foreach (TreeViewItem treeViewItem in currentDeleteItem.Items)
            {
                string req = $@"DELETE FROM Student where ID = {treeViewItem.Tag.ToString().Split()[0]}";
                isDeleted = DeleteTable(req, false);
                if (!isDeleted)
                {
                    MessageBox.Show("Не получилось удалить запись!", "Ошибка!", MessageBoxButton.OK);
                    return;
                }
            }
            MessageBox.Show("Все студенты из бригады были удалены!", "Успех!", MessageBoxButton.OK);
        }

        private void DeleteNode_Click(object sender, RoutedEventArgs e)
        {
            currentDeleteItem = (TreeViewItem)BrigadesView.SelectedItem;
            string level = currentDeleteItem.Tag.ToString().Split()[1];
            string req = "";
            int id = Int32.Parse(currentDeleteItem.Tag.ToString().Split()[0]);

            if (level == "0")
            {
                req = $@"DELETE FROM Brigade where ID = {id}";
            }
            if (level == "1")
            {
                DeleteStudentsFromBrigade();
            }
            if (level == "2")
            {
                req = $@"DELETE FROM Student where ID = {id}";
            }
            if (level == "0" || level == "2") DeleteTable(req);
            DeleteEntry(level);
        }

        private void UpdateMainNodesHeader()
        {
            count = 1;
            foreach (TreeViewItem treeViewItem in BrigadesView.Items)
            {
                treeViewItem.Header = $"{count++} бригада:";
            }
        }

        private void DeleteEntry(string level)
        {
            if (level == "0")
            {
                BrigadesView.Items.Remove(currentDeleteItem);
                UpdateMainNodesHeader();
            }
            if (level == "1")
            {
                currentDeleteItem.Items.Clear();
            }
            if (level == "2") ((TreeViewItem)currentDeleteItem.Parent).Items.Remove(currentDeleteItem);
        }

        #endregion

        #region Переименование

        private void RenameNode_Click(object sender, RoutedEventArgs e)
        {
            currentRenamingItem = (TreeViewItem)BrigadesView.SelectedItem;
            string renameEntry = currentRenamingItem.Tag.ToString().Split()[1];
            string renameItem = currentRenamingItem.Header.ToString();
            int renameEntryID = Int32.Parse(currentRenamingItem.Tag.ToString().Split()[0]);
            if (renameEntry == "2")
            {
                RenameStudent window = new RenameStudent(renameItem, renameEntryID);
                window.Closed += RenameStudentWindow_Closed;
                window.Owner = Window.GetWindow(this);
                window.Show();
            }
            if (renameEntry == "3")
            {
                RenameTheme window = new RenameTheme(renameItem, renameEntryID);
                window.Closed += RenameThemeWindow_Closed;
                window.Owner = Window.GetWindow(this);
                window.Show();
            }
        }

        private void RenameThemeWindow_Closed(object sender, EventArgs e)
        {
            RenameTheme window = (RenameTheme)sender;
            if (!window.isChanged)
            {
                return;
            }

            currentRenamingItem.Header = window.changedThemeFormulation;
        }

        private void RenameStudentWindow_Closed(object sender, EventArgs e)
        {
            RenameStudent window = (RenameStudent)sender;
            if (!window.isChanged)
            {
                return;
            }

            currentRenamingItem.Header = window.changedInitials;
        }

        #endregion
    }
}
