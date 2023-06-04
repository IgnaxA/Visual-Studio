using OfficeOpenXml;
using System;
using System.IO;

namespace Practice.Services
{
    public class ReportService
    {
        private readonly string desktopPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "report.xlsx");
        private static List<List<string>> actionsList = new List<List<string>>();

        public void AddActionToExcel(string actionName, string fields, DateTime actionDate, string oldFields = "")
        {
            List<string> actions = new List<string>() { actionName, actionDate.ToString(), fields };
            if (oldFields != "") actions.Add(oldFields);
            actionsList.Add(actions);

        }
        public void CreateExcel()
        {
            if (!File.Exists(desktopPath))
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Действия с базой данных");
                    worksheet.Cells[1, 1].Value = "Тип действия";
                    worksheet.Cells[1, 2].Value = "Дата и время действия";
                    worksheet.Cells[1, 3].Value = "Новые поля";
                    worksheet.Cells[1, 4].Value = "Старые поля";
                    package.SaveAs(new FileInfo(desktopPath));
                }
            }

            FileInfo fileInfo = new FileInfo(desktopPath);
            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets["Действия с базой данных"];
                foreach (List<string> actions in actionsList)
                {
                    int lastRow = worksheet.Dimension.End.Row;
                    worksheet.Cells[lastRow + 1, 1].Value = actions[0];
                    worksheet.Cells[lastRow + 1, 2].Value = actions[1];
                    worksheet.Cells[lastRow + 1, 3].Value = actions[2];
                    if (actions.Count > 3)
                    {
                        worksheet.Cells[lastRow + 1, 4].Value = actions[3];
                    }

                }

                package.Save();
            }
        }

    }
}
