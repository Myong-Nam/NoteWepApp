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

            int noteBookId = note.NoteBookId;
            NoteBook notebook = NoteBookManager.GetNoteBookbyId(noteBookId);
            int NoteBookId = notebook.NoteBookId;
            String name = notebook.Name;         
            ViewBag.name = name;

            SelectNoteBook(id);

            return View();
        }

        //새노트에서 노트북 선택하는 selectitem
        public ActionResult SelectNewNoteBook()
        {
            var allBooks = NoteBookManager.GetNoteBookList(); //노트북 전체 불러오기
            List<SelectListItem> items = new List<SelectListItem>(); //select list item 초기화

            foreach (var noteBook in allBooks)
            {
                items.Add(new SelectListItem()
                {
                    Text = noteBook.Name,
                    Value = noteBook.NoteBookId.ToString(),
                    Selected = false
                });
            }

            foreach (var item in items)
            {
                if (item.Value == NoteBookManager.DefaultNoteBook().ToString())
                {
                    item.Selected = true;
                }
            }

            ViewBag.noteBookId = items;

            return View();
        }


            //노트 디테일에서 노트북 선택하는 함수
            public ActionResult SelectNoteBook(int noteid)
        {
            var allBooks = NoteBookManager.GetNoteBookList(); //노트북 전체 불러오기
            List<SelectListItem> items = new List<SelectListItem>(); //select list item 초기화

            String noteBookId = NoteManager.GetNotebyId(noteid).NoteBookId.ToString(); //노트의 노트북아이디


            foreach (var noteBook in allBooks)
            {
                items.Add(new SelectListItem()
                {
                    Text = noteBook.Name,
                    Value = noteBook.NoteBookId.ToString(),
                    Selected = false
                });
            }

            foreach (var item in items)
            {
                if (item.Value == noteBookId)
                {
                    item.Selected = true;
                }
            }

            ViewBag.noteBookId = items;

            return View();
        }

        public ActionResult Insert(String title, String contents, string noteBookId)
        {
            int newNoteId = NoteManager.Create(title, contents, noteBookId);

            return RedirectToAction("index");
        }

        public ActionResult Create(String title, String contents)
        {
            SelectNewNoteBook();

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

        //노트 수정
        public ActionResult Update(int noteId, String title, String contents, string noteBookId)
        {
            NoteManager.Update(noteId, title, contents, noteBookId);

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