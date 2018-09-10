using Oracle.DataAccess.Client;
using System.Data.Common;

namespace DatabaseHelper.Oracle
{
	public class OracleDbHelper
	{
		public string ConnectionString { get; set; }

		public DbConnection Connection { get { return new OracleConnection(ConnectionString); } }
	}
}
