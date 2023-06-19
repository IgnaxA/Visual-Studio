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
using System.Windows.Shapes;

namespace SDLab3.SaveWindows
{
	public partial class SaveTeacherWindos : Window
	{
		private readonly string connect = Configuration.GetConnectionString();
		public bool isChanged = false;
		private bool _isAdd;
		private int studyOfficeID;
		public int primaryKey = -1;

		public SaveTeacherWindos(string infoLabel, int studyOfficeID)
		{
			InitializeComponent();
			_isAdd = true;
			InfoLabel.Content = infoLabel;
			this.studyOfficeID = studyOfficeID;
		}

		public SaveTeacherWindos(string infoLabel, int id, string WorkerInitials, int studyOfficeID)
		{
			InitializeComponent();
			_isAdd = false;
			InfoLabel.Content = infoLabel;
			this.primaryKey = id;
			this.WorkerInitials.Text = WorkerInitials;
			this.studyOfficeID = studyOfficeID;
		}

		private void DatabaseAddEntry()
		{
			try
			{
				using (var databaseConnection = new SqlConnection(connect))
				{
					databaseConnection.Open();
					var req = $@"INSERT INTO Teacher (TeacherInitials, StudyOfficeID)
                                  VALUES (@teacherInitials, @studyOfficeID); SELECT SCOPE_IDENTITY()";
					var cmd = new SqlCommand(req, databaseConnection);
					cmd.Parameters.AddWithValue("@teacherInitials", WorkerInitials.Text);
					cmd.Parameters.AddWithValue("@studyOfficeID", studyOfficeID);
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
					var req = $@"update Teacher set TeacherInitials = @teacherInitials where ID = {primaryKey}";
					var cmd = new SqlCommand(req, databaseConnection);
					cmd.Parameters.AddWithValue("@teacherInitials", WorkerInitials.Text);
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


		private void AddTeacher_Click(object sender, RoutedEventArgs e)
		{
			if (WorkerInitials.Text == null || WorkerInitials.Text.Trim() == "")
			{
				MessageBox.Show("Введите непустое значение для поля!", "Ошибка!", MessageBoxButton.OK);
				return;
			}

			if (WorkerInitials.Text.Trim().Split().Length < 2)
			{
				MessageBox.Show("Введите корректные инициалы сотрудника!");
				return;
			}

			if (_isAdd) DatabaseAddEntry();
			else DatabaseRenameEntry();
			Close();
		}

	}
}
