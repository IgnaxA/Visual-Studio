using sd_lab5.PluginsControll;

namespace sd_lab5.Service.Plugins
{
	[PluginVersion(1, 0)]
	public class ImageImproveBrightness : IPlugin
	{
		public string Name { get; set; }
		public string Author { get; set; }
		public string Version { get; set; }

        public ImageImproveBrightness()
        {
			Name = "ImageImproveBrightness";
			Author = "Ignat Panov";
			Version = GetVersion();
		}

		public void ApplyFilter(string oldImagePath, string newImagePath)
        {
            using (var image = Image.Load<Rgba32>(oldImagePath))
            {
                image.Mutate(x => x.Brightness(1.4f));
                image.Save(newImagePath);
            }
        }

		private string GetVersion()
		{
			var version = (PluginVersion)Attribute.GetCustomAttribute(typeof(ImageImproveBrightness), typeof(PluginVersion));
			return $"{version.Major}:{version.Minor}";
		}
	}
}
