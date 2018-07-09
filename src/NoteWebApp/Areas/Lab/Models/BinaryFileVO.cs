using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteWebApp.Areas.Lab.Models
{
	public class BinaryFileVO
	{
		public int BINARY_FILE_ID { get; set; }
		public string FILE_NAME { get; set; }
		public string FILE_EXT { get; set; }
		public string BINARY_DATA { get; set; }
		public int FILE_SIZE { get; set; }
		public bool IS_DELETED { get; set; }
		public DateTime? CREATED_DATE { get; set; }
		public DateTime? MODIFIED_DATE { get; set; }
		public DateTime? DELETED_DATE { get; set; }																	
	}
}