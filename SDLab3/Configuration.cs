using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDLab3
{
	public class Configuration
	{
		public static string GetConnectionString() => @"data source=.\SQLEXPRESS;initial catalog=SDLab3;trusted_connection=true";
	}
}
