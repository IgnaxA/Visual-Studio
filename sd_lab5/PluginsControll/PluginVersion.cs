namespace sd_lab5.PluginsControll
{
	[AttributeUsage(AttributeTargets.Class)]
	public class PluginVersion : Attribute
	{
		public int Major { get; set; }

		public int Minor { get; set; }

		public PluginVersion(int major, int minor)
		{
			Major = major; 
			Minor = minor;
		}
	}
}
