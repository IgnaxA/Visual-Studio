using System.Drawing;
using System;
using sd_lab5.PluginsControll;

namespace sd_lab5.Service.Plugins
{
	[PluginVersion(1, 0)]
	public class ImageMatrixImproveResolution : IPlugin
	{
		public string Name { get; set; }
		public string Author { get; set; }
		public string Version { get; set; }

        public ImageMatrixImproveResolution()
        {
			Name = "ImageMatrixImproveResolution";
			Author = "Ignat Panov";
			Version = GetVersion();
		}

		public void ApplyFilter(string oldImagePath, string newImagePath)
        {
            using (var image = Image.Load<Rgba32>(oldImagePath))
            {
                image.Mutate(x => x.GaussianSharpen(5));
                image.Save(newImagePath);
            }
        }

		private string GetVersion()
		{
			var version = (PluginVersion)Attribute.GetCustomAttribute(typeof(ImageMatrixImproveResolution), typeof(PluginVersion));
			return $"{version.Major}:{version.Minor}";
		}
	}
}
