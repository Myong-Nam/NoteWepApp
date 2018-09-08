using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteWebApp.Models
{
    public class NoteBookVO
    {
        public int NoteBookId;
        public string Name;
        public string CreatedDate;
        public int IsDeleted;
        public int IsDefault;
        public int IsShortcut;
    }

}