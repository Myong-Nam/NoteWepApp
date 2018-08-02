using System;
using System.Collections.Generic;
using System.Web.Mvc;
using NoteWebApp.Models;
using NoteWebApp.ViewModels;
using System.Text.RegularExpressions;
using System.Net;

namespace NoteWebApp.Controllers
{
	public class NoteController : Controller
	{
		// GET: Note

		//노트 리스트 가져오는 인덱스 페이지
		public ActionResult Index(string orderColumn, string orderType, string noteId, string notebookId)
		{
			
			OrderColumn defaultOrderColumn = OrderColumn.Notedate;
			OrderType defaultOrderType = OrderType.Desc;
			int defaultNoteBookId = 0;

			OrderColumn selectedOrderColumn;
			OrderType selectedOrderType;
			int selectedNotebookId;

			if (String.IsNullOrEmpty(orderColumn))
			{
				// parameter is empty
				selectedOrderColumn = defaultOrderColumn;
			}
			else
			{
				// parameter is delivered
				selectedOrderColumn = (OrderColumn) Enum.Parse(typeof(OrderColumn), orderColumn);
			}

			if (String.IsNullOrEmpty(orderType))
			{
				// parameter is empty
				selectedOrderType = defaultOrderType;
			}
			else
			{
				// parameter is delivered
				selectedOrderType = (OrderType)Enum.Parse(typeof(OrderType), orderType);
			}

			//notebookid
			selectedNotebookId = (String.IsNullOrEmpty(notebookId)) ? defaultNoteBookId : int.Parse(notebookId);


			// note list
			// Notebook id
			// default set


			var noteList = NoteManager.GetNoteList( selectedOrderColumn, selectedOrderType, selectedNotebookId );

			// 리스트 정렬 정보 (column, asc|desc)

			// note detail
			int selectedNoteid = (String.IsNullOrEmpty(noteId))? noteList[0].NoteId : int.Parse(noteId);
			Note selectedNote = NoteManager.GetNotebyId(selectedNoteid);

			NoteIndexVM model = new NoteIndexVM();

			model.NoteList = noteList;
			model.SelectedNote = selectedNote;

			OrderBy();

			return View(model);
		}

		//노트리스트 파셜뷰
		public PartialViewResult NoteList()
		{
			var noteList = NoteManager.GetNoteList(OrderColumn.Notedate, OrderType.Desc, noteBookId: 0);

			OrderBy();

			return PartialView(noteList);
		}

		//노트 상세 partial view
		public PartialViewResult Detail(int selectedNoteid)
		{
			Note selected = new Note();

			if (selectedNoteid != 0)
			{
				selected = NoteManager.GetNotebyId(selectedNoteid);
			}
			else
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

		//노트 정보 가져오는 modal partial view
		[HttpPost]
		public PartialViewResult Info(int noteid)
		{
			Note note = NoteManager.GetNotebyId(noteid);
			return PartialView(note);
		}



		//노트 리스트 보여주는 partial view
		[HttpPost]
		public PartialViewResult ShowNoteList(int order, int notebookId)
		{
			OrderColumn orderColumnName = OrderColumn.Notedate;
			OrderType orderType = OrderType.Desc;

			switch (order)
			{
				case 0:
					orderColumnName = OrderColumn.Notedate;
					orderType = OrderType.Desc;
					break;
				case 1:
					orderColumnName = OrderColumn.Notedate;
					orderType = OrderType.Asc;
					break;
				case 2:
					orderColumnName = OrderColumn.Title;
					orderType = OrderType.Desc;
					break;
				case 3:
					orderColumnName = OrderColumn.Title;
					orderType = OrderType.Asc;
					break;
				default:
					break;
			}

			var noteList = NoteManager.GetNoteList(orderColumnName, orderType, notebookId);
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

		public ActionResult OrderBy()
		{
			List<SelectListItem> items = new List<SelectListItem>();
			items.Add(new SelectListItem { Text = "만든날짜(최근 순으로)", Value = "0" });
			items.Add(new SelectListItem { Text = "만든날짜(오래된 순으로)", Value = "1" });
			items.Add(new SelectListItem { Text = "제목(내림차순)", Value = "2" });
			items.Add(new SelectListItem { Text = "제목(오름차순)", Value = "3" });
			ViewBag.order = items;

			return View(items);
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
			var noteList = NoteManager.GetNoteList(OrderColumn.Notedate, OrderType.Desc, noteBookId: -1); //-1은 휴지통
			ViewBag.noteList = noteList;

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