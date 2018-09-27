using NoteWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteWebApp.ViewModels
{
	public class NoteIndexVM
	{
		public NoteVO Note;
		public List<NoteVO> NoteList;
		public NoteVO SelectedNote;
		public NoteBookVO NoteBook;
	
	}
}