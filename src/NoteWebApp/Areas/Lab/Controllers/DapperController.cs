using NoteWebApp.Models;
using System.Data.Common;
using System.Web.Mvc;

namespace NoteWebApp.Areas.Lab.Controllers
{
	public class DapperController : Controller
	{
		// GET: Lab/Dapper
		public ActionResult Index()
		{
			DbConnection conn = App.Database.NewConnection;


			//Database.


			return View();
		}
	}
}