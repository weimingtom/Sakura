using System;

namespace Sce.Pss.HighLevel.UI
{
	public class BusyIndicator : Widget
	{
		private const int defaultBusyIndicatorWidth = 48;

		private const int defaultBusyIndicatorHeight = 48;

		private const int frameCount = 8;

		private const float frameInterval = 66.7f;

		private const float fadeAnimationTime = 200f;

		private AnimationImageBox image;

		private FadeInEffect fadeInEffect;

		private FadeOutEffect fadeOutEffect;

		public override float Width
		{
			get
			{
				return base.Width;
			}
			set
			{
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

		public BusyIndicator() : this(false)
		{
		}

		public BusyIndicator(bool autoStart)
		{
			base.Width = 48f;
			base.Height = 48f;
			this.image = new AnimationImageBox();
			this.image.Image = new ImageAsset(SystemImageAsset.BusyIndicator);
			this.image.FrameWidth = 48;
			this.image.FrameHeight = 48;
			this.image.FrameCount = 8;
			this.image.FrameInterval = 66.7f;
			base.AddChildLast(this.image);
			this.fadeInEffect = new FadeInEffect(this.image, 200f, FadeInEffectInterpolator.Linear);
			this.fadeOutEffect = new FadeOutEffect(this.image, 200f, FadeOutEffectInterpolator.Linear);
			this.fadeOutEffect.EffectStopped += new EventHandler<EventArgs>(this.fadeOutEffect_EffectStopped);
			if (autoStart)
			{
				this.image.Visible = true;
				this.image.Start();
				return;
			}
			this.image.Visible = false;
			this.image.Stop();
		}

		private void fadeOutEffect_EffectStopped(object sender, EventArgs e)
		{
			this.image.Stop();
			this.image.Visible = false;
		}

		protected override void DisposeSelf()
		{
			if (this.fadeInEffect != null)
			{
				this.fadeInEffect.Stop();
			}
			if (this.fadeOutEffect != null)
			{
				this.fadeOutEffect.Stop();
			}
			if (this.image != null && this.image.Image != null)
			{
				this.image.Image.Dispose();
			}
			base.DisposeSelf();
		}

		public void Start()
		{
			this.fadeOutEffect.Stop();
			this.fadeInEffect.Start();
			this.image.Visible = true;
			this.image.Start();
		}

		public void Stop()
		{
			this.fadeInEffect.Stop();
			this.fadeOutEffect.Start();
		}
	}
}
