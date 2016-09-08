using System;
using System.IO;

namespace AwfulRedux_iOS
{
	public class DatabaseHelpers
	{
		public static string GetiOSDatabasePath(string dbPath) {
			return Path.Combine (
				Environment.GetFolderPath (Environment.SpecialFolder.Personal),
				dbPath);
		}
	}
}
