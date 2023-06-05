using Microsoft.AspNetCore.Mvc;
using Practice.Data.Interface;

namespace Practice.Controllers
{
    public class DeadlinesController : Controller
    {
        private readonly ITeachers _teachers;
        private readonly IDeadlines _deadlines;
        private readonly IConsultations _consultations;


        public DeadlinesController(ITeachers teachers, IDeadlines deadlines, IConsultations consultations) 
        {
            _teachers = teachers;
            _deadlines = deadlines;
            _consultations = consultations;
        }

        public async Task<ViewResult> DeadlinesList()
        {
            return View();
        }
    }
}
