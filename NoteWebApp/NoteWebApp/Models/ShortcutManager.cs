using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteWebApp.Models
{
    public class ShortcutManager
    {
        //노트북 이나 노트가 0이 아닌 것을 찾아서 title을 따온다.
        public static List<object> GetShorcutList()
        {
            List<object> shortcuts = new List<object>();

            using (OracleConnection conn = new OracleConnection(DataBase.ConnectionString))
            {
                conn.Open();

                String noteSql = "SELECT NOTE.NOTEID, NOTE.TITLE, SHORTCUT.ORDERS FROM NOTE, SHORTCUT WHERE NOTE.NOTEID = SHORTCUT.NOTEID";
                String noteBookSql = "SELECT NOTEBOOK.NOTEBOOKID, NOTEBOOK.NAME, SHORTCUT.ORDERS FROM NOTEBOOK, SHORTCUT WHERE NOTEBOOK.NOTEBOOKID = SHORTCUT.NOTEBOOKID";

                OracleCommand cmd = new OracleCommand
                {
                    Connection = conn,
                    CommandText = noteSql
                };

                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Note note = new Note();
                    note.Title = reader["title"].ToString();
                    note.NoteId = int.Parse(reader["noteid"].ToString());
                    int index = int.Parse(reader["orders"].ToString()) - 1;

                    shortcuts.Add(note);
                }
                reader.Close();

                OracleCommand noteBookCmd = new OracleCommand
                {
                    Connection = conn,
                    CommandText = noteBookSql
                };

                OracleDataReader noteBookReader = noteBookCmd.ExecuteReader();
                while (noteBookReader.Read())
                {
                    NoteBook notebook = new NoteBook();
                    notebook.Name = noteBookReader["name"].ToString();
                    notebook.NoteBookId = int.Parse(noteBookReader["notebookid"].ToString());
                    int index = int.Parse(noteBookReader["orders"].ToString()) - 1;

                    shortcuts.Add(notebook);
                }
                noteBookReader.Close();

                return shortcuts;
            }
        }

        //해당 노트가 shortcut에 있는지 없는지 판별 --> 동작함.
        public static string IsNoteShorcut(int noteid)
        {
            string isshortcut = "false";

            using (OracleConnection conn = new OracleConnection(DataBase.ConnectionString))
            {
                conn.Open();

                String sql = $"SELECT NOTEID FROM SHORTCUT WHERE NOTEID = {noteid}";

                OracleCommand cmd = new OracleCommand
                {
                    Connection = conn,
                    CommandText = sql
                };

                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Nullable<int> val = int.Parse(reader["NOTEID"].ToString());

                    if (val.HasValue == true)
                    {
                        isshortcut = "true";
                    }
                    else
                    {
                        isshortcut = "false";
                    }
                }
                reader.Close();

                return isshortcut;
            }
        }

        //해당 노트의 바로가기 여부를 바꿈
        public static void ChangeNoteShortcut(int noteid, string isShortcut)
        {
            String sql = "d";

            //바로가기 아니면 해당 note 를 shortcut에서 삭제
            if (isShortcut == "false")
            {
                sql = $"Delete FROM shortcut WHERE noteid = {noteid}";
            }
            else if (isShortcut == "true")
            {
                sql = $"Insert into shortcut (noteid, orders) values ( {noteid}, {0} )";
            }

            using (OracleConnection conn = new OracleConnection(DataBase.ConnectionString))
            {
                conn.Open();

                OracleCommand cmd = new OracleCommand
                {
                    Connection = conn,
                    CommandText = sql
                };

                cmd.ExecuteNonQuery();

            }
        }
    }

}