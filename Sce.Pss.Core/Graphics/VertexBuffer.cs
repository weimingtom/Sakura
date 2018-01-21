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
	}
}
