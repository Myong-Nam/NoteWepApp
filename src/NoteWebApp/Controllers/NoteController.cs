using NoteWebApp.Models;
using NoteWebApp.ViewModels;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace NoteWebApp.Controllers
{
	public class NoteController : BaseController
	{
		// GET: Note

		public ActionResult Index()
		{

			NoteVO selected = new NoteVO();
			int id; //노트아이디

			return View();
		}

		[HttpGet]
		public ActionResult Detail(string id)
		{
			var logger = NLog.LogManager.GetCurrentClassLogger();
			logger.Info("로그 테스트입니다.");

			NoteIndexVM model = new NoteIndexVM();
			
			//int id; //노트아이디

			OrderColumn defaultOrderColumn = OrderColumn.Notedate;
			OrderType defaultOrderType = OrderType.Desc;

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

			model.NoteList = NoteDAO.GetNoteList((OrderColumn)selectedOrderColumn, (OrderType)selectedOrderType, 0);

			if (String.IsNullOrEmpty(id))
			{
				model.SelectedNote = NoteDAO.GetNotebyId(model.NoteList[0].NoteId);
				id = model.SelectedNote.NoteId.ToString();

				return RedirectToAction("detail", new { id = id });
			}

			model.SelectedNote = NoteDAO.GetNotebyId(int.Parse(id));
			
			//노트북선택 
			SelectNoteBook(int.Parse(id));

			//노트 아이디로 노트북 얻어옴
			model.NoteBook = NoteBookDAO.GetNoteBookbyId(model.SelectedNote.NoteBookId);

			return View(model);
		}

		[HttpPost]
		public PartialViewResult Info(int noteid)
		{
			var logger = NLog.LogManager.GetCurrentClassLogger();
			logger.Info($"note id: {noteid}");

			NoteVO note = NoteDAO.GetNotebyId(noteid);
			return PartialView(note);
		}

		public PartialViewResult ShowNoteList(List<NoteVO> noteList)
		{
			CultureInfo cultureEN_US = new CultureInfo("en-US");
			CultureInfo cultureEN = new CultureInfo("en");
			CultureInfo cultureKO_KR = new CultureInfo("ko-KR");
			CultureInfo culturezh_CN = new CultureInfo("zh-CN");

			Thread.CurrentThread.CurrentCulture = cultureEN;
			Thread.CurrentThread.CurrentUICulture = cultureEN;

			return PartialView(noteList);
		}


		//노트 리스트
		public ActionResult NoteListByOrder(OrderColumn orderColumn, OrderType orderType)
		{
			OrderColumn defaultOrderColumn = OrderColumn.Notedate;
			OrderType defaultOrderType = OrderType.Desc;
			int defaultNoteBookId = 0;

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

			//var noteList = NoteDAO.GetNoteList((OrderColumn)Session["OrderColumn"], (OrderType)Session["OrderType"], defaultNoteBookId);
			
			

			return View();

		}

		//새노트에서 노트북 선택하는 selectitem
		public ActionResult SelectNewNoteBook()
		{
			var allBooks = NoteBookDAO.GetNoteBookList(); //노트북 전체 불러오기
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
				if (item.Value == NoteBookDAO.DefaultNoteBook().ToString())
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
			var allBooks = NoteBookDAO.GetNoteBookList(); //노트북 전체 불러오기
			List<SelectListItem> items = new List<SelectListItem>(); //select list item 초기화

			String noteBookId = NoteDAO.GetNotebyId(noteid).NoteBookId.ToString(); //노트의 노트북아이디


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
			NoteDAO.Create(title, contents, noteBookId);

			return RedirectToAction("detail");
		}

		public ActionResult Create(String title, String contents)
		{
			SelectNewNoteBook();

			return View();
		}

		public ActionResult preDelete(int noteId)
		{
			NoteDAO.preDelete(noteId);

			return RedirectToAction("detail");
		}

		[HttpPost]
		public ActionResult Delete(int noteId)
		{
			NoteDAO.Delete(noteId);

			return RedirectToAction("deleted");
		}

		//노트 수정
		[ValidateInput(false)]
		[HttpPost]
		public ActionResult Update(int noteId, String title, String contents, string noteBookId)
		{
			NoteDAO.Update(noteId, title, contents, noteBookId);

			return RedirectToAction("Detail", new { id = noteId });
		}

		[HttpPost]
		public ActionResult Deleted()
		{
			var noteList = NoteDAO.GetNoteList(OrderColumn.Notedate, OrderType.Desc, noteBookId: -1); //-1은 휴지통
			ViewBag.noteList = noteList;

			return View();
		}

		public ActionResult DeletedNote(int noteid)
		{
			NoteVO DeletedNote = NoteDAO.GetNotebyId(noteid);
			ViewBag.DeletedNote = DeletedNote;

			return View();
		}

		public ActionResult RecoverNote(int noteid)
		{
			NoteDAO.RecoverNote(noteid);

			return RedirectToAction("Deleted");
		}

		public void UploadToDatabase()
		{
			var files = Request.Files;

			foreach (string uploadedFiles in Request.Files)
			{
				//1) 업로드된 파일 받음
				HttpPostedFileBase file = Request.Files["upload"];
				byte[] imageData = new byte[file.ContentLength];

				string CKEditorFuncNum = Request["CKEditorFuncNum"];

				file.InputStream.Read(imageData, 0, Convert.ToInt32(file.ContentLength));

				//2) 데이터 베이스에 업로드
				string sql = $"INSERT INTO BINARY_FILE (BINARY_FILE_ID, FILE_NAME, MIME_TYPE, BINARY_DATA, FILE_SIZE ) VALUES (binary_file_seq.nextval, '{file.FileName}', '{file.ContentType}', :BINARY_DATA, {file.ContentLength.ToString()})";

				OracleConnection conn = DbHelper.NewConnection();
				conn.Open();

				OracleCommand cmd = new OracleCommand();
				cmd.CommandText = sql;
				cmd.Connection = conn;
				//cmd.CommandType = CommandType.Text;

				OracleParameter param = cmd.Parameters.Add("BINARY_DATA", OracleDbType.Blob);
				param.Direction = ParameterDirection.Input;
				param.Value = imageData;

				cmd.ExecuteNonQuery();

				file.InputStream.Close();  // close를 하면 file의 속성값도 같이 날라가는 것 같다.

				cmd.Dispose();

				//3) 업로드된 파일의 url을 에디터에 전달
				string idSql = $"SELECT BINARY_FILE_ID FROM BINARY_FILE WHERE FILE_NAME = '{file.FileName}'";

				OracleCommand idCmd = new OracleCommand();
				cmd.CommandText = idSql;
				cmd.Connection = conn;


				OracleDataReader reader = cmd.ExecuteReader();

				if (reader.HasRows)
				{
					string fileId = reader["BINARY_FILE_ID"].ToString();
					string url = "http://localhost:7223/Lab/SimpleFileUpload/download/" + fileId;

					Response.Write("<script>window.parent.CKEDITOR.tools.callFunction( " + CKEditorFuncNum + ", \"" + url + "\");</script>");

					Response.End();

				} else
				{
					Debug.Print("파일이 없다.");
				}

				conn.Close();
			}

		}

	}
}