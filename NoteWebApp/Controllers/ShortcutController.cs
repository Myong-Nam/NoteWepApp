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
            List<object> shortcuts = ShortcutManager.GetShorcutList();
            ViewBag.shortcuts = shortcuts;

            return View();
        }

    }
}