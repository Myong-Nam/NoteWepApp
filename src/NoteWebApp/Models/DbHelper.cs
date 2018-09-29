using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace NoteWebApp.Models
{
	public class DbHelper
	{
		public static string ConnectionString { get; set; }

		internal static OracleConnection NewConnection()
		{
			OracleConnection conn = new OracleConnection(ConnectionString);

			return conn;
		}
	}
}