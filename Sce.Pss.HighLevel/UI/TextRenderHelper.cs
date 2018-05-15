using Sce.Pss.Core.Imaging;
using System;
using System.Collections.Generic;
using System.Text;
using Sce.Pss.Core.Graphics;

namespace Sce.Pss.HighLevel.UI
{
	internal class TextRenderHelper
	{
		private delegate List<string> ReflectLineBreak(StringBuilder text, CharMetrics[] metrics, float width, int maxLineCount);

		private delegate string ReflectTextTrimming(StringBuilder text, CharMetrics[] metrics, float width);

		public const FontAlias DefaultFontAlias = 0;

		public const int DefaultFontSize = 25;

		public const FontStyle DefaultFontStyle = 0;

		public const HorizontalAlignment DefaultHorizontalAlignment = HorizontalAlignment.Center;

		public const VerticalAlignment DefaultVerticalAlignment = VerticalAlignment.Middle;

		private const float textShadowMarginLeft = 2f;

		private const float textShadowMarginBottom = 2f;

		public static readonly UIColor DefaultTextColor = new UIColor(1f, 1f, 1f, 1f);

		public static readonly UIColor DefaultBackgroundColor = new UIColor(0f, 0f, 0f, 0f);

		private Font font;

		private HorizontalAlignment horizontalAlignment;

		private VerticalAlignment verticalAlignment;

		private float horizontalOffset;

		private float verticalOffset;

		private LineBreak lineBreak;

		private TextTrimming textTrimming;

		private float lineGap;

		private Dictionary<LineBreak, TextRenderHelper.ReflectLineBreak> ReflectLineBreakFuncs;

		private Dictionary<TextTrimming, TextRenderHelper.ReflectTextTrimming> ReflectTextTrimmingFuncs;

		private char wordSeparator = ' ';

		private char hyphen = '-';

		private string elipsis = "...";

		private List<char> delimiters;

		public Font Font
		{
			get
			{
				return this.font;
			}
			set
			{
				if (value == null)
				{
					this.font = UISystem.DefaultFont;
					return;
				}
				this.font = value;
			}
		}

		public HorizontalAlignment HorizontalAlignment
		{
			get
			{
				return this.horizontalAlignment;
			}
			set
			{
				this.horizontalAlignment = value;
			}
		}

		public VerticalAlignment VerticalAlignment
		{
			get
			{
				return this.verticalAlignment;
			}
			set
			{
				this.verticalAlignment = value;
			}
		}

		public float HorizontalOffset
		{
			get
			{
				return this.horizontalOffset / UISystem.Scale;
			}
			set
			{
				this.horizontalOffset = value * UISystem.Scale;
			}
		}

		public float VerticalOffset
		{
			get
			{
				return this.verticalOffset / UISystem.Scale;
			}
			set
			{
				this.verticalOffset = value * UISystem.Scale;
			}
		}

		public LineBreak LineBreak
		{
			get
			{
				return this.lineBreak;
			}
			set
			{
				this.lineBreak = value;
			}
		}

		public TextTrimming TextTrimming
		{
			get
			{
				return this.textTrimming;
			}
			set
			{
				this.textTrimming = value;
			}
		}

		public float LineGap
		{
			get
			{
				return this.lineGap / UISystem.Scale;
			}
			set
			{
				this.lineGap = value * UISystem.Scale;
			}
		}

		private float LineHeight
		{
			get
			{
				return (float)(this.Font.Metrics.Ascent + this.Font.Metrics.Descent + this.Font.Metrics.Leading);
			}
		}

