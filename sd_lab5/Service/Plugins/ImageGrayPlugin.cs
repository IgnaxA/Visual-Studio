using Microsoft.AspNetCore.Hosting;
using sd_lab5.PluginsControll;

namespace sd_lab5.Service.Plugins
{
    [PluginVersion(1, 0)]
    public class ImageGrayPlugin : IPlugin
    {
		public string Name { get; set; }
		public string Author { get; set; }
		public string Version { get; set; }

        public ImageGrayPlugin()
        {
            Name = "ImageGrayPlugin";
            Author = "Ignat Panov";
            Version = GetVersion();
        }

		public void ApplyFilter(string oldImagePath, string newImagePath)
        {
            using (var image = Image.Load<Rgba32>(oldImagePath))
            {
                image.Mutate(x => x.Grayscale());
                image.Save(newImagePath);
            }
        }

        private string GetVersion()
        {
            var version = (PluginVersion) Attribute.GetCustomAttribute(typeof(ImageGrayPlugin), typeof(PluginVersion));
            return $"{version.Major}:{version.Minor}";
        }
    }
}
