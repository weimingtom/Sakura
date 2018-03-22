using Sce.Pss.Core.Graphics;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D.Base
{
	public class ImmediateModeQuads<T> : IDisposable
	{
		private ImmediateMode<T> m_imm;

		private uint m_max_quads;

		private bool m_disposed = false;

		public bool Disposed
		{
			get
			{
				return this.m_disposed;
			}
		}

		public uint MaxQuads
		{
			get
			{
				return this.m_max_quads;
			}
		}

		public ImmediateModeQuads(GraphicsContextAlpha gl, uint max_quads, params VertexFormat[] formats)
		{
			int num = Math.Log2((int)(max_quads * 4u));
			if (1L << (num & 31) < (long)((ulong)(max_quads * 4u)))
			{
				num++;
			}
			max_quads = (1u << num) / 4u;
			this.m_max_quads = max_quads;
			ushort[] array = new ushort[this.m_max_quads * 6u];
			ushort[] array2 = new ushort[]
			{
				0,
				1,
				3,
				0,
				3,
				2
			};
			int i = 0;
			int num2 = 0;
			while (i < (int)max_quads)
			{
				Common.Assert(num2 + 6 <= array.Length);
				for (int j = 0; j < 6; j++)
				{
					//array[num2++] = (ushort)(i * 4 + (int)array2[j]); //FIXME: overflow
					array[num2++] = (ushort)((i * 4 + (int)array2[j]) & 0xffff);
				}
				i++;
			}
			this.m_imm = new ImmediateMode<T>(gl, max_quads * 4u, array, 4, 6, formats);
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
				this.m_disposed = true;
			}
		}

		public void ImmBeginQuads(uint num_quads)
		{
			this.m_imm.ImmBegin((DrawMode)3, num_quads * 4u);
		}

		public void ImmAddQuad(T v0, T v1, T v2, T v3)
		{
			this.m_imm.ImmVertex(v0);
			this.m_imm.ImmVertex(v1);
			this.m_imm.ImmVertex(v2);
			this.m_imm.ImmVertex(v3);
		}

		public void ImmAddQuad(T[] v)
		{
			this.m_imm.ImmVertex(v[0]);
			this.m_imm.ImmVertex(v[1]);
			this.m_imm.ImmVertex(v[2]);
			this.m_imm.ImmVertex(v[3]);
		}

		public void ImmEndQuads()
		{
			this.m_imm.ImmEndIndexing();
		}
	}
}
