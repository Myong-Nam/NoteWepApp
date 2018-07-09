using NoteWebApp.Areas.Lab.Models;
using System;
using System.Collections.Generic;
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
				var file = Request.Files[0];

				Debug.WriteLine($"file name: {file.FileName}" );

				//if (file != null && file.ContentLength > 0)
				//{
				//	var fileName = Path.GetFileName(file.FileName);
				//	var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
				//	file.SaveAs(path);
				//}
			}

			return RedirectToAction("SimpleFileUpload");
		}
    }
}