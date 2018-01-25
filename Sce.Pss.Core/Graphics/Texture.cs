using System;
using System.Diagnostics;

namespace Sce.Pss.Core.Graphics
{
	public abstract class Texture : PixelBuffer
	{
		public TextureWrapMode __wrap = TextureWrapMode.ClampToEdge;
		
		public Texture()
		{
		}
		
		public void SetWrap(TextureWrapMode mode)
		{
			this.__wrap = mode;
			//TextureWrapMode.ClampToEdge
			//Debug.Assert(false);
			//FIXME:not used
		}
		
		public void SetFilter(TextureFilterMode mag, TextureFilterMode min, TextureFilterMode mip)
		{
			//FIXME:not used
			//Debug.Assert(false);
		}
	}
}
