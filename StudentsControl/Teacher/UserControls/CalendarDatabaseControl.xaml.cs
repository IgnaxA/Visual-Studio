using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Логика взаимодействия для CalendarDatabaseControl.xaml
    /// </summary>
    public partial class CalendarDatabaseControl : UserControl
    {
        private Dictionary<DateTime, List<List<string>>> deadlinesDay = new Dictionary<DateTime, List<List<string>>>();
        private readonly Config config = new Config();
        private int teacherID = -1;

        public CalendarDatabaseControl()
        {
            InitializeComponent();
        }
        
        public CalendarDatabaseControl(int teacherID)
        {
            InitializeComponent();
            this.teacherID = teacherID;
            FillCalendar();
        }
        
        private void InsertValueInDictionary(string id, string date, string commentary, string brigadeID, DateTime dateTime)
        {
            
            List<string> deadlineList = new List<string>()
            {
            id, date, commentary, brigadeID
            };

            deadlinesDay[dateTime].Add(deadlineList);
        }

        private void ColorDeadlinesDay()
        {
            foreach (DateTime date in deadlinesDay.Keys)
            {
                if (deadlinesDay[date].Count > 0)
                {
                    CalendarDayButton calendarDayButton = new CalendarDayButton();
                    calendarDayButton.DataContext = date;
                    calendarDayButton.Background = Brushes.Green;
                    CalendarView.SelectedDate = date;
                    CalendarView.IsTodayHighlighted = true;
                }
            }
        }

        private void FillCalendar()
        {
            try
            {
                using (var databaseConnection = new SqlConnection(config.connect))
                {
                    databaseConnection.Open();
                    var req = $@"SELECT Dd.ID, Dd.Date, Dd.Commentary, Dd.BrigadeID, Br.TeacherID 
                                 FROM Deadline Dd 
	                             RIGHT JOIN Brigade Br 
                                 ON Dd.BrigadeID = Br.ID
	                             WHERE Br.TeacherID = {teacherID}";
                    
                    var cmd = new SqlCommand(req, databaseConnection);

                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        DateTime dateTime = (DateTime)dr["Date"];
                        string id = dr["ID"].ToString();
                        string date = dr["Date"].ToString();
                        string commentary = dr["Commentary"].ToString();
                        string brigadeID = dr["BrigadeID"].ToString();

                        if (deadlinesDay.ContainsKey(dateTime))
                        {
                            InsertValueInDictionary(id, date, commentary, brigadeID, dateTime);
                        }
                        else
                        {
                            List<List<string>> currentDateDeadlines = new List<List<string>>();
                            deadlinesDay[dateTime] = currentDateDeadlines;
                            InsertValueInDictionary(id, date, commentary, brigadeID, dateTime);
                        }
                        

                    }
                }

                ColorDeadlinesDay();
            }
            catch
            {

            }
        }

        private void FillTreeViewForCurrentDate(DateTime currentDate)
        {
            CurrentDayDeadlines.Items.Clear();
            if (deadlinesDay.ContainsKey(currentDate))
            {
                foreach (List<string> currentDateInfo in deadlinesDay[currentDate])
                {
                    TreeViewItem treeViewItem = new TreeViewItem()
                    {
                        Header = currentDateInfo[2]
                    };

                    CurrentDayDeadlines.Items.Add(treeViewItem);
                }
            }
        }

        private void CalendarView_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CalendarView.SelectedDate != null)
            {
                FillTreeViewForCurrentDate((DateTime)CalendarView.SelectedDate);
            }
        }
    }
}
