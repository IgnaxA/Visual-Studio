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
    /// Логика взаимодействия для ThemeControl.xaml
    /// </summary>
    public partial class ThemeDatabaseControl : UserControl
    {
        private readonly Config config = new Config();
        private int teacherID = -1;
        private bool isLoaded = false;
        private TreeViewItem currentRenamingItem;

        public ThemeDatabaseControl()
        {
            InitializeComponent();
        }

        public ThemeDatabaseControl(int teacherID)
        {
            InitializeComponent();
            this.teacherID = teacherID;
            FillThemeTree();
        }

        #region Загрузка бд

        public void FillThemeTree()
        {
            try
            {
                ThemeView.Items.Clear();
                using (var databaseConnection = new SqlConnection(config.connect))
                {
                    databaseConnection.Open();
                    var req = $@"SELECT Th.ThemeFormulation, Th.ID
                                 FROM Theme Th 
                                 WHERE NOT EXISTS (SELECT Br.ThemeID FROM Brigade Br WHERE Br.ThemeID = Th.ID) 
                                 AND Th.TeacherID = {teacherID}";
                    var cmd = new SqlCommand(req, databaseConnection);

                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        TreeViewItem themeEntry = new TreeViewItem()
                        {
                            Header = dr["ThemeFormulation"].ToString(),
                            Tag = $"{dr["ID"]} 0"
                        };

                        ThemeView.Items.Add(themeEntry);
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

        private void ThemeView_OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem treeViewItem = VisualUpwardSearch((DependencyObject)e.OriginalSource);

            if (treeViewItem != null)
            {
                treeViewItem.IsSelected = true;
                e.Handled = true;
            }
            else if (ThemeView.SelectedItem != null)
            {
                TreeViewItem selectedItem = (TreeViewItem)ThemeView.SelectedItem;
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
            AddNode.IsEnabled = isLoaded && !isVisible;
            DeleteNode.IsEnabled = isVisible;
            RenameNode.IsEnabled = isVisible;
        }

        private void ContextMenu_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            MakeEnabled(ThemeView.SelectedItem != null);
        }

        #endregion

        #region Добавление

        private void AddNode_Click(object sender, RoutedEventArgs e)
        {
            AddTheme window = new AddTheme(teacherID);
            window.Closed += AddThemeWindow_Closed;
            window.Owner = Window.GetWindow(this);
            window.Show();
        }

        private void AddThemeWindow_Closed(object sender, EventArgs e)
        {
            AddTheme window = (AddTheme)sender;
            if (!window.isAdded) return;

            TreeViewItem treeViewItem = new TreeViewItem()
            {
                Header = window.themeFormulation,
                Tag = $"{window.primaryKey} 0"
            };
            
            ThemeView.Items.Add(treeViewItem);
        }

        #endregion

        #region Удаление

        private bool DeleteTable(int themeID)
        {
            try
            {
                using (var databaseConnection = new SqlConnection(config.connect))
                {
                    databaseConnection.Open();
                    string req = $@"DELETE FROM Theme WHERE ID = {themeID}";
                    var cmd = new SqlCommand(req, databaseConnection);
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
            int themeID = Int32.Parse(((TreeViewItem)ThemeView.SelectedItem).Tag.ToString().Split()[0]);
            if (DeleteTable(themeID)) ThemeView.Items.Remove(ThemeView.SelectedItem);
        }

        #endregion

        #region Переименование

        private void RenameNode_Click(object sender, RoutedEventArgs e)
        {
            currentRenamingItem = (TreeViewItem)ThemeView.SelectedItem;
            string renameEntry = currentRenamingItem.Header.ToString();
            int renameEntryID = Int32.Parse(currentRenamingItem.Tag.ToString().Split()[0]);
            RenameTheme window = new RenameTheme(renameEntry, renameEntryID);
            window.Closed += RenameThemeWindow_Closed;
            window.Owner = Window.GetWindow(this);
            window.Show();

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

        #endregion

    }
}
