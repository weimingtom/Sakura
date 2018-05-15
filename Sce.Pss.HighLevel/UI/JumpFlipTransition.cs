using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.UI
{
	public class JumpFlipTransition : Transition
	{
		private const float fastTime = 500f;

		private const float slowTime = 1000f;

		private const float fromPosZ = 0f;

		private const float toPosZ = -600f;

		private UISprite currentSprt;

		private UISprite nextSprt;

		private float fromDegree;

		private float toDegree;

		private float time;

		public JumpFlipTransitionSpeed Speed
		{
			get;
			set;
		}

		public JumpFlipTransitionRotateDirection RotateDirection
		{
			get;
			set;
		}

		public JumpFlipTransition()
		{
			this.Speed = JumpFlipTransitionSpeed.Fast;
			this.RotateDirection = JumpFlipTransitionRotateDirection.ClockWise;
			base.DrawOrder = TransitionDrawOrder.TransitionUIElement;
		}

		protected override void OnStart()
		{
			ImageAsset currentSceneRenderedImage = base.GetCurrentSceneRenderedImage();
			this.currentSprt = new UISprite(1);
			this.currentSprt.Image = currentSceneRenderedImage;
			this.currentSprt.ShaderType = ShaderType.OffscreenTexture;
			this.currentSprt.BlendMode = BlendMode.Premultiplied;
			base.TransitionUIElement.AddChildLast(this.currentSprt);
			UISpriteUnit unit = this.currentSprt.GetUnit(0);
			unit.Width = (float)UISystem.FramebufferWidth;
			unit.Height = (float)UISystem.FramebufferHeight;
			ImageAsset nextSceneRenderedImage = base.GetNextSceneRenderedImage();
			this.nextSprt = new UISprite(1);
			this.nextSprt.Image = nextSceneRenderedImage;
			this.nextSprt.ShaderType = ShaderType.OffscreenTexture;
			this.nextSprt.BlendMode = BlendMode.Premultiplied;
			base.TransitionUIElement.AddChildLast(this.nextSprt);
			unit = this.nextSprt.GetUnit(0);
			unit.Width = (float)UISystem.FramebufferWidth;
			unit.Height = (float)UISystem.FramebufferHeight;
			this.currentSprt.Visible = true;
			this.nextSprt.Visible = false;
			this.time = ((this.Speed == JumpFlipTransitionSpeed.Fast) ? 500f : 1000f);
			this.fromDegree = 0f;
			this.toDegree = ((this.RotateDirection == JumpFlipTransitionRotateDirection.ClockWise) ? 180f : -180f);
		}

		protected override TransitionUpdateResponse OnUpdate(float elapsedTime)
		{
			if (base.TotalElapsedTime >= this.time)
			{
				this.currentSprt.Transform3D = this.GetTransform3D(0f, 180f);
				this.nextSprt.Transform3D = this.GetTransform3D(0f, 0f);
				this.currentSprt.Visible = false;
				this.nextSprt.Visible = true;
				return TransitionUpdateResponse.Finish;
			}
			float num = MathUtility.Lerp(0f, -1200f, base.TotalElapsedTime / this.time);
			float num2 = MathUtility.Lerp(this.fromDegree, this.toDegree, base.TotalElapsedTime / this.time);
			num = ((num <= -600f) ? (-1200f - num) : num);
			if (this.RotateDirection == JumpFlipTransitionRotateDirection.ClockWise)
			{
				if (num2 >= 90f)
				{
					this.currentSprt.Visible = false;
					this.nextSprt.Visible = true;
				}
			}
			else if (num2 <= -90f)
			{
				this.currentSprt.Visible = false;
				this.nextSprt.Visible = true;
			}
			this.currentSprt.Transform3D = this.GetTransform3D(num, num2);
			this.nextSprt.Transform3D = this.GetTransform3D(num, num2 + 180f);
			return TransitionUpdateResponse.Continue;
		}

		private Matrix4 GetTransform3D(float transZ, float degree)
		{
			float num = (float)UISystem.FramebufferWidth / 2f;
			float num2 = (float)UISystem.FramebufferHeight / 2f;
			Matrix4 matrix = Matrix4.Translation(new Vector3(num, num2, transZ));
			Matrix4 matrix2 = Matrix4.RotationY(degree / 360f * 2f * 3.14159274f);
			Matrix4 matrix3;
			matrix.Multiply(ref matrix2, out matrix3);
			Matrix4 matrix4 = Matrix4.Translation(new Vector3(-num, -num2, 0f));
			Matrix4 result;
			matrix3.Multiply(ref matrix4, out result);
			return result;
		}

		protected override void OnStop()
		{
			if (this.currentSprt != null)
			{
				this.currentSprt.Image.Dispose();
				this.currentSprt.Dispose();
				this.currentSprt = null;
			}
			if (this.nextSprt != null)
			{
				this.nextSprt.Image.Dispose();
				this.nextSprt.Dispose();
				this.nextSprt = null;
			}
		}
	}
}
