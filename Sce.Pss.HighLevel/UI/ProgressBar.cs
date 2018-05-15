using System;

namespace Sce.Pss.HighLevel.UI
{
	public class ProgressBar : Widget
	{
		private const float defaultProgressBarWidth = 362f;

		private const float defaultProgressBarHeight = 16f;

		private const float acceleratorOffset = 3f;

		private const float animationTime = 500f;

		private const float acceleratorScaledImageWidth = 45f;

		private float progress;

		private ProgressBarStyle style;

		private ImageBox baseImage;

		private ImageBox barImage;

		private UISprite acceleratorSprt;

		private float animationElapsedTime;

		private bool animation;

		public override float Width
		{
			get
			{
				return base.Width;
			}
			set
			{
				base.Width = value;
				if (this.baseImage != null)
				{
					this.baseImage.Width = value;
				}
				if (this.barImage != null)
				{
					this.barImage.Width = this.Width * this.progress;
				}
				if (this.acceleratorSprt != null)
				{
					UISpriteUnit unit = this.acceleratorSprt.GetUnit(0);
					unit.Width = this.barImage.Width - 6f;
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
			}
		}

		public float Progress
		{
			get
			{
				return this.progress;
			}
			set
			{
				if (this.progress != value)
				{
					this.progress = MathUtility.Clamp<float>(value, 0f, 1f);
					this.barImage.Width = this.Width * this.progress;
					UISpriteUnit unit = this.acceleratorSprt.GetUnit(0);
					unit.Width = this.barImage.Width - 6f;
					unit.U1 = 0f;
					unit.U2 = this.barImage.Width / (float)this.acceleratorSprt.Image.Width;
				}
			}
		}

		public ProgressBarStyle Style
		{
			get
			{
				return this.style;
			}
			set
			{
				if (this.style != value)
				{
					this.style = value;
					if (this.Style == ProgressBarStyle.Animation)
					{
						this.animation = true;
						this.acceleratorSprt.Visible = true;
						return;
					}
					this.animation = false;
					this.acceleratorSprt.Visible = false;
				}
			}
		}

		public ProgressBar()
		{
			base.Width = 362f;
			base.Height = 16f;
			this.progress = 0f;
			this.style = ProgressBarStyle.Normal;
			this.animationElapsedTime = 0f;
			this.baseImage = new ImageBox();
			this.baseImage.Image = new ImageAsset(SystemImageAsset.ProgressBarBase);
			this.baseImage.NinePatchMargin = AssetManager.GetNinePatchMargin(SystemImageAsset.ProgressBarBase);
			this.baseImage.ImageScaleType = ImageScaleType.NinePatch;
			this.baseImage.Width = 362f;
			this.baseImage.Height = 16f;
			base.AddChildLast(this.baseImage);
			this.barImage = new ImageBox();
			this.barImage.Image = new ImageAsset(SystemImageAsset.ProgressBarNormal);
			this.barImage.NinePatchMargin = AssetManager.GetNinePatchMargin(SystemImageAsset.ProgressBarNormal);
			this.barImage.ImageScaleType = ImageScaleType.NinePatch;
			this.barImage.Width = 0f;
			this.barImage.Height = 16f;
			base.AddChildLast(this.barImage);
			this.acceleratorSprt = new UISprite(1);
			this.acceleratorSprt.X = 3f;
			this.acceleratorSprt.Y = 3f;
			this.acceleratorSprt.ShaderType = ShaderType.Texture;
			this.acceleratorSprt.Image = new ImageAsset(SystemImageAsset.ProgressBarAccelerator);
			this.acceleratorSprt.Visible = false;
			this.acceleratorSprt.TextureWrapMode = 0;
			this.acceleratorSprt.BlendMode = BlendMode.Add;
			this.acceleratorSprt.Alpha = 0.5f;
			this.barImage.RootUIElement.AddChildLast(this.acceleratorSprt);
			UISpriteUnit unit = this.acceleratorSprt.GetUnit(0);
			unit.Width = 0f;
			unit.Height = (float)this.acceleratorSprt.Image.Height;
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
			if (this.acceleratorSprt != null)
			{
				this.acceleratorSprt.Image.Dispose();
			}
			base.DisposeSelf();
		}

		protected override void OnUpdate(float elapsedTime)
		{
			base.OnUpdate(elapsedTime);
			if (this.animation)
			{
				this.animationElapsedTime += elapsedTime;
				if (this.animationElapsedTime > 500f)
				{
					this.animationElapsedTime -= 500f;
				}
				float num = this.animationElapsedTime / 500f;
				UISpriteUnit unit = this.acceleratorSprt.GetUnit(0);
				unit.U1 = num;
				unit.U2 = this.barImage.Width / 45f + num;
			}
		}
	}
}
