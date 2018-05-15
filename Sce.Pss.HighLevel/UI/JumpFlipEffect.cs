using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.UI
{
	public class JumpFlipEffect : Effect
	{
		private const float fromPosZ = 0f;

		private const float fromDegree = 0f;

		private float time = 500f;

		private int revolution = 1;

		private JumpFlipEffectAxis rotationAxis = JumpFlipEffectAxis.Y;

		private float toPosZ = -350f;

		private float jumpDelay = 0.15f;

		private float toDegreeDelay;

		private float nonJumpTime;

		private PivotType orgCurrentPivotType;

		private PivotType orgNextPivotType;

		private float fromPosX;

		private float fromPosY;

		private float toPosX;

		private float toPosY;

		private float toDegree;

		public Widget NextWidget
		{
			get;
			set;
		}

		public int Revolution
		{
			get
			{
				return this.revolution;
			}
			set
			{
				this.revolution = value;
			}
		}

		public float JumpHeight
		{
			get
			{
				return this.toPosZ;
			}
			set
			{
				this.toPosZ = value;
			}
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

		public float JumpDelay
		{
			get
			{
				return this.jumpDelay;
			}
			set
			{
				this.jumpDelay = value;
			}
		}

		public float ToDegreeDelay
		{
			get
			{
				return this.toDegreeDelay;
			}
			set
			{
				this.toDegreeDelay = value;
			}
		}

		public JumpFlipEffectAxis RotationAxis
		{
			get
			{
				return this.rotationAxis;
			}
			set
			{
				this.rotationAxis = value;
			}
		}

		public JumpFlipEffect()
		{
			base.Widget = null;
			this.NextWidget = null;
		}

		public JumpFlipEffect(Widget currentWidget, Widget nextWidget)
		{
			base.Widget = currentWidget;
			this.NextWidget = nextWidget;
		}

		public static JumpFlipEffect CreateAndStart(Widget currentWidget, Widget nextWidget)
		{
			JumpFlipEffect jumpFlipEffect = new JumpFlipEffect(currentWidget, nextWidget);
			jumpFlipEffect.Start();
			return jumpFlipEffect;
		}

		protected override void OnStart()
		{
			if (base.Widget.Width != this.NextWidget.Width || base.Widget.Height != this.NextWidget.Height)
			{
				Console.WriteLine("Not supported different widget size.");
				return;
			}
			this.nonJumpTime = this.time * this.jumpDelay;
			this.orgCurrentPivotType = base.Widget.PivotType;
			this.orgNextPivotType = this.NextWidget.PivotType;
			base.Widget.PivotType = PivotType.MiddleCenter;
			this.NextWidget.PivotType = PivotType.MiddleCenter;
			base.Widget.ZSort = true;
			this.NextWidget.ZSort = true;
			this.fromPosX = base.Widget.X;
			this.fromPosY = base.Widget.Y;
			this.toPosX = this.NextWidget.X;
			this.toPosY = this.NextWidget.Y;
			this.toDegree = 180f;
			base.Widget.Visible = true;
			this.NextWidget.Visible = false;
		}

		protected override EffectUpdateResponse OnUpdate(float elapsedTime)
		{
			if (base.Widget.Width != this.NextWidget.Width || base.Widget.Height != this.NextWidget.Height)
			{
				return EffectUpdateResponse.Finish;
			}
			if (base.TotalElapsedTime >= this.time)
			{
				float transX = this.toPosX;
				float transY = this.toPosY;
				float transZ = 0f;
				float num = this.toDegree;
				base.Widget.Visible = false;
				this.NextWidget.Visible = true;
				base.Widget.Transform3D = this.GetTransform3D(transX, transY, transZ, num);
				this.NextWidget.Transform3D = this.GetTransform3D(transX, transY, transZ, num + 180f);
				this.UpdateWidgetVisible();
				return EffectUpdateResponse.Finish;
			}
			float num2 = base.TotalElapsedTime / this.time;
			float transX2 = (this.toPosX - this.fromPosX) * num2 * num2 + this.fromPosX;
			float transY2 = (this.toPosY - this.fromPosY) * num2 * num2 + this.fromPosY;
			float num3 = base.TotalElapsedTime / (this.time / 2f) - 1f;
			float num4 = -(this.toPosZ - 0f) * num3 * num3 - -(this.toPosZ - 0f);
			float num5;
			if (base.TotalElapsedTime < this.nonJumpTime)
			{
				num5 = MathUtility.Lerp(0f, this.toDegreeDelay, base.TotalElapsedTime / this.nonJumpTime);
			}
			else if (base.TotalElapsedTime >= this.nonJumpTime && base.TotalElapsedTime <= this.time - this.nonJumpTime)
			{
				num5 = MathUtility.Lerp(this.toDegreeDelay, this.toDegree - this.toDegreeDelay, (base.TotalElapsedTime - this.nonJumpTime) / (this.time - this.nonJumpTime * 2f));
			}
			else
			{
				num5 = MathUtility.Lerp(this.toDegree - this.toDegreeDelay, this.toDegree, (base.TotalElapsedTime - (this.time - this.nonJumpTime)) / this.nonJumpTime);
			}
			num4 = ((num4 <= this.toPosZ) ? (this.toPosZ * 2f - num4) : num4);
			base.Widget.Transform3D = this.GetTransform3D(transX2, transY2, num4, num5);
			this.NextWidget.Transform3D = this.GetTransform3D(transX2, transY2, num4, num5 + 180f);
			this.UpdateWidgetVisible();
			return EffectUpdateResponse.Continue;
		}

		private Matrix4 GetTransform3D(float transX, float transY, float transZ, float degree)
		{
			Matrix4 matrix = Matrix4.Translation(new Vector3(transX, transY, transZ));
			Matrix4 matrix2;
			if (this.rotationAxis == JumpFlipEffectAxis.X)
			{
				matrix2 = Matrix4.RotationX(degree / 360f * 2f * 3.14159274f * (float)this.revolution);
			}
			else
			{
				matrix2 = Matrix4.RotationY(degree / 360f * 2f * 3.14159274f * (float)this.revolution);
			}
			Matrix4 result;
			matrix.Multiply(ref matrix2, out result);
			return result;
		}

		private void UpdateWidgetVisible()
		{
			Matrix4 matrix = base.Widget.Transform3D;
			Widget parent = base.Widget.Parent;
			while (parent != null)
			{
				Matrix4 transform3D = parent.Transform3D;
				PivotType pivotType = parent.PivotType;
				switch (pivotType)
				{
				case PivotType.TopCenter:
					goto IL_63;
				case PivotType.TopRight:
					goto IL_7F;
				default:
					switch (pivotType)
					{
					case PivotType.MiddleCenter:
						goto IL_63;
					case PivotType.MiddleRight:
						goto IL_7F;
					default:
						switch (pivotType)
						{
						case PivotType.BottomCenter:
							goto IL_63;
						case PivotType.BottomRight:
							goto IL_7F;
						}
						break;
					}
					break;
				}
				IL_93:
				PivotType pivotType2 = parent.PivotType;
				switch (pivotType2)
				{
				case PivotType.MiddleLeft:
				case PivotType.MiddleCenter:
				case PivotType.MiddleRight:
					transform3D.M42 -= parent.Height / 2f;
					break;
				default:
					switch (pivotType2)
					{
					case PivotType.BottomLeft:
					case PivotType.BottomCenter:
					case PivotType.BottomRight:
						transform3D.M42 -= parent.Height;
						break;
					}
					break;
				}
				matrix = transform3D * matrix;
				parent = parent.Parent;
				continue;
				IL_63:
				transform3D.M41 -= parent.Width / 2f;
				goto IL_93;
				IL_7F:
				transform3D.M41 -= parent.Width;
				goto IL_93;
			}
			Vector3 vector = new Vector3((float)UISystem.FramebufferWidth / 2f, (float)UISystem.FramebufferHeight / 2f, -1000f);
			Vector3 xyz = matrix.ColumnW.Xyz;
			float num = matrix.ColumnZ.Xyz.Dot(vector - xyz);
			if (num < 0f)
			{
				base.Widget.Visible = true;
				this.NextWidget.Visible = false;
				return;
			}
			base.Widget.Visible = false;
			this.NextWidget.Visible = true;
		}

		protected override void OnStop()
		{
			if (base.Widget.Width != this.NextWidget.Width || base.Widget.Height != this.NextWidget.Height)
			{
				return;
			}
			base.Widget.ZSort = false;
			base.Widget.PivotType = this.orgCurrentPivotType;
			this.NextWidget.ZSort = false;
			this.NextWidget.PivotType = this.orgNextPivotType;
		}
	}
}
