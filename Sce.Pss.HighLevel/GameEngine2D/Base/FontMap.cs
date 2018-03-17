using Sce.Pss.Core;
using Sce.Pss.Core.Graphics;
using Sce.Pss.Core.Imaging;
using System;
using System.Collections.Generic;

namespace Sce.Pss.HighLevel.GameEngine2D.Base
{
	public class FontMap : IDisposable
	{
		public struct CharData
		{
			public Bounds2 UV;

			public Vector2 PixelSize;
		}

		public Texture2D Texture;

		public Dictionary<char, FontMap.CharData> CharSet;

		public float CharPixelHeight;

		public static string AsciiCharSet = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";

		private bool m_disposed = false;

		private FontMap.CharData[] m_ascii_char_data;

		private bool[] m_ascii_char_data_valid;

		public bool Disposed
		{
			get
			{
				return this.m_disposed;
			}
		}

		public FontMap(Font font, int fontmap_width = 512)
		{
			this.Initialize(font, FontMap.AsciiCharSet, fontmap_width);
		}

		public FontMap(Font font, string charset, int fontmap_width = 512)
		{
			this.Initialize(font, charset, fontmap_width);
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				Common.DisposeAndNullify<Texture2D>(ref this.Texture);
				this.m_disposed = true;
			}
		}

		public void Initialize(Font font, string charset, int fontmap_width = 512)
		{
			this.CharSet = new Dictionary<char, FontMap.CharData>();
			this.CharPixelHeight = (float)font.Metrics.Height;
			Image image = null;
			Vector2i vector2i = new Vector2i(0, 0);
			for (int i = 0; i < 2; i++)
			{
				Vector2i a = new Vector2i(0, 0);
				int num = 0;
				for (int j = 0; j < charset.Length; j++)
				{
					if (!this.CharSet.ContainsKey(charset[j]))
					{
						Vector2i value = new Vector2i(font.GetTextWidth(charset[j].ToString(), 0, 1), font.Metrics.Height);
						num = Common.Max(num, value.Y);
						if (a.X + value.X > fontmap_width)
						{
							a.X = 0;
							a.Y += num;
							num = 0;
							Common.Assert(value.Y <= fontmap_width);
						}
						if (i > 0)
						{
							image.DrawText(charset[j].ToString(), new ImageColor(255, 255, 255, 255), font, new ImagePosition(a.X, a.Y));
							Bounds2 uV = new Bounds2(a.Vector2() / vector2i.Vector2(), (a + value).Vector2() / vector2i.Vector2());
							uV = uV.OutrageousYVCoordFlip().OutrageousYTopBottomSwap();
							this.CharSet.Add(charset[j], new FontMap.CharData
							{
								UV = uV,
								PixelSize = value.Vector2()
							});
						}
						a.X += value.X;
						if (i == 0)
						{
							vector2i.X = Common.Max(vector2i.X, a.X);
							vector2i.Y = Common.Max(vector2i.Y, a.Y + num);
						}
					}
				}
				if (i == 0)
				{
					image = new Image((ImageMode)1, new ImageSize(vector2i.X, vector2i.Y), new ImageColor(0, 0, 0, 0));
					this.CharSet.Clear();
				}
			}
			this.Texture = new Texture2D(image.Size.Width, image.Size.Height, false, (PixelFormat)8);
			this.Texture.SetPixels(0, image.ToBuffer());
			image.Dispose();
			this.m_ascii_char_data = new FontMap.CharData[FontMap.AsciiCharSet.Length];
			this.m_ascii_char_data_valid = new bool[FontMap.AsciiCharSet.Length];
			for (int j = 0; j < FontMap.AsciiCharSet.Length; j++)
			{
				FontMap.CharData charData;
				this.m_ascii_char_data_valid[j] = this.CharSet.TryGetValue(FontMap.AsciiCharSet[j], out charData);
				this.m_ascii_char_data[j] = charData;
			}
			font.Dispose();
		}

		public bool TryGetCharData(char c, out FontMap.CharData cdata)
		{
			int num = (int)(c - ' ');
			bool result;
			if (num >= 0 && num < FontMap.AsciiCharSet.Length)
			{
				cdata = this.m_ascii_char_data[num];
				result = this.m_ascii_char_data_valid[num];
			}
			else if (!this.CharSet.TryGetValue(c, out cdata))
			{
				Console.WriteLine("The character [" + c + "] is not present in the FontMap you are trying to use. Please double check the input character set.");
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}
	}
}
