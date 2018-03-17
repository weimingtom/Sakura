using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D.Base
{
	public class Camera2D : ICamera
	{
		internal struct Data
		{
			internal Vector2 m_support_unit_vec;

			internal float m_support_scale;

			internal bool m_support_is_y;

			internal Vector2 m_center;

			internal float m_aspect;

			internal float m_znear;

			internal float m_zfar;
		}

		private GraphicsContextAlpha GL;

		private DrawHelpers m_draw_helpers;

		private Camera2D.Data m_data;

		private int m_push_depth;

		private bool m_prev_touch_state;

		private bool m_touch_state;

		private int m_drag_mode;

		private Vector2 m_drag_start_pos;

		private Vector2 m_drag_start_pos_ncs;

		private Camera2D.Data m_data_start;

		public Vector2 Center
		{
			get
			{
				return this.m_data.m_center;
			}
			set
			{
				this.m_data.m_center = value;
			}
		}

		public float Aspect
		{
			get
			{
				return this.m_data.m_aspect;
			}
			set
			{
				this.m_data.m_aspect = value;
			}
		}

		public float Znear
		{
			get
			{
				return this.m_data.m_znear;
			}
			set
			{
				this.m_data.m_znear = value;
			}
		}

		public float Zfar
		{
			get
			{
				return this.m_data.m_zfar;
			}
			set
			{
				this.m_data.m_zfar = value;
			}
		}

		public Camera2D(GraphicsContextAlpha gl, DrawHelpers draw_helpers)
		{
			this.GL = gl;
			this.m_draw_helpers = draw_helpers;
			this.m_data.m_support_scale = 1f;
			this.m_data.m_support_unit_vec = Math._01;
			this.m_data.m_support_is_y = true;
			this.m_data.m_center = Math._00;
			this.m_data.m_aspect = 1f;
			this.m_data.m_znear = -1f;
			this.m_data.m_zfar = 1f;
			this.m_push_depth = 0;
			this.m_prev_touch_state = false;
			this.m_touch_state = false;
			this.m_drag_mode = 0;
			this.m_drag_start_pos = Math._00;
		}

		public void SetViewX(Vector2 support, Vector2 center)
		{
			this.m_data.m_support_scale = support.Length();
			this.m_data.m_support_unit_vec = support / this.m_data.m_support_scale;
			this.m_data.m_center = center;
			this.m_data.m_support_is_y = false;
			this.SetAspectFromViewport();
		}

		public void SetViewY(Vector2 support, Vector2 center)
		{
			this.m_data.m_support_scale = support.Length();
			this.m_data.m_support_unit_vec = support / this.m_data.m_support_scale;
			this.m_data.m_center = center;
			this.m_data.m_support_is_y = true;
			this.SetAspectFromViewport();
		}

		public void SetViewFromWidthAndCenter(float width, Vector2 center)
		{
			this.SetViewX(new Vector2(width * 0.5f, 0f), center);
		}

		public void SetViewFromHeightAndCenter(float height, Vector2 center)
		{
			this.SetViewY(new Vector2(0f, height * 0.5f), center);
		}

		public void SetViewFromHeightAndBottomLeft(float height, Vector2 bottom_left)
		{
			this.SetAspectFromViewport();
			float num = height * this.Aspect;
			this.SetViewY(new Vector2(0f, height * 0.5f), bottom_left + new Vector2(num, height) * 0.5f);
		}

		public void SetViewFromWidthAndBottomLeft(float width, Vector2 bottom_left)
		{
			this.SetAspectFromViewport();
			float num = width / this.Aspect;
			this.SetViewX(new Vector2(width * 0.5f, 0f), bottom_left + new Vector2(width, num) * 0.5f);
		}

		public void SetViewFromViewport()
		{
			Bounds2 viewportf = this.GL.GetViewportf();
			Common.Assert(viewportf.Size.Y != 0f);
			this.SetViewFromHeightAndCenter(viewportf.Size.Y, viewportf.Center);
			this.m_data.m_aspect = viewportf.Aspect;
		}

		public void SetAspectFromViewport()
		{
			this.m_data.m_aspect = this.GL.GetViewportf().Aspect;
		}

		public Vector2 X()
		{
			return (this.m_data.m_support_is_y ? (-Math.Perp(this.m_data.m_support_unit_vec) * this.m_data.m_aspect) : this.m_data.m_support_unit_vec) * this.m_data.m_support_scale;
		}

		public Vector2 Y()
		{
			return Math.Perp(this.X()) / this.m_data.m_aspect;
		}

		public Bounds2 CalcBounds()
		{
			Vector2 vector = this.X();
			Vector2 vector2 = this.Y();
			Vector2 center = this.Center;
			Bounds2 result = new Bounds2(center, center);
			result.Add(center - vector - vector2);
			result.Add(center + vector - vector2);
			result.Add(center + vector + vector2);
			result.Add(center - vector + vector2);
			return result;
		}

		public Matrix4 GetTransform()
		{
			return new Matrix4(this.X().Normalize().Xy00, this.Y().Normalize().Xy00, Math._0010, this.Center.Xy01);
		}

		public Matrix3 NormalizedToWorldMatrix()
		{
			return new Matrix3(this.X().Xy0, this.Y().Xy0, this.Center.Xy1);
		}

		public Vector2 NormalizedToWorld(Vector2 bottom_left_minus_1_minus_1_top_left_1_1_normalized_screen_pos)
		{
			return (this.NormalizedToWorldMatrix() * bottom_left_minus_1_minus_1_top_left_1_1_normalized_screen_pos.Xy1).Xy;
		}

		public Vector2 GetTouchPos(int nth = 0, bool prev = false)
		{
			return this.NormalizedToWorld(prev ? Input2.Touch.GetData(0u)[nth].PreviousPos : Input2.Touch.GetData(0u)[nth].Pos);
		}

		private float zoom_curve(float x, float a, float right_scale)
		{
			float num = 1f;
			if (x > 0f)
			{
				num = right_scale;
			}
			float num2 = Math.Sign(x);
			return 1f + num * num2 * (1f - FMath.Exp(-a * FMath.Abs(x / num)));
		}

		public void Navigate(int control)
		{
			this.m_prev_touch_state = this.m_touch_state;
			this.m_touch_state = Input2.Touch00.Down;
			bool flag = this.m_touch_state && !this.m_prev_touch_state;
			bool touch_state = this.m_touch_state;
			if (control == 1)
			{
				if (touch_state)
				{
					if (flag)
					{
						this.m_drag_start_pos = this.GetTouchPos(0, false);
						this.m_data_start = this.m_data;
						this.m_drag_mode = 1;
					}
					if (this.m_drag_mode == 1)
					{
						Camera2D.Data data = this.m_data;
						this.m_data = this.m_data_start;
						Vector2 touchPos = this.GetTouchPos(0, !flag);
						this.m_data = data;
						this.m_data.m_center = this.m_data_start.m_center + (this.m_drag_start_pos - touchPos);
					}
				}
				else
				{
					this.m_drag_mode = 0;
				}
			}
			if (control == 2)
			{
				if (touch_state)
				{
					if (flag)
					{
						this.m_drag_start_pos = this.GetTouchPos(0, false);
						this.m_drag_start_pos_ncs = Input2.Touch00.Pos;
						this.m_data_start = this.m_data;
						this.m_drag_mode = 2;
					}
					if (this.m_drag_mode == 2)
					{
						Camera2D.Data data = this.m_data;
						this.m_data = this.m_data_start;
						Vector2 touchPos2 = this.GetTouchPos(0, !flag);
						Vector2 vector = (!flag) ? Input2.Touch00.PreviousPos : Input2.Touch00.Pos;
						this.m_data = data;
						Vector2 vector2 = vector - this.m_drag_start_pos_ncs;
						this.m_data.m_support_scale = this.m_data_start.m_support_scale * this.zoom_curve(-vector2.Y, 2f, 5f);
						this.m_data.m_support_scale = FMath.Clamp(this.m_data.m_support_scale, this.m_data_start.m_support_scale / 100f, this.m_data_start.m_support_scale * 10f);
						this.m_data.m_support_scale = FMath.Max(this.m_data.m_support_scale, 0.0001f);
					}
				}
				else
				{
					this.m_drag_mode = 0;
				}
			}
		}

		public void Push()
		{
			Common.Assert(this.GetTransform().IsOrthonormal(0.0001f));
			this.m_push_depth++;
			float num = this.X().Length();
			float num2 = this.Y().Length();
			this.GL.ProjectionMatrix.Push();
			this.GL.ProjectionMatrix.Set(Matrix4.Ortho(-num, num, -num2, num2, this.m_data.m_znear, this.m_data.m_zfar));
			this.GL.ViewMatrix.Push();
			this.GL.ViewMatrix.Set1(this.GetTransform().InverseOrthonormal());
			this.GL.ModelMatrix.Push();
		}

		public void Pop()
		{
			Common.Assert(this.m_push_depth > 0);
			this.m_push_depth--;
			this.GL.ModelMatrix.Pop();
			this.GL.ViewMatrix.Pop();
			this.GL.ProjectionMatrix.Pop();
		}

		public void DebugDraw(float step)
		{
			this.m_draw_helpers.DrawDefaultGrid(this.CalcBounds(), step);
			this.m_draw_helpers.DrawCoordinateSystem2D();
		}

		public float GetPixelSize()
		{
			Bounds2 viewportf = this.GL.GetViewportf();
			return this.X().Length() / (viewportf.Size.X * 0.5f);
		}

		public void SetTouchPlaneMatrix(Matrix4 mat)
		{
		}
	}
}
