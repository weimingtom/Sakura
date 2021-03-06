﻿using System;
using System.Diagnostics;

using Sce.Pss.HighLevel.GameEngine2D.Base;
using Sce.Pss.HighLevel.GameEngine2D;
using SirAwesome;
using PuzzleGameDemo;
using Sce.Pss.HighLevel.UI;

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
		
		//FIXME:Array class not sure
		//???to and from not used
		public void SetVertices(Array vertices, int to, int from, int count)
		{
			//Debug.Assert(false);
//			Debug.WriteLine("===================>SetVertices index 0???");
			if (to == 0 && from == 0)// && vertices.Length == count)
			{
				if (vertices is Vector4[])
				{
					Vector4[] vertices_ = (vertices as Vector4[]); //FIXME:
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
				else if (vertices is DrawHelpers.Vertex[])
				{
					DrawHelpers.Vertex[] vertices_ = (vertices as DrawHelpers.Vertex[]); //FIXME:
					{
						float[] vertices2 = new float[count * 4];
						for (int i = 0; i < count; ++i)
						{
							vertices2[i * 4 + 0] = vertices_[i].Position.X;
							vertices2[i * 4 + 1] = vertices_[i].Position.Y;
							vertices2[i * 4 + 2] = vertices_[i].Position.Z;
							vertices2[i * 4 + 3] = vertices_[i].Position.W;
						}
						SetVertices(0, vertices2);	
					}
					{
						float[] vertices3 = new float[count * 4];
						for (int i = 0; i < count; ++i)
						{
							//1.0f;//
							vertices3[i * 4 + 0] = vertices_[i].Color.X;
							vertices3[i * 4 + 1] = vertices_[i].Color.Y;
							vertices3[i * 4 + 2] = vertices_[i].Color.Z;
							vertices3[i * 4 + 3] = vertices_[i].Color.W;
						}
						SetVertices(1, vertices3);
					}
				}
				else if (vertices is ParticleSystem.Vertex[])
				{
					ParticleSystem.Vertex[] vertices_ = (vertices as ParticleSystem.Vertex[]); //FIXME:
					{
						float[] vertices2 = new float[count * 4];
						for (int i = 0; i < count; ++i)
						{
							vertices2[i * 4 + 0] = vertices_[i].XYUV.X;
							vertices2[i * 4 + 1] = vertices_[i].XYUV.Y;
							vertices2[i * 4 + 2] = vertices_[i].XYUV.Z;
							vertices2[i * 4 + 3] = vertices_[i].XYUV.W;
						}
						SetVertices(0, vertices2);	
					}
					{
						float[] vertices3 = new float[count * 4];
						for (int i = 0; i < count; ++i)
						{
							//1.0f;//
							vertices3[i * 4 + 0] = vertices_[i].Color.X;
							vertices3[i * 4 + 1] = vertices_[i].Color.Y;
							vertices3[i * 4 + 2] = vertices_[i].Color.Z;
							vertices3[i * 4 + 3] = vertices_[i].Color.W;
						}
						SetVertices(1, vertices3);
					}
				}
				else if (vertices is SirAwesome.Support.TextureTileMapManager.VertexData[])
				{
					SirAwesome.Support.TextureTileMapManager.VertexData[] vertices_ = (vertices as SirAwesome.Support.TextureTileMapManager.VertexData[]); //FIXME:
					{
						float[] vertices2 = new float[count * 2];
						for (int i = 0; i < count; ++i)
						{
							vertices2[i * 2 + 0] = vertices_[i].position.X;
							vertices2[i * 2 + 1] = vertices_[i].position.Y;
						}
						SetVertices(0, vertices2);	
					}
					{
						float[] vertices3 = new float[count * 2];
						for (int i = 0; i < count; ++i)
						{
							vertices3[i * 2 + 0] = vertices_[i].uv.X;
							vertices3[i * 2 + 1] = vertices_[i].uv.Y;
						}
						SetVertices(1, vertices3);
					}
				}
				else if (vertices is SirAwesome.Support.ParticleEffectsManager.VertexData[])
				{
					SirAwesome.Support.ParticleEffectsManager.VertexData[] vertices_ = (vertices as SirAwesome.Support.ParticleEffectsManager.VertexData[]); //FIXME:
					{
						float[] vertices2 = new float[count * 2];
						for (int i = 0; i < count; ++i)
						{
							vertices2[i * 2 + 0] = vertices_[i].position.X;
							vertices2[i * 2 + 1] = vertices_[i].position.Y;
						}
						SetVertices(0, vertices2);	
					}
					{
						float[] vertices3 = new float[count * 2];
						for (int i = 0; i < count; ++i)
						{
							vertices3[i * 2 + 0] = vertices_[i].uv.X;
							vertices3[i * 2 + 1] = vertices_[i].uv.Y;
						}
						SetVertices(1, vertices3);
					}
					{
						float[] vertices4 = new float[count * 4];
						for (int i = 0; i < count; ++i)
						{
							vertices4[i * 4 + 0] = vertices_[i].color.X;
							vertices4[i * 4 + 1] = vertices_[i].color.Y;
							vertices4[i * 4 + 2] = vertices_[i].color.Z;
							vertices4[i * 4 + 3] = vertices_[i].color.W;
						}
						SetVertices(2, vertices4);
					}
				}
				else if (vertices is PuzzleGameDemo.Support.ParticleEffectsManager.VertexData[])
				{
					PuzzleGameDemo.Support.ParticleEffectsManager.VertexData[] vertices_ = (vertices as PuzzleGameDemo.Support.ParticleEffectsManager.VertexData[]); //FIXME:
					{
						float[] vertices2 = new float[count * 2];
						for (int i = 0; i < count; ++i)
						{
							vertices2[i * 2 + 0] = vertices_[i].position.X;
							vertices2[i * 2 + 1] = vertices_[i].position.Y;
						}
						SetVertices(0, vertices2);	
					}
					{
						float[] vertices3 = new float[count * 2];
						for (int i = 0; i < count; ++i)
						{
							vertices3[i * 2 + 0] = vertices_[i].uv.X;
							vertices3[i * 2 + 1] = vertices_[i].uv.Y;
						}
						SetVertices(1, vertices3);
					}
					{
						float[] vertices4 = new float[count * 4];
						for (int i = 0; i < count; ++i)
						{
							vertices4[i * 4 + 0] = vertices_[i].color.X;
							vertices4[i * 4 + 1] = vertices_[i].color.Y;
							vertices4[i * 4 + 2] = vertices_[i].color.Z;
							vertices4[i * 4 + 3] = vertices_[i].color.W;
						}
						SetVertices(2, vertices4);
					}
				}
				else
				{
					Debug.Assert(false);
				}
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
		
		public void SetVertices (int stream, float[] vertices, int arg1, int arg2, int arg3)
		{
			if (arg1 == 0 && arg2 == 0)// && vertices.Length != arg3)
			{
//				if (vertices.Length != arg3)
//				{
//					Debug.Assert(false); //size不同 //FIXME:好像没影响 //arg3 * 3(float3) == vertices.Length
//				}
				SetVertices(stream, vertices);
			}
			else
			{
				Debug.Assert(false);
			}
		}
		
		//FIXME:???这个参考上面is UIColor[]
		public void SetVertices (int stream, UIColor[] vertices, int from, int to, int count)
		{
			if (to == 0 && from == 0)// && vertices.Length == count)
			{
				if (vertices is UIColor[])
				{
					UIColor[] vertices_ = (vertices as UIColor[]); //FIXME:
					{
						float[] vertices2 = new float[count * 4];
						for (int i = 0; i < count; ++i)
						{
							vertices2[i * 4 + 0] = vertices_[i].R;
							vertices2[i * 4 + 1] = vertices_[i].G;
							vertices2[i * 4 + 2] = vertices_[i].B;
							vertices2[i * 4 + 3] = vertices_[i].A;
						}
						SetVertices(stream, vertices2);
					}
				}
				else
				{
					Debug.Assert(false);
				}
			}
			else
			{
				Debug.Assert(false);
			}
		}
	}
}
