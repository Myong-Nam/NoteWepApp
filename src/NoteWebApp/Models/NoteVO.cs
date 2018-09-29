using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteWebApp.Models
{
	public class NoteVO
	{
		public int NoteId { get; set; }
		public string Title { get; set; }
		public string Contents { get; set; }
		public string NoteDate { get; set; }
		public DateTime FullDate { get; set; }
		public string UpdatedDate { get; set; }
		public int IsDeleted { get; set; }
		public int NoteBookId { get; set; }
		public Boolean IsShortcut { get; set; }
		public List<TagVO> TagList { get; set; }
	}
}