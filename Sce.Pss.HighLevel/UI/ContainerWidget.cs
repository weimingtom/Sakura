using System;
using System.Collections.Generic;

namespace Sce.Pss.HighLevel.UI
{
	public class ContainerWidget : Widget
	{
		public new virtual IEnumerable<Widget> Children
		{
			get
			{
				return base.Children;
			}
		}

		public override float Width
		{
			get
			{
				return base.Width;
			}
			set
			{
				this.updateWidth(base.Width, value);
				base.Width = value;
			}
		}

		public override float Height
		{
			get
			{
				return base.Height;
			}
			set
			{
				this.updateHeight(base.Height, value);
				base.Height = value;
			}
		}

		public virtual new void AddChildFirst(Widget child)
		{
			base.AddChildFirst(child);
		}

		public virtual new void AddChildLast(Widget child)
		{
			base.AddChildLast(child);
		}

		public virtual new void InsertChildBefore(Widget child, Widget nextChild)
		{
			base.InsertChildBefore(child, nextChild);
		}

		public virtual new void InsertChildAfter(Widget child, Widget prevChild)
		{
			base.InsertChildAfter(child, prevChild);
		}

		public virtual new void RemoveChild(Widget child)
		{
			if (child != null)
			{
				base.RemoveChild(child);
			}
		}

		internal void updateWidth(float srcWidth, float dstWidth)
		{
			if (srcWidth > 0f && dstWidth > 0f)
			{
				for (LinkedTree<Widget> linkedTree = base.LinkedTree.FirstChild; linkedTree != null; linkedTree = linkedTree.NextSibling)
				{
					Widget value = linkedTree.Value;
					int num = (int)(value.Anchors & (Anchors)240);
					if (num <= 32)
					{
						if (num != 0)
						{
							if (num != 16)
							{
								if (num == 32)
								{
									float num2 = value.X + value.Width;
									float num3 = srcWidth - num2;
									this.scaleWidth(value, dstWidth / srcWidth);
									value.X = dstWidth - num3 - value.Width;
								}
							}
							else
							{
								this.scaleWidth(value, dstWidth / srcWidth);
							}
						}
						else
						{
							this.scaleX(value, dstWidth / srcWidth);
							this.scaleWidth(value, dstWidth / srcWidth);
						}
					}
					else if (num != 48)
					{
						if (num != 64)
						{
							if (num == 96)
							{
								value.X += dstWidth - srcWidth;
							}
						}
						else
						{
							value.X += (dstWidth - srcWidth) / 2f;
						}
					}
					else
					{
						float num4 = value.Width + dstWidth - srcWidth;
						if (num4 > 0f)
						{
							value.Width = num4;
						}
					}
				}
			}
		}

		internal void updateHeight(float srcHeight, float dstHeight)
		{
			if (srcHeight > 0f && dstHeight > 0f)
			{
				for (LinkedTree<Widget> linkedTree = base.LinkedTree.FirstChild; linkedTree != null; linkedTree = linkedTree.NextSibling)
				{
					Widget value = linkedTree.Value;
					switch (value.Anchors & (Anchors)15)
					{
					case Anchors.None:
						this.scaleY(value, dstHeight / srcHeight);
						this.scaleHeight(value, dstHeight / srcHeight);
						break;
					case Anchors.Top:
						this.scaleHeight(value, dstHeight / srcHeight);
						break;
					case Anchors.Bottom:
					{
						float num = value.Y + value.Height;
						float num2 = srcHeight - num;
						this.scaleHeight(value, dstHeight / srcHeight);
						value.Y = dstHeight - num2 - value.Height;
						break;
					}
					case Anchors.Top | Anchors.Bottom:
					{
						float num3 = value.Height + dstHeight - srcHeight;
						if (num3 > 0f)
						{
							value.Height = num3;
						}
						break;
					}
					case Anchors.Height:
						value.Y += (dstHeight - srcHeight) / 2f;
						break;
					case Anchors.Bottom | Anchors.Height:
						value.Y += dstHeight - srcHeight;
						break;
					}
				}
			}
		}

		private void scaleX(Widget widget, float rate)
		{
			widget.X *= rate;
		}

		private void scaleY(Widget widget, float rate)
		{
			widget.Y *= rate;
		}

		private void scaleWidth(Widget widget, float rate)
		{
			float num = widget.Width * rate;
			if (num > 0f)
			{
				widget.Width = num;
			}
		}

		private void scaleHeight(Widget widget, float rate)
		{
			float num = widget.Height * rate;
			if (num > 0f)
			{
				widget.Height = num;
			}
		}
	}
}
