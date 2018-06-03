using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NoteWebApp.Models;

namespace NoteWebApp.Controllers
{
    public class NoteController : Controller
    {
        // GET: Note
        public ActionResult Index()
        { 
            var noteList = NoteManager.GetNoteList().ToList();
            ViewBag.noteList = noteList;

            return View();
        }

        public ActionResult Detail(int id)
        {
            Note note = NoteManager.GetNotebyId(id);
            ViewBag.noteDetail = note;


            return View();
        }

        public ActionResult Insert(String title, String contents)
        {
            int newNoteId = NoteManager.Create(title, contents);

            return RedirectToAction("index");
        }

        public ActionResult Create(String title, String contents)
        {
            return View();
        }

        public ActionResult preDelete(int noteId)
        {
            NoteManager.preDelete(noteId);

            return RedirectToAction("index");
        }

        [HttpPost]
        public ActionResult Delete(int noteId)
        {
            NoteManager.Delete(noteId);

            return RedirectToAction("deleted");
        }

        public ActionResult Update(int noteId, String title, String contents)
        {
            NoteManager.Update(noteId, title, contents);

            return RedirectToAction("index");
        }

        public ActionResult Deleted()
        {
            var DeletedNoteList = NoteManager.GetDeletedNoteList().ToList();
            ViewBag.DeletedNoteList = DeletedNoteList;

            return View();
        }

        public ActionResult DeletedNote(int noteid)
        {
            Note DeletedNote = NoteManager.GetNotebyId(noteid);
            ViewBag.DeletedNote = DeletedNote;

            return View();
        }

        public ActionResult RecoverNote(int noteid)
        {
            NoteManager.RecoverNote(noteid);

            return RedirectToAction("Deleted");
        }

    }
}