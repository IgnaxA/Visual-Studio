namespace sd_lab5.PluginsControll
{
	public interface IPlugin
	{
		public string Name { get; set; }

		public string Author { get; set; }

		public string Version { get; set; }

		void ApplyFilter(string oldImagePath, string newImagePath);
	}
}
