using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteWebApp.Models
{
    public class NoteBookVO
    {
		public int NoteBookId { get; set; }
		public string Name { get; set; }
		public string CreatedDate { get; set; }
		public int IsDeleted { get; set; }
		public int IsDefault { get; set; }
		public int IsShortcut { get; set; }
    }
}