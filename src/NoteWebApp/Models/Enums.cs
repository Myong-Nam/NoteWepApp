using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteWebApp.Models
{
	public static class Enums
	{
		public enum OrderColumn
		{
			Notedate,
			Title
		}

		public enum OrderType
		{
			Desc, Asc
		}
	}
}