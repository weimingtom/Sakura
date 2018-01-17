using System;
using System.Collections.Generic;

namespace Sce.Pss.Core.Input
{
	public static class Touch
	{
		public static List<TouchData> __data = new List<TouchData>();
		
		public static List<TouchData> GetData(int deviceIndex)
		{
			return __data;
		}
	}
}
