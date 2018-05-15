using System;

namespace Sce.Pss.HighLevel.UI
{
	public class PushTransition : Transition
	{
		private const float defaultTime = 300f;

		private float from;

		private float to;

		private UISprite currentSprt;

		private UISprite nextSprt;

		private AnimationInterpolator interpolatorCallback;

		public float Time
		{
			get;
			set;
		}

		public FourWayDirection MoveDirection
		{
			get;
			set;
		}

		public PushTransitionInterpolator Interpolator
		{
			get;
			set;
		}

		public AnimationInterpolator CustomInterpolator
		{
			get;
			set;
		}

		public PushTransition()
		{
			this.Time = 300f;
			this.MoveDirection = FourWayDirection.Left;
			this.Interpolator = PushTransitionInterpolator.EaseOutQuad;
			base.DrawOrder = TransitionDrawOrder.TransitionUIElement;
		}

		public PushTransition(float time, FourWayDirection direction, PushTransitionInterpolator interpolator)
		{
			this.Time = time;
			this.MoveDirection = direction;
			this.Interpolator = interpolator;
			base.DrawOrder = TransitionDrawOrder.TransitionUIElement;
		}

		protected override void OnStart()
		{
			ImageAsset currentSceneRenderedImage = base.GetCurrentSceneRenderedImage();
			this.currentSprt = new UISprite(1);
			base.TransitionUIElement.AddChildLast(this.currentSprt);
			this.currentSprt.ShaderType = ShaderType.OffscreenTexture;
			this.currentSprt.BlendMode = BlendMode.Premultiplied;
			this.currentSprt.Image = currentSceneRenderedImage;
			UISpriteUnit unit = this.currentSprt.GetUnit(0);
			unit.Width = (float)UISystem.FramebufferWidth;
			unit.Height = (float)UISystem.FramebufferHeight;
			ImageAsset nextSceneRenderedImage = base.GetNextSceneRenderedImage();
			this.nextSprt = new UISprite(1);
			base.TransitionUIElement.AddChildLast(this.nextSprt);
			this.nextSprt.ShaderType = ShaderType.OffscreenTexture;
			this.nextSprt.BlendMode = BlendMode.Premultiplied;
			this.nextSprt.Image = nextSceneRenderedImage;
			unit = this.nextSprt.GetUnit(0);
			unit.Width = (float)UISystem.FramebufferWidth;
			unit.Height = (float)UISystem.FramebufferHeight;
			this.from = 0f;
			switch (this.MoveDirection)
			{
			case FourWayDirection.Up:
				this.nextSprt.X = 0f;
				this.nextSprt.Y = (this.to = (float)(-(float)UISystem.FramebufferHeight));
				break;
			case FourWayDirection.Down:
				this.nextSprt.X = 0f;
				this.nextSprt.Y = (this.to = (float)UISystem.FramebufferHeight);
				break;
			case FourWayDirection.Left:
				this.nextSprt.X = (this.to = (float)(-(float)UISystem.FramebufferWidth));
				this.nextSprt.Y = 0f;
				break;
			case FourWayDirection.Right:
				this.nextSprt.X = (this.to = (float)UISystem.FramebufferWidth);
				this.nextSprt.Y = 0f;
				break;
			}
			switch (this.Interpolator)
			{
			case PushTransitionInterpolator.Linear:
				this.interpolatorCallback = new AnimationInterpolator(AnimationUtility.LinearInterpolator);
				return;
			case PushTransitionInterpolator.EaseOutQuad:
				this.interpolatorCallback = new AnimationInterpolator(AnimationUtility.EaseOutQuadInterpolator);
				return;
			case PushTransitionInterpolator.Overshoot:
				this.interpolatorCallback = new AnimationInterpolator(AnimationUtility.OvershootInterpolator);
				return;
			case PushTransitionInterpolator.Elastic:
				this.interpolatorCallback = new AnimationInterpolator(AnimationUtility.ElasticInterpolator);
				return;
			case PushTransitionInterpolator.Custom:
				if (this.CustomInterpolator != null)
				{
					this.interpolatorCallback = this.CustomInterpolator;
					return;
				}
				this.interpolatorCallback = new AnimationInterpolator(AnimationUtility.LinearInterpolator);
				return;
			default:
				this.interpolatorCallback = new AnimationInterpolator(AnimationUtility.LinearInterpolator);
				return;
			}
		}

		protected override TransitionUpdateResponse OnUpdate(float elapsedTime)
		{
			if (base.TotalElapsedTime < this.Time)
			{
				switch (this.MoveDirection)
				{
				case FourWayDirection.Up:
					this.currentSprt.Y = this.interpolatorCallback(this.from, this.to, base.TotalElapsedTime / this.Time);
					this.nextSprt.Y = this.interpolatorCallback(this.from + (float)UISystem.FramebufferHeight, this.to + (float)UISystem.FramebufferHeight, base.TotalElapsedTime / this.Time);
					break;
				case FourWayDirection.Down:
					this.currentSprt.Y = this.interpolatorCallback(this.from, this.to, base.TotalElapsedTime / this.Time);
					this.nextSprt.Y = this.interpolatorCallback(this.from - (float)UISystem.FramebufferHeight, this.to - (float)UISystem.FramebufferHeight, base.TotalElapsedTime / this.Time);
					break;
				case FourWayDirection.Left:
					this.currentSprt.X = this.interpolatorCallback(this.from, this.to, base.TotalElapsedTime / this.Time);
					this.nextSprt.X = this.interpolatorCallback(this.from + (float)UISystem.FramebufferWidth, this.to + (float)UISystem.FramebufferWidth, base.TotalElapsedTime / this.Time);
					break;
				case FourWayDirection.Right:
					this.currentSprt.X = this.interpolatorCallback(this.from, this.to, base.TotalElapsedTime / this.Time);
					this.nextSprt.X = this.interpolatorCallback(this.from - (float)UISystem.FramebufferWidth, this.to - (float)UISystem.FramebufferWidth, base.TotalElapsedTime / this.Time);
					break;
				}
				return TransitionUpdateResponse.Continue;
			}
			switch (this.MoveDirection)
			{
			case FourWayDirection.Up:
				this.currentSprt.Y = this.to;
				this.nextSprt.Y = this.to + (float)UISystem.FramebufferHeight;
				break;
			case FourWayDirection.Down:
				this.currentSprt.Y = this.to;
				this.nextSprt.Y = this.to - (float)UISystem.FramebufferHeight;
				break;
			case FourWayDirection.Left:
				this.currentSprt.X = this.to;
				this.nextSprt.X = this.to + (float)UISystem.FramebufferWidth;
				break;
			case FourWayDirection.Right:
				this.currentSprt.X = this.to;
				this.nextSprt.X = this.to - (float)UISystem.FramebufferWidth;
				break;
			}
			return TransitionUpdateResponse.Finish;
		}

		protected override void OnStop()
		{
			if (this.currentSprt != null)
			{
				this.currentSprt.Image.Dispose();
				this.currentSprt.Dispose();
			}
			if (this.nextSprt != null)
			{
				this.nextSprt.Image.Dispose();
				this.nextSprt.Dispose();
			}
		}
	}
}
