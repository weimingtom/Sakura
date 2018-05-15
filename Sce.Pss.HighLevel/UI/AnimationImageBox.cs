using System;

namespace Sce.Pss.HighLevel.UI
{
	public class AnimationImageBox : Widget
	{
		private UISprite sprt;

		private int frameWidth;

		private int frameHeight;

		private int frameCount;

		private int frameIndex;

		private float frameInterval;

		private float animateElapsedTime;

		private bool animation;

		private bool needUpdateSprite = true;

		public int FrameWidth
		{
			get
			{
				return this.frameWidth;
			}
			set
			{
				this.Stop();
				this.frameWidth = value;
				this.needUpdateSprite = true;
			}
		}

		public int FrameHeight
		{
			get
			{
				return this.frameHeight;
			}
			set
			{
				this.Stop();
				this.frameHeight = value;
				this.needUpdateSprite = true;
			}
		}

		public int FrameCount
		{
			get
			{
				return this.frameCount;
			}
			set
			{
				this.Stop();
				this.frameCount = ((value > 0) ? value : 0);
				this.needUpdateSprite = true;
			}
		}

		public float FrameInterval
		{
			get
			{
				return this.frameInterval;
			}
			set
			{
				this.Stop();
				this.frameInterval = ((value != 0f) ? value : 1f);
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
				this.Stop();
				this.sprt.Image = value;
				this.needUpdateSprite = true;
			}
		}

		public AnimationImageBox()
		{
			this.sprt = new UISprite(1);
			base.RootUIElement.AddChildLast(this.sprt);
			this.sprt.ShaderType = ShaderType.Texture;
			this.frameWidth = 0;
			this.frameHeight = 0;
			this.frameCount = 0;
			this.frameIndex = 0;
			this.frameInterval = 33.3f;
			this.animateElapsedTime = 0f;
		}

		public void Start()
		{
			this.animation = true;
		}

		public void Stop()
		{
			this.animation = false;
		}

		protected override void OnUpdate(float elapsedTime)
		{
			base.OnUpdate(elapsedTime);
			if (this.animation)
			{
				this.animateElapsedTime += elapsedTime;
				if (this.animateElapsedTime >= this.frameInterval)
				{
					int num = (int)(this.animateElapsedTime / this.frameInterval);
					int num2 = this.frameIndex + num;
					this.animateElapsedTime -= this.frameInterval * (float)num;
					num2 -= num2 / this.frameCount * this.frameCount;
					if (num2 != this.frameIndex)
					{
						this.frameIndex = num2;
						this.needUpdateSprite = true;
					}
				}
			}
		}

		protected internal override void Render()
		{
			if (this.needUpdateSprite)
			{
				this.UpdateUISpriteBeforeRender();
			}
			base.Render();
		}

		private void UpdateUISpriteBeforeRender()
		{
			if (this.frameWidth > 0 && this.frameHeight > 0 && this.frameCount > 0 && this.sprt.Image != null)
			{
				if (this.sprt.Image.Ready)
				{
					this.sprt.Visible = true;
					int num = this.sprt.Image.Width / this.frameWidth;
					UISpriteUnit unit = this.sprt.GetUnit(0);
					unit.X = 0f;
					unit.Y = 0f;
					unit.Width = (float)this.frameWidth;
					unit.Height = (float)this.frameHeight;
					unit.U1 = (float)(this.frameWidth * (this.frameIndex % num)) / (float)this.sprt.Image.Width;
					unit.V1 = (float)(this.frameHeight * (this.frameIndex / num)) / (float)this.sprt.Image.Height;
					unit.U2 = (float)(this.frameWidth * (this.frameIndex % num) + this.frameWidth) / (float)this.sprt.Image.Width;
					unit.V2 = (float)(this.frameHeight * (this.frameIndex / num) + this.frameHeight) / (float)this.sprt.Image.Height;
					this.needUpdateSprite = false;
					return;
				}
			}
			else
			{
				this.sprt.Visible = false;
				this.needUpdateSprite = false;
			}
		}
	}
}
