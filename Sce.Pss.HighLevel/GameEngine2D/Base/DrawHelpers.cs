using Sce.Pss.Core;
using Sce.Pss.Core.Graphics;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D.Base
{
	public class DrawHelpers : IDisposable
	{
		public struct Vertex
		{
			public Vector4 Position;

			public Vector4 Color;

			public Vertex(Vector4 pos, Vector4 col)
			{
				this.Position = pos;
				this.Color = col;
			}

			public Vertex(Vector2 pos, Vector4 col)
			{
				this.Position = pos.Xy01;
				this.Color = col;
			}
		}

		public class ArrowParams
		{
			public float HeadRadius;

			public float HeadLen;

			public float BodyRadius;

			public float Scale;

			public uint HalfMask;

			public float Offset;

			public ArrowParams(float r = 1f)
			{
				this.HeadRadius = 0.06f * r;
				this.HeadLen = 0.2f * r;
				this.BodyRadius = 0.02f * r;
				this.Scale = 1f;
				this.HalfMask = 3u;
				this.Offset = 0f;
			}
		}

		private GraphicsContextAlpha GL;

		private ImmediateMode<DrawHelpers.Vertex> m_imm;

		private ShaderProgram m_shader_program;

		private Vector4 m_current_color;

		private uint m_shader_depth;

		private uint m_view_matrix_tag;

		private uint m_model_matrix_tag;

		private uint m_projection_matrix_tag;

		private bool m_disposed = false;

		public bool Disposed
		{
			get
			{
				return this.m_disposed;
			}
		}

		public DrawHelpers(GraphicsContextAlpha gl, uint max_vertices)
		{
			this.GL = gl;
			this.m_shader_program = Common.CreateShaderProgram("cg/default.cgx");
			this.m_shader_program.SetUniformBinding(0, "MVP");
			this.m_shader_program.SetAttributeBinding(0, "p");
			this.m_shader_program.SetAttributeBinding(1, "vin_color");
			this.m_current_color = Colors.Magenta;
			this.m_shader_depth = 0u;
			this.m_view_matrix_tag = 4294967295u;
			this.m_model_matrix_tag = 4294967295u;
			this.m_projection_matrix_tag = 4294967295u;
			this.m_imm = new ImmediateMode<DrawHelpers.Vertex>(gl, max_vertices, null, 0, 0, new VertexFormat[]
			{
			                                                   	(VertexFormat)259,
			                                                   	(VertexFormat)259
			});
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.m_imm.Dispose();
				Common.DisposeAndNullify<ShaderProgram>(ref this.m_shader_program);
				this.m_disposed = true;
			}
		}

		public void ImmBegin(DrawMode mode, uint max_vertices_intended)
		{
			this.m_imm.ImmBegin(mode, max_vertices_intended);
		}

		public void ImmVertex(DrawHelpers.Vertex v)
		{
			this.m_imm.ImmVertex(v);
		}

		public void ImmVertex(Vector4 pos)
		{
			this.m_imm.ImmVertex(new DrawHelpers.Vertex(pos, this.m_current_color));
		}

		public void ImmVertex(Vector2 pos)
		{
			this.m_imm.ImmVertex(new DrawHelpers.Vertex(pos.Xy01, this.m_current_color));
		}

		public void ImmEnd()
		{
			this.m_imm.ImmEnd();
		}

		public void ShaderPush()
		{
			if (this.m_view_matrix_tag != this.GL.ViewMatrix.Tag || this.m_model_matrix_tag != this.GL.ModelMatrix.Tag || this.m_projection_matrix_tag != this.GL.ProjectionMatrix.Tag)
			{
				this.m_model_matrix_tag = this.GL.ModelMatrix.Tag;
				this.m_view_matrix_tag = this.GL.ViewMatrix.Tag;
				this.m_projection_matrix_tag = this.GL.ProjectionMatrix.Tag;
				Matrix4 mVP = this.GL.GetMVP();
				this.m_shader_program.SetUniformValue(0, ref mVP);
			}
			if (this.m_shader_depth++ == 0u)
			{
				this.GL.Context.SetShaderProgram(this.m_shader_program);
			}
		}

		public void ShaderPop()
		{
			Common.Assert(this.m_shader_depth > 0u);
			if ((this.m_shader_depth -= 1u) != 0u)
			{
			}
		}

		public void SetColor(Vector4 value)
		{
			this.m_current_color = value;
		}

		public void DrawBounds2Fill(Bounds2 bounds)
		{
			this.ShaderPush();
			this.ImmBegin((DrawMode)4, 4u);
			this.ImmVertex(new DrawHelpers.Vertex(bounds.Point01.Xy01, this.m_current_color));
			this.ImmVertex(new DrawHelpers.Vertex(bounds.Point00.Xy01, this.m_current_color));
			this.ImmVertex(new DrawHelpers.Vertex(bounds.Point11.Xy01, this.m_current_color));
			this.ImmVertex(new DrawHelpers.Vertex(bounds.Point10.Xy01, this.m_current_color));
			this.ImmEnd();
			this.ShaderPop();
		}

		public void DrawBounds2(Bounds2 bounds)
		{
			this.ShaderPush();
			this.ImmBegin((DrawMode)2, 5u);
			this.ImmVertex(new DrawHelpers.Vertex(bounds.Point00.Xy01, this.m_current_color));
			this.ImmVertex(new DrawHelpers.Vertex(bounds.Point10.Xy01, this.m_current_color));
			this.ImmVertex(new DrawHelpers.Vertex(bounds.Point11.Xy01, this.m_current_color));
			this.ImmVertex(new DrawHelpers.Vertex(bounds.Point01.Xy01, this.m_current_color));
			this.ImmVertex(new DrawHelpers.Vertex(bounds.Point00.Xy01, this.m_current_color));
			this.ImmEnd();
			this.ShaderPop();
		}

		public void DrawConvexPoly2(ConvexPoly2 convex_poly)
		{
			if (convex_poly.Planes.Length != 0)
			{
				this.ShaderPush();
				this.ImmBegin((DrawMode)2, (uint)(convex_poly.Planes.Length + 1));
				Plane2[] planes = convex_poly.Planes;
				for (int i = 0; i < planes.Length; i++)
				{
					Plane2 plane = planes[i];
					this.ImmVertex(new DrawHelpers.Vertex(plane.Base, this.m_current_color));
				}
				this.ImmVertex(new DrawHelpers.Vertex(convex_poly.Planes[0].Base, this.m_current_color));
				this.ImmEnd();
				this.ShaderPop();
			}
		}

		public void DrawDisk(Vector2 center, float radius, uint n)
		{
			this.ShaderPush();
			this.ImmBegin((DrawMode)5, n);
			for (uint num = 0u; num < n; num += 1u)
			{
				Vector2 vector = Vector2.Rotation(num / (n - 1u) * Math.TwicePi);
				this.ImmVertex(new DrawHelpers.Vertex((center + vector * radius).Xy01, this.m_current_color));
			}
			this.ImmEnd();
			this.ShaderPop();
		}

		public void DrawCircle(Vector2 center, float radius, uint n)
		{
			this.ShaderPush();
			this.ImmBegin((DrawMode)2, n);
			for (uint num = 0u; num < n; num += 1u)
			{
				Vector2 vector = Vector2.Rotation(num / (n - 1u) * Math.TwicePi);
				this.ImmVertex(new DrawHelpers.Vertex((center + vector * radius).Xy01, this.m_current_color));
			}
			this.ImmEnd();
			this.ShaderPop();
		}

		public void DrawCoordinateSystem2D(Matrix3 mat, DrawHelpers.ArrowParams ap = null)
		{
			if (ap == null)
			{
				ap = new DrawHelpers.ArrowParams(1f);
			}
			this.ShaderPush();
			this.ImmBegin((DrawMode)3, 18u);
			this.SetColor(Colors.Red);
			this.DrawArrow(mat.Z.Xy, mat.Z.Xy + mat.X.Xy, ap);
			this.SetColor(Colors.Green);
			this.DrawArrow(mat.Z.Xy, mat.Z.Xy + mat.Y.Xy, ap);
			this.ImmEnd();
			this.ShaderPop();
		}

		public void DrawCoordinateSystem2D()
		{
			this.DrawCoordinateSystem2D(Matrix3.Identity, null);
		}

		public void DrawArrow(Vector2 start_point, Vector2 end_point, DrawHelpers.ArrowParams ap)
		{
			Vector2 vector = (end_point - start_point).Normalize();
			Vector2 vector2 = Math.Perp(vector);
			start_point += vector2 * ap.Offset;
			end_point += vector2 * ap.Offset;
			ap.BodyRadius *= ap.Scale;
			ap.HeadRadius *= ap.Scale;
			ap.HeadLen *= ap.Scale;
			float num = ap.HeadRadius;
			float num2 = ap.HeadRadius;
			float num3 = ap.BodyRadius;
			float num4 = ap.BodyRadius;
			if ((ap.HalfMask & 1u) == 0u)
			{
				num2 = 0f;
				num4 = 0f;
			}
			if ((ap.HalfMask & 2u) == 0u)
			{
				num = 0f;
				num3 = 0f;
			}
			this.ShaderPush();
			bool immActive = this.m_imm.ImmActive;
			if (ap.BodyRadius == 0f && !immActive)
			{
				this.ImmBegin((DrawMode)1, 2u);
				this.ImmVertex(start_point);
				this.ImmVertex(end_point);
				this.ImmEnd();
			}
			if (!immActive)
			{
				this.ImmBegin((DrawMode)3, 9u);
			}
			if (ap.BodyRadius != 0f)
			{
				Vector2 pos = start_point + num3 * vector2;
				Vector2 pos2 = start_point - num4 * vector2;
				Vector2 pos3 = end_point - vector * ap.HeadLen + num3 * vector2;
				Vector2 pos4 = end_point - vector * ap.HeadLen - num4 * vector2;
				this.ImmVertex(pos);
				this.ImmVertex(pos2);
				this.ImmVertex(pos3);
				this.ImmVertex(pos2);
				this.ImmVertex(pos4);
				this.ImmVertex(pos3);
			}
			this.ImmVertex(end_point - vector * ap.HeadLen - num2 * vector2);
			this.ImmVertex(end_point);
			this.ImmVertex(end_point - vector * ap.HeadLen + num * vector2);
			if (!immActive)
			{
				this.ImmEnd();
			}
			this.ShaderPop();
		}

		public void DrawLineSegment(Vector2 A, Vector2 B)
		{
			this.ShaderPush();
			this.ImmBegin((DrawMode)1, 2u);
			this.ImmVertex(A);
			this.ImmVertex(B);
			this.ImmEnd();
			this.ShaderPop();
		}

		public void DrawInfiniteLine(Vector2 A, Vector2 v)
		{
			this.ShaderPush();
			v *= 10000f;
			this.ImmBegin((DrawMode)1, 2u);
			this.ImmVertex(A - v);
			this.ImmVertex(A + v);
			this.ImmEnd();
			this.ShaderPop();
		}

		public void DrawRulers(Bounds2 bounds, float step_x, float step_y)
		{
			step_x = FMath.Max(step_x, 0f);
			step_y = FMath.Max(step_y, 0f);
			if (step_x >= 1.401298E-45f)
			{
				if (step_y >= 1.401298E-45f)
				{
					float x = bounds.Min.X;
					float x2 = bounds.Max.X;
					float y = bounds.Min.Y;
					float y2 = bounds.Max.Y;
					int num = (int)(x / step_x);
					int num2 = (int)(x2 / step_x);
					int num3 = (int)(y / step_y);
					int num4 = (int)(y2 / step_y);
					this.ShaderPush();
					bool flag = num2 - num + 1 < 1000;
					bool flag2 = num4 - num3 + 1 < 1000;
					this.ImmBegin((DrawMode)1, (uint)((flag ? ((num2 - num + 1) * 2) : 0) + (flag2 ? ((num4 - num3 + 1) * 2) : 0)));
					if (flag)
					{
						for (int i = num; i <= num2; i++)
						{
							this.ImmVertex(new Vector2((float)i * step_x, y));
							this.ImmVertex(new Vector2((float)i * step_x, y2));
						}
					}
					if (flag2)
					{
						for (int i = num3; i <= num4; i++)
						{
							this.ImmVertex(new Vector2(x, (float)i * step_y));
							this.ImmVertex(new Vector2(x2, (float)i * step_y));
						}
					}
					this.ImmEnd();
					this.ShaderPop();
				}
			}
		}

		public void DrawAxis(Bounds2 clipping_bounds, float thickness)
		{
			this.GL.Context.SetLineWidth(thickness);
			float num = 0f;
			float num2 = 0f;
			this.ShaderPush();
			this.ImmBegin((DrawMode)1, 4u);
			this.ImmVertex(new Vector2(clipping_bounds.Min.X, num2));
			this.ImmVertex(new Vector2(clipping_bounds.Max.X, num2));
			this.ImmVertex(new Vector2(num, clipping_bounds.Min.Y));
			this.ImmVertex(new Vector2(num, clipping_bounds.Max.Y));
			this.ImmEnd();
			this.ShaderPop();
			this.GL.Context.SetLineWidth(1f);
		}

		public void DrawDefaultGrid(Bounds2 clipping_bounds, Vector2 step, Vector4 rulers_color, Vector4 axis_color)
		{
			this.ShaderPush();
			this.SetColor(rulers_color);
			this.DrawRulers(clipping_bounds, step.X, step.Y);
			this.GL.Context.Disable((EnableMode)4);
			this.SetColor(axis_color);
			this.DrawAxis(clipping_bounds, 2f);
			this.ShaderPop();
		}

		public void DrawDefaultGrid(Bounds2 clipping_bounds, float step)
		{
			this.GL.Context.Enable((EnableMode)4);
			this.GL.Context.SetBlendFunc(new BlendFunc((BlendFuncMode)0, (BlendFuncFactor)4, (BlendFuncFactor)1));
			this.DrawDefaultGrid(clipping_bounds, new Vector2(step), Colors.Grey30 * 0.5f, Colors.Black);
		}
	}
}
