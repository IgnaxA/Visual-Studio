using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
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

namespace SDLab3.SaveWindows
{
	/// <summary>
	/// Логика взаимодействия для SaveStudyOfficeWindow.xaml
	/// </summary>
	public partial class SaveStudyOfficeWindow : Window
	{
		private readonly string connect = Configuration.GetConnectionString();
		private readonly bool _isAdd;
		public bool isChanged = false;
		public int primaryKey = -1;

		public SaveStudyOfficeWindow(string labelName)
		{
			InitializeComponent();
			InfoLabel.Content = labelName;
			_isAdd = true;
		}

		public SaveStudyOfficeWindow(string labelName, int id, string WorkerInitials, string Email)
		{
			InitializeComponent();
			InfoLabel.Content = labelName;
			_isAdd = false;
			primaryKey = id;
			this.WorkerInitials.Text = WorkerInitials;
			this.Email.Text = Email;
		}

		private bool CheckEmailAdress()
		{
			try
			{
				MailAddress tempAdress = new MailAddress(Email.Text);
				return true;
			}
			catch
			{
				return false;
			}
		}

		private void DatabaseAddEntry()
		{
			try
			{
				using (var databaseConnection = new SqlConnection(connect))
				{
					databaseConnection.Open();
					var req = $@"INSERT INTO StudyOffice (WorkerInitials, Email)
                                  VALUES (@studyOfficeWorkerInitials, @studyOfficeWorkerEmail); SELECT SCOPE_IDENTITY()";
					var cmd = new SqlCommand(req, databaseConnection);
					cmd.Parameters.AddWithValue("@studyOfficeWorkerInitials", WorkerInitials.Text);
					cmd.Parameters.AddWithValue("@studyOfficeWorkerEmail", Email.Text);
					var dr = cmd.ExecuteScalar();
					primaryKey = Int32.Parse(dr.ToString());
					MessageBox.Show($"Запись о {WorkerInitials.Text} успешно добавлена!", "Успешно", MessageBoxButton.OK);
				}
			}
			catch
			{
				primaryKey = -1;
				MessageBox.Show("Нет подключения к бд!", "Ошибка!", MessageBoxButton.OK);
			}
		}

		private void DatabaseRenameEntry()
		{
			try
			{
				using (var databaseConnection = new SqlConnection(connect))
				{
					databaseConnection.Open();
					var req = $@"update StudyOffice set WorkerInitials = @studyOfficeWorkerInitials, Email = @studyOfficeWorkerEmail where ID = {primaryKey}";
					var cmd = new SqlCommand(req, databaseConnection);
					cmd.Parameters.AddWithValue("@studyOfficeWorkerInitials", WorkerInitials.Text);
					cmd.Parameters.AddWithValue("@studyOfficeWorkerEmail", Email.Text);
					var dr = cmd.ExecuteNonQuery();
					isChanged = true;
					MessageBox.Show($"Запись о {WorkerInitials.Text} успешно изменена!", "Успешно", MessageBoxButton.OK);
				}
			}
			catch
			{
				isChanged = false;
				MessageBox.Show("Нет подключения к бд!", "Ошибка!", MessageBoxButton.OK);
			}
		}

		private void AddStudyOffice_Click(object sender, RoutedEventArgs e)
		{

			if (WorkerInitials.Text.Trim() == "" || WorkerInitials.Text == null || Email.Text.Trim() == "" || Email.Text == null)
			{
				MessageBox.Show("Введите непустые значения для полей!", "Ошибка!", MessageBoxButton.OK);
				return;
			}

			if (WorkerInitials.Text.Trim().Split().Length < 2)
			{
				MessageBox.Show("Введите корректные инициалы сотрудника!");
				return;
			}

			if (!CheckEmailAdress())
			{
				MessageBox.Show("Введите корректный email адрес!");
				return;
			}

			if (_isAdd) DatabaseAddEntry();
			else DatabaseRenameEntry();
			Close();
		}



	}
}
