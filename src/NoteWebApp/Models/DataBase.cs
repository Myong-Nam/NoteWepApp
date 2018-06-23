using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteWebApp.Models
{
    public class DataBase
    {
        public static string ConnectionString { get { return "Data Source = XE; USER ID = Note_lab; PASSWORD = note_lab;"; } }
    }
}