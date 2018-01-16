﻿using System;
using System.Diagnostics;
using OpenTK.Graphics.ES20;

namespace Sce.Pss.Core.Graphics
{
	public class Texture2D : Texture
	{
		//https://github.com/PyramidTechnologies/ThermalTalk/blob/40818af6ee73b4514bfedb40623083d0576830bf/ThermalTalk.Imaging/ImageExt.cs
		public static byte[] __toBuffer(System.Drawing.Bitmap img)
		{
            if (img == null || img.Size.IsEmpty)
            {
                return new byte[0];
            }

            System.Drawing.Imaging.BitmapData bitmapData = null;

            // This rectangle selects the entirety of the source bitmap for locking bits into memory
            var rect = new System.Drawing.Rectangle(0, 0, img.Width, img.Height);

            try
            {
                // Acquire a lock on the image data so we can extra into our own byte stream
                // Note: Currently only supports data as 32bit, 4 channel 8-bit color
                bitmapData = img.LockBits(
                    rect,
                    System.Drawing.Imaging.ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

                // Create the output buffer
                int length = Math.Abs(bitmapData.Stride) * bitmapData.Height;
                byte[] results = new byte[length];

                // Copy from unmanaged to managed memory
                IntPtr ptr = bitmapData.Scan0;
                System.Runtime.InteropServices.Marshal.Copy(ptr, results, 0, length);

                return results;

            }
            finally
            {
                if (bitmapData != null)
                    img.UnlockBits(bitmapData);
            }
  		}		
		public static int __get2(int n)
		{
			int result = 2;
			int k = 2;
			for (int i = 0; i < 32; i++)
			{
				k *= 2;
				if (k > n)
				{
					result = k;
					break;
				}
			}
			return result;
		}
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
		
		public Texture2D(string fileName, bool mipmap)
		{
			string imgname = fileName.Replace("/Application/", "./");
			System.Drawing.Bitmap __img = new System.Drawing.Bitmap(imgname);
			this.__width = __img.Width;
			this.__height = __img.Height;
			System.Drawing.Bitmap __img2 = new System.Drawing.Bitmap(this.__width, this.__height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			using (System.Drawing.Graphics drawing = System.Drawing.Graphics.FromImage(__img2))
			{
				drawing.DrawImage(__img, 0, 0);
				drawing.Save();
			}
			SetPixels(0, __toBuffer(__img2), 0, 0, __img2.Width, __img2.Height);
			this.__mipmap = mipmap;
		}
		
		//FIXME:level not used
		public void SetPixels(int level, byte[] pixels, int dx, int dy, int dw, int dh)
		{
			//pixels
			this.__width = __get2(Math.Max(dx + dw, this.__width));
			this.__height = __get2(Math.Max(dy + dh, this.__height));
			__pixels = new byte[this.__width * this.__height * 4]; //FIXME:
			for (int j = 0; j < dh; ++j)
			{
				for (int i = 0; i < dw; ++i)
				{
					if ((j + dy) * __width * 4 + (i + dx) * 4 + 3 < __pixels.Length)
					{
						__pixels[(j + dy) * __width * 4 + (i + dx) * 4 + 0] = pixels[j * dw * 4 + i * 4 + 2];
						__pixels[(j + dy) * __width * 4 + (i + dx) * 4 + 1] = pixels[j * dw * 4 + i * 4 + 1];
						__pixels[(j + dy) * __width * 4 + (i + dx) * 4 + 2] = pixels[j * dw * 4 + i * 4 + 0];
						__pixels[(j + dy) * __width * 4 + (i + dx) * 4 + 3] = pixels[j * dw * 4 + i * 4 + 3];
					}
				}
			}
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
			                       OpenTK.Graphics.ES20.PixelFormat.Rgba, PixelType.UnsignedByte, __pixels);
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
