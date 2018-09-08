﻿using NoteWebApp.Models;
using NoteWebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteWebApp.Controllers
{
	public class TagController : Controller
	{
		// GET: Tag
		public ActionResult Index()
		{
			List<Tag> tagList = TagManager.GetTagList();

			return View(tagList);
		}

		public ActionResult Create(string tagName)
		{
			string msg = TagManager.Create(tagName);
			ViewBag.msg = msg;

			return View();
		}

		public ActionResult AddTagToNote(string tagName, int noteId)
		{
			TagManager.AddTagToNote(tagName, noteId);

			return RedirectToAction("../note/index");
		}

		public ActionResult List(string orderColumn, string orderType, string noteId, string tagId)
		{
			OrderColumn defaultOrderColumn = OrderColumn.Notedate;
			OrderType defaultOrderType = OrderType.Desc;

			OrderColumn selectedOrderColumn;
			OrderType selectedOrderType;
			int selectedTagId;

			if (String.IsNullOrEmpty(orderColumn))
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
				selectedOrderColumn = (OrderColumn)Enum.Parse(typeof(OrderColumn), orderColumn);
				Session["OrderColumn"] = selectedOrderColumn;
			}

			if (String.IsNullOrEmpty(orderType))
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
				selectedOrderType = (OrderType)Enum.Parse(typeof(OrderType), orderType);
				Session["OrderType"] = selectedOrderType;
			}



			//Tagid
			selectedTagId = int.Parse(tagId);


			var noteList = TagManager.GetNoteListByTagId(selectedTagId);

			// 리스트 정렬 정보 (column, asc|desc)

			// note detail
			int selectedNoteid = noteList[0].NoteId;
			Note selectedNote = NoteManager.GetNotebyId(selectedNoteid);

			NoteIndexVM model = new NoteIndexVM();

			model.NoteList = noteList;
			model.SelectedNote = selectedNote;
			int notebookIdParsedInt = int.Parse(tagId);
			model.NoteBook = NoteBookManager.GetNoteBookbyId(notebookIdParsedInt);


			//바로가기 여부 보여줌
			//ViewBag.isShortCut = ShortcutManager.IsShorcut(id, 1);

			return View(model);
		}
	}
}