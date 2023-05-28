using Kursach.UserControls.CRUD.Team;
using Kursach.UserControls.CRUD.Theme;
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
    /// Логика взаимодействия для ThemeUserControl.xaml
    /// </summary>
    public partial class ThemeUserControl : UserControl
    {
        private int teacherID = -1;
        private bool isLoaded = false;
        private TreeViewItem renameItem;

        public ThemeUserControl()
        {
            InitializeComponent();
        }
        
        public ThemeUserControl(int teacherID)
        {
            InitializeComponent();
            this.teacherID = teacherID;
            FillThemeTree();
        }

        public void FillThemeTree()
        {
            try
            {
                ThemesView.Items.Clear();
                using (var databaseConnection = new SqlConnection(Config.GetConnect()))
                {
                    databaseConnection.Open();
                    var req = $@"SELECT Th.ThemeFormulation, Th.ID
                                 FROM Theme Th 
                                 WHERE NOT EXISTS (SELECT Te.ThemeID FROM Team Te WHERE Te.ThemeID = Th.ID) 
                                 AND Th.TeacherID = {teacherID}";
                    var cmd = new SqlCommand(req, databaseConnection);

                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        TreeViewItem themeEntry = new TreeViewItem()
                        {
                            Header = $"{dr["ThemeFormulation"]}",
                            Tag = $"{dr["ID"]}"
                        };
                        ThemesView.Items.Add(themeEntry);
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


        #region RMC select

        private void ThemesView_OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem treeViewItem = VisualUpwardSearch((DependencyObject)e.OriginalSource);

            if (treeViewItem != null)
            {
                treeViewItem.IsSelected = true;
                e.Handled = true;
            }
            else if (ThemesView.SelectedItem != null)
            {
                TreeViewItem selectedItem = (TreeViewItem)ThemesView.SelectedItem;
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

        private void ThemesView_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            MakeEnabled(ThemesView.SelectedItem != null);
        }

        #endregion

        #region Adding

        private void AddNode_Click(object sender, RoutedEventArgs e)
        {
            AddThemeUserControl addThemeUserControl = new AddThemeUserControl(teacherID, "Сохранить");
            ThemeUserControlGrid.Children.Add(addThemeUserControl);
            ThemesView.Visibility = Visibility.Collapsed;
            addThemeUserControl.Visibility = Visibility.Visible;
            addThemeUserControl.IsVisibleChanged += AddThemeUserControl_IsVisibleChanged;
        }

        private void AddThemeUserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ThemesView.Visibility = Visibility.Visible;
            AddThemeUserControl userControl = (AddThemeUserControl) sender;
            if (!userControl.isAdded)
            {
                return;
            }
            TreeViewItem treeViewItem = new TreeViewItem()
            {
                Header = userControl.themeFormulation,
                Tag = userControl.themeID
            };
            ThemesView.Items.Add(treeViewItem);
            ThemeUserControlGrid.Children.Remove(userControl);
            MessageBox.Show("Тема добавлена!", "Успех!", MessageBoxButton.OK);

        }

        #endregion

        #region Deleting

        private bool DatabaseDelete(int themeID)
        {
            try
            {
                using (var databaseConnection = new SqlConnection(Config.GetConnect()))
                {
                    databaseConnection.Open();
                    var req = $@"DELETE FROM Theme WHERE ID = {themeID}";
                    var cmd = new SqlCommand(req, databaseConnection);
                    var dr = cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch
            {
                MessageBox.Show("Нет подключения к бд!", "Ошибка!", MessageBoxButton.OK);
                return false;
            }
        }

        private void DeleteNode_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem deleteItem = (TreeViewItem)ThemesView.SelectedItem;
            if (DatabaseDelete(Int32.Parse(deleteItem.Tag.ToString())))
            {
                ThemesView.Items.Remove(deleteItem);
                MessageBox.Show("Запись удалена!", "Успех!", MessageBoxButton.OK);
            }

        }

        #endregion

        #region Renaming

        private void RenameNode_Click(object sender, RoutedEventArgs e)
        {
            renameItem = (TreeViewItem)ThemesView.SelectedItem;
            AddThemeUserControl addThemeUserControl = new AddThemeUserControl(teacherID, "Изменить", Int32.Parse(renameItem.Tag.ToString()), renameItem.Header.ToString());
            ThemeUserControlGrid.Children.Add(addThemeUserControl);
            ThemesView.Visibility = Visibility.Collapsed;
            addThemeUserControl.Visibility = Visibility.Visible;
            addThemeUserControl.IsVisibleChanged += RenameThemeUserControl_IsVisibleChanged; ;
        }

        private void RenameThemeUserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ThemesView.Visibility = Visibility.Visible;
            AddThemeUserControl userControl = (AddThemeUserControl)sender;
            if (!userControl.isAdded)
            {
                return;
            }
            renameItem.Header = userControl.themeFormulation;
            ThemeUserControlGrid.Children.Remove(userControl);
            MessageBox.Show("Тема изменена!", "Успех!", MessageBoxButton.OK);
        }

        #endregion
    }
}
