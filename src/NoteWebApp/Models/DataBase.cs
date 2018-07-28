using System.Configuration;

namespace NoteWebApp.Models
{
	public class DataBase
	{
		public static string ConnectionString
		{
			get {
				return ConfigurationManager.ConnectionStrings["MainDB"].ToString();
				// MainDB
				// AWS
			}
		}

	}
}