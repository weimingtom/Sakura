using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.UI
{
	public class FlipBoardTransition : Transition
	{
		private float time = 720f;

		private AnimationInterpolator interpolatorCallback;

		private UISprite currentUpperSprt;

		private UISprite currentLowerSprt;

		private UISprite nextUpperSprt;

		private UISprite nextLowerSprt;

		private float degree;

		public FlipBoardEffectInterpolator Interpolator
		{
			get;
			set;
		}

		public AnimationInterpolator CustomInterpolator
		{
			get;
			set;
		}

		public float Time
		{
			get
			{
				return this.time;
			}
			set
			{
				this.time = value;
			}
		}

		public FlipBoardTransition()
		{
			base.DrawOrder = TransitionDrawOrder.TransitionUIElement;
		}

		protected override void OnStart()
		{
			if (this.Interpolator == FlipBoardEffectInterpolator.Custom && this.CustomInterpolator != null)
			{
				this.interpolatorCallback = this.CustomInterpolator;
			}
			else
			{
				this.interpolatorCallback = new AnimationInterpolator(AnimationUtility.FlipBounceInterpolator);
			}
			ImageAsset currentSceneRenderedImage = base.GetCurrentSceneRenderedImage();
			ImageAsset nextSceneRenderedImage = base.GetNextSceneRenderedImage();
			float width = (float)UISystem.FramebufferWidth;
			float num = (float)UISystem.FramebufferHeight / 2f;
			float width2 = (float)UISystem.FramebufferWidth;
			float height = (float)UISystem.FramebufferHeight / 2f;
			this.currentUpperSprt = new UISprite(1);
			this.currentUpperSprt.X = 0f;
			this.currentUpperSprt.Y = 0f;
			this.currentUpperSprt.Image = currentSceneRenderedImage;
			this.currentUpperSprt.ShaderType = ShaderType.OffscreenTexture;
			this.currentUpperSprt.BlendMode = BlendMode.Premultiplied;
			UISpriteUnit unit = this.currentUpperSprt.GetUnit(0);
			unit.Width = width;
			unit.Height = num;
			unit.U1 = 0f;
			unit.V1 = 0f;
			unit.U2 = 1f;
			unit.V2 = 0.5f;
			this.currentLowerSprt = new UISprite(1);
			this.currentLowerSprt.X = 0f;
			this.currentLowerSprt.Y = num;
			this.currentLowerSprt.Image = currentSceneRenderedImage;
			this.currentLowerSprt.ShaderType = ShaderType.OffscreenTexture;
			this.currentLowerSprt.BlendMode = BlendMode.Premultiplied;
			UISpriteUnit unit2 = this.currentLowerSprt.GetUnit(0);
			unit2.Width = width;
			unit2.Height = num;
			unit2.U1 = 0f;
			unit2.V1 = 0.5f;
			unit2.U2 = 1f;
			unit2.V2 = 1f;
			this.nextUpperSprt = new UISprite(1);
			this.nextUpperSprt.X = 0f;
			this.nextUpperSprt.Y = 0f;
			this.nextUpperSprt.Image = nextSceneRenderedImage;
			this.nextUpperSprt.ShaderType = ShaderType.OffscreenTexture;
			this.nextUpperSprt.BlendMode = BlendMode.Premultiplied;
			UISpriteUnit unit3 = this.nextUpperSprt.GetUnit(0);
			unit3.Width = width2;
			unit3.Height = height;
			unit3.U1 = 0f;
			unit3.V1 = 0f;
			unit3.U2 = 1f;
			unit3.V2 = 0.5f;
			this.nextLowerSprt = new UISprite(1);
			this.nextLowerSprt.X = 0f;
			this.nextLowerSprt.Y = (float)UISystem.FramebufferHeight / 2f;
			this.nextLowerSprt.ShaderType = ShaderType.OffscreenTexture;
			this.nextLowerSprt.BlendMode = BlendMode.Premultiplied;
			this.nextLowerSprt.Image = nextSceneRenderedImage;
			UISpriteUnit unit4 = this.nextLowerSprt.GetUnit(0);
			unit4.Width = width2;
			unit4.Height = height;
			unit4.U1 = 0f;
			unit4.V1 = 0.5f;
			unit4.U2 = 1f;
			unit4.V2 = 1f;
			base.TransitionUIElement.AddChildLast(this.currentLowerSprt);
			base.TransitionUIElement.AddChildLast(this.nextUpperSprt);
			base.TransitionUIElement.AddChildLast(this.currentUpperSprt);
			base.TransitionUIElement.AddChildLast(this.nextLowerSprt);
			this.currentUpperSprt.Visible = true;
			this.currentLowerSprt.Visible = true;
			this.nextUpperSprt.Visible = false;
			this.nextLowerSprt.Visible = false;
			this.degree = 0f;
		}

		protected override TransitionUpdateResponse OnUpdate(float elapsedTime)
		{
			this.degree = this.interpolatorCallback(0f, 180f, base.TotalElapsedTime / this.Time);
			this.degree = MathUtility.Clamp<float>(this.degree, 0f, 180f);
			if ((double)this.degree < 90.0)
			{
				this.currentUpperSprt.Visible = true;
				this.currentLowerSprt.Visible = true;
				this.nextUpperSprt.Visible = true;
				this.nextLowerSprt.Visible = false;
				this.RotateCurrentUpperSprite();
			}
			else
			{
				this.currentUpperSprt.Visible = false;
				this.currentLowerSprt.Visible = true;
				this.nextUpperSprt.Visible = true;
				this.nextLowerSprt.Visible = true;
				this.RotateNextLowerSprite();
			}
			if (base.TotalElapsedTime >= this.time)
			{
				return TransitionUpdateResponse.Finish;
			}
			return TransitionUpdateResponse.Continue;
		}

		protected override void OnStop()
		{
			if (this.currentUpperSprt != null)
			{
				this.currentUpperSprt.Image.Dispose();
				this.currentUpperSprt.Dispose();
				this.currentUpperSprt = null;
			}
			if (this.currentLowerSprt != null)
			{
				this.currentLowerSprt.Image.Dispose();
				this.currentLowerSprt.Dispose();
				this.currentLowerSprt = null;
			}
			if (this.nextUpperSprt != null)
			{
				this.nextUpperSprt.Image.Dispose();
				this.nextUpperSprt.Dispose();
				this.nextUpperSprt = null;
			}
			if (this.nextLowerSprt != null)
			{
				this.nextLowerSprt.Image.Dispose();
				this.nextLowerSprt.Dispose();
				this.nextLowerSprt = null;
			}
		}

		private void RotateCurrentUpperSprite()
		{
			float num = this.degree / 360f * 2f * 3.14159274f;
			UISpriteUnit unit = this.currentUpperSprt.GetUnit(0);
			Matrix4 matrix = Matrix4.Translation(new Vector3(0f, unit.Height, 0f));
			Matrix4 matrix2 = Matrix4.RotationX(num);
			Matrix4 matrix3;
			matrix.Multiply(ref matrix2, out matrix3);
			Matrix4 matrix4 = Matrix4.Translation(new Vector3(0f, -unit.Height, 0f));
			Matrix4 transform3D;
			matrix3.Multiply(ref matrix4, out transform3D);
			this.currentUpperSprt.Transform3D = transform3D;
		}

		private void RotateNextLowerSprite()
		{
			float num = (this.degree - 180f) / 360f * 2f * 3.14159274f;
			UISpriteUnit unit = this.nextLowerSprt.GetUnit(0);
			Matrix4 matrix = Matrix4.Translation(new Vector3(0f, unit.Height, 0f));
			Matrix4 matrix2 = Matrix4.RotationX(num);
			Matrix4 transform3D;
			matrix.Multiply(ref matrix2, out transform3D);
			this.nextLowerSprt.Transform3D = transform3D;
		}
	}
}
