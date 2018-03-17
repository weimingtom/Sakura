using System;
using System.Diagnostics;

namespace Sce.Pss.Core.Graphics
{
	public class VertexBuffer : IDisposable
	{
		public int __vertexCount;
		public int __indexCount;
		public object[] __verticesArr = new object[100]; //FIXME:maybe one
		public VertexFormat[] __formatsArr = new VertexFormat[100]; //FIXME:maybe one
		public ushort[] __indices;
		
		public VertexBuffer (int vertexCount, params VertexFormat[] formats)
		{
			__vertexCount = vertexCount;
			for (int i = 0; i < formats.Length; ++i)
			{
				__formatsArr[i] = formats[i];
			}
			__indexCount = 0;
		}
		
		public VertexBuffer (int vertexCount, int indexCount, params VertexFormat[] formats)
		{
			__vertexCount = vertexCount;
			for (int i = 0; i < formats.Length; ++i)
			{
				__formatsArr[i] = formats[i];
			}
			__indexCount = indexCount;
		}
		
//		public void SetVertices (int stream, Array vertices)
//		{
//		}
		
		public void SetVertices (int stream, float[] vertices)
		{
			__verticesArr[stream] = vertices;
		}
		
		public void Dispose ()
		{
			
		}
		
		public int VertexCount
		{
			get
			{
				return __vertexCount;//this.vertexCount;
			}
		}
		
		public void SetIndices (ushort[] indices)
		{
			this.__indices = indices;
		}
		
		public int IndexCount
		{
			get
			{
				//Debug.Assert(false);
				return __indexCount;//FIXME:
			}
		}
		
		public void SetVertices(Array vertices, int to, int from, int count)
		{
			//Debug.Assert(false);
//			Debug.WriteLine("===================>SetVertices index 0???");
			if (to == 0 && from == 0)
			{
				Vector4[] vertices_ = (vertices as Vector4[]);
				float[] vertices2 = new float[count * 4];
				for (int i = 0; i < count; ++i)
				{
					vertices2[i * 4 + 0] = vertices_[i].X;
					vertices2[i * 4 + 1] = vertices_[i].Y;
					vertices2[i * 4 + 2] = vertices_[i].Z;
					vertices2[i * 4 + 3] = vertices_[i].W;
				}
				SetVertices(0, vertices2);
			}
			else
			{
				Debug.Assert(false);
			}
		}
		
		public void SetIndices(ushort[] indices, int to, int from, int count)
		{
			//Debug.Assert(false);
			if (to == 0 && from == 0)
			{
				ushort[] indices2 = new ushort[count];
				for (int i = 0; i < count; ++i)
				{
					indices2[i] = indices[i];
				}
				SetIndices(indices2);
			}
			else
			{
				Debug.Assert(false);
			}
		}
	}
}
