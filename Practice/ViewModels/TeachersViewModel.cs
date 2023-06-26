using Practice.Data.Interface;

namespace Practice.ViewModels
{
    public class TeachersViewModel
    {
        public Teacher getTeacher { get; set; }
        
        public List<Faculty> faculties { get; set; }

        public List<Course> courses { get; set; }
    }
}
