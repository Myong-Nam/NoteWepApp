using NoteWebApp.Areas.Lab.Models;
using NoteWebApp.Models;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace NoteWebApp.Areas.Lab.Controllers
{
    public class SimpleFileUploadController : Controller
    {
        // GET: Lab/SimpleFileUpload
        public ActionResult Index()
        {
			return RedirectToAction("List");
        }

		public ActionResult List()
		{
			List<BinaryFileVO> binaryFileVOList = BinaryFile.GetVOList();

			return View(binaryFileVOList);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id">BinaryFileId</param>
		/// <returns></returns>
		public FileContentResult Download(string id)
		{
			OracleConnection conn = new OracleConnection(DataBase.ConnectionString);
			conn.Open();

			string sql = $"select * from binary_file where binary_file_id = {id}";

			OracleCommand cmd = new OracleCommand
			{
				Connection = conn,
				CommandText = sql
			};

			OracleDataReader reader = cmd.ExecuteReader();

			byte[] blob = null;
			string contentType = "";

			while (reader.Read())
			{
				blob = (byte[])reader["binary_data"];
				contentType = reader["MIME_TYPE"].ToString();
			}

			reader.Close();
			cmd.Dispose();
			conn.Close();

			return File(blob, contentType);
		}

		[HttpPost]
		public ActionResult Upload()
		{
			if (Request.Files.Count > 0)
			{
				HttpPostedFileBase file = Request.Files[0];

				Debug.Print($"file name: {file.FileName}");
				Debug.Print($"file.ContentLength: {file.ContentLength}");

				byte[] imageData = new byte[file.ContentLength];

				// imageData에 file 내용을 적재한다.
				file.InputStream.Read(imageData, 0, Convert.ToInt32(file.ContentLength));

				// TODO: 보안을 위해서 나중에 oracle parameter를 사용하기.
				string sql = $"INSERT INTO BINARY_FILE (BINARY_FILE_ID, FILE_NAME, MIME_TYPE, BINARY_DATA, FILE_SIZE ) VALUES (binary_file_seq.nextval, '{file.FileName}', '{file.ContentType}', :BINARY_DATA, {file.ContentLength.ToString()})";

				Debug.Print("Debug.Print: " + sql);
				Debug.WriteLine("Debug.WriteLine: " + sql);
				Console.WriteLine("Console.WriteLine: " + sql);

				OracleConnection conn = new OracleConnection(DataBase.ConnectionString);
				conn.Open();
				OracleCommand cmd = new OracleCommand();
				cmd.CommandText = sql;
				cmd.Connection = conn;
				cmd.CommandType = CommandType.Text;

				OracleParameter param = cmd.Parameters.Add("BINARY_DATA", OracleDbType.Blob);
				param.Direction = ParameterDirection.Input;
				param.Value = imageData;

				cmd.ExecuteNonQuery();

				file.InputStream.Close();  // close를 하면 file의 속성값도 같이 날라가는 것 같다.

				cmd.Dispose();
				conn.Close();
			}

			return RedirectToAction("List");
		}
	}
}
