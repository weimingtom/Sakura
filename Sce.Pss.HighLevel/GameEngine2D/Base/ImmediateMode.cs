using Sce.Pss.Core.Graphics;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D.Base
{
	public class ImmediateMode<T> : IDisposable
	{
		private GraphicsContextAlpha GL;

		private VertexBufferPool m_vbuf_pool;

		private VertexBuffer m_current_vertex_buffer;

		private T[] m_vertices_tmp;

		private uint m_max_vertices;

		private uint m_max_indices;

		private DrawMode m_prim;

		private uint m_prim_start;

		private uint m_pos;

		private uint m_frame_count;

		private uint m_max_vertices_intended;

		private bool m_active;

		private int m_vertices_per_primitive;

		private int m_indices_per_primitive;

		private bool m_disposed = false;

		public bool Disposed
		{
			get
			{
				return this.m_disposed;
			}
		}

		public bool ImmActive
		{
			get
			{
				return this.m_active;
			}
		}

		public uint MaxVertices
		{
			get
			{
				return this.m_max_vertices;
			}
		}

		public ImmediateMode(GraphicsContextAlpha gl, uint max_vertices, ushort[] indices, int vertices_per_primitive, int indices_per_primitive, params VertexFormat[] formats)
		{
			this.GL = gl;
			this.m_max_vertices = max_vertices;
			this.m_vertices_per_primitive = vertices_per_primitive;
			this.m_indices_per_primitive = indices_per_primitive;
			this.m_vertices_tmp = new T[this.m_max_vertices];
			this.m_frame_count = 4294967295u;
			this.m_pos = 0u;
			if (indices != null)
			{
				this.m_max_indices = (uint)indices.Length;
				Common.Assert(this.m_vertices_per_primitive != 0);
				Common.Assert(this.m_indices_per_primitive != 0);
				Common.Assert((ulong)this.m_max_vertices / (ulong)((long)this.m_vertices_per_primitive) * (ulong)((long)this.m_vertices_per_primitive) == (ulong)this.m_max_vertices);
				Common.Assert((ulong)this.m_max_indices / (ulong)((long)this.m_indices_per_primitive) * (ulong)((long)this.m_indices_per_primitive) == (ulong)this.m_max_indices);
				Common.Assert((ulong)this.m_max_vertices / (ulong)((long)this.m_vertices_per_primitive) == (ulong)this.m_max_indices / (ulong)((long)this.m_indices_per_primitive));
			}
			else
			{
				this.m_max_indices = 0u;
			}
			this.m_vbuf_pool = new VertexBufferPool(indices, this.m_vertices_per_primitive, this.m_indices_per_primitive, formats);
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
				this.m_vbuf_pool.Dispose();
				this.m_disposed = true;
			}
		}

		public void ImmBegin(DrawMode mode, uint max_vertices_intended)
		{
			if (this.m_frame_count != Common.FrameCount)
			{
				this.m_frame_count = Common.FrameCount;
				this.m_vbuf_pool.OnFrameChanged();
			}
			this.m_current_vertex_buffer = this.m_vbuf_pool.GetAVertexBuffer((int)max_vertices_intended);
			this.m_pos = 0u;
			this.m_prim_start = this.m_pos;
			this.m_prim = mode;
			this.m_max_vertices_intended = max_vertices_intended;
			this.m_active = true;
		}

		public void ImmVertex(T vertex)
		{
			this.m_vertices_tmp[(int)((UIntPtr)(this.m_pos++))] = vertex;
		}

		private void imm_end_prelude()
		{
			Common.Assert(this.m_pos - this.m_prim_start <= this.m_max_vertices_intended, "You added more vertices than you said you would.");
			this.GL.Context.SetVertexBuffer(0, this.m_current_vertex_buffer);
			this.m_current_vertex_buffer.SetVertices(this.m_vertices_tmp, (int)this.m_prim_start, (int)this.m_prim_start, (int)(this.m_pos - this.m_prim_start));
		}

		public void ImmEnd()
		{
			this.imm_end_prelude();
			this.GL.Context.DrawArrays(this.m_prim, (int)this.m_prim_start, (int)(this.m_pos - this.m_prim_start));
			this.GL.DebugStats.OnDrawArray();
			this.m_active = false;
		}

		public void ImmEndIndexing()
		{
			this.imm_end_prelude();
			this.GL.Context.DrawArrays(this.m_prim, (int)(this.m_prim_start / (uint)this.m_vertices_per_primitive * (uint)this.m_indices_per_primitive), (int)((this.m_pos - this.m_prim_start) / (uint)this.m_vertices_per_primitive * (uint)this.m_indices_per_primitive));
			this.GL.DebugStats.OnDrawArray();
		}
	}
}
