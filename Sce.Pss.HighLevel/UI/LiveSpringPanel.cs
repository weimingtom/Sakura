using Sce.Pss.Core;
using Sce.Pss.Core.Input;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Sce.Pss.HighLevel.UI
{
	public class LiveSpringPanel : Panel
	{
		private class WidgetInfo
		{
			public Vector3 originalPos;

			public PivotType originalPivot;

			public float weight;

			public Matrix4 prevTransform3D;

			public float[] springConstants;

			public float[] dampingConstants;

			public float[] springLimitations;

			public float[] velocities;

			public float[] displacements;

			public bool[] useSpecifiedValues;

			public float[] prevDisplacements;
		}

		private const float distanceToAngleRate = 0.0125663709f;

		private bool reflectMotionAcceleration;

		private float defaultDampingConstant;

		private float defaultSpringConstant;

		private Dictionary<Widget, LiveSpringPanel.WidgetInfo> widgetInfos;

		private Vector3 prevPanelPos;

		private Vector3 prevPanelVelocity;

		private Vector3 userAcceleration;

		private Vector3 sensorAcceleration;

		private Vector3 moveAcceleration;

		private bool initPrevPanelPos;

		private bool initPrevPanelVelocity;

		public bool ReflectSensorAcceleration
		{
			get;
			set;
		}

		public bool ReflectMotionAcceleration
		{
			get
			{
				return this.reflectMotionAcceleration;
			}
			set
			{
				this.initPrevPanelPos = false;
				this.initPrevPanelVelocity = false;
				this.reflectMotionAcceleration = value;
			}
		}

		public LiveSpringPanel()
		{
			this.widgetInfos = new Dictionary<Widget, LiveSpringPanel.WidgetInfo>();
			this.defaultDampingConstant = 0.2f;
			this.defaultSpringConstant = 0.3f;
			this.ReflectSensorAcceleration = true;
			this.ReflectMotionAcceleration = true;
			this.initPrevPanelPos = false;
			this.initPrevPanelVelocity = false;
		}

		protected override void OnUpdate(float elapsedTime)
		{
			base.OnUpdate(elapsedTime);
			if (elapsedTime > 100f)
			{
				return;
			}
			this.CalculateMoveAcceleration(elapsedTime);
			foreach (Widget current in this.Children)
			{
				this.UpdateWidgetsPosition(current, elapsedTime);
			}
		}

		protected internal override void OnMotionEvent(MotionEvent motionEvent)
		{
			base.OnMotionEvent(motionEvent);
			if (this.ReflectSensorAcceleration)
			{
				this.sensorAcceleration = Motion.GetData(0).Acceleration;
				this.sensorAcceleration.Y = -this.sensorAcceleration.Y;
				this.sensorAcceleration.Z = -this.sensorAcceleration.Z;
			}
		}

		private void CalculateMoveAcceleration(float elapsedTime)
		{
			if (this.ReflectMotionAcceleration && elapsedTime > 1.401298E-45f)
			{
				Vector3 axisW = base.LocalToWorld.AxisW;
				if (this.initPrevPanelPos)
				{
					Vector3 vector = Vector3.Multiply(Vector3.Subtract(axisW, this.prevPanelPos), 1f / elapsedTime);
					if (this.initPrevPanelVelocity)
					{
						this.moveAcceleration = Vector3.Subtract(vector, this.prevPanelVelocity);
						this.moveAcceleration.X = this.ClipAcceleration(this.moveAcceleration.X);
						this.moveAcceleration.Y = this.ClipAcceleration(this.moveAcceleration.Y);
						this.moveAcceleration.Z = this.ClipAcceleration(this.moveAcceleration.Z);
					}
					this.prevPanelVelocity = vector;
					this.initPrevPanelVelocity = true;
				}
				this.prevPanelPos = axisW;
				this.initPrevPanelPos = true;
			}
		}

		private void UpdateWidgetsPosition(Widget widget, float elapsedTime)
		{
			LiveSpringPanel.WidgetInfo widgetInfo = this.widgetInfos[widget];
			widgetInfo.originalPivot = widget.PivotType;
			widget.PivotType = PivotType.MiddleCenter;
			if (widgetInfo.prevTransform3D != widget.Transform3D)
			{
				widgetInfo.originalPos = (Matrix4.Translation(-widgetInfo.prevDisplacements[3], -widgetInfo.prevDisplacements[4], -widgetInfo.prevDisplacements[5]) * widget.Transform3D * Matrix4.RotationXyz(-widgetInfo.prevDisplacements[0], -widgetInfo.prevDisplacements[1], -widgetInfo.prevDisplacements[2])).ColumnW.Xyz;
			}
			foreach (SpringType springType in Enum.GetValues(typeof(SpringType)))
			{
				if (widgetInfo.useSpecifiedValues[(int)springType])
				{
					widgetInfo.useSpecifiedValues[(int)springType] = false;
				}
				else
				{
					float num = this.CalculateExternalAcceleration(widgetInfo, springType);
					float num2 = (-widgetInfo.springConstants[(int)springType] * widgetInfo.displacements[(int)springType] + widgetInfo.dampingConstants[(int)springType] * widgetInfo.velocities[(int)springType]) / widgetInfo.weight + num;
					widgetInfo.velocities[(int)springType] += num2 * elapsedTime;
					widgetInfo.displacements[(int)springType] += widgetInfo.velocities[(int)springType];
					widgetInfo.displacements[(int)springType] *= widgetInfo.springLimitations[(int)springType];
				}
				widgetInfo.prevDisplacements[(int)springType] = widgetInfo.displacements[(int)springType];
			}
			float num3 = widgetInfo.displacements[3];
			float num4 = widgetInfo.displacements[4];
			float num5 = widgetInfo.displacements[5];
			float num6 = widgetInfo.displacements[0];
			float num7 = widgetInfo.displacements[1];
			float num8 = widgetInfo.displacements[2];
			widget.Transform3D = Matrix4.Translation(num3, num4, num5) * Matrix4.Translation(widgetInfo.originalPos) * Matrix4.RotationXyz(num6, num7, num8);
			widgetInfo.prevTransform3D = widget.Transform3D;
			widget.PivotType = widgetInfo.originalPivot;
		}

		private float CalculateExternalAcceleration(LiveSpringPanel.WidgetInfo info, SpringType type)
		{
			float acceleration = 0f;
			switch (type)
			{
			case SpringType.AngleAxisX:
			case SpringType.PositionY:
				acceleration = this.sensorAcceleration.Y + this.moveAcceleration.Y + this.userAcceleration.Y;
				break;
			case SpringType.AngleAxisY:
				acceleration = -(this.sensorAcceleration.X + this.moveAcceleration.X + this.userAcceleration.X);
				break;
			case SpringType.AngleAxisZ:
			{
				bool flag = false;
				bool[] useSpecifiedValues = info.useSpecifiedValues;
				for (int i = 0; i < useSpecifiedValues.Length; i++)
				{
					bool flag2 = useSpecifiedValues[i];
					flag |= flag2;
				}
				if (!flag)
				{
					float num = info.displacements[3];
					float num2 = info.displacements[4];
					float num3 = FMath.Abs(num);
					float num4 = FMath.Abs(num2);
					bool flag3;
					if (num * num2 > 0f)
					{
						flag3 = (num3 > num4);
					}
					else
					{
						flag3 = (num3 < num4);
					}
					float num5 = FMath.Min(FMath.Abs(num3 - num4), FMath.Min(num3, num4)) / 200f;
					float num6 = flag3 ? num5 : (-num5);
					float num7 = info.displacements[0];
					float num8 = info.displacements[1];
					float num9 = FMath.Abs(num7);
					float num10 = FMath.Abs(num8);
					if (-num7 * num8 > 0f)
					{
						flag3 = (num9 < num10);
					}
					else
					{
						flag3 = (num9 > num10);
					}
					num5 = FMath.Min(FMath.Abs(num9 - num10), FMath.Min(num9, num10)) / 20f;
					float num11 = flag3 ? num5 : (-num5);
					acceleration = num6 + num11;
				}
				break;
			}
			case SpringType.PositionX:
				acceleration = this.sensorAcceleration.X + this.moveAcceleration.X + this.userAcceleration.X;
				break;
			case SpringType.PositionZ:
				acceleration = this.sensorAcceleration.Z + this.moveAcceleration.Z + this.userAcceleration.Z;
				break;
			}
			if (type == SpringType.PositionX || type == SpringType.PositionY || type == SpringType.PositionZ)
			{
				return this.ClipAcceleration(acceleration);
			}
			return this.ClipAcceleration(acceleration) * 0.0125663709f;
		}

		public override void AddChildFirst(Widget child)
		{
			base.AddChildFirst(child);
			this.AddWidgetInfo(child);
		}

		public override void AddChildLast(Widget child)
		{
			base.AddChildLast(child);
			this.AddWidgetInfo(child);
		}

		public override void InsertChildBefore(Widget child, Widget nextChild)
		{
			base.InsertChildBefore(child, nextChild);
			this.AddWidgetInfo(child);
		}

		public override void InsertChildAfter(Widget child, Widget prevChild)
		{
			base.InsertChildAfter(child, prevChild);
			this.AddWidgetInfo(child);
		}

		public override void RemoveChild(Widget child)
		{
			base.RemoveChild(child);
			this.removeWidgetInfo(child);
		}

		private void AddWidgetInfo(Widget widget)
		{
			int num = Enum.GetNames(typeof(SpringType)).Length;
			LiveSpringPanel.WidgetInfo widgetInfo = new LiveSpringPanel.WidgetInfo
			{
				originalPivot = widget.PivotType,
				weight = 200f,
				springConstants = new float[num],
				dampingConstants = new float[num],
				springLimitations = new float[num],
				velocities = new float[num],
				displacements = new float[num],
				useSpecifiedValues = new bool[num],
				prevDisplacements = new float[num]
			};
			this.widgetInfos.Add(widget, widgetInfo);
			foreach (SpringType type in Enum.GetValues(typeof(SpringType)))
			{
				this.SetSpringConstant(widget, type, this.defaultSpringConstant);
				this.SetDampingConstant(widget, type, this.defaultDampingConstant);
			}
			widget.PivotType = PivotType.MiddleCenter;
			widgetInfo.originalPos = widget.Transform3D.ColumnW.Xyz;
			widgetInfo.prevTransform3D = widget.Transform3D;
			widget.PivotType = widgetInfo.originalPivot;
		}

		private void removeWidgetInfo(Widget widget)
		{
			this.widgetInfos.Remove(widget);
		}

		public void SetDampingConstant(Widget widget, SpringType type, float dampingConstant)
		{
			this.SetValue(widget, type, dampingConstant, delegate(Widget w, SpringType t, float value)
			{
				value = FMath.Clamp(value, 0f, 1f);
				this.widgetInfos[w].dampingConstants[(int)t] = -10f * value;
			});
		}

		public float GetDampingConstant(Widget widget, SpringType type)
		{
			if (this.widgetInfos.ContainsKey(widget) && type != SpringType.All)
			{
				return this.widgetInfos[widget].dampingConstants[(int)type];
			}
			return 0f;
		}

		public void SetSpringConstant(Widget widget, SpringType type, float springConstant)
		{
			this.SetValue(widget, type, springConstant, delegate(Widget w, SpringType t, float value)
			{
				value = FMath.Clamp(value, 0f, 1f);
				this.widgetInfos[w].springConstants[(int)t] = 4f * value + 1f;
				this.widgetInfos[w].springLimitations[(int)t] = 1f - FMath.Pow(value, 5f);
			});
		}

		public float GetSpringConstant(Widget widget, SpringType type)
		{
			if (this.widgetInfos.ContainsKey(widget) && type != SpringType.All)
			{
				return this.widgetInfos[widget].springConstants[(int)type];
			}
			return 0f;
		}

		public void SetDisplacement(Widget widget, SpringType type, float displacement)
		{
			this.SetValue(widget, type, displacement, delegate(Widget w, SpringType t, float value)
			{
			    this.widgetInfos[w].displacements[(int)t] = value;
				this.widgetInfos[w].useSpecifiedValues[(int)t] = true;
			});
		}

		public float GetDisplacement(Widget widget, SpringType type)
		{
			if (this.widgetInfos.ContainsKey(widget) && type != SpringType.All)
			{
				return this.widgetInfos[widget].displacements[(int)type];
			}
			return 0f;
		}

		private void SetValue(Widget widget, SpringType type, float value, Action<Widget, SpringType, float> setAction)
		{
			foreach (Widget current in this.Children)
			{
				if (widget == null || widget == current)
				{
					foreach (SpringType springType in Enum.GetValues(typeof(SpringType)))
					{
						if ((type == SpringType.All || type == springType) && this.widgetInfos.ContainsKey(current))
						{
							setAction.Invoke(current, springType, value);
						}
					}
				}
			}
		}

		public void AddAcceleraton(float x, float y, float z)
		{
			this.userAcceleration.X = this.ClipAcceleration(x);
			this.userAcceleration.Y = this.ClipAcceleration(y);
			this.userAcceleration.Z = this.ClipAcceleration(z);
		}

		private float ClipAcceleration(float acceleration)
		{
			return FMath.Clamp(acceleration, -1f, 1f);
		}
	}
}
