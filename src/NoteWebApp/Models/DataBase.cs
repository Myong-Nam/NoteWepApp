using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace NoteWebApp.Models
{
	public class DataBase
	{
		public static string ConnectionString
		{
			get {
				return ConfigurationManager.ConnectionStrings["MainDB"].ToString();
			}
		}
	}
}