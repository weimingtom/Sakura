using Sce.Pss.Core;
using System;
using Sce.Pss.Core.Environment;

namespace Sce.Pss.HighLevel.UI
{
	internal class DefaultNavigationTransition : Transition
	{
		internal enum TransitionType
		{
			Push,
			Pop
		}

		private static readonly float animationTime = 500f;

		private static readonly float minAlpha = 0f;

		private static readonly float maxAlpha = 1f;

		private DefaultNavigationTransition.TransitionType transitionType;

		private UISprite sprtCurrent;

		private float fromPosXCurrent;

		private float toPosXCurrent;

		private float fromPosZCurrent;

		private float toPosZCurrent;

		private float fromDegreeCurrent;

		private float toDegreeCurrent;

		private UISprite sprtNext;

		private float fromPosXNext;

		private float toPosXNext;

		private float fromPosZNext;

		private float toPosZNext;

		private float fromDegreeNext;

		private float toDegreeNext;

		public DefaultNavigationTransition(DefaultNavigationTransition.TransitionType type)
		{
			this.transitionType = type;
			base.DrawOrder = TransitionDrawOrder.TransitionUIElement;
			float num = (float)(-(float)UISystem.FramebufferWidth) * 0.121f;
			float num2 = (float)UISystem.FramebufferWidth * 0.156f;
			float num3 = -1.57079637f;
			float num4 = (float)UISystem.FramebufferWidth;
			float num5 = (float)(-(float)UISystem.FramebufferWidth) * 0.344f;
			float num6 = 1.57079637f;
			if (this.transitionType == DefaultNavigationTransition.TransitionType.Push)
			{
				this.fromPosXCurrent = 0f;
				this.toPosXCurrent = num;
				this.fromPosZCurrent = 0f;
				this.toPosZCurrent = num2;
				this.fromDegreeCurrent = 0f;
				this.toDegreeCurrent = num3;
				this.fromPosXNext = num4;
				this.toPosXNext = 0f;
				this.fromPosZNext = num5;
				this.toPosZNext = 0f;
				this.fromDegreeNext = num6;
				this.toDegreeNext = 0f;
				return;
			}
			this.fromPosXCurrent = 0f;
			this.toPosXCurrent = num4;
			this.fromPosZCurrent = 0f;
			this.toPosZCurrent = num5;
			this.fromDegreeCurrent = 0f;
			this.toDegreeCurrent = num6;
			this.fromPosXNext = num;
			this.toPosXNext = 0f;
			this.fromPosZNext = num2;
			this.toPosZNext = 0f;
			this.fromDegreeNext = num3;
			this.toDegreeNext = 0f;
		}

		protected override void OnStart()
		{
			ImageAsset currentSceneRenderedImage = base.GetCurrentSceneRenderedImage();
			this.sprtCurrent = new UISprite(1);
			this.sprtCurrent.Image = currentSceneRenderedImage;
			this.sprtCurrent.ShaderType = ShaderType.OffscreenTexture;
			this.sprtCurrent.BlendMode = BlendMode.Premultiplied;
			this.sprtCurrent.Alpha = DefaultNavigationTransition.maxAlpha;
			this.sprtCurrent.Visible = true;
			base.TransitionUIElement.AddChildLast(this.sprtCurrent);
			UISpriteUnit unit = this.sprtCurrent.GetUnit(0);
			unit.Width = (float)UISystem.FramebufferWidth;
			unit.Height = (float)UISystem.FramebufferHeight;
			ImageAsset nextSceneRenderedImage = base.GetNextSceneRenderedImage();
			this.sprtNext = new UISprite(1);
			this.sprtNext.Image = nextSceneRenderedImage;
			this.sprtNext.ShaderType = ShaderType.OffscreenTexture;
			this.sprtNext.BlendMode = BlendMode.Premultiplied;
			this.sprtNext.Alpha = DefaultNavigationTransition.minAlpha;
			this.sprtNext.Visible = true;
			base.TransitionUIElement.AddChildLast(this.sprtNext);
			unit = this.sprtNext.GetUnit(0);
			unit.Width = (float)UISystem.FramebufferWidth;
			unit.Height = (float)UISystem.FramebufferHeight;
		}

		protected override TransitionUpdateResponse OnUpdate(float elapsedTime)
		{
			if (base.TotalElapsedTime >= DefaultNavigationTransition.animationTime)
			{
				this.sprtCurrent.Transform3D = this.GetTransform3D(this.toPosXCurrent, this.toPosZCurrent, this.toDegreeCurrent);
				this.sprtNext.Transform3D = this.GetTransform3D(this.toPosXNext, this.toPosZNext, this.toDegreeNext);
				this.sprtCurrent.Visible = false;
				this.sprtNext.Visible = true;
				return TransitionUpdateResponse.Finish;
			}
			float transX = AnimationUtility.EaseOutQuartInterpolator(this.fromPosXCurrent, this.toPosXCurrent, base.TotalElapsedTime / DefaultNavigationTransition.animationTime);
			float transZ = AnimationUtility.EaseOutQuartInterpolator(this.fromPosZCurrent, this.toPosZCurrent, base.TotalElapsedTime / DefaultNavigationTransition.animationTime);
			float degree = AnimationUtility.EaseOutQuartInterpolator(this.fromDegreeCurrent, this.toDegreeCurrent, base.TotalElapsedTime / DefaultNavigationTransition.animationTime);
			float alpha = AnimationUtility.LinearInterpolator(DefaultNavigationTransition.maxAlpha, DefaultNavigationTransition.minAlpha, base.TotalElapsedTime / DefaultNavigationTransition.animationTime);
			this.sprtCurrent.Transform3D = this.GetTransform3D(transX, transZ, degree);
			this.sprtCurrent.Alpha = alpha;
			float transX2 = AnimationUtility.EaseOutQuartInterpolator(this.fromPosXNext, this.toPosXNext, base.TotalElapsedTime / DefaultNavigationTransition.animationTime);
			float transZ2 = AnimationUtility.EaseOutQuartInterpolator(this.fromPosZNext, this.toPosZNext, base.TotalElapsedTime / DefaultNavigationTransition.animationTime);
			float degree2 = AnimationUtility.EaseOutQuartInterpolator(this.fromDegreeNext, this.toDegreeNext, base.TotalElapsedTime / DefaultNavigationTransition.animationTime);
			float alpha2 = AnimationUtility.LinearInterpolator(DefaultNavigationTransition.minAlpha, DefaultNavigationTransition.maxAlpha, base.TotalElapsedTime / DefaultNavigationTransition.animationTime);
			this.sprtNext.Transform3D = this.GetTransform3D(transX2, transZ2, degree2);
			this.sprtNext.Alpha = alpha2;
			return TransitionUpdateResponse.Continue;
		}

		private Matrix4 GetTransform3D(float transX, float transZ, float degree)
		{
			Matrix4 matrix = Matrix4.Translation(new Vector3(transX, 0f, transZ));
			Matrix4 matrix2 = Matrix4.RotationY(degree);
			Matrix4 result;
			matrix.Multiply(ref matrix2, out result);
			return result;
		}

		protected override void OnStop()
		{
			if (this.sprtCurrent != null)
			{
				this.sprtCurrent.Image.Dispose();
				this.sprtCurrent.Dispose();
			}
			if (this.sprtNext != null)
			{
				this.sprtNext.Image.Dispose();
				this.sprtNext.Dispose();
			}
		}
	}
}
