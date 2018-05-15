using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.UI
{
	public class TiltDropTransition : Transition
	{
		private enum RotateDirection
		{
			ClockWise,
			CounterClockWise
		}

		private enum LeftPosition
		{
			LeftTop,
			RightTop,
			RightBottom,
			LeftBottom
		}

		private const float timeStep = 40f;

		private TiltDropTransition.RotateDirection rotateDirection;

		private float speed;

		private Random rand;

		private UISprite currentSprt;

		private float leftTime;

		private float T;

		private float fromX;

		private float fromY;

		private float outsideMargin;

		private float dropDirection;

		private float pivotOffsetX;

		private float pivotOffsetY;

		private float targetRadianSpeed;

		private float targetRadian;

		private float targetOffsetXSpeed;

		private float targetOffsetX;

		private float targetOffsetYSpeed;

		private float targetOffsetY;

		private TiltDropTransition.LeftPosition leftPosition;

		private float leftOffsetX;

		private float leftOffsetY;

		public float DropDirection
		{
			get
			{
				return this.dropDirection;
			}
			set
			{
				this.dropDirection = value;
			}
		}

		public float Speed
		{
			get
			{
				return this.speed;
			}
			set
			{
				this.speed = value;
			}
		}

		public TiltDropTransition()
		{
			base.DrawOrder = TransitionDrawOrder.NS_TE;
			this.rotateDirection = TiltDropTransition.RotateDirection.CounterClockWise;
			this.speed = 1f;
			this.rand = new Random();
		}

		protected override void OnStart()
		{
			ImageAsset currentSceneRenderedImage = base.GetCurrentSceneRenderedImage();
			this.currentSprt = new UISprite(1);
			this.currentSprt.ShaderType = ShaderType.OffscreenTexture;
			this.currentSprt.BlendMode = BlendMode.Premultiplied;
			this.currentSprt.Image = currentSceneRenderedImage;
			UISpriteUnit unit = this.currentSprt.GetUnit(0);
			unit.Width = (float)UISystem.FramebufferWidth;
			unit.Height = (float)UISystem.FramebufferHeight;
			unit.X = (float)(-(float)UISystem.FramebufferWidth) / 2f;
			unit.Y = (float)(-(float)UISystem.FramebufferHeight) / 2f;
			this.currentSprt.X = (float)UISystem.FramebufferWidth / 2f;
			this.currentSprt.Y = (float)UISystem.FramebufferHeight / 2f;
			base.TransitionUIElement.AddChildLast(this.currentSprt);
			this.pivotOffsetX = (float)UISystem.FramebufferWidth / 2f;
			this.pivotOffsetY = (float)UISystem.FramebufferHeight / 2f;
			this.fromX = 0f;
			this.fromY = 0f;
			this.outsideMargin = (float)Math.Sqrt((double)(this.pivotOffsetX * this.pivotOffsetX + this.pivotOffsetY * this.pivotOffsetY));
			this.targetRadianSpeed = 0f;
			this.targetRadian = 0f;
			this.targetOffsetXSpeed = 0f;
			this.targetOffsetX = 0f;
			this.targetOffsetYSpeed = 0f;
			this.targetOffsetY = 0f;
			this.T = 40f;
			this.leftTime = this.T / (14f + 4f * (float)this.rand.NextDouble());
			this.setupLeftPos();
		}

		protected override TransitionUpdateResponse OnUpdate(float elapsedTime)
		{
			this.CalcPosition(base.TotalElapsedTime / 40f * this.speed, elapsedTime / 40f * this.speed);
			this.currentSprt.X = this.targetOffsetX + this.fromX;
			this.currentSprt.Y = this.targetOffsetY + this.fromY;
			this.CalcMatrix();
			if (this.currentSprt.X < -this.outsideMargin || this.currentSprt.X > (float)UISystem.FramebufferWidth + this.outsideMargin || this.currentSprt.Y < -this.outsideMargin || this.currentSprt.Y > (float)UISystem.FramebufferHeight + this.outsideMargin)
			{
				return TransitionUpdateResponse.Finish;
			}
			return TransitionUpdateResponse.Continue;
		}

		protected override void OnStop()
		{
			if (this.currentSprt != null)
			{
				this.currentSprt.Image.Dispose();
				this.currentSprt.Dispose();
			}
		}

		private void setupLeftPos()
		{
			float num = (float)Math.Atan2((double)UISystem.FramebufferHeight, (double)UISystem.FramebufferWidth);
			float num2;
			for (num2 = this.dropDirection + (float)(this.rand.NextDouble() - 0.5) / 100f; num2 < 0f; num2 += 6.28318548f)
			{
			}
			while (num2 > 6.28318548f)
			{
				num2 -= 6.28318548f;
			}
			if (1.5707963267948966 - (double)num > (double)num2)
			{
				this.leftPosition = TiltDropTransition.LeftPosition.LeftTop;
				this.rotateDirection = TiltDropTransition.RotateDirection.ClockWise;
			}
			else if (1.5707963267948966 > (double)num2)
			{
				this.leftPosition = TiltDropTransition.LeftPosition.RightBottom;
				this.rotateDirection = TiltDropTransition.RotateDirection.CounterClockWise;
			}
			else if (1.5707963267948966 + (double)num > (double)num2)
			{
				this.leftPosition = TiltDropTransition.LeftPosition.RightTop;
				this.rotateDirection = TiltDropTransition.RotateDirection.ClockWise;
			}
			else if (4.71238898038469 > (double)num2)
			{
				this.leftPosition = TiltDropTransition.LeftPosition.LeftBottom;
				this.rotateDirection = TiltDropTransition.RotateDirection.CounterClockWise;
			}
			else if (4.71238898038469 - (double)num > (double)num2)
			{
				this.leftPosition = TiltDropTransition.LeftPosition.RightBottom;
				this.rotateDirection = TiltDropTransition.RotateDirection.ClockWise;
			}
			else if (4.71238898038469 > (double)num2)
			{
				this.leftPosition = TiltDropTransition.LeftPosition.LeftTop;
				this.rotateDirection = TiltDropTransition.RotateDirection.CounterClockWise;
			}
			else if (4.71238898038469 + (double)num > (double)num2)
			{
				this.leftPosition = TiltDropTransition.LeftPosition.LeftBottom;
				this.rotateDirection = TiltDropTransition.RotateDirection.ClockWise;
			}
			else
			{
				this.leftPosition = TiltDropTransition.LeftPosition.RightTop;
				this.rotateDirection = TiltDropTransition.RotateDirection.CounterClockWise;
			}
			float num3;
			this.calcRotaitonOffset(this.leftTime, out num3, out this.leftOffsetX, out this.leftOffsetY);
		}

		private void calcRotaitonOffset(float time, out float radian, out float offsetX, out float offsetY)
		{
			radian = (1f + (float)Math.Sin((double)(time * 6.28318548f / this.T - 1.57079637f))) / 2f * 3.14159274f;
			offsetX = -(float)Math.Sin((double)radian) * this.pivotOffsetY;
			offsetY = (float)Math.Sin((double)radian) * this.pivotOffsetX;
			switch (this.rotateDirection)
			{
			case TiltDropTransition.RotateDirection.ClockWise:
				switch (this.leftPosition)
				{
				case TiltDropTransition.LeftPosition.LeftTop:
					break;
				case TiltDropTransition.LeftPosition.RightTop:
					offsetY = -offsetY;
					return;
				case TiltDropTransition.LeftPosition.RightBottom:
					offsetX = -offsetX;
					offsetY = -offsetY;
					return;
				case TiltDropTransition.LeftPosition.LeftBottom:
					offsetX = -offsetX;
					return;
				default:
					return;
				}
				break;
			case TiltDropTransition.RotateDirection.CounterClockWise:
				radian = -radian;
				switch (this.leftPosition)
				{
				case TiltDropTransition.LeftPosition.LeftTop:
					offsetX = -offsetX;
					offsetY = -offsetY;
					return;
				case TiltDropTransition.LeftPosition.RightTop:
					offsetX = -offsetX;
					return;
				case TiltDropTransition.LeftPosition.RightBottom:
					break;
				case TiltDropTransition.LeftPosition.LeftBottom:
					offsetY = -offsetY;
					break;
				default:
					return;
				}
				break;
			default:
				return;
			}
		}

		private void CalcPosition(float t, float elapsedTime)
		{
			if (t < this.leftTime)
			{
				float num;
				float num2;
				float num3;
				this.calcRotaitonOffset(t, out num, out num2, out num3);
				this.targetRadianSpeed = (num - this.targetRadian) / elapsedTime;
				this.targetRadian = num;
				this.targetOffsetXSpeed = (num2 - this.targetOffsetX) / elapsedTime;
				this.targetOffsetX = num2;
				this.targetOffsetYSpeed = (num3 - this.targetOffsetY) / elapsedTime;
				this.targetOffsetY = num3;
				return;
			}
			float num4 = t - this.leftTime;
			float num5 = 4.9f * num4 * num4;
			this.targetOffsetX = -num5 * (float)Math.Sin((double)this.dropDirection) + this.targetOffsetXSpeed * num4 + this.leftOffsetX;
			this.targetOffsetY = num5 * (float)Math.Cos((double)this.dropDirection) + this.targetOffsetYSpeed * num4 + this.leftOffsetY;
			this.targetRadian += this.targetRadianSpeed * elapsedTime;
		}

		private void CalcMatrix()
		{
			Matrix4 matrix = Matrix4.Translation(new Vector3(this.pivotOffsetX + this.targetOffsetX, this.pivotOffsetY + this.targetOffsetY, 0f));
			Matrix4 matrix2 = Matrix4.RotationZ(this.targetRadian);
			Matrix4 transform3D;
			matrix.Multiply(ref matrix2, out transform3D);
			this.currentSprt.Transform3D = transform3D;
		}
	}
}
