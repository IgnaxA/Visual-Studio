using StudentsControl.Teacher.Rename;
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
using System.Windows.Shapes;

namespace StudentsControl.Teacher.Adding
{
    /// <summary>
    /// Логика взаимодействия для AddStudent.xaml
    /// </summary>
    public partial class AddStudent : Window
    {
        private readonly Config config = new Config();
        private TreeViewItem renameItem;
        private bool isMakeRequest = false;
        private int brigadeID = -1;
        public bool isAdded = false;

        public AddStudent()
        {
            InitializeComponent();
        }

        public AddStudent(int brigadeID, bool isMakeRequest = true)
        {
            InitializeComponent();
            this.isMakeRequest = isMakeRequest;
            this.brigadeID = brigadeID;
        }

        #region Выбор элемента ПКМ
        private void BrigadesView_OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem treeViewItem = VisualUpwardSearch((DependencyObject)e.OriginalSource);

            if (treeViewItem != null)
            {
                treeViewItem.IsSelected = true;
                e.Handled = true;
            }
            else if (StudentsList.SelectedItem != null)
            {
                TreeViewItem selectedItem = (TreeViewItem)StudentsList.SelectedItem;
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
            DeleteNode.IsEnabled = isVisible;
            RenameNode.IsEnabled = isVisible;
        }

        private void ContextMenu_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            MakeEnabled(StudentsList.SelectedItem != null);
        }


        #endregion

        #region Добавление

        private void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            AddStudentEntry window = new AddStudentEntry();
            window.Closed += AddStudentEntryWindow_Closed;
            window.Owner = this;
            window.Show();
        }

        private void AddStudentEntryWindow_Closed(object sender, EventArgs e)
        {
            AddStudentEntry window = (AddStudentEntry) sender;
            if (window.studentInitials == "")
            {
                return;
            }

            TreeViewItem student = new TreeViewItem()
            {
                Header = window.studentInitials
            };
            StudentsList.Items.Add(student);
        }

        #endregion

        private int DatabaseAddEntry(string studentInitials)
        {
            int primaryKey;
            try
            {
                using (var databaseConnection = new SqlConnection(config.connect))
                {
                    databaseConnection.Open();
                    var req = $@"INSERT INTO Student (Initials, BrigadeID)
                                  VALUES ('{studentInitials}', {brigadeID}); SELECT SCOPE_IDENTITY()";
                    var cmd = new SqlCommand(req, databaseConnection);
                    var dr = cmd.ExecuteScalar();
                    primaryKey = Int32.Parse(dr.ToString());
                }
            }
            catch
            {
                isAdded = false;
                MessageBox.Show("Нет подключения к бд!", "Ошибка!", MessageBoxButton.OK);
                return -1;
            }
            return primaryKey;
        }

        private void Done_Click(object sender, RoutedEventArgs e)
        {
            if (StudentsList.Items.Count == 0)
            {
                return;
            }
            isAdded = true;
            if (!isMakeRequest)
            {
                Close();
                return;
            }

            foreach (TreeViewItem treeViewItem in StudentsList.Items)
            {
                treeViewItem.Tag = $"{DatabaseAddEntry(treeViewItem.Header.ToString())} 2";
            }
            if (isAdded) MessageBox.Show("Все записи о студентах были успешно добавлены!", "Успех!", MessageBoxButton.OK);
            Close();
        }

        private void DeleteNode_Click(object sender, RoutedEventArgs e)
        {
            StudentsList.Items.Remove(StudentsList.SelectedItem);
        }

        #region Переименование

        private void RenameNode_Click(object sender, RoutedEventArgs e)
        {
            renameItem = (TreeViewItem) StudentsList.SelectedItem;
            RenameStudent window = new RenameStudent(renameItem.Header.ToString(), false);
            window.Closed += RenameStudentWindow_Closed;
            window.Owner = this;
            window.Show();
        }

        private void RenameStudentWindow_Closed(object sender, EventArgs e)
        {
            RenameStudent window = (RenameStudent)sender;
            if (!window.isChanged)
            {
                return;
            }

            renameItem.Header = window.changedInitials;
        }

        #endregion
    }
}
