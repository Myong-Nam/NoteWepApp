using NoteWebApp.Models;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteWebApp.Areas.Lab.Models
{
	public class BinaryFile
	{
		internal static List<BinaryFileVO> GetVOList()
		{
			List<BinaryFileVO> binaryFileVOList = new List<BinaryFileVO>();
			OracleConnection conn = new OracleConnection(DataBase.ConnectionString);
			conn.Open();
			string sql = "SELECT * FROM BINARY_FILE";
			OracleCommand cmd = new OracleCommand { Connection = conn, CommandText = sql };
			OracleDataReader reader = cmd.ExecuteReader();

			while (reader.Read())
			{
				BinaryFileVO binaryFileVO = new BinaryFileVO();

				binaryFileVO.BINARY_FILE_ID = int.Parse(reader["NOTEBOOKID"].ToString());
				binaryFileVO.FILE_NAME = reader["FILE_NAME"].ToString();
				binaryFileVO.FILE_EXT = reader["FILE_EXT"].ToString();
				binaryFileVO.BINARY_DATA = "(BLOB)";
				binaryFileVO.FILE_SIZE = int.Parse(reader["FILE_SIZE"].ToString());
				binaryFileVO.IS_DELETED = (reader["IS_DELETED"].ToString() == "1") ? true : false;
				binaryFileVO.CREATED_DATE = Convert.ToDateTime(reader["CREATED_DATE"]);
				binaryFileVO.MODIFIED_DATE = Convert.ToDateTime(reader["MODIFIED_DATE"]);
				binaryFileVO.DELETED_DATE = Convert.ToDateTime(reader["DELETED_DATE"]);

				binaryFileVOList.Add(binaryFileVO);
			}

			reader.Close();
			conn.Close();

			return binaryFileVOList;
		}
	}
}