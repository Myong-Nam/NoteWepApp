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
		public static List<BinaryFileVO> GetVOList()
		{
			List<BinaryFileVO> binaryFileVOList = new List<BinaryFileVO>();
			OracleConnection conn = DbHelper.NewConnection();
			conn.Open();
			string sql = "SELECT * FROM BINARY_FILE order by binary_file_id desc";
			OracleCommand cmd = new OracleCommand { Connection = conn, CommandText = sql };
			OracleDataReader reader = cmd.ExecuteReader();

			while (reader.Read())
			{
				BinaryFileVO binaryFileVO = new BinaryFileVO();

				binaryFileVO.BINARY_FILE_ID = int.Parse(reader["BINARY_FILE_ID"].ToString());
				binaryFileVO.FILE_NAME = reader["FILE_NAME"].ToString();
				binaryFileVO.MIME_TYPE = reader["MIME_TYPE"].ToString();
				binaryFileVO.BINARY_DATA = "(BLOB)";
				binaryFileVO.FILE_SIZE = int.Parse(reader["FILE_SIZE"].ToString());
				binaryFileVO.IS_DELETED = (reader["IS_DELETED"].ToString() == "1") ? true : false;
				binaryFileVO.CREATED_DATE = Convert.ToDateTime(reader["CREATED_DATE"]);
				binaryFileVO.MODIFIED_DATE = Convert.ToDateTime(reader["MODIFIED_DATE"]);

				if (!(reader["DELETED_DATE"] is DBNull))
				{
					binaryFileVO.DELETED_DATE = Convert.ToDateTime(reader["DELETED_DATE"]);
				}

				binaryFileVOList.Add(binaryFileVO);
			}

			reader.Close();
			conn.Close();

			return binaryFileVOList;
		}
	}
}