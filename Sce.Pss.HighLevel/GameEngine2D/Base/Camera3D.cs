using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D.Base
{
	public class Camera3D : ICamera
	{
		public struct Ray
		{
			public Vector4 Start;

			public Vector4 Direction;
		}

		private GraphicsContextAlpha GL;

		private DrawHelpers m_draw_helpers;

		private int m_push_depth;

		private Bounds2 m_last_2d_bounds;

		public Vector3 Eye;

		public Vector3 Center;

		public Vector3 Up;

		public Frustum Frustum;

		public Matrix4 TouchPlaneMatrix = Matrix4.Identity;

		public Camera3D(GraphicsContextAlpha gl, DrawHelpers draw_helpers)
		{
			this.GL = gl;
			this.m_draw_helpers = draw_helpers;
			this.m_push_depth = 0;
			this.Frustum = new Frustum();
		}

		public void SetFromCamera2D(Camera2D cam2d)
		{
			Vector2 vector = cam2d.Y();
			float num = vector.Length() / FMath.Tan(0.5f * this.Frustum.FovY);
			this.Eye = cam2d.Center.Xy0 + num * Math._001;
			this.Center = cam2d.Center.Xy0;
			this.Up = vector.Xy0.Normalize();
			this.m_last_2d_bounds = cam2d.CalcBounds();
		}

		public void SetAspectFromViewport()
		{
			this.Frustum.Aspect = this.GL.GetViewportf().Aspect;
		}

		public Matrix4 GetTransform()
		{
			return Math.LookAt(this.Eye, this.Center, this.Up);
		}

		public void Push()
		{
			this.m_push_depth++;
			this.SetAspectFromViewport();
			this.GL.ProjectionMatrix.Push();
			this.GL.ProjectionMatrix.Set(this.Frustum.Matrix);
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
		}

		private Camera3D.Ray calc_view_ray(Vector2 bottom_left_minus_1_minus_1_top_left_1_1_normalized_screen_pos)
		{
			Matrix4 transform = this.GetTransform();
			Vector4 vector = transform * this.Frustum.GetPoint(bottom_left_minus_1_minus_1_top_left_1_1_normalized_screen_pos, this.Frustum.Znear);
			return new Camera3D.Ray
			{
				Start = vector,
				Direction = (vector - transform.ColumnW).Normalize()
			};
		}

		public Vector2 NormalizedToWorld(Vector2 bottom_left_minus_1_minus_1_top_left_1_1_normalized_screen_pos)
		{
			Camera3D.Ray ray = this.calc_view_ray(bottom_left_minus_1_minus_1_top_left_1_1_normalized_screen_pos);
			Vector4 columnW = this.TouchPlaneMatrix.ColumnW;
			Vector4 columnZ = this.TouchPlaneMatrix.ColumnZ;
			float num = -(ray.Start - columnW).Dot(columnZ) / ray.Direction.Dot(columnZ);
			Vector2 result;
			if (num < 0f)
			{
				result = Math._00;
			}
			else
			{
				result = (this.TouchPlaneMatrix.InverseOrthonormal() * (ray.Start + ray.Direction * num)).Xy;
			}
			return result;
		}

		public Vector2 GetTouchPos(int nth = 0, bool prev = false)
		{
			return this.NormalizedToWorld(prev ? Input2.Touch.GetData(0u)[nth].PreviousPos : Input2.Touch.GetData(0u)[nth].Pos);
		}

		public Bounds2 CalcBounds()
		{
			return this.m_last_2d_bounds;
		}

		public void SetViewFromViewport()
		{
			Camera2D camera2D = new Camera2D(this.GL, this.m_draw_helpers);
			camera2D.SetViewFromViewport();
			this.SetFromCamera2D(camera2D);
		}

		public float GetPixelSize()
		{
			Bounds2 viewportf = this.GL.GetViewportf();
			return this.m_last_2d_bounds.Size.X * 0.5f / (viewportf.Size.X * 0.5f);
		}

		public void Navigate(int control)
		{
		}

		public void SetTouchPlaneMatrix(Matrix4 mat)
		{
			this.TouchPlaneMatrix = mat;
		}
	}
}
