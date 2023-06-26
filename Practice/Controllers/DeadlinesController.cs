using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Practice.Data.Interface;
using Practice.Services;
using Practice.ViewModels;

namespace Practice.Controllers
{
    public class DeadlinesController : Controller
    {
        private readonly ITeachers _teachers;
        private readonly IDeadlines _deadlines;
        private readonly IConsultations _consultations;
        private readonly ITeams _teams;
        private readonly ReportService _report;

        public DeadlinesController(ITeachers teachers, IDeadlines deadlines, IConsultations consultations, ITeams teams, ReportService report) 
        {
            _teachers = teachers;
            _deadlines = deadlines;
            _consultations = consultations;
            _teams = teams;
            _report = report;
        }

        public async Task<ViewResult> DeadlinesList()
        {
            DeadlinesViewModel deadline = new DeadlinesViewModel();
            deadline.getTeacher = await _teachers.GetEntityDeadline(1);
            return View(deadline);
        }

        [HttpGet]
        public async Task<IActionResult> DeadlineSave(int deadlineId, int themeId)
        {
            DeadlinesSaveViewModel model = new DeadlinesSaveViewModel();
            ViewBag.themes = (await _teachers.GetEntity(1)).Themes;
            ViewBag.deadlineTeamId = themeId;
            switch (deadlineId)
            {
                case 0:
                    ViewBag.tableName = "Создать запись:";
                    model.AttendanceMark = -1;
                    model.TeamId = themeId;
                    break;

                default:
                    ViewBag.tableName = "Изменить запись:";
                    model.Id = deadlineId;
                    Deadline deadline = await _deadlines.GetEntity(deadlineId);
                    model.TeamId = deadline.TeamId;
                    model.DealineDate = deadline.DeadLineDate;
                    model.Commentary = deadline.Commentary;
                    model.AttendanceMark = deadline.AttendanceMark;
                    break;
            }
            return PartialView("DeadlineSave", model);
        }

        [HttpPost]
        public async Task<IActionResult> DeadlineSave(DeadlinesSaveViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("DeadlinesList");
            }

            switch (model.Id)
            {
                case 0:
                    Deadline deadline = new Deadline();
                    deadline.DeadLineDate = model.DealineDate;
                    deadline.Commentary = model.Commentary;
                    deadline.AttendanceMark = (byte)model.AttendanceMark;
                    deadline.Team = await _teams.GetEntity(model.TeamId);
                    _report.AddActionToExcel("Добавление:", $"{deadline.DeadLineDate}: {deadline.Commentary}", DateTime.Now);
                    await _deadlines.AddEntity(deadline);
                    break;

                default:
                    Deadline existingDeadline = await _deadlines.GetEntity(model.Id);
                    
                    _report.AddActionToExcel("Изменение:", $"{existingDeadline.DeadLineDate} :  {existingDeadline.Commentary}", DateTime.Now, $"{model.DealineDate} :  {model.Commentary}");
                    existingDeadline.DeadLineDate = model.DealineDate;
                    existingDeadline.Commentary = model.Commentary;
                    existingDeadline.AttendanceMark = (byte)model.AttendanceMark;
                    existingDeadline.Team = await _teams.GetEntity(model.TeamId);
                    await _deadlines.UpdateEntity(existingDeadline);
                    break;
            }
            return RedirectToAction("DeadlinesList");
        }

        [HttpGet]
        public async Task<IActionResult> ConsultationSave(int deadlineId, int consultationId)
        {
            ConsultationSaveViewModel model = new ConsultationSaveViewModel();
            model.DeadlineId = deadlineId;
            switch (consultationId)
            {
                case 0:
                    
                    break;

                default:
                    Consultation consultation = await _consultations.GetEntity(consultationId);
                    model.Id = consultationId;
                    model.ConsultationDate = consultation.Date;
                    model.AttendanceMark = consultation.AttendanceMark;
                    break;
            }
            return PartialView("ConsultationSave", model);
        }

        [HttpPost]
        public async Task<IActionResult> ConsultationSave(ConsultationSaveViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("DeadlinesList");
            }

            switch (model.Id)
            {
                case 0:
                    Consultation consultation = new Consultation();
                    consultation.AttendanceMark = (byte)model.AttendanceMark;
                    consultation.Date = model.ConsultationDate;
                    consultation.DeadlineId = model.DeadlineId;
                    await _consultations.AddEntity(consultation);
                    _report.AddActionToExcel("Добавление:", $"Консультация: {consultation.Date}", DateTime.Now);
                    break;

                default:
                    Consultation existingConsultation = await _consultations.GetEntity(model.Id);
                    _report.AddActionToExcel("Изменение консультации:", $"{existingConsultation.Date} :  {existingConsultation.AttendanceMark}", DateTime.Now, $"{model.ConsultationDate} :  {model.AttendanceMark}");
                    existingConsultation.Date = model.ConsultationDate;
                    existingConsultation.AttendanceMark = (byte)model.AttendanceMark;
                    await _consultations.UpdateEntity(existingConsultation);
                    break;
            }

            return RedirectToAction("DeadlinesList");
        }

        [HttpPost]
        public async Task<IActionResult> DeadlineDelete(int deadlineId)
        {
            Deadline deadline = await _deadlines.GetEntity(deadlineId);
            _report.AddActionToExcel("Удаление дедлайна:", $"{deadline.DeadLineDate}: {deadline.Commentary}", DateTime.Now);
            await _deadlines.DeleteEntity(deadline);
            return RedirectToAction("DeadlinesList");
        }

        [HttpPost]
        public async Task<IActionResult> ConsultationDelete(int consultationID)
        {
            Consultation consultation = await _consultations.GetEntity(consultationID);
            _report.AddActionToExcel("Удаление консультации:", $"{consultation.Date}", DateTime.Now);
            await _consultations.DeleteEntity(consultation);
            return RedirectToAction("DeadlinesList");
        }
    }
}
