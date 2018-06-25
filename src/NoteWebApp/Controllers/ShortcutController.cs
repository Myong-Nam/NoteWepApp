using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NoteWebApp.Models;

namespace NoteWebApp.Controllers
{
	public class ShortcutController : Controller
	{
		// GET: Shortcut
		public ActionResult Index()
		{
			Dictionary<int, object> shortcuts = ShortcutManager.GetShortcuts();

			List<object> shortCutList = new List<object>(shortcuts.Values);

			ViewBag.shortCutList = shortCutList;

			return View();
		}

		public ActionResult ChangeBook(int id, string isShortcut)
		{
			ShortcutManager.ChangeShortcut(id, isShortcut, 1);

			return RedirectToAction("../NoteBook/List/" + id);
		}

		public ActionResult ChangeNote(int id, string isShortcut)
		{
			ShortcutManager.ChangeShortcut(id, isShortcut, 0);

			return RedirectToAction("../Note");
		}

	}
}