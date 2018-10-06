using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteWebApp.Controllers
{
    public class UserController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
		public ActionResult Login(string loginId, string password)
		{

			return View();
		}
	}
}