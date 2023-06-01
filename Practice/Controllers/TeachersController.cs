using Microsoft.AspNetCore.Mvc;
using Practice.Data.Interface;
using Practice.ViewModels;

namespace Practice.Controllers
{
    public class TeachersController : Controller
    {
        private readonly ITeachers _teacher;
        private readonly IThemes _themes;
        private readonly IStudents _students;
        private readonly IFaculties _faculties;
        private readonly ICourses _courses;
        private readonly IRoles _roles;

        public TeachersController(ITeachers teacher, IThemes themes, IStudents students, IFaculties faculties, ICourses courses, IRoles roles)
        {
            _teacher = teacher;
            _themes = themes;
            _students = students;
            _faculties = faculties;
            _courses = courses;
            _roles = roles;
        }

        public async Task<ViewResult> TeachersList()
        {
            TeachersViewModel teachers = new TeachersViewModel();
            teachers.getTeacher = await _teacher.GetEntity(1);
            return View(teachers);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTheme(int themeId)
        {
            await _themes.DeleteEntity(await _themes.GetEntity(themeId));
            return RedirectToAction("TeachersList");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStudent(int studentId)
        {
            await _students.DeleteEntity(await _students.GetEntity(studentId));
            return RedirectToAction("TeachersList");
        }

        [HttpGet]
        public async Task<IActionResult> SaveTheme(int themeId)
        {
            CreateThemeViewModel model = new CreateThemeViewModel();
            ViewBag.tableName = "";
            switch (themeId)
            {
                case 0:
                    ViewBag.tableName = "Создать запись:";
                    break;

                default:
                    ViewBag.tableName = "Изменить запись:";
                    model.id = themeId;
                    model.ThemeFormulation =  (await _themes.GetEntity(themeId)).ThemeFormulation;
                    model.MaterialsLink =  (await _themes.GetEntity(themeId)).Teams.FirstOrDefault().MaterialsLink;
                    break;
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveTheme(CreateThemeViewModel model)
        {
            if (!ModelState.IsValid) 
            {
                return View(model);
            }

            switch(model.id)
            {
                case 0:
                    Theme theme = new Theme()
                    {
                        ThemeFormulation = model.ThemeFormulation,
                        TeacherId = 1
                    };
                    theme.Teams.Add(new Team() { MaterialsLink = model.MaterialsLink });
                    await _themes.AddEntity(theme);
                    break;

                default:
                    Theme existingTheme = await _themes.GetEntity(model.id);
                    existingTheme.ThemeFormulation = model.ThemeFormulation;
                    existingTheme.Teams.FirstOrDefault().MaterialsLink = model.MaterialsLink;
                    await _themes.UpdateEntity(existingTheme);
                    break;
            }

            return RedirectToAction("TeachersList");
        }

        [HttpGet]
        public async Task<IActionResult> ShowStudentInfo(int id)
        {
            StudentViewModel model = new StudentViewModel();
            ViewBag.themes = (await _teacher.GetEntity(1)).Themes;
            ViewBag.faculties = await _faculties.GetEntities();
            ViewBag.courses = await _courses.GetEntities();
            ViewBag.roles = await _roles.GetEntities();
            switch (id)
            {
                case 0:

                    break;

                default:
                    Student student = await _students.GetEntity(id);
                    model.Id = student.Id;
                    model.Initials = student.Initials;
                    model.Email = student.Email;
                    model.CourseId = student.CourseId;
                    model.FacultyId = student.FacultyId;
                    model.RoleId = student.RoleId == null ? 0 : student.RoleId;
                    ViewBag.studentTheme = (await _students.GetEntity(id)).Team.Theme.Id;
                    break;
            }

            return PartialView("ShowStudent", model);
        }
    }
}