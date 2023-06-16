using Newtonsoft.Json.Linq;
using sd_lab5.PluginsControll;
using sd_lab5.Service.Plugins;

namespace sd_lab5.Service
{
    public class UploadImageProcessedService
    {
        private List<IPlugin> pluginsList = new List<IPlugin>();
		private readonly IWebHostEnvironment _hostEnvironment;

		public UploadImageProcessedService(IWebHostEnvironment hostEnvironment) 
        {
			_hostEnvironment = hostEnvironment;
        }

		public List<IPlugin> GetPlugins()
		{
			CreatePlugins();
			return pluginsList;
		}

		private List<string> ParseJson()
		{
			List<string> plugins = new List<string>();

			string json = File.ReadAllText("appsettings.json");
			JObject appSettings = JObject.Parse(json);
			JObject allowedPlugins = (JObject)appSettings["AllowedPlugins"];

			foreach (var plugin in allowedPlugins)
			{
				plugins.Add(plugin.Key);
			}

			return plugins;
		}

		public void CreatePlugins()
		{
            pluginsList.Clear();
            List<string> plugins = ParseJson();

            foreach (var plugin in plugins)
            {
				var pluginType = Type.GetType("sd_lab5.Service.Plugins." + plugin);
				if (pluginType == null) continue;

				var pluginInstance = Activator.CreateInstance(pluginType) as IPlugin;
				if (pluginInstance == null) continue;


				pluginsList.Add(pluginInstance);
			}
		}

        public Dictionary<string, string> SaveImageAsync(IFormFile image)
        {
			Dictionary<string, string> imagePlugins = new Dictionary<string, string>();

			string wwwRootPath = _hostEnvironment.WebRootPath;
            string imageName = Path.GetFileNameWithoutExtension(image.FileName);
            string imageExtension = Path.GetExtension(image.FileName);
			string imageFullName = imageName + imageExtension;
			string imagePath = Path.Combine(wwwRootPath + "/img", imageFullName);
			imagePlugins.Add("Оригинальное изображение:", $"~/img/{imageFullName}");

			CreatePlugins();
			CreateImage(imagePath, image);

			foreach (var plugin in pluginsList)
			{
				string pluginImageName = imageName + plugin.Name + imageExtension;
				string pluginImagePath = Path.Combine(wwwRootPath + "/img", pluginImageName);

				plugin.ApplyFilter(imagePath, pluginImagePath);

				imagePlugins.Add(plugin.Name, $"~/img/{pluginImageName}");
			}

			return imagePlugins;
        }

		private void CreateImage(string imagePath, IFormFile image)
		{
			using (var fileStream = new FileStream(imagePath, FileMode.Create))
			{
				image.CopyTo(fileStream);
			}
		}
	}
}
