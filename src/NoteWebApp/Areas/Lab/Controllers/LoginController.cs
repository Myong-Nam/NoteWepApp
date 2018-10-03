using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteWebApp.Areas.Lab.Controllers
{
    public class LoginController : Controller
    {
        // GET: Lab/Login
        public ActionResult Index()
        {
            return View();
        }
		public ActionResult register()
		{
			return View();
		}
	}
}