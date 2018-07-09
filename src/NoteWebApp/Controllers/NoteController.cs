using System;
using System.Collections.Generic;
using System.IO;
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
			var noteList = NoteManager.GetNoteList(0, 0);
			ViewBag.noteList = noteList;

			return View();
		}

		public PartialViewResult Detail(int selectedNoteid)
		{
			Note selected = new Note();

			if (selectedNoteid != 0)
			{
				selected = NoteManager.GetNotebyId(selectedNoteid);
			} else
			{
				selected = NoteManager.GetNotebyId(40);
			}

			//바로가기 여부 보여줌(노트면 0, 노트북이면 1)
			ViewBag.isShortCut = ShortcutManager.IsShorcut(selectedNoteid, 0);


			//노트북선택 
			SelectNoteBook(selectedNoteid);

			//노트 아이디로 노트북 얻어옴
			int noteBookId = selected.NoteBookId;
			NoteBook notebook = NoteBookManager.GetNoteBookbyId(noteBookId);
			int NoteBookId = notebook.NoteBookId;
			ViewBag.name = notebook.Name;

			return PartialView(selected);
		}

		//노트 리스트 보여주는 partial view
		public PartialViewResult NoteList(string order, int notebookId)
		{
			int orderId = Int32.Parse(order);
			//int bookId = Int32.Parse(notebookId);
			var noteList = NoteManager.GetNoteList(orderId, notebookId);
			foreach (var item in noteList)
			{
				if (item.Title == "")
				{
					item.Title = "제목 없음";
				}
			}
			return PartialView(noteList);
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

		[ValidateInput(false)]
		public ActionResult Insert(String title, String contents, String noteBookId)
		{
			NoteManager.Create(title, contents, noteBookId);

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
		[ValidateInput(false)]
		public ActionResult Update(int noteId, String title, String contents, string noteBookId)
		{
			NoteManager.Update(noteId, title, contents, noteBookId);

			return RedirectToAction("index");
		}

		public ActionResult Deleted()
		{
			var DeletedNoteList = NoteManager.GetDeletedNoteList();
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