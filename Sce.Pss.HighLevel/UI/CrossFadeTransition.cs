using System;

namespace Sce.Pss.HighLevel.UI
{
	public class CrossFadeTransition : Transition
	{
		private const float defaultTime = 300f;

		private UISprite currentSprt;

		private UISprite nextSprt;

		private AnimationInterpolator customNextSceneInterpolator;

		private AnimationInterpolator customCurrentSceneInterpolator;

		private bool nextSceneForward;

		private bool texturize;

		private AnimationInterpolator nextInterpolatorCallback;

		private AnimationInterpolator currentInterpolatorCallback;

		public float Time
		{
			get;
			set;
		}

		public CrossFadeTransitionInterpolator Interpolator
		{
			get;
			set;
		}

		public AnimationInterpolator CustomNextSceneInterpolator
		{
			get
			{
				return this.customNextSceneInterpolator;
			}
			set
			{
				this.customNextSceneInterpolator = value;
			}
		}

		public AnimationInterpolator CustomCurrentSceneInterpolator
		{
			get
			{
				return this.customCurrentSceneInterpolator;
			}
			set
			{
				this.customCurrentSceneInterpolator = value;
			}
		}

		public bool NextSceneFront
		{
			get
			{
				return this.nextSceneForward;
			}
			set
			{
				this.nextSceneForward = value;
			}
		}

		internal bool Texturize
		{
			get
			{
				return this.texturize;
			}
			set
			{
				this.texturize = value;
			}
		}

		public CrossFadeTransition()
		{
			this.Time = 300f;
			this.Interpolator = CrossFadeTransitionInterpolator.EaseOutQuad;
			base.DrawOrder = TransitionDrawOrder.CS_NS;
		}

		public CrossFadeTransition(float time, CrossFadeTransitionInterpolator interpolator)
		{
			this.Time = time;
			this.Interpolator = interpolator;
		}

		protected override void OnStart()
		{
			if (this.texturize)
			{
				base.DrawOrder = TransitionDrawOrder.TransitionUIElement;
			}
			else if (this.nextSceneForward)
			{
				base.DrawOrder = TransitionDrawOrder.CS_NS;
			}
			else
			{
				base.DrawOrder = TransitionDrawOrder.NS_CS;
			}
			switch (this.Interpolator)
			{
			case CrossFadeTransitionInterpolator.EaseOutQuad:
				if (this.nextSceneForward)
				{
					this.nextInterpolatorCallback = new AnimationInterpolator(AnimationUtility.EaseOutQuadInterpolator);
					this.currentInterpolatorCallback = new AnimationInterpolator(this.constInterpolator);
					goto IL_145;
				}
				this.nextInterpolatorCallback = new AnimationInterpolator(this.constInterpolator);
				this.currentInterpolatorCallback = new AnimationInterpolator(AnimationUtility.EaseOutQuadInterpolator);
				goto IL_145;
			case CrossFadeTransitionInterpolator.Custom:
				if (this.customNextSceneInterpolator != null)
				{
					this.nextInterpolatorCallback = this.customNextSceneInterpolator;
				}
				else
				{
					this.nextInterpolatorCallback = new AnimationInterpolator(this.constInterpolator);
				}
				if (this.customCurrentSceneInterpolator != null)
				{
					this.currentInterpolatorCallback = this.customCurrentSceneInterpolator;
					goto IL_145;
				}
				this.currentInterpolatorCallback = new AnimationInterpolator(this.constInterpolator);
				goto IL_145;
			}
			if (this.nextSceneForward)
			{
				this.nextInterpolatorCallback = new AnimationInterpolator(AnimationUtility.LinearInterpolator);
				this.currentInterpolatorCallback = new AnimationInterpolator(this.constInterpolator);
			}
			else
			{
				this.nextInterpolatorCallback = new AnimationInterpolator(this.constInterpolator);
				this.currentInterpolatorCallback = new AnimationInterpolator(AnimationUtility.LinearInterpolator);
			}
			IL_145:
			if (this.texturize)
			{
				ImageAsset currentSceneRenderedImage = base.GetCurrentSceneRenderedImage();
				if (this.currentSprt == null)
				{
					this.currentSprt = new UISprite(1);
					this.currentSprt.ShaderType = ShaderType.OffscreenTexture;
					this.currentSprt.BlendMode = BlendMode.Premultiplied;
				}
				this.currentSprt.Image = currentSceneRenderedImage;
				UISpriteUnit unit = this.currentSprt.GetUnit(0);
				unit.Width = (float)UISystem.FramebufferWidth;
				unit.Height = (float)UISystem.FramebufferHeight;
				ImageAsset nextSceneRenderedImage = base.GetNextSceneRenderedImage();
				if (this.nextSprt == null)
				{
					this.nextSprt = new UISprite(1);
					this.nextSprt.ShaderType = ShaderType.OffscreenTexture;
					this.nextSprt.BlendMode = BlendMode.Premultiplied;
				}
				this.nextSprt.Image = nextSceneRenderedImage;
				unit = this.nextSprt.GetUnit(0);
				unit.Width = (float)UISystem.FramebufferWidth;
				unit.Height = (float)UISystem.FramebufferHeight;
				if (this.nextSceneForward)
				{
					base.TransitionUIElement.AddChildLast(this.currentSprt);
					base.TransitionUIElement.AddChildLast(this.nextSprt);
				}
				else
				{
					base.TransitionUIElement.AddChildLast(this.nextSprt);
					base.TransitionUIElement.AddChildLast(this.currentSprt);
				}
				this.nextSprt.Alpha = 0f;
			}
			base.NextScene.RootWidget.Alpha = 0f;
		}

		protected override TransitionUpdateResponse OnUpdate(float elapsedTime)
		{
			if (base.TotalElapsedTime < this.Time)
			{
				float num = this.nextInterpolatorCallback(0f, 1f, base.TotalElapsedTime / this.Time);
				num = MathUtility.Clamp<float>(num, 0f, 1f);
				float num2 = this.currentInterpolatorCallback(1f, 0f, base.TotalElapsedTime / this.Time);
				num2 = MathUtility.Clamp<float>(num2, 0f, 1f);
				if (this.nextSprt != null && this.currentSprt != null)
				{
					this.nextSprt.Alpha = num;
					this.currentSprt.Alpha = num2;
				}
				else
				{
					base.NextScene.RootWidget.Alpha = num;
					UISystem.CurrentScene.RootWidget.Alpha = num2;
				}
				return TransitionUpdateResponse.Continue;
			}
			return TransitionUpdateResponse.Finish;
		}

		protected override void OnStop()
		{
			if (this.nextSprt != null)
			{
				base.TransitionUIElement.RemoveChild(this.nextSprt);
				this.nextSprt.Image.Dispose();
				this.nextSprt.Dispose();
				this.nextSprt = null;
			}
			if (this.currentSprt != null)
			{
				base.TransitionUIElement.RemoveChild(this.currentSprt);
				this.currentSprt.Image.Dispose();
				this.currentSprt.Dispose();
				this.currentSprt = null;
			}
			base.NextScene.RootWidget.Alpha = 1f;
			UISystem.CurrentScene.RootWidget.Alpha = 1f;
		}

		private float constInterpolator(float from, float to, float ratio)
		{
			return 1f;
		}
	}
}
