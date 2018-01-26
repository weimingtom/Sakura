using System;

namespace Sce.Pss.Core.Graphics
{
	public class PixelBuffer : IDisposable
	{
		public PixelBuffer()
		{
		}
		
		public virtual int Width
		{
			get
			{
				return 0;//this.width;
			}
		}

		public virtual int Height
		{
			get
			{
				return 0;//this.height;
			}
		}
		
		public virtual void Dispose()
		{
			
		}
	}
}
