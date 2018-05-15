using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.UI
{
	public class BunjeeJumpEffect : Effect
	{
		private const float g = 9.8f;

		private const float vLimit = 0.3f;

		private const float timeStep = 50f;

		private float elasticity;

		private float x;

		private float y;

		private float x0;

		private float y0;

		private float v0;

		private float turnTime;

		private float rotateDegreeZ;

		private float rotateDegreeDelta;

		private float turn;

		private PivotType orgPivotType;

		private int bounceCount;

		public float Elasticity
		{
			get
			{
				return this.elasticity;
			}
			set
			{
				this.elasticity = MathUtility.Clamp<float>(value, 0f, 1f);
			}
		}

		private float TargetX
		{
			get;
			set;
		}

		private float TargetY
		{
			get;
			set;
		}

		public BunjeeJumpEffect()
		{
			base.Widget = null;
			this.Elasticity = 0.4f;
			this.TargetX = 0f;
			this.TargetY = 0f;
		}

		public BunjeeJumpEffect(Widget widget, float elasticity)
		{
			base.Widget = widget;
			this.Elasticity = elasticity;
		}

		public static BunjeeJumpEffect CreateAndStart(Widget widget, float elasticity)
		{
			BunjeeJumpEffect bunjeeJumpEffect = new BunjeeJumpEffect(widget, elasticity);
			bunjeeJumpEffect.Start();
			return bunjeeJumpEffect;
		}

		protected override void OnStart()
		{
			if (base.Widget == null)
			{
				return;
			}
			this.orgPivotType = base.Widget.PivotType;
			base.Widget.PivotType = PivotType.TopLeft;
			this.TargetX = base.Widget.X;
			this.TargetY = base.Widget.Y;
			this.x = 0f;
			this.y = 0f;
			this.x0 = base.Widget.X;
			this.y0 = -480f;
			this.v0 = this.CalcFirstVelocity();
			this.turnTime = 0f;
			this.rotateDegreeZ = 0f;
			this.rotateDegreeDelta = 0.35f;
			Random random = new Random();
			switch (random.Next(2))
			{
			case 0:
				this.turn = 1f;
				break;
			case 1:
				this.turn = -1f;
				break;
			default:
				this.turn = 1f;
				break;
			}
			this.bounceCount = 0;
			base.Widget.Y = this.y0;
			base.Widget.Visible = true;
		}

		protected override EffectUpdateResponse OnUpdate(float elapsedTime)
		{
			if (base.Widget == null)
			{
				return EffectUpdateResponse.Finish;
			}
			float num = base.TotalElapsedTime / 50f;
			bool flag = false;
			this.CalcPosition(num - this.turnTime);
			base.Widget.X = this.x0 + this.x;
			base.Widget.Y = this.y0 + this.y;
			this.rotateDegreeZ += this.turn * this.rotateDegreeDelta * elapsedTime / 50f;
			if (base.Widget.Y >= this.TargetY)
			{
				base.Widget.Y = this.TargetY;
				this.turnTime = num;
				this.x0 = base.Widget.X;
				this.y0 = base.Widget.Y;
				this.x = 0f;
				this.y = 0f;
				this.turn *= -1f;
				this.bounceCount++;
				if (this.bounceCount == 1)
				{
					this.v0 = 9.8f * num * this.Elasticity;
					if (this.v0 > 35f)
					{
						this.v0 = 35f;
					}
					this.rotateDegreeDelta = 1.7f * Math.Abs(this.rotateDegreeZ) / (2f * this.v0 / 9.8f);
				}
				else
				{
					this.v0 *= this.Elasticity;
					this.rotateDegreeDelta = 1.25f * Math.Abs(this.rotateDegreeZ) / (2f * this.v0 / 9.8f);
					if (this.bounceCount >= 4)
					{
						this.v0 *= this.Elasticity;
						this.rotateDegreeDelta = Math.Abs(this.rotateDegreeZ) / (2f * this.v0 / 9.8f);
						if (this.rotateDegreeZ < 0.5f)
						{
							this.rotateDegreeZ = 0f;
							flag = true;
						}
					}
				}
			}
			this.RotateWidget();
			if (!flag)
			{
				return EffectUpdateResponse.Continue;
			}
			return EffectUpdateResponse.Finish;
		}

		protected override void OnStop()
		{
			if (base.Widget == null)
			{
				return;
			}
			base.Widget.PivotType = this.orgPivotType;
		}

		private void CalcPosition(float t)
		{
			this.y = 4.9f * t * t - this.v0 * t;
			this.x = 0f;
		}

		private float CalcFirstVelocity()
		{
			if (this.TargetY == base.Widget.Y)
			{
				return 0f;
			}
			return (this.TargetX - base.Widget.X) / (float)Math.Sqrt((double)(2f * (this.TargetY - base.Widget.Y) / 9.8f));
		}

		private void RotateWidget()
		{
			Matrix4 matrix = Matrix4.Translation(new Vector3(base.Widget.X, base.Widget.Y, 0f));
			Matrix4 matrix2 = Matrix4.Translation(new Vector3(base.Widget.Width / 2f, base.Widget.Height / 2f, 0f));
			Matrix4 matrix3;
			matrix.Multiply(ref matrix2, out matrix3);
			Matrix4 matrix4 = Matrix4.RotationZ(this.rotateDegreeZ / 360f * 2f * 3.14159274f);
			Matrix4 matrix5;
			matrix3.Multiply(ref matrix4, out matrix5);
			Matrix4 matrix6 = Matrix4.Translation(new Vector3(-base.Widget.Width / 2f, -base.Widget.Height / 2f, 0f));
			Matrix4 transform3D;
			matrix5.Multiply(ref matrix6, out transform3D);
			base.Widget.Transform3D = transform3D;
		}
	}
}
