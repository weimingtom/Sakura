using Sce.Pss.Core.Imaging;
using System;
using System.Collections.Generic;
using Sce.Pss.Core.Graphics;

namespace Sce.Pss.HighLevel.UI
{
	public class ImageBox : Widget
	{
		private delegate void SetupUISpriteImageScale();

		private NinePatchMargin ninePatchMargin;

		private ImageScaleType scale;

		private ImageRect cropArea;

		private Dictionary<ImageScaleType, ImageBox.SetupUISpriteImageScale> setupSpriteImageScale;

		private UISprite sprt;

		private UIPrimitive ninePatchPrim;

		private float clipImageX;

		private float clipImageY;

		private float clipImageWidth;

		private float clipImageHeight;

		private bool needUpdateSprite = true;

		public override float Width
		{
			get
			{
				return base.Width;
			}
			set
			{
				base.Width = value;
				this.needUpdateSprite = true;
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
				this.needUpdateSprite = true;
			}
		}

		public ImageAsset Image
		{
			get
			{
				return this.sprt.Image;
			}
			set
			{
				this.sprt.Image = value;
				this.ninePatchPrim.Image = value;
				this.needUpdateSprite = true;
			}
		}

		public NinePatchMargin NinePatchMargin
		{
			get
			{
				return this.ninePatchMargin;
			}
			set
			{
				this.ninePatchMargin = value;
				this.needUpdateSprite = true;
			}
		}

		public ImageScaleType ImageScaleType
		{
			get
			{
				return this.scale;
			}
			set
			{
				this.scale = value;
				this.needUpdateSprite = true;
			}
		}

		public ImageRect CropArea
		{
			get
			{
				return this.cropArea;
			}
			set
			{
				this.cropArea = value;
				this.needUpdateSprite = true;
			}
		}

		public ImageBox()
		{
			this.sprt = new UISprite(1);
			base.RootUIElement.AddChildLast(this.sprt);
			this.sprt.ShaderType = ShaderType.Texture;
			this.ninePatchPrim = new UIPrimitive((DrawMode)4, 16, 28);
			base.RootUIElement.AddChildLast(this.ninePatchPrim);
			this.ninePatchPrim.ShaderType = ShaderType.Texture;
			this.ninePatchPrim.Visible = false;
			this.ninePatchMargin = NinePatchMargin.Zero;
			this.scale = ImageScaleType.Stretch;
			this.cropArea = new ImageRect(0, 0, 0, 0);
			this.clipImageX = 0f;
			this.clipImageY = 0f;
			this.clipImageWidth = 0f;
			this.clipImageHeight = 0f;
			Dictionary<ImageScaleType, ImageBox.SetupUISpriteImageScale> dictionary = new Dictionary<ImageScaleType, ImageBox.SetupUISpriteImageScale>();
			dictionary.Add(ImageScaleType.Stretch, new ImageBox.SetupUISpriteImageScale(this.SetupUISpriteNormal));
			dictionary.Add(ImageScaleType.AspectInside, new ImageBox.SetupUISpriteImageScale(this.SetupUISpriteAspectInside));
			dictionary.Add(ImageScaleType.AspectOutside, new ImageBox.SetupUISpriteImageScale(this.SetupUISpriteAspectOutside));
			dictionary.Add(ImageScaleType.NinePatch, new ImageBox.SetupUISpriteImageScale(this.SetupUIPrimitiveNinePatch));
			dictionary.Add(ImageScaleType.Center, new ImageBox.SetupUISpriteImageScale(this.SetupUISpriteNone));
			this.setupSpriteImageScale = dictionary;
		}

		private void updateSprite()
		{
			if (this.sprt.Image == null)
			{
				this.sprt.Visible = false;
				this.ninePatchPrim.Visible = false;
				this.needUpdateSprite = false;
				return;
			}
			if (this.sprt.Image.Ready && this.sprt.Image.Width > 0 && this.sprt.Image.Height > 0)
			{
				if (this.CropArea.X != 0 || this.CropArea.Y != 0 || this.CropArea.Width != 0 || this.CropArea.Height != 0)
				{
					this.clipImageX = (float)this.CropArea.X;
					this.clipImageY = (float)this.CropArea.Y;
					this.clipImageWidth = (float)this.CropArea.Width;
					this.clipImageHeight = (float)this.CropArea.Height;
				}
				else
				{
					this.clipImageX = 0f;
					this.clipImageY = 0f;
					this.clipImageWidth = (float)this.sprt.Image.Width;
					this.clipImageHeight = (float)this.sprt.Image.Height;
				}
				this.setupSpriteImageScale[this.scale]();
				this.needUpdateSprite = false;
			}
		}

		protected internal override void Render()
		{
			if (this.needUpdateSprite)
			{
				this.updateSprite();
			}
			base.Render();
		}

