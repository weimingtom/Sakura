using System;
using System.Diagnostics;
using OpenTK.Graphics.ES20;

namespace Sce.Pss.Core.Graphics
{
	public class Texture2D : Texture
	{
		private int __width = -1;
		private int __height = -1;
		private bool __mipmap;
		private PixelFormat __format;
		private byte[] __pixels;
		public int __textureId = -1;
		public int __dx, __dy, __dw, __dh;
		
//		public Texture2D()
//		{
//		}
		
		public Texture2D(int width, int height, bool mipmap, PixelFormat format) 
		{
			this.__width = width;
			this.__height = height;
			this.__mipmap = mipmap;
			this.__format = format;
		}
		
		public void SetPixels(int level, byte[] pixels, int dx, int dy, int dw, int dh)
		{
			__pixels = pixels; //FIXME:
			__dx = dx;
			__dy = dy;
			__dw = dw;
			__dh = dh;
			
#if true
			{
			   	//FIXME:tex
			   	//https://github.com/ebeisecker/Playpen/blob/b3fc7e08940e19566c06a8673438e3747832a650/iOSGLEssentials/iOSGLEssentials/OpenGLRenderer.cs
			   	GL.PixelStore ( PixelStoreParameter.UnpackAlignment, 1 ); //GL_UNPACK_ALIGNMENT
			  	GL.GenTextures(1, out __textureId); //glGenTextures ( 1, &textureId );
			   	GL.BindTexture (TextureTarget.Texture2D, __textureId); //glBindTexture ( GL_TEXTURE_2D, textureId );
			   	//PixelInternalFormat.Rgba
			   	GL.TexImage2D<byte>(TextureTarget2d.Texture2D, 0, TextureComponentCount.Rgba, __width, __height, 0, 
			                       OpenTK.Graphics.ES20.PixelFormat.Rgba, PixelType.UnsignedByte, pixels);
			   	Debug.WriteLine("==============>pixels.Length == " + pixels.Length);
			  	 //glTexImage2D ( GL_TEXTURE_2D, 0, GL_RGB, 2, 2, 0, GL_RGB, GL_UNSIGNED_BYTE, pixels );
			   	GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Nearest);// glTexParameteri ( GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST );
			   	GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Nearest);//glTexParameteri ( GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST );
			}
#else
			{
				byte[] pixels2 = new byte[]{
			      	255,   0,   0, // Red
			       	0, 255,   0, // Green
			        0,   0, 255, // Blue
			      	255, 255,   0  // Yellow
			   	};
			   	GL.PixelStore ( PixelStoreParameter.UnpackAlignment, 1 );
			  	GL.GenTextures(1, out __textureId);
			   	GL.BindTexture (TextureTarget.Texture2D, __textureId);
			   	GL.TexImage2D<byte>(TextureTarget2d.Texture2D, 0, TextureComponentCount.Rgb, 2, 2, 0, 
			                       OpenTK.Graphics.ES20.PixelFormat.Rgb, PixelType.UnsignedByte, pixels2);
			   	Debug.WriteLine("==============>pixels2.Length == " + pixels2.Length);
			   	GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Nearest);
			   	GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Nearest);
			}
#endif
		}
		
		public Texture2D ShallowCopy()
		{
			Texture2D result = new Texture2D(this.__width, this.__height, this.__mipmap, this.__format);
			result.__pixels = this.__pixels; //FIXME:
			result.__textureId = this.__textureId;
			result.__dx = this.__dx;
			result.__dy = this.__dy;
			result.__dw = this.__dw;
			result.__dh = this.__dh;
			return result;
		}
		
		public override int Width
		{
			get
			{
				return this.__width;//this.width;
			}
		}

		public override int Height
		{
		    get
			{
				return this.__height;//this.height;
			}
		}
	}
}
