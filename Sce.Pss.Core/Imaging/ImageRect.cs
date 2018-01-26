using System;
namespace Sce.Pss.Core.Imaging
{
	public struct ImageRect
	{
		public int X;
		public int Y;
		public int Width;
		public int Height;
		
		public ImageRect (int x, int y, int width, int height)
		{
			this.X = x;
			this.Y = y;
			this.Width = width;
			this.Height = height;
		}
	}
}

