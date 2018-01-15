using System;

namespace Sce.Pss.Core.Graphics
{
	public class VertexBuffer : IDisposable
	{
		public int __vertexCount;
		public object[] __verticesArr = new object[100];
		public VertexFormat[] __formatsArr = new VertexFormat[100];
		
		public VertexBuffer (int vertexCount, params VertexFormat[] formats)
		{
			__vertexCount = vertexCount;
			for (int i = 0; i < formats.Length; ++i)
			{
				__formatsArr[i] = formats[i];
			}
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
	}
}
