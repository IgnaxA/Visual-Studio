using Microsoft.AspNetCore.Mvc;
using sd_lab5.Service;
using sd_lab5.ViewModel;

namespace sd_lab5.Controllers
{
    public class PluginController : Controller
    {
        private readonly UploadImageProcessedService _uploadImageService;
        private readonly IWebHostEnvironment _hostEnvironment;

        public PluginController(UploadImageProcessedService uploadImageService, IWebHostEnvironment hostEnvironment)
        {
            _uploadImageService = uploadImageService;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult ShowPlugins()
        {
            ShowPluginsViewModel model = new ShowPluginsViewModel()
            {
                pluginsList = _uploadImageService.GetPlugins()
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult UploadImage()
        {
            UploadImageViewModel model = new UploadImageViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(UploadImageViewModel model)
        {
            ModelState.Remove("pluginsImages");
            if (!ModelState.IsValid) 
            {
                return View(model);
            }
            model.pluginsImages = _uploadImageService.SaveImageAsync(model.Image);


            return View(model);
        }
    }
}