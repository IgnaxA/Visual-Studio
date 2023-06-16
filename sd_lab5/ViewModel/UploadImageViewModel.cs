using Microsoft.AspNetCore.Mvc;
using sd_lab5.PluginsControll;
using System.ComponentModel.DataAnnotations;

namespace sd_lab5.ViewModel
{
    public class UploadImageViewModel
    {
        [Required(ErrorMessage="Выберите изображение!")]
		[BindProperty]
		public IFormFile Image { get; set; }

        public Dictionary<string, string> pluginsImages { get; set; }
	}
}
