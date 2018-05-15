using Sce.Pss.Core;
using System;
using System.Collections.Generic;

namespace Sce.Pss.HighLevel.UI
{
	public class LiveJumpPanel : Panel
	{
		private class JumpWidgetInfo
		{
			public Widget Widget;

			public float JumpDlayTime;

			public Vector3 Axes;

			public PivotType originalPivot;
		}

		private List<LiveJumpPanel.JumpWidgetInfo> jumpWidgetInfoList;

		private float totalElapsedTime;

		private float jumpDelayTime = 2f;

		private float jumpTime = 500f;

		private float jumpHeight = 150f;

		private float maxBlurTime = 480f;

		private float blurBoundTime = 170f;

		private float blurBoundDistance = 12f;

		private float tiltAngle = 0.174532935f;

		private float boundDistanceN = 40f;

		public float JumpHeight
		{
			get
			{
				return this.jumpHeight;
			}
			set
			{
				this.jumpHeight = value;
			}
		}

		public float JumpDelayTime
		{
			get
			{
				return this.jumpDelayTime;
			}
			set
			{
				this.jumpDelayTime = value;
			}
		}

		public float JumpTime
		{
			get
			{
				return this.jumpTime;
			}
			set
			{
				this.jumpTime = value;
			}
		}

		public float TiltAngle
		{
			get
			{
				return this.tiltAngle;
			}
			set
			{
				this.tiltAngle = value;
			}
		}

		public LiveJumpPanel()
		{
			this.jumpWidgetInfoList = new List<LiveJumpPanel.JumpWidgetInfo>();
		}

		public void Jump()
		{
			this.Jump(this.Width / 2f, this.Height / 2f);
		}

		public void Jump(float x, float y)
		{
			if (this.jumpWidgetInfoList.Count > 0)
			{
				return;
			}
			foreach (Widget current in this.Children)
			{
				Vector2 vector = this.GetCenterPos(current) - new Vector2(x, y);
				LiveJumpPanel.JumpWidgetInfo jumpWidgetInfo = new LiveJumpPanel.JumpWidgetInfo();
				jumpWidgetInfo.originalPivot = current.PivotType;
				jumpWidgetInfo.Widget = current;
				current.PivotType = PivotType.MiddleCenter;
				current.ZSort = true;
				jumpWidgetInfo.JumpDlayTime = vector.Length() * this.jumpDelayTime;
				jumpWidgetInfo.Axes = new Vector3(vector.Y, -vector.X, 0f).Normalize();
				this.jumpWidgetInfoList.Add(jumpWidgetInfo);
			}
			this.totalElapsedTime = 0f;
			if (this.jumpTime < 1000f)
			{
				this.maxBlurTime = 480f;
				this.blurBoundTime = 170f;
				return;
			}
			this.blurBoundDistance += (this.jumpTime - 1000f) / this.boundDistanceN;
			this.maxBlurTime = this.jumpTime / 2f;
			this.blurBoundTime = this.maxBlurTime / 3f;
		}

		private Vector2 GetCenterPos(Widget widget)
		{
			return new Vector2(widget.X, widget.Y);
		}

		protected override void OnUpdate(float elapsedTime)
		{
			base.OnUpdate(elapsedTime);
			if (this.jumpWidgetInfoList.Count > 0)
			{
				List<LiveJumpPanel.JumpWidgetInfo> list = new List<LiveJumpPanel.JumpWidgetInfo>();
				this.totalElapsedTime += elapsedTime;
				using (List<LiveJumpPanel.JumpWidgetInfo>.Enumerator enumerator = this.jumpWidgetInfoList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						LiveJumpPanel.JumpWidgetInfo current = enumerator.Current;
						float num = this.totalElapsedTime - current.JumpDlayTime;
						if (num >= 0f)
						{
							if (num < this.jumpTime + this.maxBlurTime)
							{
								Widget widget = current.Widget;
								Matrix4 transform3D = Matrix4.RotationAxis(current.Axes, this.GetRotationAngle(num));
								transform3D.ColumnW = (new Vector4(widget.X, widget.Y, this.GetJumpDistance(num), 1f));
								widget.Transform3D = transform3D;
							}
							else
							{
								list.Add(current);
							}
						}
					}
				}
				using (List<LiveJumpPanel.JumpWidgetInfo>.Enumerator enumerator2 = list.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						LiveJumpPanel.JumpWidgetInfo current2 = enumerator2.Current;
						current2.Widget.Transform3D = Matrix4.Translation(new Vector3(current2.Widget.X, current2.Widget.Y, 0f));
						current2.Widget.PivotType = current2.originalPivot;
						current2.Widget.ZSort = false;
						this.jumpWidgetInfoList.Remove(current2);
					}
				}
			}
		}

		private float GetJumpDistance(float elapsedTime)
		{
			if (elapsedTime < this.jumpTime)
			{
				float num = elapsedTime / (this.jumpTime / 2f) - 1f;
				return this.jumpHeight * num * num - this.jumpHeight;
			}
			float num2 = elapsedTime - this.jumpTime;
			return -this.blurBoundDistance / 2f * ((1f - num2 / this.maxBlurTime) * (1f - (float)Math.Cos((double)MathUtility.DegreeToRadian(360f * num2 / this.blurBoundTime))));
		}

		private float GetRotationAngle(float elapsedTime)
		{
			if (elapsedTime < this.jumpTime)
			{
				float num = elapsedTime / this.jumpTime * 2f * 3.14159274f;
				return this.tiltAngle * (float)Math.Sin((double)num);
			}
			float num2 = elapsedTime - this.jumpTime;
			return -this.tiltAngle * ((1f - num2 / this.maxBlurTime) * (float)Math.Sin((double)(num2 / this.blurBoundTime) * 3.1415926535897931 * 2.0));
		}
	}
}
