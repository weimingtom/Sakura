using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.UI
{
	public class ScrollBar : Widget
	{
		private float defaultScrollBarHorizontalWidth = 482f;

		private float defaultScrollBarHorizontalHeight = 10f;

		private float defaultScrollBarVerticalWidth = 10f;

		private float defaultScrollBarVerticalHeight = 260f;

		private float scrollBarMinWidth = 10f;

		private float scrollBarMinHeight = 10f;

		private float length;

		private float barPosition;

		private float barLength;

		private ImageBox baseImage;

		private ImageBox barImage;

		public override float Width
		{
			get
			{
				return base.Width;
			}
			set
			{
				base.Width = value;
				if (this.Orientation == ScrollBarOrientation.Horizontal)
				{
					if (this.baseImage != null)
					{
						this.baseImage.Width = value;
						this.UpdateView();
						return;
					}
				}
				else if (base.Width < this.scrollBarMinWidth)
				{
					base.Width = this.scrollBarMinWidth;
				}
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
				base.Height = value;
				if (this.Orientation == ScrollBarOrientation.Vertical)
				{
					if (this.baseImage != null)
					{
						this.baseImage.Height = value;
						this.UpdateView();
						return;
					}
				}
				else if (base.Height < this.scrollBarMinHeight)
				{
					base.Height = this.scrollBarMinHeight;
				}
			}
		}

		public float Length
		{
			get
			{
				return this.length;
			}
			set
			{
				this.length = value;
				this.UpdateView();
			}
		}

		public ScrollBarOrientation Orientation
		{
			get;
			private set;
		}

		public float BarPosition
		{
			get
			{
				return this.barPosition;
			}
			set
			{
				this.barPosition = value;
				this.UpdateView();
			}
		}

		public float BarLength
		{
			get
			{
				return this.barLength;
			}
			set
			{
				this.barLength = value;
				this.UpdateView();
			}
		}

		public ScrollBar(ScrollBarOrientation orientation)
		{
			this.Orientation = orientation;
			this.baseImage = new ImageBox();
			base.AddChildLast(this.baseImage);
			this.barImage = new ImageBox();
			base.AddChildLast(this.barImage);
			switch (this.Orientation)
			{
			case ScrollBarOrientation.Horizontal:
				base.Width = this.defaultScrollBarHorizontalWidth;
				base.Height = this.defaultScrollBarHorizontalHeight;
				this.baseImage.Image = new ImageAsset(SystemImageAsset.ScrollBarHorizontalBackground);
				this.baseImage.Width = this.defaultScrollBarHorizontalWidth;
				this.baseImage.Height = this.defaultScrollBarHorizontalHeight;
				this.baseImage.NinePatchMargin = AssetManager.GetNinePatchMargin(SystemImageAsset.ScrollBarHorizontalBackground);
				this.baseImage.ImageScaleType = ImageScaleType.NinePatch;
				this.barImage.Image = new ImageAsset(SystemImageAsset.ScrollBarHorizontalBar);
				this.barImage.Width = this.defaultScrollBarHorizontalWidth;
				this.barImage.Height = this.defaultScrollBarHorizontalHeight;
				this.barImage.NinePatchMargin = AssetManager.GetNinePatchMargin(SystemImageAsset.ScrollBarHorizontalBar);
				this.barImage.ImageScaleType = ImageScaleType.NinePatch;
				return;
			case ScrollBarOrientation.Vertical:
				base.Width = this.defaultScrollBarVerticalWidth;
				base.Height = this.defaultScrollBarVerticalHeight;
				this.baseImage.Image = new ImageAsset(SystemImageAsset.ScrollBarVerticalBackground);
				this.baseImage.Width = this.defaultScrollBarVerticalWidth;
				this.baseImage.Height = this.defaultScrollBarVerticalHeight;
				this.baseImage.NinePatchMargin = AssetManager.GetNinePatchMargin(SystemImageAsset.ScrollBarVerticalBackground);
				this.baseImage.ImageScaleType = ImageScaleType.NinePatch;
				this.barImage.Image = new ImageAsset(SystemImageAsset.ScrollBarVerticalBar);
				this.barImage.Width = this.defaultScrollBarVerticalWidth;
				this.barImage.Height = this.defaultScrollBarVerticalHeight;
				this.barImage.NinePatchMargin = AssetManager.GetNinePatchMargin(SystemImageAsset.ScrollBarVerticalBar);
				this.barImage.ImageScaleType = ImageScaleType.NinePatch;
				return;
			default:
				return;
			}
		}

		protected override void DisposeSelf()
		{
			if (this.baseImage != null)
			{
				this.baseImage.Image.Dispose();
			}
			if (this.barImage != null)
			{
				this.barImage.Image.Dispose();
			}
			base.DisposeSelf();
		}

		private void UpdateView()
		{
			if (this.barImage == null || this.baseImage == null)
			{
				return;
			}
			if (this.Width <= 0f || this.Height <= 0f || this.Length <= 0f)
			{
				this.barImage.Visible = false;
				this.baseImage.Visible = false;
				return;
			}
			this.barImage.Visible = true;
			this.baseImage.Visible = true;
			float num = FMath.Clamp(this.barLength, 0f, this.length);
			this.barPosition = FMath.Clamp(this.barPosition, 0f, this.length - num);
			switch (this.Orientation)
			{
			case ScrollBarOrientation.Horizontal:
				this.barImage.Width = this.Width * (num / this.length);
				this.barImage.X = this.Width * (this.barPosition / this.length);
				return;
			case ScrollBarOrientation.Vertical:
				this.barImage.Height = this.Height * (num / this.Length);
				this.barImage.Y = this.Height * (this.barPosition / this.length);
				return;
			default:
				return;
			}
		}
	}
}
