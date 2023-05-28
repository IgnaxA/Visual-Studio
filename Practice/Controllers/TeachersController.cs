using Microsoft.AspNetCore.Mvc;
using Practice.Data.Interface;
using Practice.ViewModels;

namespace Practice.Controllers
{
    public class TeachersController : Controller
    {
        private readonly ITeachers _teacher;

        public TeachersController(ITeachers teacher)
        {
            _teacher = teacher;
        }

        public async Task<ViewResult> TeachersList()
        {
            TeachersViewModel teachers = new TeachersViewModel();
            teachers.getTeacher = await _teacher.GetTeacher(1);
            return View(teachers);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStudent(int studentId)
        {
            await _teacher.DeleteStudent(await _teacher.GetStudent(studentId));
            return RedirectToAction("TeachersList");
        }

        [HttpGet]
        public ViewResult CreateTheme()
        {
            return View(new CreateThemeViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateTheme(CreateThemeViewModel model)
        {
            if (!ModelState.IsValid) 
            {
                return View(model);
            }


            if (model.id == 0)
            {
                await _teacher.AddTheme(model);
            }
            else
            {

            }
            return RedirectToAction("TeachersList");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTheme(int themeId)
        {
            await _teacher.DeleteTheme(await _teacher.GetTheme(themeId));
            return RedirectToAction("TeachersList");
        }
    }
}