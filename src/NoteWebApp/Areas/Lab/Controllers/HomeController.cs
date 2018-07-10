using NoteWebApp.Areas.Lab.Models;
using NoteWebApp.Models;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteWebApp.Areas.Lab.Controllers
{
    public class HomeController : Controller
    {
        // GET: Lab/Home
        public ActionResult Index()
        {
            return View();
        }

		public ActionResult SimpleFileUpload()
		{
			List<BinaryFileVO> binaryFileVOList = BinaryFile.GetVOList();

			return View(binaryFileVOList);
		}

		[HttpPost]
		public ActionResult Upload()
		{
			if (Request.Files.Count > 0)
			{
				HttpPostedFileBase file = Request.Files[0];

				Debug.Print($"file name: {file.FileName}" );
				Debug.Print($"file.ContentLength: {file.ContentLength}");

				var stream = file.InputStream;

				byte[] ImageData = new byte[file.ContentLength];

				stream.Read(ImageData, 0, Convert.ToInt32(file.ContentLength));

				string sql = $"INSERT INTO BINARY_FILE (BINARY_FILE_ID, FILE_NAME, MIME_TYPE, BINARY_DATA, FILE_SIZE ) VALUES (binary_file_seq.nextval, '{file.FileName}', '{file.ContentType}', :1, {file.ContentLength.ToString()})";

				Debug.Print("Debug.Print: " + sql);
				Debug.WriteLine("Debug.WriteLine: " + sql);
				Console.WriteLine("Console.WriteLine: " + sql);

				OracleConnection conn = new OracleConnection(DataBase.ConnectionString);
				conn.Open();
				OracleCommand cmd = new OracleCommand();
				cmd.CommandText = sql;
				cmd.Connection = conn;
				cmd.CommandType = CommandType.Text;

				OracleParameter param = cmd.Parameters.Add("blobtodb", OracleDbType.Blob);
				param.Direction = ParameterDirection.Input;
				param.Value = ImageData;

				cmd.ExecuteNonQuery();

				stream.Close();  // close를 하면 file의 속성값도 같이 날라가는 것 같다.
			}

			return RedirectToAction("SimpleFileUpload");
		}
    }
}