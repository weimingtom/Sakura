using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace Sce.Pss.Core.Imaging
{
	public class Image
	{
		private string __filename = null;
		public System.Drawing.Bitmap __img;
		
		public Image(byte[] bytes)
		{
			Debug.Assert(false);
		}
		
		private Image(System.Drawing.Bitmap img)
		{
			this.__img = img;
		}
		
		public Image(string filename)
		{
			this.__filename = filename.Replace("/Application/", "./");
		}
		
		public Image(ImageMode mode, ImageSize size, ImageColor color)
		{
			//Debug.Assert(mode == ImageMode.Rgba); //FIXME:???
			//PixelFormat.
			__img = new System.Drawing.Bitmap(size.Width, 
			                                  size.Height, 
			                                  PixelFormat.Format32bppArgb);
			System.Drawing.Color _backColor = System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
			using (System.Drawing.Graphics drawing = System.Drawing.Graphics.FromImage(__img))
			{
				drawing.Clear(_backColor);
				drawing.Save();
			}
		}
		
		public void DrawText(string text, ImageColor color, Font font, ImagePosition position)
		{
			System.Drawing.Color _color = System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
			System.Drawing.Color _backColor = System.Drawing.Color.FromArgb(255, 255, 0, 0);
			using (System.Drawing.Graphics drawing = System.Drawing.Graphics.FromImage(__img))
			using (Brush textBrush = new SolidBrush(_color))
			{
				//Debug.WriteLine("==============>DrawText: " + text);
				//drawing.Clear(_backColor2);
				//http://bbs.csdn.net/topics/350255409
				drawing.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
				//drawing.DrawString(text, font.__font, textBrush, position.X, position.Y); //see measureWidth, StringFormat should set same time
				StringFormat sf = StringFormat.GenericTypographic;
    			sf.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
				drawing.DrawString(text, font.__font, textBrush, position.X, position.Y, sf);
				drawing.Save();
			}
		}
		
		//https://github.com/PyramidTechnologies/ThermalTalk/blob/40818af6ee73b4514bfedb40623083d0576830bf/ThermalTalk.Imaging/ImageExt.cs
		public byte[] ToBuffer()
		{
            if (__img == null || __img.Size.IsEmpty)
            {
                return new byte[0];
            }

            BitmapData bitmapData = null;

            // This rectangle selects the entirety of the source bitmap for locking bits into memory
            var rect = new Rectangle(0, 0, __img.Width, __img.Height);

            try
            {
                // Acquire a lock on the image data so we can extra into our own byte stream
                // Note: Currently only supports data as 32bit, 4 channel 8-bit color
                bitmapData = __img.LockBits(
                    rect,
                    ImageLockMode.ReadOnly,
                    PixelFormat.Format32bppPArgb);

                // Create the output buffer
                int length = Math.Abs(bitmapData.Stride) * bitmapData.Height;
                byte[] results = new byte[length];

                // Copy from unmanaged to managed memory
                IntPtr ptr = bitmapData.Scan0;
                Marshal.Copy(ptr, results, 0, length);

                return results;

            }
            finally
            {
                if (bitmapData != null)
                    __img.UnlockBits(bitmapData);
            }
  		}
		
		public void Dispose()
		{
			if (__img != null)
			{
				__img.Dispose();
				__img = null;
			}
		}
		
		public void Decode()
		{
			//Debug.Assert(false);
			using (Bitmap bmp = new System.Drawing.Bitmap(this.__filename))
			{
				this.__img = new System.Drawing.Bitmap(bmp.Width, bmp.Height, PixelFormat.Format32bppArgb);
				using (System.Drawing.Graphics drawing = System.Drawing.Graphics.FromImage(this.__img))
				{
					drawing.DrawImage(bmp, 0, 0);
					drawing.Save();
				}
			}
		}
		
		public ImageSize Size
		{
			get
			{
				if (this.__filename != null && this.__img == null)
				{
					Decode();
				}
				return new ImageSize(this.__img.Width, this.__img.Height);
			}
		}
		
		public void DrawImage(Image source, ImagePosition position)
		{
			using (System.Drawing.Graphics drawing = System.Drawing.Graphics.FromImage(this.__img))
			{
				drawing.DrawImage(source.__img, position.X, position.Y);
				drawing.Save();
			}
		}
		
		public Image Crop(ImageRect rect)
		{
			System.Drawing.Rectangle rect2 = new System.Drawing.Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
			Bitmap img2 = this.__img.Clone(rect2, this.__img.PixelFormat);
			return new Image(img2);
		}
		
		public Image Resize(ImageSize size)
		{
			System.Drawing.Bitmap img = new System.Drawing.Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);
			using (System.Drawing.Graphics drawing = System.Drawing.Graphics.FromImage(img))
			{
				drawing.DrawImage(this.__img, 0, 0, size.Width, size.Height);
				drawing.Save();
			}
			return new Image(img);
		}
		
		public void __saveToFile(string filename) 
		{
//			Debug.Assert(false);
			this.__img.Save(filename, ImageFormat.Png);
		}
	}
}
