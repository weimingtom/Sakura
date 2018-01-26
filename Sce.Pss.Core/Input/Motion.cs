using System;

namespace Sce.Pss.Core.Input
{
	public static class Motion
	{
		public static MotionData __data = new MotionData();
		
		public static MotionData GetData(int deviceIndex)
		{
			return __data;
		}
	}
}
