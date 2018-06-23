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
			var noteList = NoteManager.GetNoteList(0).ToList();
			ViewBag.noteList = noteList;

			//orders
			List<SelectListItem> orders = new List<SelectListItem>();
			orders.Add(new SelectListItem()
			{
				Text = "만든날짜(오래된 순으로)",
				Value = "0",
				Selected = true
			});
			orders.Add(new SelectListItem()
			{
				Text = "만든날짜(최근 순으로)",
				Value = "1",
				Selected = false
			});
			orders.Add(new SelectListItem()
			{
				Text = "제목(오름차순)",
				Value = "2",
				Selected = false
			});
			orders.Add(new SelectListItem()
			{
				Text = "제목(내림차순)",
				Value = "3",
				Selected = false
			});

			ViewBag.orders = orders;



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

		//노트리스트 불러오기, 순서별로.
		public PartialViewResult NoteList(string order, int noteBookid)
		{
			var noteList = NoteManager.GetNoteList(noteBookid).ToList();
			
			if (order == "0")
			{
				noteList = noteList.OrderBy(x => x.FullDate).ToList();
			} else if (order == "1")
			{
				noteList = noteList.OrderBy(x => x.FullDate).Reverse().ToList();
			} else if (order == "2")
			{
				noteList = noteList.OrderBy(x => x.Title).ToList();
			} else
			{
				noteList = noteList.OrderBy(x => x.Title).Reverse().ToList();
			}

			ViewBag.noteList = noteList;
			return PartialView();
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
		[ValidateInput(false)]
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