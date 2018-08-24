using NoteWebApp.Models;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteWebApp.Areas.Lab.Controllers
{
	public class DragDropFileUploadController : Controller
	{
		// GET: Lab/DragDropFileUpload
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult FileUploadToFileSystem()
		{
			return View();
		}

		public ActionResult FileUploadToDatabase()
		{
			return View();
		}

		[HttpPost]
		public ActionResult UploadToFileSystem()
		{
			long size = 0;
			var files = Request.Files;

			foreach (string fileName in Request.Files)
			{
				HttpPostedFileBase file = Request.Files[fileName];

				string projectRoot = Path.GetDirectoryName(Server.MapPath("~"));
				string parentPath = Path.GetDirectoryName(projectRoot);
				string filenameWithPath = $@"{parentPath}/temp/{fileName}";

				size += file.ContentLength;

				file.SaveAs(filenameWithPath);
			}

			string message = $"{files.Count} file(s) / {size} bytes uploaded successfully!";

			return Json(message);
		}

		public ActionResult UploadToDatabase()
		{
			long size = 0;
			var files = Request.Files;

			foreach (string fileName in Request.Files)
			{
				HttpPostedFileBase file = Request.Files[fileName];

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

			string message = $"{files.Count} file(s) / {size} bytes uploaded successfully!";

			return Json(message);
		}

	}
}