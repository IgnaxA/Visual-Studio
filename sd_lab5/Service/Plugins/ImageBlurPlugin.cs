using sd_lab5.PluginsControll;

namespace sd_lab5.Service.Plugins
{
	[PluginVersion(1, 0)]
	public class ImageBlurPlugin : IPlugin
	{
		public string Name { get; set; }
		public string Author { get; set; }
		public string Version { get; set; }

		public ImageBlurPlugin()
		{
			Name = "ImageBlurPlugin";
			Author = "Ignat Panov";
			Version = GetVersion();
		}

		public void ApplyFilter(string oldImagePath, string newImagePath)
		{
			using (var image = Image.Load<Rgba32>(oldImagePath))
			{
				image.Mutate(x => x.GaussianBlur(5));
				image.Save(newImagePath);
			}
		}

		private string GetVersion()
		{
			var version = (PluginVersion)Attribute.GetCustomAttribute(typeof(ImageBlurPlugin), typeof(PluginVersion));
			return $"{version.Major}:{version.Minor}";
		}
	}
}
