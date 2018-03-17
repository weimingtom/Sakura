using Sce.Pss.Core.Graphics;
using System;
using System.Collections.Generic;

namespace Sce.Pss.HighLevel.GameEngine2D.Base
{
	internal class VertexBufferPool : IDisposable
	{
		private class Entry
		{
			internal VertexBuffer m_vertex_buffer;

			internal uint m_frame_count;
		}

		private class PerSizeList
		{
			internal VertexBufferPool m_parent;

			internal int m_max_vertices;

			internal List<VertexBufferPool.Entry> m_active_list;

			internal List<VertexBufferPool.Entry> m_free_list;

			public PerSizeList(int max_vertices)
			{
				this.m_max_vertices = max_vertices;
				this.m_active_list = new List<VertexBufferPool.Entry>();
				this.m_free_list = new List<VertexBufferPool.Entry>();
			}

			public VertexBuffer GetAVertexBuffer()
			{
				if (this.m_free_list.Count == 0)
				{
					VertexBuffer vertexBuffer;
					if (null == this.m_parent.m_indices)
					{
						vertexBuffer = new VertexBuffer(this.m_max_vertices, 0, this.m_parent.m_format);
					}
					else
					{
						int num = this.m_max_vertices / this.m_parent.m_vertices_per_primitive * this.m_parent.m_indices_per_primitive;
						vertexBuffer = new VertexBuffer(this.m_max_vertices, num, this.m_parent.m_format);
						Common.Assert(num <= this.m_parent.m_indices.Length);
						vertexBuffer.SetIndices(this.m_parent.m_indices, 0, 0, num);
					}
					this.m_free_list.Add(new VertexBufferPool.Entry
					{
						m_vertex_buffer = vertexBuffer,
						m_frame_count = Common.FrameCount
					});
				}
				VertexBufferPool.Entry entry = this.m_free_list[this.m_free_list.Count - 1];
				this.m_free_list.RemoveAt(this.m_free_list.Count - 1);
				this.m_active_list.Add(entry);
				return entry.m_vertex_buffer;
			}
		}

		private bool m_disposed = false;

		private List<VertexBufferPool.PerSizeList> m_per_size_lists;

		private ushort[] m_indices;

		private int m_vertices_per_primitive;

		private int m_indices_per_primitive;

		private VertexFormat[] m_format;

		public int DisposeInterval = 1800;

		public bool Disposed
		{
			get
			{
				return this.m_disposed;
			}
		}

		public VertexBufferPool(ushort[] indices_model, int vertices_per_primitive, int indices_per_primitive, params VertexFormat[] formats)
		{
			this.m_per_size_lists = new List<VertexBufferPool.PerSizeList>();
			this.m_indices = indices_model;
			this.m_vertices_per_primitive = vertices_per_primitive;
			this.m_indices_per_primitive = indices_per_primitive;
			this.m_format = formats;
			for (int i = 0; i < 20; i++)
			{
				this.m_per_size_lists.Add(new VertexBufferPool.PerSizeList(1 << i));
				this.m_per_size_lists[this.m_per_size_lists.Count - 1].m_parent = this;
			}
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
				foreach (VertexBufferPool.PerSizeList current in this.m_per_size_lists)
				{
					foreach (VertexBufferPool.Entry current2 in current.m_active_list)
					{
						Common.DisposeAndNullify<VertexBuffer>(ref current2.m_vertex_buffer);
					}
					current.m_active_list.Clear();
					foreach (VertexBufferPool.Entry current2 in current.m_free_list)
					{
						Common.DisposeAndNullify<VertexBuffer>(ref current2.m_vertex_buffer);
					}
					current.m_free_list.Clear();
				}
				this.m_disposed = true;
			}
		}

		public void OnFrameChanged()
		{
			foreach (VertexBufferPool.PerSizeList current in this.m_per_size_lists)
			{
				foreach (VertexBufferPool.Entry current2 in current.m_active_list)
				{
					if ((ulong)(Common.FrameCount - current2.m_frame_count) > (ulong)((long)this.DisposeInterval))
					{
						Common.DisposeAndNullify<VertexBuffer>(ref current2.m_vertex_buffer);
					}
					else
					{
						current.m_free_list.Add(current2);
					}
				}
				current.m_active_list.Clear();
			}
		}

		public VertexBuffer GetAVertexBuffer(int max_vertices)
		{
			int num = Math.Log2(max_vertices);
			if (1 << num < max_vertices)
			{
				num++;
			}
			return this.m_per_size_lists[num].GetAVertexBuffer();
		}

		public void Dump()
		{
			foreach (VertexBufferPool.PerSizeList current in this.m_per_size_lists)
			{
				if (current.m_free_list.Count != 0 || current.m_active_list.Count != 0)
				{
					Console.WriteLine(string.Concat(new object[]
					{
						Common.FrameCount,
						" ",
						current.m_max_vertices,
						" vertices : ",
						current.m_free_list.Count,
						current.m_active_list.Count
					}));
				}
			}
		}
	}
}
