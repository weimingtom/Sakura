using System;
using Sce.Pss.Core.Graphics;

namespace Sce.Pss.Core.Imaging
{
	public struct FontMetrics
	{
		public int Ascent;

		public int Descent;

		public int Leading;

		public int Height
		{
			get
			{
				return this.Ascent + this.Descent + this.Leading;
			}
		}
	}
}