		public TextRenderHelper()
		{
			List<char> list = new List<char>();
			list.Add(',');
			list.Add('.');
			list.Add('。');
			list.Add('、');
			this.delimiters = list;
//			base..ctor(); //FIXME:???
			this.Font = UISystem.DefaultFont;
			this.horizontalAlignment = HorizontalAlignment.Center;
			this.verticalAlignment = VerticalAlignment.Middle;
			this.horizontalOffset = 0f;
			this.verticalOffset = 0f;
			this.lineBreak = LineBreak.Character;
			this.textTrimming = TextTrimming.None;
			this.lineGap = 0f;
			Dictionary<LineBreak, TextRenderHelper.ReflectLineBreak> dictionary = new Dictionary<LineBreak, TextRenderHelper.ReflectLineBreak>();
			dictionary.Add(LineBreak.Character, new TextRenderHelper.ReflectLineBreak(this.ReflectLineBreakCharacter));
			dictionary.Add(LineBreak.Word, new TextRenderHelper.ReflectLineBreak(this.ReflectLineBreakWord));
			dictionary.Add(LineBreak.Hyphenation, new TextRenderHelper.ReflectLineBreak(this.ReflectLineBreakHyphenation));
			dictionary.Add(LineBreak.AtCode, new TextRenderHelper.ReflectLineBreak(this.ReflectLineBreakAtCode));
			this.ReflectLineBreakFuncs = dictionary;
			Dictionary<TextTrimming, TextRenderHelper.ReflectTextTrimming> dictionary2 = new Dictionary<TextTrimming, TextRenderHelper.ReflectTextTrimming>();
			dictionary2.Add(TextTrimming.None, new TextRenderHelper.ReflectTextTrimming(this.ReflectTextTrimmingNone));
			dictionary2.Add(TextTrimming.Character, new TextRenderHelper.ReflectTextTrimming(this.ReflectTextTrimmingCharacter));
			dictionary2.Add(TextTrimming.Word, new TextRenderHelper.ReflectTextTrimming(this.ReflectTextTrimmingWord));
			dictionary2.Add(TextTrimming.EllipsisCharacter, new TextRenderHelper.ReflectTextTrimming(this.ReflectTextTrimmingEllipsisCharacter));
			dictionary2.Add(TextTrimming.EllipsisWord, new TextRenderHelper.ReflectTextTrimming(this.ReflectTextTrimmingEllipsisWord));
			this.ReflectTextTrimmingFuncs = dictionary2;
		}

		private float GetMultiLineHeight(int lineCount)
		{
			if (lineCount == 0)
			{
				return 0f;
			}
			float num = this.LineHeight * (float)lineCount - (float)this.Font.Metrics.Leading + this.lineGap * (float)(lineCount - 1);
			num += this.verticalOffset;
			return num + 2f;
		}

		private int GetMaxLineCount(float height)
		{
			int num = (int)((height - this.verticalOffset - 2f) / (this.LineHeight + this.lineGap));
			int num2 = (int)((height - this.verticalOffset - 2f) % (this.LineHeight + this.lineGap));
			if (num == 0 || (float)num2 > this.LineHeight + 2f)
			{
				num++;
			}
			return num;
		}

		private float GetLineWidth(CharMetrics[] metrics, int startIndex, int length)
		{
			if (length == 0)
			{
				return 0f;
			}
			int num = startIndex + length - 1;
			float num2 = metrics[num].X + metrics[num].HorizontalAdvance - metrics[startIndex].X;
			num2 += this.horizontalOffset;
			return num2 + 2f;
		}

		private float GetLineWidth(string text)
		{
			float num = (float)this.Font.GetTextWidth(text);
			num += this.horizontalOffset;
			return num + 2f;
		}

		public ImageSize GetImageSize(string text)
		{
			if (!string.IsNullOrEmpty(text))
			{
				ImageSize result = new ImageSize(0, 0);
				List<string> list = this.SplitLineBreakeCode(new StringBuilder(text));
				using (List<string>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string current = enumerator.Current;
						float lineWidth = this.GetLineWidth(current);
						if (lineWidth > (float)result.Width)
						{
							result.Width = (int)(lineWidth / UISystem.Scale);
						}
					}
				}
				result.Height = (int)(this.GetMultiLineHeight(list.Count) / UISystem.Scale);
				return result;
			}
			return new ImageSize(0, 0);
		}

