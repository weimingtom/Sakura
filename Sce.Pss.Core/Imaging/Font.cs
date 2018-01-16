using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Sce.Pss.Core.Graphics;

//https://github.com/gwahba/WinToolkit/blob/a239a43011f69f4d0f4b8690736b284157030a66/ImageHelper.cs
//https://github.com/dtsudo/DT-Sudoku/blob/f08defa3bf79f487ad6971e47e261d9f8074249e/Dependencies/AgateLib/Agate031/Source/Drivers/AgateLib.WinForms/BitmapFontUtil.cs
//https://github.com/fpawel/Mil82/blob/c675d6545c8dc231ccf2238a405395082803fc44/SysText/Metrics.cs
namespace Sce.Pss.Core.Imaging
{
	public class Font
	{
//		public Font()
//		{
//			
//		}
		
		public System.Drawing.Font __font;
		private FontAlias __alias; //always system
		private int __size; //24
		private FontStyle __style; //Regular
		private FontMetrics __metrics;
		
		public Font(FontAlias alias, int size, FontStyle style)
		{
			this.__alias = alias;
			this.__size = size;
			this.__style = style;
			System.Drawing.FontStyle _style = System.Drawing.FontStyle.Regular;
			switch (style)
			{
				case FontStyle.Regular:
					_style = System.Drawing.FontStyle.Regular;
					break;
					
				case FontStyle.Bold:
					_style = System.Drawing.FontStyle.Bold;
					break;
					
				case FontStyle.Italic:
					_style = System.Drawing.FontStyle.Italic;
					break;
			}
			this.__font = new System.Drawing.Font("Arial", (int)(__size * 0.8), _style); //FIXME:???0.8
			//this.__font = new System.Drawing.Font("Arial", __size, _style); //FIXME:???0.8
			//this.__font = new System.Drawing.Font(/*"宋体"*/FontFamily.GenericSansSerif, __size, _style);
			
			this.__metrics = new FontMetrics();
			
			
			IntPtr hdc = GetDC(IntPtr.Zero);
			IntPtr handle = __font.ToHfont();

			try
			{
				IntPtr handle2 = SelectObject(hdc, handle);
				TEXTMETRIC tEXTMETRIC = new TEXTMETRIC();
				GetTextMetrics(hdc, ref tEXTMETRIC);
				__metrics.Ascent = tEXTMETRIC.tmAscent;
				__metrics.Descent = tEXTMETRIC.tmDescent;
				__metrics.Leading = tEXTMETRIC.tmHeight - tEXTMETRIC.tmAscent - tEXTMETRIC.tmDescent;
				SelectObject(hdc, handle2);
			}
			finally
			{
				DeleteObject(handle);
				ReleaseDC(IntPtr.Zero, hdc);
			}
		}
		
		public void Dispose()
		{
			this.__font.Dispose();
		}
		
		public int GetTextWidth(string text, int offset, int len)
		{
			System.Drawing.Image img;
			System.Drawing.Graphics drawing;
			SizeF textSize;
			using (img = new System.Drawing.Bitmap(1, 1))
			using (drawing = System.Drawing.Graphics.FromImage(img))
			{
				textSize = drawing.MeasureString(text, __font);
			}
			int w = (int)textSize.Width;
			return w;
		}
		
		public FontMetrics Metrics
		{
			get
			{
				return __metrics;
			}
		}
		
		//https://github.com/alcexhim/AwesomeControls/blob/bc69f5033e10fe6882c35558fb3625fcfbb6c382/AwesomeControls/DesignerTools.cs
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct TEXTMETRIC
		{
			public int tmHeight;
			public int tmAscent;
			public int tmDescent;
			public int tmInternalLeading;
			public int tmExternalLeading;
			public int tmAveCharWidth;
			public int tmMaxCharWidth;
			public int tmWeight;
			public int tmOverhang;
			public int tmDigitizedAspectX;
			public int tmDigitizedAspectY;
			public char tmFirstChar;
			public char tmLastChar;
			public char tmDefaultChar;
			public char tmBreakChar;
			public byte tmItalic;
			public byte tmUnderlined;
			public byte tmStruckOut;
			public byte tmPitchAndFamily;
			public byte tmCharSet;
		}
		
		[DllImport("gdi32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
		[DllImport("gdi32.dll", CharSet = CharSet.Auto)]
		public static extern bool DeleteObject(IntPtr hObject);
		[DllImport("gdi32.dll", CharSet = CharSet.Auto)]
		public static extern bool GetTextMetrics(IntPtr hdc, ref TEXTMETRIC tm);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetDC(IntPtr hWnd);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static private extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);        
	}
}
