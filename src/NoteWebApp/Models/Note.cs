﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteWebApp.Models
{
	public class Note
	{
		public int NoteId;
		public string Title;
		public string Contents;
		public string NoteDate;
		public DateTime FullDate;
		public int IsDeleted;
		public int NoteBookId;
		public int IsShortcut;
	}
}