		public float GetTotalHeight(string text, float width)
		{
			if (!string.IsNullOrEmpty(text) && width > 0f)
			{
				width *= UISystem.Scale;
				List<string> list = new List<string>();
				List<string> list2 = this.SplitLineBreakeCode(new StringBuilder(text));
				using (List<string>.Enumerator enumerator = list2.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string current = enumerator.Current;
						CharMetrics[] textMetrics = this.Font.GetTextMetrics(current);
						float lineWidth = this.GetLineWidth(textMetrics, 0, textMetrics.Length);
						List<string> list3 = new List<string>();
						if (lineWidth > width)
						{
							list3 = this.ReflectLineBreakFuncs[this.LineBreak](new StringBuilder(current), textMetrics, width, 2147483647);
						}
						else
						{
							list3.Add(current);
						}
						using (List<string>.Enumerator enumerator2 = list3.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								string current2 = enumerator2.Current;
								list.Add(current2);
							}
						}
					}
				}
				return this.GetMultiLineHeight(list.Count) / UISystem.Scale;
			}
			return 0f;
		}

		public ImageAsset DrawText(ref string text, int width, int height)
		{
			//FIXME:这里写入内存位图
			ImageColor imageColor = new ImageColor((int)(TextRenderHelper.DefaultBackgroundColor.R * 255f), (int)(TextRenderHelper.DefaultBackgroundColor.G * 255f), (int)(TextRenderHelper.DefaultBackgroundColor.B * 255f), (int)(TextRenderHelper.DefaultBackgroundColor.A * 255f));
			ImageColor imageColor2 = new ImageColor((int)(TextRenderHelper.DefaultTextColor.R * 255f), (int)(TextRenderHelper.DefaultTextColor.G * 255f), (int)(TextRenderHelper.DefaultTextColor.B * 255f), (int)(TextRenderHelper.DefaultTextColor.A * 255f));
			bool flag = string.IsNullOrEmpty(text);
			if (UISystem.Scaled)
			{
				width = (int)((float)width * UISystem.Scale);
				height = (int)((float)height * UISystem.Scale);
			}
			if (width <= 0)
			{
				width = 1;
				flag = true;
			}
			if (height <= 0)
			{
				height = 1;
				flag = true;
			}
			int maxTextureSize = UISystem.GraphicsContext.Caps.MaxTextureSize;
			if (width > maxTextureSize)
			{
				width = maxTextureSize;
			}
			if (height > maxTextureSize)
			{
				height = maxTextureSize;
			}
			Image image = new Image(ImageMode.A/*(ImageMode)1*/, new ImageSize(width, height), imageColor);
			ImageAsset imageAsset;
			if (flag)
			{
				imageAsset = new ImageAsset(image, (PixelFormat)10);
				image.Dispose();
				imageAsset.AdjustScaledSize = true;
				return imageAsset;
			}
			List<string> list = this.SplitText(new StringBuilder(text), (float)width, (float)height);
			float num = this.GetMultiLineHeight(list.Count) - this.verticalOffset;
			float num2 = 0f;
			switch (this.VerticalAlignment)
			{
			case VerticalAlignment.Top:
				num2 = 0f;
				break;
			case VerticalAlignment.Middle:
				num2 = ((float)height - num + (float)this.Font.Metrics.Descent) / 2f - 0.001f;
				break;
			case VerticalAlignment.Bottom:
				num2 = (float)height - num;
				break;
			}
			using (List<string>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string current = enumerator.Current;
					float num3 = this.GetLineWidth(current) - this.horizontalOffset;
					float num4 = 0f;
					switch (this.HorizontalAlignment)
					{
					case HorizontalAlignment.Left:
						num4 = 0f;
						break;
					case HorizontalAlignment.Center:
						num4 = ((float)width - num3) / 2f;
						break;
					case HorizontalAlignment.Right:
						num4 = (float)width - num3;
						break;
					}
					image.DrawText(current, imageColor2, this.Font, new ImagePosition((int)(num4 + this.horizontalOffset), (int)(num2 + this.verticalOffset)));
					num2 += this.LineHeight + this.lineGap;
				}
			}
			//image.__saveToFile("a.png"); //FIXME:调试看是否正确
			imageAsset = new ImageAsset(image, PixelFormat.Alpha);//(PixelFormat)10);
			imageAsset.AdjustScaledSize = true;
			image.Dispose();
			return imageAsset;
		}

		private List<string> SplitText(StringBuilder text, float width, float height)
		{
			List<string> list = new List<string>();
			List<string> list2 = this.SplitLineBreakeCode(text);
			int maxLineCount = this.GetMaxLineCount(height);
			if (this.LineBreak == LineBreak.AtCode)
			{
				for (int i = 0; i < list2.Count; i++)
				{
					if (i >= maxLineCount)
					{
						break;
					}
					CharMetrics[] textMetrics = this.Font.GetTextMetrics(list2[i]);
					if (this.GetLineWidth(textMetrics, 0, textMetrics.Length) > width)
					{
						list.Add(this.ReflectTextTrimmingFuncs[this.TextTrimming](new StringBuilder(list2[i]), textMetrics, width));
					}
					else
					{
						list.Add(list2[i]);
					}
				}
			}
			else
			{
				using (List<string>.Enumerator enumerator = list2.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string current = enumerator.Current;
						CharMetrics[] textMetrics2 = this.Font.GetTextMetrics(current);
						float lineWidth = this.GetLineWidth(textMetrics2, 0, textMetrics2.Length);
						if (lineWidth > width)
						{
							list.AddRange(this.ReflectLineBreakFuncs[this.LineBreak](new StringBuilder(current), textMetrics2, width, maxLineCount - list.Count));
						}
						else
						{
							list.Add(current);
						}
						if (list.Count >= maxLineCount)
						{
							string text2 = list[list.Count - 1];
							CharMetrics[] textMetrics3 = this.Font.GetTextMetrics(text2);
							if (this.GetLineWidth(textMetrics3, 0, textMetrics3.Length) > width)
							{
								list[list.Count - 1] = this.ReflectTextTrimmingFuncs[this.TextTrimming](new StringBuilder(text2), textMetrics3, width);
								break;
							}
							break;
						}
					}
				}
			}
			return list;
		}

		private List<string> SplitLineBreakeCode(StringBuilder text)
		{
			List<string> list = new List<string>();
			int num = 0;
			int num2 = 0;
			bool flag = false;
			for (int i = 0; i < text.Length; i++)
			{
				if (text[i].Equals('\r'))
				{
					list.Add(text.ToString(num2, num));
					num = 0;
					num2 = i + 1;
					flag = true;
				}
				else if (text[i].Equals('\n'))
				{
					if (!flag)
					{
						list.Add(text.ToString(num2, num));
					}
					num = 0;
					num2 = i + 1;
					flag = false;
				}
				else
				{
					num++;
					flag = false;
				}
			}
			if (num != 0)
			{
				list.Add(text.ToString(num2, num));
			}
			return list;
		}

		private List<string> ReflectLineBreakCharacter(StringBuilder text, CharMetrics[] metrics, float width, int maxLineCount)
		{
			List<string> list = new List<string>();
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < metrics.Length; i++)
			{
				float lineWidth = this.GetLineWidth(metrics, num2, num + 1);
				if (lineWidth > width && num != 0)
				{
					if (list.Count + 1 >= maxLineCount)
					{
						list.Add(text.ToString(num2, text.Length - num2));
						return list;
					}
					if (num == 1)
					{
						list.Add(text.ToString(num2, num));
						num = 0;
					}
					else if (this.delimiters.Contains(text[i]))
					{
						list.Add(text.ToString(num2, num - 1));
						num = 1;
					}
					else
					{
						list.Add(text.ToString(num2, num));
						num = 0;
					}
					num2 = i - num;
					i--;
				}
				else
				{
					num++;
				}
			}
			if (num != 0)
			{
				list.Add(text.ToString(num2, num));
			}
			return list;
		}

		private List<string> ReflectLineBreakWord(StringBuilder text, CharMetrics[] metrics, float width, int maxLineCount)
		{
			List<string> list = new List<string>();
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; i < metrics.Length; i++)
			{
				float lineWidth = this.GetLineWidth(metrics, num3, num + 1);
				if (lineWidth > width && num != 0)
				{
					if (list.Count + 1 >= maxLineCount)
					{
						list.Add(text.ToString(num3, text.Length - num3));
						return list;
					}
					if (num2 != 0)
					{
						list.Add(text.ToString(num3, num2));
						num -= num2;
						num2 = 0;
					}
					else if (num == 1)
					{
						list.Add(text.ToString(num3, num));
						num = 0;
					}
					else if (this.delimiters.Contains(text[i]))
					{
						list.Add(text.ToString(num3, num - 1));
						num = 1;
					}
					else
					{
						list.Add(text.ToString(num3, num));
						num = 0;
					}
					num3 = i - num;
					i--;
				}
				else
				{
					num++;
					if (this.wordSeparator.Equals(text[i]))
					{
						num2 = num;
					}
				}
			}
			if (num != 0)
			{
				list.Add(text.ToString(num3, num));
			}
			return list;
		}

		private List<string> ReflectLineBreakHyphenation(StringBuilder text, CharMetrics[] metrics, float width, int maxLineCount)
		{
			List<string> list = new List<string>();
			int num = 0;
			int num2 = 0;
			char c = ' ';
			for (int i = 0; i < metrics.Length; i++)
			{
				float num3 = this.GetLineWidth(metrics, num2, num + 1) + (float)this.Font.GetTextWidth(this.hyphen.ToString());
				if (num3 > width && num != 0)
				{
					if (list.Count + 1 >= maxLineCount)
					{
						list.Add(text.ToString(num2, text.Length - num2));
						return list;
					}
					if (num == 1)
					{
						if (c.Equals(this.wordSeparator) || text[i].Equals(this.wordSeparator))
						{
							list.Add(text.ToString(num2, num));
						}
						else
						{
							list.Add(text.ToString(num2, num) + this.hyphen);
						}
						num = 0;
					}
					else if (this.delimiters.Contains(text[i]))
					{
						if (c.Equals(this.wordSeparator) || text[i].Equals(this.wordSeparator))
						{
							list.Add(text.ToString(num2, num - 1));
						}
						else
						{
							list.Add(text.ToString(num2, num - 1) + this.hyphen);
						}
						num = 1;
					}
					else
					{
						if (c.Equals(this.wordSeparator) || text[i].Equals(this.wordSeparator))
						{
							list.Add(text.ToString(num2, num));
						}
						else
						{
							list.Add(text.ToString(num2, num) + this.hyphen);
						}
						num = 0;
					}
					num2 = i - num;
					i--;
				}
				else
				{
					c = text[i];
					num++;
				}
			}
			if (num != 0)
			{
				list.Add(text.ToString(num2, num));
			}
			return list;
		}

		private List<string> ReflectLineBreakAtCode(StringBuilder text, CharMetrics[] metrics, float width, int maxLineCount)
		{
			List<string> list = new List<string>();
			list.Add(text.ToString());
			return list;
		}

		private string ReflectTextTrimmingNone(StringBuilder text, CharMetrics[] metrics, float width)
		{
			return text.ToString();
		}

		private string ReflectTextTrimmingCharacter(StringBuilder text, CharMetrics[] metrics, float width)
		{
			for (int i = 0; i < metrics.Length; i++)
			{
				float lineWidth = this.GetLineWidth(metrics, 0, i + 1);
				if (lineWidth > width && i != 0)
				{
					return text.ToString(0, i);
				}
			}
			return text.ToString();
		}

		private string ReflectTextTrimmingWord(StringBuilder text, CharMetrics[] metrics, float width)
		{
			int num = 0;
			int i = 0;
			while (i < metrics.Length)
			{
				float lineWidth = this.GetLineWidth(metrics, 0, i + 1);
				if (lineWidth > width && i != 0)
				{
					if (num != 0)
					{
						return text.ToString(0, num);
					}
					return text.ToString(0, i);
				}
				else
				{
					if (text[i].Equals(this.wordSeparator))
					{
						num = i + 1;
					}
					i++;
				}
			}
			return text.ToString();
		}

		private string ReflectTextTrimmingEllipsisCharacter(StringBuilder text, CharMetrics[] metrics, float width)
		{
			for (int i = 0; i < metrics.Length; i++)
			{
				float num = this.GetLineWidth(metrics, 0, i + 1) + (float)this.Font.GetTextWidth(this.elipsis);
				if (num > width)
				{
					return text.ToString(0, i) + this.elipsis;
				}
			}
			return text.ToString() + this.elipsis;
		}

		private string ReflectTextTrimmingEllipsisWord(StringBuilder text, CharMetrics[] metrics, float width)
		{
			int num = 0;
			int i = 0;
			while (i < metrics.Length)
			{
				float num2 = this.GetLineWidth(metrics, 0, i + 1) + (float)this.Font.GetTextWidth(this.elipsis);
				if (num2 > width)
				{
					if (num != 0)
					{
						return text.ToString(0, num) + this.elipsis;
					}
					return text.ToString(0, i) + this.elipsis;
				}
				else
				{
					if (text[i].Equals(this.wordSeparator))
					{
						num = i + 1;
					}
					i++;
				}
			}
			return text.ToString() + this.elipsis;
		}
	}
}
