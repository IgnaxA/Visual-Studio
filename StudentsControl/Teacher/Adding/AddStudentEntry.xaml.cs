using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Логика взаимодействия для AddStudentEntry.xaml
    /// </summary>
    public partial class AddStudentEntry : Window
    {
        public string studentInitials = "";

        public AddStudentEntry()
        {
            InitializeComponent();
        }

        #region Обработка ввода
        private void StudentInitials_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex r = new Regex("[^А-Яа-я]+");

            e.Handled = r.IsMatch(e.Text);
        }

        private void StudentInitials_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                if (StudentInitials.Text.Length == 0 || (StudentInitials.Text.Length != 0 && StudentInitials.Text[StudentInitials.Text.Length - 1] == ' '))
                {
                    e.Handled = true;
                }
            }
        }
        #endregion

        private void AddEntry_Click(object sender, RoutedEventArgs e)
        {
            if (StudentInitials.Text.Trim().Split().Length < 2)
            {
                MessageBox.Show("Введите корректные инициалы студента!", "Ошибка!", MessageBoxButton.OK);
                return;
            }
            studentInitials = StudentInitials.Text;
            Close();
        }
    }
}
