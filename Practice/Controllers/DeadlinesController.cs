using Microsoft.AspNetCore.Mvc;

namespace Practice.Controllers
{
    public class DeadlinesController : Controller
    {
        public DeadlinesController() 
        {

        }

        public async Task<ViewResult> DeadlinesList()
        {
            return View();
        }
    }
}
