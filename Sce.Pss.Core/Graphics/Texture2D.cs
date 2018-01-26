using System;
using System.Diagnostics;
using OpenTK.Graphics.ES20;

namespace Sce.Pss.Core.Graphics
{
	public class Texture2D : Texture
	{
		//FIXME:Should be false, for compatibility with Windows XP and low performance (scale and copy many times)
		//FIXME:see GL_OES_texture_npot
		//FIXME:if set __supportNPOT true under Windows XP, using wrap mode Repeat get black texture, see
		//GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)All.Repeat);
		public bool __supportNPOT = false; 
		
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
		public int __get2(int n)
		{
			if (!__supportNPOT)
			{
				int result = 2;
				int k = 2;
				for (int i = 0; i < 32; i++)
				{
					k *= 2;
					if (k >= n)
					{
						result = k;
						break;
					}
				}
				return result;
			} 
			else
			{
				//allow non power of two textures
				//http://stackoverflow.com/questions/11069441/non-power-of-two-textures-in-ios
				//http://blog.csdn.net/mvplinchen888/article/details/16946565
				//
				//https://github.com/k-t/SharpHaven/blob/f0f0eda97f2c6d9841ae3c10bb406e5725f74d8e/SharpHaven/Graphics/Texture.cs
				return n;
			}
		}
		private int __width = -1;
		private int __height = -1;
		public int __widthTex = -1;
		public int __heightTex = -1;
		private bool __mipmap;
		private PixelFormat __format;
		public byte[] __pixels;
		public int __textureId = -1;
		public int __dx, __dy, __dw, __dh;
		private PixelBufferOption __option;
		
		private static int __maxtextureId = 0;
		
//		public Texture2D()
//		{
//		}
		
