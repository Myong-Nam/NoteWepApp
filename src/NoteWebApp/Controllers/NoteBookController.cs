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
		/*
		 목적 : 노트북 리스트 보여줌
		 준비물 : 노트북을 모두 가져오는 메소드 : GetNoteBookList()
		 (1) GetNoteBookList()로 가져온 노트북리스트를 viewbag에 넣음.
	 */
		public ActionResult Index()
		{
			var noteBookList = NoteBookManager.GetNoteBookList();
			int count = NoteBookManager.CountInBook(0);
			ViewBag.noteBookList = noteBookList;
			ViewBag.count = count;
			
			return View();
		}



		/*
		 목적 : 새 노트북 추가
		 준비물 : 새 노트북 이름, 노트북을 새로 추가하는 메소드 : NoteBookManager.Create(name);
		 (1) NoteBookManager.Create(name)을 실행
		 (2) 생성후 index페이지로 리다이렉트
		*/
		public ActionResult Insert(String name)
		{
			int noteBook = NoteBookManager.Create(name);

			return RedirectToAction("index");
		}

		/*
		 목적 : 새 노트북 추가할 수 있는 페이지
		 준비물 : 없음
		 (1) 새로운 노트북 이름을 넣을 수 있는 페이지 생성
		*/
		public ActionResult Create()
		{
			return View();
		}

		/*
		 목적 : 노트북 수정하는 페이지
		 준비물 : 노트북 새 이름, 노트북 아이디, 노트북 불러오는 메소드 : NoteBookManager.GetNoteBookbyId(id)
		 (1) 노트북 아이디를 이용해 NoteBookManager.GetNoteBookbyId(id)로 노트북 불러옴
		*/
		public ActionResult Info(int id)
		{
			NoteBook noteBook = NoteBookManager.GetNoteBookbyId(id);
			ViewBag.noteBook = noteBook;
			int defaultValue = noteBook.IsDefault;
			Boolean isDefault = false;
			if (defaultValue == 1)
			{
				isDefault = true;
			}

			ViewBag.isDefault = isDefault;

			return View();
		}

		/*
		 목적 : 노트북 수정
		 준비물 : 노트북 새 이름, 노트북 아이디, 노트북 수정하는 메소드 : NoteBookManager.Update(noteBookId, name);
		 (1) 노트북 아이디와 이름으로 노트북을 수정하는 메소드 불러옴(NoteBookManager.Update(noteBookId, name))
		 (2) 삭제 후 index페이지로 리다이렉트
		   */
		public ActionResult Update(int noteBookId, string name, Boolean isdefault)
		{
			int Isdefault = 0;

			if (isdefault == true)
			{
				Isdefault = 1;
			}
			NoteBookManager.Update(noteBookId, name, Isdefault);

			return RedirectToAction("List", new { id = noteBookId });
		}


		/*
		 목적 : 노트북 삭제
		 준비물 :노트북 아이디, 노트북 삭제하는 메소드 : NoteBookManager.Delete(id);
		 (1) 노트북 아이디로 삭제하는 메소드 불러옴(NoteBookManager.Delete(id))
		 (2) 삭제 후 index페이지로 리다이렉트
		*/
		public ActionResult Delete(int id)
		{
			NoteBookManager.Delete(id);

			return RedirectToAction("index");

		}

		/*
		 목적 : 노트북 내 노트 리스트 불러오기
		 준비물 : 노트북 아이디, 노트북 불러오는 메소드, 노트북 내 노트 불러오는 메소드
		 (1) 노트북 아이디로 노트북의 제목을 불러온다.
		 (2) 노트북 아이디로 해당 노트북아이디를 가진 노트를 불러와 리스트화한다.
		 (3) 위 두개를 Viewbag에 넣는다.
		*/
		public ActionResult List(int id)
		{
			NoteBook noteBook = NoteBookManager.GetNoteBookbyId(id);
			ViewBag.noteBook = noteBook;

			var noteList = NoteManager.GetNoteList(0, id); //노트리스트
			if (noteList.Count == 0)
			{
				string msg = "노트를 추가하세요.";
				ViewBag.firstNoteId = 0;
			}
			else
			{
				int firstNoteId = noteList[0].NoteId;
				ViewBag.noteBook = noteBook;
				ViewBag.firstNoteId = firstNoteId;
			}

			//바로가기 여부 보여줌
			ViewBag.isShortCut = ShortcutManager.IsShorcut(id, 1);

			return View();
		}
	}
}