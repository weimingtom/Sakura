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
		public System.Drawing.Bitmap __img;
		
		public Image(ImageMode mode, ImageSize size, ImageColor color)
		{
			if (mode != ImageMode.Rgba)
			{
				Debug.Assert(false);
			}
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
				Debug.WriteLine("==============>DrawText: " + text);
				//drawing.Clear(_backColor);
				//http://bbs.csdn.net/topics/350255409
				drawing.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
				drawing.DrawString(text, font.__font, textBrush, position.X, position.Y);
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
			
		}
	}
}
