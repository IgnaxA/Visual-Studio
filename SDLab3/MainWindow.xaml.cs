using SDLab3.AddWindows;
using SDLab3.RenameWindows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SDLab3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TreeViewItem currentAddSelectedItem;
        private TreeViewItem currentRemoveSelectedItem;
        private TreeViewItem currentRenameSelectedItem;
        private string connect = @"data source=.\SQLEXPRESS;initial catalog=SDLab3;trusted_connection=true";
        private bool canCreateNewWindow = true;
        private bool isLoaded = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Загрузка БД (Готово)

        private void Button_Load_Click(object sender = null, RoutedEventArgs e = null)
        {
            TreeViewItem currentItemStudyOffice = new TreeViewItem()
            {
                Header = "",
                Tag = "-1 -1"
            };
            TreeViewItem currentItemTeacher = new TreeViewItem()
            {
                Header = "",
                Tag = "-1 -1"
            };
            try
            {
                DatabaseView.Items.Clear();
                using (var databaseConnection = new SqlConnection(connect))
                {
                    databaseConnection.Open();
                    var req = @"select Th.ID as ThemeID, Th.ThemeFormulation, Te.ID as TeacherID, Te.TeacherInitials, SO.ID as WorkerID, SO.WorkerInitials, SO.Email
		                        from Theme Th right join Teacher Te 
		                        on Th.TeacherID = Te.ID
		                        right join StudyOffice SO on Te.StudyOfficeID = SO.ID";
                    var cmd = new SqlCommand(req, databaseConnection);

                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        bool isContainsWorkerEntry = true, isContainsTeacherEntry = true;
                        TreeViewItem workerEntry = new TreeViewItem()
                        {
                            Header = dr["WorkerInitials"],
                            Tag = $"{dr["WorkerID"]} 0 {dr["Email"]}"
                        };

                        
                        if (!currentItemStudyOffice.Tag.ToString().Split()[0].Equals(workerEntry.Tag.ToString().Split()[0]))
                        {
                            currentItemStudyOffice = workerEntry;
                            isContainsWorkerEntry = false;
                        }

                        if (dr["TeacherInitials"] == null || dr["TeacherInitials"].ToString() == "")
                        {
                            DatabaseView.Items.Add(currentItemStudyOffice);
                            continue;
                        }

                        TreeViewItem teacherEntry = new TreeViewItem()
                        {
                            Header = dr["TeacherInitials"],
                            Tag = $"{dr["TeacherID"]} 1 {dr["WorkerID"]}"
                        };

                        if (!teacherEntry.Tag.ToString().Split()[0].Equals(currentItemTeacher.Tag.ToString().Split()[0]))
                        {
                            currentItemTeacher = teacherEntry;
                            isContainsTeacherEntry = false;
                        }

                        if (dr["ThemeFormulation"] == null || dr["ThemeFormulation"].ToString() == "")
                        {
                            currentItemStudyOffice.Items.Add(currentItemTeacher);
                            if (!isContainsWorkerEntry) DatabaseView.Items.Add(currentItemStudyOffice);
                            continue;
                        }

                        TreeViewItem themeEntry = new TreeViewItem()
                        {
                            Header = dr["ThemeFormulation"],
                            Tag = $"{dr["ThemeID"]} 2"
                        };
                        currentItemTeacher.Items.Add(themeEntry);
                        if (!isContainsTeacherEntry) currentItemStudyOffice.Items.Add(currentItemTeacher);
                        if (!isContainsWorkerEntry) DatabaseView.Items.Add(currentItemStudyOffice);
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

        #region Выбор элемента ПКМ (Готово)

        private void DatabaseView_OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem treeViewItem = VisualUpwardSearch((DependencyObject) e.OriginalSource);

            if (treeViewItem != null)
            {
                treeViewItem.IsSelected = true;
                e.Handled = true;
            }
            else if (DatabaseView.SelectedItem != null)
            {
                TreeViewItem selectedItem = (TreeViewItem) DatabaseView.SelectedItem;
                selectedItem.IsSelected = false;
                e.Handled = true;
            }
        }

        static TreeViewItem VisualUpwardSearch(DependencyObject source)
        {
            while (source != null && !(source is TreeViewItem))
                source = VisualTreeHelper.GetParent(source);

            return (TreeViewItem) source;
        }

        private void MakeEnabled(bool isVisible)
        {
            AddNode.IsEnabled = isLoaded;
            DeleteNode.IsEnabled = isVisible;
            RenameNode.IsEnabled = isVisible;
        }

        private void ContextMenu_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            MakeEnabled(DatabaseView.SelectedItem != null);
            AddNode.Visibility = isLoaded && DatabaseView.SelectedItem != null && ((TreeViewItem)DatabaseView.SelectedItem).Tag.ToString().Split()[1] == "2" ? Visibility.Collapsed : AddNode.Visibility = Visibility.Visible;
        }

        #endregion

        #region Добавление записи (Готов)

        private void AddNode_Click(object sender, RoutedEventArgs e)
        {
            if (!canCreateNewWindow)
            {
                MessageBox.Show("Перед тем, как открыть новое окно добавления, закройте предыдущее!");
                return;
            }
            canCreateNewWindow = false;
            string level = "", id = "";

            if (DatabaseView.SelectedItem != null)
            {
                level = ((TreeViewItem)DatabaseView.SelectedItem).Tag.ToString().Split()[1];
                id = ((TreeViewItem)DatabaseView.SelectedItem).Tag.ToString().Split()[0];
            }

            currentAddSelectedItem = (TreeViewItem)DatabaseView.SelectedItem;
            if (DatabaseView.SelectedItem == null)
            {
                AddStudyOfficeWindow addWindow = new AddStudyOfficeWindow(connect);
                addWindow.Show();
                addWindow.Closed += AddStudyOfficeWindow_Closed;
            }
            else if (level == "0")
            {
                AddTeacherWindow addWindow = new AddTeacherWindow(connect, Int32.Parse(id));
                addWindow.Show();
                addWindow.Closed += AddTeacherWindow_Closed;
            }
            else if (level == "1")
            {
                AddThemeWindow addWindow = new AddThemeWindow(connect, Int32.Parse(id));
                addWindow.Show();
                addWindow.Closed += AddThemeWindow_Closed;
            }
        }

        private void AddStudyOfficeWindow_Closed(object sender, EventArgs e)
        {
            canCreateNewWindow = true;
            AddStudyOfficeWindow window = (AddStudyOfficeWindow)sender;
            if (window.primaryKey == -1)
            {
                return;
            }

            TreeViewItem treeViewItem = new TreeViewItem()
            {
                Header = $"{window.WorkerInitials.Text}",
                Tag = $"{window.primaryKey} 0 {window.Email.Text}"
            };
            DatabaseView.Items.Add(treeViewItem);
        }

        private void AddTeacherWindow_Closed(object sender, EventArgs e)
        {
            canCreateNewWindow = true;
            AddTeacherWindow window = (AddTeacherWindow)sender;
            if (window.primaryKey == -1)
            {
                return;
            }

            TreeViewItem treeViewItem = new TreeViewItem()
            {
                Header = $"{window.WorkerInitials.Text}",
                Tag = $"{window.primaryKey} 1 {currentAddSelectedItem.Tag.ToString().Split()[0]}"
            };

            currentAddSelectedItem.Items.Add(treeViewItem);
        }

        private void AddThemeWindow_Closed(object sender, EventArgs e)
        {
            canCreateNewWindow = true;
            AddThemeWindow window = (AddThemeWindow)sender;

            if (window.primaryKey == -1)
            {
                return;
            }
            
            TreeViewItem treeViewItem = new TreeViewItem()
            {
                Header = window.ThemeFormulation.Text,
                Tag = $"{window.primaryKey} 2"
            };
            currentAddSelectedItem.Items.Add(treeViewItem);

        }

        #endregion

        #region Удаление записи (Готово)

        private bool DeleteTable(int id, string req)
        {
            try
            {
                using (var databaseConnection = new SqlConnection(connect))
                {
                    databaseConnection.Open();
                    var cmd = new SqlCommand(req, databaseConnection);
                    cmd.Parameters.AddWithValue("@id", id);
                    var dr = cmd.ExecuteNonQuery();
                    MessageBox.Show($"Запись успешно удалена!", "Успешно", MessageBoxButton.OK);
                    return true;
                }
            }
            catch
            {
                MessageBox.Show("Нет подключения к бд!", "Ошибка!", MessageBoxButton.OK);
                return false;
            }
        }

        private void DeleteNode_Click(object sender, RoutedEventArgs e)
        {
            string req = $@"delete from @table where ID = @id";
            string level = ((TreeViewItem)DatabaseView.SelectedItem).Tag.ToString().Split()[1];
            string id = ((TreeViewItem)DatabaseView.SelectedItem).Tag.ToString().Split()[0];
            currentRemoveSelectedItem = (TreeViewItem)DatabaseView.SelectedItem;
            if (level == "0")
            {
                req = $@"delete from StudyOffice where ID = @id";
            }
            else if (level == "1")
            {
                req = $@"delete from Teacher where ID = @id";
            }
            else if (level == "2")
            {
                req = $@"delete from Theme where ID = @id";
            }

            if (DeleteTable(Int32.Parse(id), req)) UpdateTreeAfterDelete(Int32.Parse(level));
        }

        private void UpdateTreeAfterDelete(int itenLevel)
        {
            if (itenLevel == 0)
            {
                DatabaseView.Items.Remove(currentRemoveSelectedItem);
            }
            if (itenLevel == 1 || itenLevel == 2)
            {
                ((TreeViewItem)currentRemoveSelectedItem.Parent).Items.Remove(currentRemoveSelectedItem);
            }
        }

        #endregion

        #region Переименование

        private void RenameNode_Click(object sender, RoutedEventArgs e)
        {
            string level = ((TreeViewItem)DatabaseView.SelectedItem).Tag.ToString().Split()[1];
            string id = ((TreeViewItem)DatabaseView.SelectedItem).Tag.ToString().Split()[0];

            currentRenameSelectedItem = (TreeViewItem)DatabaseView.SelectedItem;
            if (level == "0")
            {
                RenameStudyOfficeWindow window = new RenameStudyOfficeWindow(connect, Int32.Parse(id), ((TreeViewItem)DatabaseView.SelectedItem).Header.ToString(), ((TreeViewItem)DatabaseView.SelectedItem).Tag.ToString().Split()[2]);
                window.Show();
                window.Closed += RenameStudyOfficeWindow_Closed;
            }
            if (level == "1")
            {
                RenameTeacherWindow window = new RenameTeacherWindow(connect, Int32.Parse(id), ((TreeViewItem)DatabaseView.SelectedItem).Header.ToString(), Int32.Parse(((TreeViewItem)DatabaseView.SelectedItem).Tag.ToString().Split()[2]));
                window.Show();
                window.Closed += RenameTeacher_Closed;
            }
        }

        private void RenameTeacher_Closed(object sender, EventArgs e)
        {
            RenameTeacherWindow window = (RenameTeacherWindow) sender;

            if (!window.isChanged)
            {
                return;
            }

            currentRenameSelectedItem.Header = window.WorkerInitials.Text;
        }

        private void RenameStudyOfficeWindow_Closed(object sender, EventArgs e)
        {
            RenameStudyOfficeWindow window = (RenameStudyOfficeWindow) sender;

            if (!window.isChanged)
            {
                return;
            }

            currentRenameSelectedItem.Header = window.WorkerInitials.Text;
            List<string> currentTag = new List<string>(currentRenameSelectedItem.Tag.ToString().Split());
            currentTag[2] = window.Email.Text;
            currentRenameSelectedItem.Tag = $"{currentTag[0]} {currentTag[1]} {currentTag[2]}";
        }
        
        #endregion

    }
}
