using System;

namespace Sce.Pss.HighLevel.UI
{
	public struct NinePatchMargin
	{
		public static readonly NinePatchMargin Zero = default(NinePatchMargin);

		public int Left;

		public int Top;

		public int Right;

		public int Bottom;

		public NinePatchMargin(int left, int top, int right, int bottom)
		{
			this.Left = left;
			this.Top = top;
			this.Right = right;
			this.Bottom = bottom;
		}
	}
}
