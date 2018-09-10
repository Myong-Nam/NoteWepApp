using Dapper;
using Oracle.DataAccess.Client;
using System.Web.Mvc;

namespace NoteWebApp.Areas.Lab.Controllers
{
	public class DapperController : Controller
	{
		// GET: Lab/Dapper
		public ActionResult Index()
		{
			string connectionString = "Data Source = XE; USER ID = Note; PASSWORD = note;";

			using (var conn = new OracleConnection(connectionString))
			{
				int result = conn.ExecuteScalar<int>("select count(*) from note");

				ViewBag.Result = result;
			}

			return View();
		}
	}
}