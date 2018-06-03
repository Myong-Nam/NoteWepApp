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
        // 노트북 리스트 보여줌
        public ActionResult Index()
        {
            var noteBookList = NoteBookManager.GetNoteBookList().ToList();
            ViewBag.noteBookList = noteBookList;

            return View();
        }

        public ActionResult Insert(String name)
        {
            int noteBook = NoteBookManager.Create(name);

            return RedirectToAction("index");
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Revise(int id)
        {
            NoteBook noteBook = NoteBookManager.GetNoteBookbyId(id);
            ViewBag.noteBook = noteBook;

            return View();
        }

        public ActionResult Update(int noteBookId, string name)
        {
            NoteBookManager.Update(noteBookId, name);

            return RedirectToAction("index");
        }

        public ActionResult Delete(int id)
        {
            NoteBookManager.Delete(id);

            return RedirectToAction("index");

        }

        // 노트북 내 노트 리스트 보여줌
        public ActionResult List(int id)
        {
            var name = NoteBookManager.GetNoteBookbyId(id).Name;
            var noteList = NoteBookManager.NotesInNoteBook(id).ToList();
            ViewBag.noteList = noteList;
            ViewBag.name = name;

            return View();
        }
    }
}