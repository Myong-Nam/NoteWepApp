using NoteWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteWebApp.Controllers
{
	public class TagController : Controller
	{
		// GET: Tag
		public ActionResult Index()
		{
			List<Tag> tagList = TagManager.GetTagList();

			return View(tagList);
		}

		public ActionResult Create(string tagName)
		{
			string msg = TagManager.Create(tagName);
			ViewBag.msg = msg;

			return View();
		}
	}
}