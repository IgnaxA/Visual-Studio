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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace SDLab3.SaveWindows
{
	/// <summary>
	/// Логика взаимодействия для SaveThemeWindow.xaml
	/// </summary>
	public partial class SaveThemeWindow : Window
	{
		private string connect = Configuration.GetConnectionString();
		public bool isChanged = false;
		private readonly bool _isAdd;
		private int teacherID;
		public int primaryKey = -1;

		public SaveThemeWindow(string labelInfo, int teacherID)
		{
			InitializeComponent();
			_isAdd = true;
			InfoLabel.Content = labelInfo;
			this.teacherID = teacherID;
		}
		
		public SaveThemeWindow(string labelInfo, int id, string themeFormulation)
		{
			InitializeComponent();
			_isAdd = false;
			InfoLabel.Content = labelInfo;
			primaryKey = id;
			ThemeFormulation.Text = themeFormulation;
		}

		private void DatabaseAddEntry()
		{
			try
			{
				using (var databaseConnection = new SqlConnection(connect))
				{
					databaseConnection.Open();
					var req = $@"INSERT INTO Theme (ThemeFormulation, TeacherID)
                                  VALUES (@themeFormulation, @teacherID); SELECT SCOPE_IDENTITY()";
					var cmd = new SqlCommand(req, databaseConnection);
					cmd.Parameters.AddWithValue("@themeFormulation", ThemeFormulation.Text);
					cmd.Parameters.AddWithValue("@teacherID", teacherID);
					var dr = cmd.ExecuteScalar();
					primaryKey = Int32.Parse(dr.ToString());
					MessageBox.Show($"Запись о теме: '{ThemeFormulation.Text}' успешно добавлена!", "Успешно", MessageBoxButton.OK);
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
					var req = $@"update Theme set ThemeFormulation = @themeFormulation where ID = {primaryKey}";
					var cmd = new SqlCommand(req, databaseConnection);
					cmd.Parameters.AddWithValue("@themeFormulation", ThemeFormulation.Text);
					var dr = cmd.ExecuteNonQuery();
					isChanged = true;
					MessageBox.Show($"Запись о {ThemeFormulation.Text} успешно изменена!", "Успешно", MessageBoxButton.OK);
				}
			}
			catch
			{
				isChanged = false;
				MessageBox.Show("Нет подключения к бд!", "Ошибка!", MessageBoxButton.OK);
			}
		}

		private void AddTheme_Click(object sender, RoutedEventArgs e)
		{
			if (ThemeFormulation.Text == null || ThemeFormulation.Text.Trim() == "")
			{
				MessageBox.Show("Введите непустое значение для поля!", "Ошибка!", MessageBoxButton.OK);
				return;
			}
			if (_isAdd) DatabaseAddEntry();
			else DatabaseRenameEntry();
			Close();
		}
	}
}
