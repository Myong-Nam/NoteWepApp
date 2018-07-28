using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteWebApp.Areas.Lab.Controllers
{
    public class Html5Controller : Controller
    {
        // GET: Lab/Html5
        public ActionResult Index()
        {

            return View();
        }

		public ActionResult DragAndDrop()
		{
			return View();
		}
    }
}