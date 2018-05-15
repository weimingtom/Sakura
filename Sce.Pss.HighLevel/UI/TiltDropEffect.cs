using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.UI
{
	public class TiltDropEffect : Effect
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

		private TiltDropEffect.RotateDirection rotateDirection;

		private float speed;

		private Random rand;

		private PivotType originalPivotType;

		private Matrix4 originalPosition;

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

		private TiltDropEffect.LeftPosition leftPosition;

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

		public TiltDropEffect()
		{
			this.init(null);
		}

		public TiltDropEffect(Widget widget)
		{
			this.init(widget);
		}

		private void init(Widget widget)
		{
			base.Widget = widget;
			this.Speed = this.speed;
			this.rotateDirection = TiltDropEffect.RotateDirection.ClockWise;
			this.speed = 1f;
			this.rand = new Random();
		}

		public static TiltDropEffect CreateAndStart(Widget widget)
		{
			TiltDropEffect tiltDropEffect = new TiltDropEffect(widget);
			tiltDropEffect.Start();
			return tiltDropEffect;
		}

		protected override void OnStart()
		{
			if (base.Widget == null)
			{
				return;
			}
			Matrix4 identity = Matrix4.Identity;
			identity.M41 = base.Widget.X;
			identity.M42 = base.Widget.Y;
			base.Widget.Transform3D = identity;
			this.originalPosition = identity;
			this.originalPivotType = base.Widget.PivotType;
			base.Widget.PivotType = PivotType.MiddleCenter;
			this.pivotOffsetX = base.Widget.Width / 2f;
			this.pivotOffsetY = base.Widget.Height / 2f;
			this.fromX = base.Widget.X;
			this.fromY = base.Widget.Y;
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
			base.Widget.Visible = true;
		}

		protected override EffectUpdateResponse OnUpdate(float elapsedTime)
		{
			if (base.Widget == null)
			{
				return EffectUpdateResponse.Finish;
			}
			this.CalcPosition(base.TotalElapsedTime / 40f * this.speed, elapsedTime / 40f * this.speed);
			base.Widget.X = this.targetOffsetX + this.fromX;
			base.Widget.Y = this.targetOffsetY + this.fromY;
			this.CalcMatrix();
			if (base.Widget.X < -this.outsideMargin || base.Widget.X > (float)UISystem.FramebufferWidth + this.outsideMargin || base.Widget.Y < -this.outsideMargin || base.Widget.Y > (float)UISystem.FramebufferHeight + this.outsideMargin)
			{
				return EffectUpdateResponse.Finish;
			}
			return EffectUpdateResponse.Continue;
		}

		protected override void OnStop()
		{
			base.Widget.Visible = false;
			base.Widget.PivotType = this.originalPivotType;
			base.Widget.Transform3D = this.originalPosition;
		}

		private void setupLeftPos()
		{
			float num = (float)Math.Atan2((double)base.Widget.Height, (double)base.Widget.Width);
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
				this.leftPosition = TiltDropEffect.LeftPosition.LeftTop;
				this.rotateDirection = TiltDropEffect.RotateDirection.ClockWise;
			}
			else if (1.5707963267948966 > (double)num2)
			{
				this.leftPosition = TiltDropEffect.LeftPosition.RightBottom;
				this.rotateDirection = TiltDropEffect.RotateDirection.CounterClockWise;
			}
			else if (1.5707963267948966 + (double)num > (double)num2)
			{
				this.leftPosition = TiltDropEffect.LeftPosition.RightTop;
				this.rotateDirection = TiltDropEffect.RotateDirection.ClockWise;
			}
			else if (4.71238898038469 > (double)num2)
			{
				this.leftPosition = TiltDropEffect.LeftPosition.LeftBottom;
				this.rotateDirection = TiltDropEffect.RotateDirection.CounterClockWise;
			}
			else if (4.71238898038469 - (double)num > (double)num2)
			{
				this.leftPosition = TiltDropEffect.LeftPosition.RightBottom;
				this.rotateDirection = TiltDropEffect.RotateDirection.ClockWise;
			}
			else if (4.71238898038469 > (double)num2)
			{
				this.leftPosition = TiltDropEffect.LeftPosition.LeftTop;
				this.rotateDirection = TiltDropEffect.RotateDirection.CounterClockWise;
			}
			else if (4.71238898038469 + (double)num > (double)num2)
			{
				this.leftPosition = TiltDropEffect.LeftPosition.LeftBottom;
				this.rotateDirection = TiltDropEffect.RotateDirection.ClockWise;
			}
			else
			{
				this.leftPosition = TiltDropEffect.LeftPosition.RightTop;
				this.rotateDirection = TiltDropEffect.RotateDirection.CounterClockWise;
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
			case TiltDropEffect.RotateDirection.ClockWise:
				switch (this.leftPosition)
				{
				case TiltDropEffect.LeftPosition.LeftTop:
					break;
				case TiltDropEffect.LeftPosition.RightTop:
					offsetY = -offsetY;
					return;
				case TiltDropEffect.LeftPosition.RightBottom:
					offsetX = -offsetX;
					offsetY = -offsetY;
					return;
				case TiltDropEffect.LeftPosition.LeftBottom:
					offsetX = -offsetX;
					return;
				default:
					return;
				}
				break;
			case TiltDropEffect.RotateDirection.CounterClockWise:
				radian = -radian;
				switch (this.leftPosition)
				{
				case TiltDropEffect.LeftPosition.LeftTop:
					offsetX = -offsetX;
					offsetY = -offsetY;
					return;
				case TiltDropEffect.LeftPosition.RightTop:
					offsetX = -offsetX;
					return;
				case TiltDropEffect.LeftPosition.RightBottom:
					break;
				case TiltDropEffect.LeftPosition.LeftBottom:
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
			Matrix4 matrix = Matrix4.Translation(new Vector3(base.Widget.X, base.Widget.Y, 0f));
			Matrix4 matrix2 = Matrix4.RotationZ(this.targetRadian);
			Matrix4 transform3D;
			matrix.Multiply(ref matrix2, out transform3D);
			base.Widget.Transform3D = transform3D;
		}
	}
}
