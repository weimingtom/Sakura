using System;

using System.Diagnostics;

namespace Sce.Pss.Core.Environment
{
	public static class SystemParameters
	{	
		public static string Language
		{
			get
			{
				//FIXME:
				return "zh-Hans";
			}
		}

		public static GamePadButtonMeaning GamePadButtonMeaning
		{
			get
			{
				//FIXME:
				return GamePadButtonMeaning.CrossIsEnter;
			}
		}

		public static YesNoLayout YesNoLayout
		{
			get
			{
				//FIXME:
				return YesNoLayout.YesIsLeft;;
			}
		}
		
		public static float DisplayDpiX
		{
			get
			{
//				Debug.Assert(false);
				return 100f;
			}
		}
		
		public static float DisplayDpiY
		{
			get
			{
//				Debug.Assert(false);
				return 100f;
			}
		}
	}
}
