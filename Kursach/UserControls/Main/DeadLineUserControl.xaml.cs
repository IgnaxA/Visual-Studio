using Kursach.UserControls.CRUD.Deadline;
using Kursach.UserControls.CRUD.Theme;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
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
    /// Логика взаимодействия для DeadLineUserControl.xaml
    /// </summary>
    public partial class DeadLineUserControl : UserControl
    {
        private bool isLoaded = false;
        private int teacherID = -1;
        private int status = -1;
        private TreeViewItem addingItem;
        private TreeViewItem removingItem;
        private TreeViewItem renamingItem;

        public DeadLineUserControl()
        {
            InitializeComponent();
        }
        
        public DeadLineUserControl(int teacherID)
        {
            InitializeComponent();
            this.teacherID = teacherID;
            FillDeadLineTree(-1);

        }

        public void FillDeadLineTree(int status)
        {
            try
            {
                DeadLinesView.Items.Clear();
                TreeViewItem currentItemTeam = new TreeViewItem()
                {
                    Header = "",
                    Tag = "-1|-1"
                };
                using (var databaseConnection = new SqlConnection(Config.GetConnect()))
                {
                    databaseConnection.Open();
                    var req = $@"SELECT De.ID as DeadlineID, De.DeadLineDate, De.Commentary, De.AttendanceMark, Th.ThemeFormulation, De.TeamID, Te.ThemeID
                                 FROM Deadline De
                                 LEFT JOIN Team Te
                                 ON Te.ID = De.TeamID
                                 LEFT JOIN Theme Th
                                 ON Th.ID = Te.ThemeID
                                 WHERE Th.TeacherID = {teacherID}
                                 ORDER BY Te.ID, De.DeadLineDate";
                    var cmd = new SqlCommand(req, databaseConnection);

                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        bool isContainsTeam = true;
                        TreeViewItem teamEntry = new TreeViewItem()
                        {
                            Header = $"{dr["ThemeFormulation"]}",
                            Tag = $"{dr["TeamID"]}|0|{dr["ThemeID"]}",
                            IsExpanded = true
                        };

                        if (!currentItemTeam.Tag.ToString().Split('|')[0].Equals(teamEntry.Tag.ToString().Split('|')[0]))
                        {
                            currentItemTeam = teamEntry;
                            isContainsTeam = false;
                        }

                        TreeViewItem deadlineEntry = new TreeViewItem()
                        {
                            Header = $"{dr["DeadLineDate"].ToString().Split()[0]}",
                            Tag = $"{dr["DeadlineID"]}|1|{dr["AttendanceMark"]}",
                            IsExpanded = true,
                            Foreground = Brushes.Black 
                        };

                        string temp = dr["DeadLineDate"].ToString().Split()[0];
                        DateTime t1 = DateTime.ParseExact(temp, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);

                        if (t1 < DateTime.Today && (byte)dr["AttendanceMark"] == 0)
                        {
                            deadlineEntry.Background = Brushes.Red;
                        }
                        else if ((byte)dr["AttendanceMark"] == 1)
                        {
                            deadlineEntry.Background = Brushes.LightGreen;
                        }
                        else if ((byte)dr["AttendanceMark"] == 2)
                        {
                            deadlineEntry.Background = Brushes.Yellow;
                        }


                        TreeViewItem commentaryEntry = new TreeViewItem()
                        {
                            Header = $"{dr["Commentary"]}",
                            Tag = $"-1|2"
                        };

                        deadlineEntry.Items.Add(commentaryEntry);

                        if (!isContainsTeam)
                        {
                            DeadLinesView.Items.Add(currentItemTeam);
                        }
                        currentItemTeam.Items.Add(deadlineEntry);

                    }
                    isLoaded = true;
                    ChangeVisibility(status);
                }
            }
            catch
            {
                isLoaded = false;
                MessageBox.Show("При загрузку БД произошла ошибка!");
            }
        }

        public void ChangeVisibility(int status)
        {
            this.status = status;
            foreach (TreeViewItem treeViewItem in DeadLinesView.Items)
            {
                treeViewItem.Visibility = Visibility.Visible;
                int count = 0;
                foreach (TreeViewItem date in treeViewItem.Items)
                {
                    DateTime t1 = DateTime.ParseExact(date.Header.ToString(), "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    int attendanceMark = Int32.Parse(date.Tag.ToString().Split('|')[2]);
                    if (status == -1)
                    {
                        date.Visibility = Visibility.Visible;
                    }
                    else if (status == 3 && t1 >= DateTime.Today && attendanceMark == 0)
                    {
                        date.Visibility = Visibility.Visible;
                    }
                    else if (status == 0 && t1 < DateTime.Today && status == attendanceMark)
                    {
                        date.Visibility = Visibility.Visible;
                    }
                    else if (status == attendanceMark && status != 0 && status != 3)
                    {
                        date.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        date.Visibility = Visibility.Collapsed;
                        ++count;
                    }
                }
                if (count == treeViewItem.Items.Count)
                {
                    treeViewItem.Visibility = Visibility.Collapsed;
                }
            }
        }


        #region RMC select

        private void DeadLinesView_OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem treeViewItem = VisualUpwardSearch((DependencyObject)e.OriginalSource);

            if (treeViewItem != null)
            {
                treeViewItem.IsSelected = true;
                e.Handled = true;
            }
            else if (DeadLinesView.SelectedItem != null)
            {
                TreeViewItem selectedItem = (TreeViewItem)DeadLinesView.SelectedItem;
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
            AddNode.IsEnabled = isVisible && ((TreeViewItem)DeadLinesView.SelectedItem).Tag.ToString().Split('|')[1] == "0";
            DeleteNode.IsEnabled = isVisible && ((TreeViewItem)DeadLinesView.SelectedItem).Tag.ToString().Split('|')[1] == "1";
            RenameNode.IsEnabled = isVisible && ((TreeViewItem)DeadLinesView.SelectedItem).Tag.ToString().Split('|')[1] == "1";
        }

        private void DeadLinesView_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            MakeEnabled(DeadLinesView.SelectedItem != null);
        }

        #endregion

        #region Adding

        private void AddNode_Click(object sender, RoutedEventArgs e)
        {
            addingItem = (TreeViewItem)DeadLinesView.SelectedItem;
            int teamID = Int32.Parse(addingItem.Tag.ToString().Split('|')[0]);
            AddDeadlineUserControl addDeadlineUserControl = new AddDeadlineUserControl(teacherID, teamID);
            DeadLineUserControlGrid.Children.Add(addDeadlineUserControl);
            DeadLinesView.Visibility = Visibility.Collapsed;
            addDeadlineUserControl.Visibility = Visibility.Visible;
            addDeadlineUserControl.IsVisibleChanged += AddDeadlineUserControl_IsVisibleChanged;
        }

        private void AddDeadlineUserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            DeadLinesView.Visibility = Visibility.Visible;
            AddDeadlineUserControl userControl = (AddDeadlineUserControl) sender;
            if (!userControl.isAdded)
            {
                DeadLineUserControlGrid.Children.Remove(userControl);
                return;
            }

            TreeViewItem newDeadline = new TreeViewItem()
            {
                Header = userControl.date.ToString().Split()[0],
                Tag = $"{userControl.deadlineID}|1|0",
                IsExpanded = true,
            };

            TreeViewItem newCommentary = new TreeViewItem()
            {
                Header = userControl.commentary,
                Tag = $"-1|2"
            };

            newDeadline.Items.Add(newCommentary);
            foreach (TreeViewItem treeViewItem in DeadLinesView.Items)
            {
                if (treeViewItem.Tag.ToString().Split('|')[0] == userControl.teamID.ToString())
                {
                    treeViewItem.Items.Add(newDeadline);
                    break;
                }
            }
            ChangeVisibility(status);
            DeadLineUserControlGrid.Children.Remove(userControl);
            MessageBox.Show("Запись успешно добавлена!", "Успех!", MessageBoxButton.OK);
        }

        #endregion

        #region Deleting

        private void DatabaseDelete()
        {
            try
            {
                using (var databaseConnection = new SqlConnection(Config.GetConnect()))
                {
                    databaseConnection.Open();
                    var req = $"DELETE FROM Deadline WHERE ID = {removingItem.Tag.ToString().Split('|')[0]}";
                    var cmd = new SqlCommand(req, databaseConnection);
                    var dr = cmd.ExecuteNonQuery();
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
            removingItem = (TreeViewItem) DeadLinesView.SelectedItem;
            DatabaseDelete();
            ((TreeViewItem)removingItem.Parent).Items.Remove(removingItem);
            MessageBox.Show("Дедлайн успешно удален!", "Успех!", MessageBoxButton.OK);
        }

        #endregion

        #region Renaming

        private void RenameNode_Click(object sender, RoutedEventArgs e)
        {
            renamingItem = (TreeViewItem) DeadLinesView.SelectedItem;
            int teamID = Int32.Parse(((TreeViewItem)renamingItem.Parent).Tag.ToString().Split('|')[0]), deadlineID = Int32.Parse(renamingItem.Tag.ToString().Split('|')[0]);
            string commentary = ((TreeViewItem)renamingItem.Items[0]).Header.ToString(), date = renamingItem.Header.ToString();

            AddDeadlineUserControl addDeadlineUserControl = new AddDeadlineUserControl(teacherID, teamID, deadlineID, commentary, date);
            DeadLineUserControlGrid.Children.Add(addDeadlineUserControl);
            DeadLinesView.Visibility = Visibility.Collapsed;
            addDeadlineUserControl.Visibility = Visibility.Visible;
            addDeadlineUserControl.IsVisibleChanged += RenameDeadlineUserControl_IsVisibleChanged; ;

        }

        private void RenameDeadlineUserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            DeadLinesView.Visibility = Visibility.Visible;
            AddDeadlineUserControl userControl = (AddDeadlineUserControl) sender;
            if (!userControl.isAdded)
            {
                DeadLineUserControlGrid.Children.Remove(userControl);
                return;
            }

            ((TreeViewItem)renamingItem.Parent).Items.Remove(renamingItem);

            TreeViewItem newDeadline = new TreeViewItem()
            {
                Header = userControl.date.ToString().Split()[0],
                Tag = $"{userControl.deadlineID}|1|{userControl.attendanceMark}",
                IsExpanded = true,
            };

            DateTime t1 = DateTime.ParseExact(userControl.date.ToString().Split()[0], "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);

            if (t1 < DateTime.Today && userControl.attendanceMark == 0)
            {
                newDeadline.Background = Brushes.Red;
            }
            else if (userControl.attendanceMark == 1)
            {
                newDeadline.Background = Brushes.LightGreen;
            }
            else if (userControl.attendanceMark == 2)
            {
                newDeadline.Background = Brushes.Yellow;
            }

            TreeViewItem newCommentary = new TreeViewItem()
            {
                Header = userControl.commentary,
                Tag = $"-1|2"
            };

            newDeadline.Items.Add(newCommentary);
            foreach (TreeViewItem treeViewItem in DeadLinesView.Items)
            {
                if (treeViewItem.Tag.ToString().Split('|')[0] == userControl.teamID.ToString())
                {
                    treeViewItem.Items.Add(newDeadline);
                    break;
                }
            }
            ChangeVisibility(status);
            DeadLineUserControlGrid.Children.Remove(userControl);
            MessageBox.Show("Запись успешно добавлена!", "Успех!", MessageBoxButton.OK);


        }

        #endregion



    }
}
