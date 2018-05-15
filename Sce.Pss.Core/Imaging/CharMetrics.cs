using System;
using System.Diagnostics;

namespace Sce.Pss.Core.Imaging
{
	//see public CharMetrics[] GetTextMetrics(string text)
	public struct CharMetrics
	{
//		public CharMetrics()
//		{
////			Debug.Assert(false);
//		}
		
		private float __x;
		public float X
		{
			get
			{
//				Debug.Assert(false);
//				return 0;
				return __x;
			}
			set
			{
				__x = value;
			}
		}
		
		private float __horizontalAdvance;
		public float HorizontalAdvance
		{
			get
			{
//				Debug.Assert(false);
//				return 0;
				return __horizontalAdvance;
			}
			set
			{
				__horizontalAdvance = value;
			}
		}
	}
}
