using DbHelper;

namespace NoteWebApp.Models
{
	/// <summary>
	/// Application안 어디에서든지 접근할 수 있는 객체.
	/// </summary>
	public static class App
	{
		/// <summary>
		/// Application_Start에서 초기화
		/// </summary>
		public static Database Database { get; set; }
	}
}