using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbHelper
{
	public class Database
	{
		public string ConnectionString { get; set; }
		public DbConnection NewConnection { get { return new OracleConnection(ConnectionString); } }
	}
}
