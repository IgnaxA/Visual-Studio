using sd_lab5.PluginsControll;

namespace sd_lab5.Service.Plugins
{
	[PluginVersion(1, 0)]
	public class ImageMedianMatrixFilterPlugin : IPlugin
	{
		public string Name { get; set; }
		public string Author { get; set; }
		public string Version { get; set; }

		public ImageMedianMatrixFilterPlugin()
		{
			Name = "ImageMedianMatrixFilterPlugin";
			Author = "Ignat Panov";
			Version = GetVersion();
		}

		public void ApplyFilter(string oldImagePath, string newImagePath)
		{
			using (var image = Image.Load<Rgba32>(oldImagePath))
			{
				image.Mutate(x => x.MedianBlur(5, true));
				image.Save(newImagePath);
			}
		}

		private string GetVersion()
		{
			var version = (PluginVersion)Attribute.GetCustomAttribute(typeof(ImageMedianMatrixFilterPlugin), typeof(PluginVersion));
			return $"{version.Major}:{version.Minor}";
		}
	}
}