		private void SetupUISpriteNormal()
		{
			this.sprt.Visible = true;
			this.ninePatchPrim.Visible = false;
			float num = (float)this.sprt.Image.Width;
			float num2 = (float)this.sprt.Image.Height;
			UISpriteUnit unit = this.sprt.GetUnit(0);
			unit.X = 0f;
			unit.Y = 0f;
			unit.Width = this.Width;
			unit.Height = this.Height;
			unit.U1 = this.clipImageX / num;
			unit.V1 = this.clipImageY / num2;
			unit.U2 = (this.clipImageX + this.clipImageWidth) / num;
			unit.V2 = (this.clipImageY + this.clipImageHeight) / num2;
		}

		private void SetupUISpriteAspectInside()
		{
			this.sprt.Visible = true;
			this.ninePatchPrim.Visible = false;
			float num = (float)this.sprt.Image.Width;
			float num2 = (float)this.sprt.Image.Height;
			float num3 = this.Width / this.clipImageWidth;
			float num4 = this.Height / this.clipImageHeight;
			float num5 = (num3 < num4) ? num3 : num4;
			float num6 = this.clipImageWidth * num5;
			float num7 = this.clipImageHeight * num5;
			UISpriteUnit unit = this.sprt.GetUnit(0);
			unit.X = (this.Width - num6) / 2f;
			unit.Y = (this.Height - num7) / 2f;
			unit.Width = num6;
			unit.Height = num7;
			unit.U1 = this.clipImageX / num;
			unit.V1 = this.clipImageY / num2;
			unit.U2 = (this.clipImageX + this.clipImageWidth) / num;
			unit.V2 = (this.clipImageY + this.clipImageHeight) / num2;
		}

		private void SetupUISpriteAspectOutside()
		{
			this.sprt.Visible = true;
			this.ninePatchPrim.Visible = false;
			float num = (float)this.sprt.Image.Width;
			float num2 = (float)this.sprt.Image.Height;
			float num3 = this.Width / this.clipImageWidth;
			float num4 = this.Height / this.clipImageHeight;
			float num5 = (num3 > num4) ? num3 : num4;
			float num6 = num * num5;
			float num7 = num2 * num5;
			float num8 = this.clipImageX * num5;
			float num9 = this.clipImageY * num5;
			float num10 = this.clipImageWidth * num5;
			float num11 = this.clipImageHeight * num5;
			float num12 = num8 + (num10 - this.Width) / 2f;
			float num13 = num9 + (num11 - this.Height) / 2f;
			UISpriteUnit unit = this.sprt.GetUnit(0);
			unit.X = 0f;
			unit.Y = 0f;
			unit.Width = this.Width;
			unit.Height = this.Height;
			unit.U1 = num12 / num6;
			unit.V1 = num13 / num7;
			unit.U2 = (num12 + this.Width) / num6;
			unit.V2 = (num13 + this.Height) / num7;
		}

		private void SetupUIPrimitiveNinePatch()
		{
			this.sprt.Visible = false;
			this.ninePatchPrim.Visible = true;
			UIPrimitiveUtility.SetupNinePatch(this.ninePatchPrim, this.Width, this.Height, 0f, 0f, this.cropArea, this.ninePatchMargin);
		}

		private void SetupUISpriteNone()
		{
			this.sprt.Visible = true;
			this.ninePatchPrim.Visible = false;
			float num = (float)this.sprt.Image.Width;
			float num2 = (float)this.sprt.Image.Height;
			float num3 = Math.Abs(this.Width - this.clipImageWidth) / 2f;
			float num4 = Math.Abs(this.Height - this.clipImageHeight) / 2f;
			UISpriteUnit unit = this.sprt.GetUnit(0);
			if (this.clipImageWidth > this.Width)
			{
				unit.X = 0f;
				unit.Width = this.Width;
				unit.U1 = (this.clipImageX + num3) / num;
				unit.U2 = (this.clipImageX + num3 + this.Width) / num;
			}
			else
			{
				unit.X = num3;
				unit.Width = this.clipImageWidth;
				unit.U1 = this.clipImageX / num;
				unit.U2 = (this.clipImageX + this.clipImageWidth) / num;
			}
			if (this.clipImageHeight > this.Height)
			{
				unit.Y = 0f;
				unit.Height = this.Height;
				unit.V1 = (this.clipImageY + num4) / num2;
				unit.V2 = (this.clipImageY + num4 + this.Height) / num2;
				return;
			}
			unit.Y = num4;
			unit.Height = this.clipImageHeight;
			unit.V1 = this.clipImageY / num2;
			unit.V2 = (this.clipImageY + this.clipImageHeight) / num2;
		}
	}
}
