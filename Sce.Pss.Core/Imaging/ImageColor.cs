using System;

namespace Sce.Pss.Core.Imaging
{
	public struct ImageColor
	{
		public int R;

		public int G;

		public int B;

		public int A;

		public ImageColor(int r, int g, int b, int a)
		{
			this.R = r;
			this.G = g;
			this.B = b;
			this.A = a;
		}
	}
}