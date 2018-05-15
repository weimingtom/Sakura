using System;

namespace Sce.Pss.HighLevel.UI
{
	public class SlideTransition : Transition
	{
		private const float defaultTime = 300f;

		private MoveTarget moveTarget;

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

		public MoveTarget MoveTarget
		{
			get
			{
				return this.moveTarget;
			}
			set
			{
				this.moveTarget = value;
				if (this.moveTarget == MoveTarget.NextScene)
				{
					base.DrawOrder = TransitionDrawOrder.CS_TE;
					return;
				}
				base.DrawOrder = TransitionDrawOrder.NS_TE;
			}
		}

		public SlideTransitionInterpolator Interpolator
		{
			get;
			set;
		}

		public AnimationInterpolator CustomInterpolator
		{
			get;
			set;
		}

		public SlideTransition()
		{
			this.Time = 300f;
			this.MoveDirection = FourWayDirection.Left;
			this.MoveTarget = MoveTarget.NextScene;
			this.Interpolator = SlideTransitionInterpolator.EaseOutQuad;
		}

		public SlideTransition(float time, FourWayDirection direction, MoveTarget moveTarget, SlideTransitionInterpolator interpolator)
		{
			this.Time = time;
			this.MoveDirection = direction;
			this.MoveTarget = moveTarget;
			this.Interpolator = interpolator;
		}

		protected override void OnStart()
		{
			switch (this.MoveTarget)
			{
			case MoveTarget.NextScene:
			{
				ImageAsset nextSceneRenderedImage = base.GetNextSceneRenderedImage();
				this.nextSprt = new UISprite(1);
				this.nextSprt.ShaderType = ShaderType.OffscreenTexture;
				this.nextSprt.BlendMode = BlendMode.Premultiplied;
				this.nextSprt.Image = nextSceneRenderedImage;
				UISpriteUnit unit = this.nextSprt.GetUnit(0);
				unit.Width = (float)UISystem.FramebufferWidth;
				unit.Height = (float)UISystem.FramebufferHeight;
				base.TransitionUIElement.AddChildLast(this.nextSprt);
				this.to = 0f;
				switch (this.MoveDirection)
				{
				case FourWayDirection.Up:
					this.nextSprt.X = 0f;
					this.nextSprt.Y = (this.from = (float)UISystem.FramebufferHeight);
					break;
				case FourWayDirection.Down:
					this.nextSprt.X = 0f;
					this.nextSprt.Y = (this.from = (float)(-(float)UISystem.FramebufferHeight));
					break;
				case FourWayDirection.Left:
					this.nextSprt.X = (this.from = (float)UISystem.FramebufferWidth);
					this.nextSprt.Y = 0f;
					break;
				case FourWayDirection.Right:
					this.nextSprt.X = (this.from = (float)(-(float)UISystem.FramebufferWidth));
					this.nextSprt.Y = 0f;
					break;
				}
				break;
			}
			case MoveTarget.CurrentScene:
			{
				ImageAsset currentSceneRenderedImage = base.GetCurrentSceneRenderedImage();
				this.currentSprt = new UISprite(1);
				this.currentSprt.ShaderType = ShaderType.OffscreenTexture;
				this.currentSprt.BlendMode = BlendMode.Premultiplied;
				this.currentSprt.Image = currentSceneRenderedImage;
				UISpriteUnit unit = this.currentSprt.GetUnit(0);
				unit.Width = (float)UISystem.FramebufferWidth;
				unit.Height = (float)UISystem.FramebufferHeight;
				base.TransitionUIElement.AddChildLast(this.currentSprt);
				this.from = 0f;
				switch (this.MoveDirection)
				{
				case FourWayDirection.Up:
					this.to = (float)(-(float)UISystem.FramebufferHeight);
					break;
				case FourWayDirection.Down:
					this.to = (float)UISystem.FramebufferHeight;
					break;
				case FourWayDirection.Left:
					this.to = (float)(-(float)UISystem.FramebufferWidth);
					break;
				case FourWayDirection.Right:
					this.to = (float)UISystem.FramebufferWidth;
					break;
				}
				break;
			}
			}
			switch (this.Interpolator)
			{
			case SlideTransitionInterpolator.Linear:
				this.interpolatorCallback = new AnimationInterpolator(AnimationUtility.LinearInterpolator);
				return;
			case SlideTransitionInterpolator.EaseOutQuad:
				this.interpolatorCallback = new AnimationInterpolator(AnimationUtility.EaseOutQuadInterpolator);
				return;
			case SlideTransitionInterpolator.Overshoot:
				this.interpolatorCallback = new AnimationInterpolator(AnimationUtility.OvershootInterpolator);
				return;
			case SlideTransitionInterpolator.Elastic:
				this.interpolatorCallback = new AnimationInterpolator(AnimationUtility.ElasticInterpolator);
				return;
			case SlideTransitionInterpolator.Custom:
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
				switch (this.MoveTarget)
				{
				case MoveTarget.NextScene:
					switch (this.MoveDirection)
					{
					case FourWayDirection.Up:
					case FourWayDirection.Down:
						this.nextSprt.Y = this.interpolatorCallback(this.from, this.to, base.TotalElapsedTime / this.Time);
						break;
					case FourWayDirection.Left:
					case FourWayDirection.Right:
						this.nextSprt.X = this.interpolatorCallback(this.from, this.to, base.TotalElapsedTime / this.Time);
						break;
					}
					break;
				case MoveTarget.CurrentScene:
					switch (this.MoveDirection)
					{
					case FourWayDirection.Up:
					case FourWayDirection.Down:
						this.currentSprt.Y = this.interpolatorCallback(this.from, this.to, base.TotalElapsedTime / this.Time);
						break;
					case FourWayDirection.Left:
					case FourWayDirection.Right:
						this.currentSprt.X = this.interpolatorCallback(this.from, this.to, base.TotalElapsedTime / this.Time);
						break;
					}
					break;
				}
				return TransitionUpdateResponse.Continue;
			}
			switch (this.MoveTarget)
			{
			case MoveTarget.NextScene:
				switch (this.MoveDirection)
				{
				case FourWayDirection.Up:
				case FourWayDirection.Down:
					this.nextSprt.Y = this.to;
					break;
				case FourWayDirection.Left:
				case FourWayDirection.Right:
					this.nextSprt.X = this.to;
					break;
				}
				break;
			case MoveTarget.CurrentScene:
				switch (this.MoveDirection)
				{
				case FourWayDirection.Up:
				case FourWayDirection.Down:
					this.currentSprt.Y = this.to;
					break;
				case FourWayDirection.Left:
				case FourWayDirection.Right:
					this.currentSprt.X = this.to;
					break;
				}
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
