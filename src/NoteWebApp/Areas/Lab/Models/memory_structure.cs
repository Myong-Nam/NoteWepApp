using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace NoteWebApp.Areas.Lab.Models
{
	public class memory_structure
	{
		void Main()
		{
			int a;
			int b;

			a = 10;
			b = 20;

			int result;

			result = Sum(a, b);			
		}

		private int Sum(int a, int b)
		{
			int c;
			//c = a + b; 

			c = a + b;

			return c;
		}

		private int Sum2(int a, int b)
		{
			int c = Sum(a, b);
			return c;
		}
	}
}