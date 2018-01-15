using System;
using Sakura.OpenTK;

namespace Sce.Pss.Core.Graphics
{
	public class FrameBuffer
	{
		public FrameBuffer()
		{
			
		}
		
		public float AspectRatio
		{
			get
			{
				return (float)MyGameWindow.getWidth() / (float)MyGameWindow.getHeight();//(float)this.state.width / (float)this.state.height;
			}
		}
		
		public int Width
		{
			get
			{
				return MyGameWindow.getWidth();//this.state.width;
			}
		}

		public int Height
		{
			get
			{
				return MyGameWindow.getHeight();//this.state.height;
			}
		}
	}
}
