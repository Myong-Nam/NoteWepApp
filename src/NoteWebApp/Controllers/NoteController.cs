using NoteWebApp.Models;
using NoteWebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace NoteWebApp.Controllers
{
	public class NoteController : Controller
	{
		// GET: Note

		[HttpGet]
		public ActionResult Index()
		{
			Note selected = new Note();
			int id; //노트아이디

			OrderColumn defaultOrderColumn = OrderColumn.Notedate;
			OrderType defaultOrderType = OrderType.Desc;
			int defaultNoteBookId = 0;

			OrderColumn selectedOrderColumn;
			OrderType selectedOrderType;	

			if (Session["OrderColumn"] != null)
			{
				selectedOrderColumn = (OrderColumn)Enum.Parse(typeof(OrderColumn), Session["OrderColumn"].ToString()); 
			}
			else
			{
				// use default value
				selectedOrderColumn = defaultOrderColumn;

			}

			if (Session["OrderType"] != null)
			{
				selectedOrderType = (OrderType)Enum.Parse(typeof(OrderType), Session["OrderType"].ToString());
			}
			else
			{
				// use default value
				selectedOrderType = defaultOrderType;
			}


			List<Note> noteList = NoteManager.GetNoteList((OrderColumn)selectedOrderColumn, selectedOrderType, defaultNoteBookId);
			ViewBag.noteList = noteList;

			//특정 노트 아이디가 있을떄
			if (Request.QueryString["id"] != null)
			{	
				id = int.Parse(Request.QueryString["id"]);

				selected = NoteManager.GetNotebyId(id);

			} else //노트 아이디 없을 때는 가장 최근 노트 가져오기
			{
				selected = NoteManager.GetNotebyId(noteList[0].NoteId);
				id = selected.NoteId;
				
			}

			NoteIndexVM model = new NoteIndexVM();
			model.SelectedNote = selected;
			model.SelectedNote.TagList = TagManager.GetTagListByNote(selected.NoteId);

			//바로가기 여부 보여줌(노트면 0, 노트북이면 1)
			ViewBag.isShortCut = ShortcutManager.IsShorcut(id, 0);


			//노트북선택 
			SelectNoteBook(id);

			//노트 아이디로 노트북 얻어옴
			int noteBookId = selected.NoteBookId;
			model.NoteBook = NoteBookManager.GetNoteBookbyId(noteBookId);

			return View(model);
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
		
		public PartialViewResult ShowNoteList(OrderColumn orderColumn, OrderType orderType)
		{
			OrderColumn defaultOrderColumn = OrderColumn.Notedate;
			OrderType defaultOrderType = OrderType.Desc;

			OrderColumn selectedOrderColumn;
			OrderType selectedOrderType;

			if (String.IsNullOrEmpty(orderColumn.ToString()))
			{
				// parameter is empty
				//selectedOrderColumn = defaultOrderColumn;

				if (Session["OrderColumn"] != null)
				{
					// do nothing
				}
				else
				{
					// use default value
					Session["OrderColumn"] = defaultOrderColumn;
				}

			}
			else
			{
				// parameter is delivered by user
				selectedOrderColumn = (OrderColumn)Enum.Parse(typeof(OrderColumn), orderColumn.ToString());
				Session["OrderColumn"] = selectedOrderColumn;
			}

			if (String.IsNullOrEmpty(orderType.ToString()))
			{
				// parameter is empty
				//selectedOrderColumn = defaultOrderColumn;

				if (Session["OrderType"] != null)
				{
					// do nothing
				}
				else
				{
					// use default value
					Session["OrderType"] = defaultOrderType;
				}
			}
			else
			{
				// parameter is delivered by user
				selectedOrderType = (OrderType)Enum.Parse(typeof(OrderType), orderType.ToString());
				Session["OrderType"] = selectedOrderType;
			}	

			var noteList = NoteManager.GetNoteList((OrderColumn)Session["OrderColumn"], (OrderType)Session["OrderType"], 0);

			// 리스트 정렬 정보 (column, asc|desc)


			//바로가기 여부 보여줌
			//ViewBag.isShortCut = ShortcutManager.IsShorcut(id, 1);

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