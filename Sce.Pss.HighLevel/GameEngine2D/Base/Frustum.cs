using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D.Base
{
	public class Frustum
	{
		private bool m_is_fovy;

		private float m_fov;

		public float Aspect = 1f;

		public float Znear = 0.1f;

		public float Zfar = 1000f;

		public Matrix4 Matrix
		{
			get
			{
				return Matrix4.Perspective(this.FovY, this.Aspect, this.Znear, this.Zfar);
			}
		}

		public float FovX
		{
			get
			{
				float result;
				if (!this.m_is_fovy)
				{
					result = this.m_fov;
				}
				else
				{
					result = 2f * FMath.Atan(FMath.Tan(this.m_fov * 0.5f) * this.Aspect);
				}
				return result;
			}
			set
			{
				this.m_fov = value;
				this.m_is_fovy = false;
			}
		}

		public float FovY
		{
			get
			{
				float result;
				if (this.m_is_fovy)
				{
					result = this.m_fov;
				}
				else
				{
					result = 2f * FMath.Atan(FMath.Tan(this.m_fov * 0.5f) / this.Aspect);
				}
				return result;
			}
			set
			{
				this.m_fov = value;
				this.m_is_fovy = true;
			}
		}

		public Frustum()
		{
			this.FovY = Math.Deg2Rad(53f);
		}

		public Vector4 GetPoint(Vector2 screen_normalized_pos, float z)
		{
			float num = z * FMath.Tan(this.FovY * 0.5f);
			float num2 = this.Aspect * num;
			Vector4 xy = (screen_normalized_pos * new Vector2(num2, num)).Xy01;
			xy.Z = -z;
			return xy;
		}
	}
}