		public Texture2D(int width, int height, bool mipmap, PixelFormat format) 
		{
			this.__width = width;
			this.__height = height;
			this.__mipmap = mipmap;
			this.__format = format;
			this.__option = PixelBufferOption.None; //FIXME:???
			using (System.Drawing.Bitmap __img2 = new System.Drawing.Bitmap(this.__width, this.__height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
			{
				using (System.Drawing.Graphics drawing = System.Drawing.Graphics.FromImage(__img2))
				{
					drawing.Clear(System.Drawing.Color.White);
					drawing.Save();
				}
				__SetPixels(0, __img2/*__toBuffer(__img2)*/, 0, 0, __img2.Width, __img2.Height);
			}
		}
		
		public Texture2D (int width, int height, bool mipmap, PixelFormat format, PixelBufferOption option)
		{
			this.__width = width;
			this.__height = height;
			this.__mipmap = mipmap;
			this.__format = format;
			this.__option = option;
			using (System.Drawing.Bitmap __img2 = new System.Drawing.Bitmap(this.__width, this.__height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
			{
				using (System.Drawing.Graphics drawing = System.Drawing.Graphics.FromImage(__img2))
				{
					drawing.Clear(System.Drawing.Color.White); //FIXME:??????
					drawing.Save();
				}
				__SetPixels(0, __img2/*__toBuffer(__img2)*/, 0, 0, __img2.Width, __img2.Height);
			}
		}
		
		public Texture2D(string fileName, bool mipmap)
		{
			string imgname = fileName.Replace("/Application/", "./");
			using (System.Drawing.Bitmap __img = new System.Drawing.Bitmap(imgname))
			{
				//FIXME:??? I don't know why it's right when width 256 is changed to 250,
				//see here: (in BgModel.cs)
				//meshGrid = BasicMeshFactory.CreatePlane( width, depth,
	            //                                     divW, divH, 0.25f, 0.25f );
				if (fileName.Contains("renga.png")) 
				{
					this.__width = 250;
					this.__height = 250;
					using (System.Drawing.Bitmap __img2 = new System.Drawing.Bitmap(this.__width, this.__height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
					{
						using (System.Drawing.Graphics drawing = System.Drawing.Graphics.FromImage(__img2))
						{
							//drawing.DrawImage(__img, 0, 0);
							drawing.DrawImage(__img, 0, 0, 250, 250);
//							drawing.DrawImage(__img, new System.Drawing.Rectangle(0, 0, 250, 250), 
//							                  new System.Drawing.Rectangle(0, 0, 512, 512),
//							                  System.Drawing.GraphicsUnit.Pixel);
							drawing.Save();
						}
						__SetPixels(0, __img2/*__toBuffer(__img2)*/, 0, 0, __img2.Width, __img2.Height);
					}
					this.__mipmap = mipmap;
				}
				else
				{
					this.__width = __img.Width;
					this.__height = __img.Height;
					using (System.Drawing.Bitmap __img2 = new System.Drawing.Bitmap(this.__width, this.__height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
					{
						using (System.Drawing.Graphics drawing = System.Drawing.Graphics.FromImage(__img2))
						{
							drawing.DrawImage(__img, 0, 0);
							drawing.Save();
						}
						__SetPixels(0, __img2/*__toBuffer(__img2)*/, 0, 0, __img2.Width, __img2.Height);
					}
					this.__mipmap = mipmap;				
				}
			}
		}
		
		public void SetPixels(int level, byte[] pixels)
		{
			this.SetPixels(level, pixels, 0, 0, this.__width, this.__height);
		}
		
		//FIXME:update not implemented, with glTexSubImage2D
		//FIXME:level not used
		public void SetPixels(int level, byte[] pixels, int dx, int dy, int dw, int dh)
		{
			//FIXME: for SampleDraw.DrawText()
			__supportNPOT = true; //NOTE:bug for repeat wrap mode
			if (__supportNPOT)
			{
				this.__SetPixels(level, pixels, dx, dy, dw, dh);
			}
			else
			{
				//https://github.com/davidhart/PhotoTunesPrototype/blob/2f8e0ef86f9ac6969637a2c1136723d4542e3bee/PhotoTunesDebugApp/PhotoTunesDebugApp/ImageFilter.cs
	            //Convert byte[] back to Bitmap
				using (System.Drawing.Bitmap oriBitmap =
	                   new System.Drawing.Bitmap(dw, dh, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
	            {
		            System.Drawing.Imaging.BitmapData oriBmData = oriBitmap.LockBits(
		            	new System.Drawing.Rectangle(0, 0, dw, dh), 
		            	System.Drawing.Imaging.ImageLockMode.WriteOnly, 
		            	System.Drawing.Imaging.PixelFormat.Format32bppArgb);
		            System.Runtime.InteropServices.Marshal.Copy(pixels, 0, oriBmData.Scan0, pixels.Length);
		            oriBitmap.UnlockBits(oriBmData);
		            this.__SetPixels(level, oriBitmap, dx, dy, dw, dh);
	            }
			}
		}
		
		//FIXME:should be private
		private void __SetPixels(int level, System.Drawing.Bitmap img, int dx, int dy, int dw, int dh)
		{
			if (__supportNPOT)
			{
				//not scale, not good
				byte[] pixels = __toBuffer(img);
				__SetPixels(level, pixels, dx, dy, dw, dh);
			}
			else
			{
				int widthTex = __get2(Math.Max(dx + dw, this.__width));
				int heightTex = __get2(Math.Max(dy + dh, this.__height));
				using (System.Drawing.Bitmap img2 = new System.Drawing.Bitmap(widthTex, heightTex, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
				{
					using (System.Drawing.Graphics drawing = System.Drawing.Graphics.FromImage(img2))
					{
						drawing.DrawImage(img, new System.Drawing.Rectangle(0, 0, widthTex, heightTex), 
						                  new System.Drawing.Rectangle(0, 0, img.Width, img.Height),
						                  System.Drawing.GraphicsUnit.Pixel);
						drawing.Save();
					}
					byte[] pixels = __toBuffer(img2);
					__SetPixels(level, pixels, dx, dy, widthTex, heightTex);
				}
			}
		}
		
		private void __SetPixels(int level, byte[] pixels, int dx, int dy, int dw, int dh)
		{
			//pixels
			this.__widthTex = __get2(Math.Max(dx + dw, this.__width));
			this.__heightTex = __get2(Math.Max(dy + dh, this.__height));
//			if (!__supportNPOT)
//			{
//				this.__width = this.__widthTex;
//				this.__height = this.__heightTex;
//			}
			__pixels = new byte[this.__widthTex * this.__heightTex * 4]; //FIXME:
			for (int j = 0; j < dh; ++j)
			{
				for (int i = 0; i < dw; ++i)
				{
					if ((j + dy) * __widthTex * 4 + (i + dx) * 4 + 3 < __pixels.Length)
					{
						__pixels[(j + dy) * __widthTex * 4 + (i + dx) * 4 + 0] = pixels[j * dw * 4 + i * 4 + 2];
						__pixels[(j + dy) * __widthTex * 4 + (i + dx) * 4 + 1] = pixels[j * dw * 4 + i * 4 + 1];
						__pixels[(j + dy) * __widthTex * 4 + (i + dx) * 4 + 2] = pixels[j * dw * 4 + i * 4 + 0];
						__pixels[(j + dy) * __widthTex * 4 + (i + dx) * 4 + 3] = pixels[j * dw * 4 + i * 4 + 3];
					}
				}
			}
			__dx = dx;
			__dy = dy;
			__dw = dw;
			__dh = dh;
			
#if true
			{
				//???
				//https://github.com/zphilip/PerceptualImaging/blob/9f2af79d4f318eea77a16811a9aa931ecdae219f/Visualization/ImageTextButton.cs
				//GL.Enable(EnableCap.Texture2D);
				
			   	//FIXME:tex
			   	//https://github.com/ebeisecker/Playpen/blob/b3fc7e08940e19566c06a8673438e3747832a650/iOSGLEssentials/iOSGLEssentials/OpenGLRenderer.cs
			   	GL.PixelStore ( PixelStoreParameter.UnpackAlignment, 1 ); //GL_UNPACK_ALIGNMENT
			  	GL.GenTextures(1, out __textureId); //glGenTextures ( 1, &textureId );
			  	//https://stackoverflow.com/questions/9726793/how-to-load-and-draw-texture-correctly-in-opengles-with-mono-for-android
			  	//GL.shad
			  	
			  	//check textures count overflow
			  	if (__textureId > __maxtextureId)
			  	{
			  		__maxtextureId = __textureId;
			  		Debug.WriteLine("==========__maxtextureId == " + __maxtextureId);
			  	}
			  	
			  	GL.BindTexture (TextureTarget.Texture2D, __textureId); //glBindTexture ( GL_TEXTURE_2D, textureId );
			    //PixelInternalFormat.Rgba
			   	GL.TexImage2D<byte>(TextureTarget2d.Texture2D, 0, TextureComponentCount.Rgba, __widthTex, __heightTex, 0, 
			                       OpenTK.Graphics.ES20.PixelFormat.Rgba, PixelType.UnsignedByte, __pixels);
			   	//Bgra
			   	//Debug.WriteLine("==============>pixels.Length == " + pixels.Length);
			  	 //glTexImage2D ( GL_TEXTURE_2D, 0, GL_RGB, 2, 2, 0, GL_RGB, GL_UNSIGNED_BYTE, pixels );
//			   	GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Nearest);// glTexParameteri ( GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST );
//			   	GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Nearest);//glTexParameteri ( GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST );
				
			  	//smooth
//			  	GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
//            	GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);
			   	GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);
			   	GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);
#if true
				//allow non power of two textures
				//http://stackoverflow.com/questions/11069441/non-power-of-two-textures-in-ios
				//http://blog.csdn.net/mvplinchen888/article/details/16946565
			   	GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)All.ClampToEdge);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)All.ClampToEdge);
#else
				//FIXME:???
				switch (this.__wrap)
				{
					case TextureWrapMode.ClampToEdge:
						//FIXME:???
					   	GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)All.ClampToEdge);
						GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)All.ClampToEdge);							
						break;
						
					case TextureWrapMode.Repeat:
						//FIXME:???
					   	GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)All.Repeat);
						GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)All.Repeat);							
						break;
						
					default:
						Debug.Assert(false);
						break;
				}
#endif
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
			   	GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)All.ClampToEdge);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)All.ClampToEdge);
			}
#endif
		}
		
		public override void Dispose()
		{
			//Debug.WriteLine("===================");
			base.Dispose();
			GL.DeleteTexture(this.__textureId);
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
