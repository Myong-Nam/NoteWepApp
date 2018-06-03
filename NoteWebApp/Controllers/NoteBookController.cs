using NoteWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteWebApp.Controllers
{
    public class NoteBookController : Controller
    {
        // GET: NoteBook
        public ActionResult Index()
        {
            var noteBookList = NoteBookManager.GetNoteBookList().ToList();
            ViewBag.noteBookList = noteBookList;

            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Update()
        {
            return View();
        }

        public ActionResult Delete()
        {
            return View();
        }
    }
}