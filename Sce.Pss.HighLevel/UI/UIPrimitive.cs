using Sce.Pss.Core;
using Sce.Pss.Core.Graphics;
using System;
using System.Collections.Generic;

namespace Sce.Pss.HighLevel.UI
{
	public class UIPrimitive : UIElement
	{
		private bool needUpdateVertexAll = true;

		private bool needUpdateIndices = true;

		private int vertexCount;

		private int maxIndexCount;

		private ushort[] indices;

		private int indexCount;

		private UIPrimitiveVertex[] vertices;

		private VertexBuffer vertexBuffer;

		public DrawMode DrawMode
		{
			get;
			set;
		}

		public int VertexCount
		{
			get
			{
				return this.vertexCount;
			}
			set
			{
				if (value < 0 || value > this.MaxVertexCount)
				{
					throw new ArgumentOutOfRangeException();
				}
				this.vertexCount = value;
				this.needUpdateVertexAll = true;
			}
		}

		public int MaxVertexCount
		{
			get;
			private set;
		}

		public int MaxIndexCount
		{
			get
			{
				return this.maxIndexCount;
			}
		}

		public int IndexCount
		{
			get
			{
				return this.indexCount;
			}
			set
			{
				if (value < 0 || value > this.maxIndexCount)
				{
					throw new ArgumentOutOfRangeException();
				}
				this.indexCount = value;
				this.needUpdateIndices = true;
			}
		}

		public UIPrimitive(DrawMode drawMode, int maxVertexCount) : this(drawMode, maxVertexCount, 0)
		{
		}

		public UIPrimitive(DrawMode drawMode, int maxVertexCount, int maxIndexCount)
		{
			this.DrawMode = drawMode;
			maxVertexCount = ((maxVertexCount > 0) ? maxVertexCount : 0);
			this.vertexCount = maxVertexCount;
			this.MaxVertexCount = maxVertexCount;
			this.vertices = new UIPrimitiveVertex[maxVertexCount];
			for (int i = 0; i < maxVertexCount; i++)
			{
				this.vertices[i] = new UIPrimitiveVertex();
			}
			this.indexCount = maxIndexCount;
			this.maxIndexCount = maxIndexCount;
			this.indices = new ushort[this.maxIndexCount];
			this.vertexBuffer = new VertexBuffer(maxVertexCount, maxIndexCount, new VertexFormat[]
			{
				VertexFormat.Float3,//258,
				VertexFormat.Float4,//259,
				VertexFormat.Float2 //257
			});
		}

		protected override void DisposeSelf()
		{
			if (this.vertexBuffer != null)
			{
				this.vertexBuffer.Dispose();
				this.vertexBuffer = null;
			}
			base.DisposeSelf();
		}

		public void SetIndices(ushort[] indices)
		{
			int num = (this.maxIndexCount > indices.Length) ? indices.Length : this.MaxIndexCount;
			for (int i = 0; i < num; i++)
			{
				this.indices[i] = indices[i];
			}
			this.needUpdateIndices = true;
		}

		public UIPrimitiveVertex GetVertex(int index)
		{
			return this.vertices[index];
		}

		protected internal override void Render()
		{
			if (!this.isExistRenderedVertex())
			{
				return;
			}
			GraphicsContext graphicsContext = UISystem.GraphicsContext;
			ShaderProgramManager.ShaderProgramUnit shaderProgramUnit = ShaderProgramManager.GetShaderProgramUnit(base.InternalShaderType);
			graphicsContext.SetShaderProgram(shaderProgramUnit.ShaderProgram);
			Texture2D texture = base.GetTexture();
			graphicsContext.SetTexture(0, texture);
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			for (int i = 0; i < this.vertexCount; i++)
			{
				UIPrimitiveVertex uIPrimitiveVertex = this.vertices[i];
				flag |= uIPrimitiveVertex.NeedUpdatePosition;
				flag2 |= uIPrimitiveVertex.NeedUpdateColor;
				flag3 |= uIPrimitiveVertex.NeedUpdateTexcoord;
				uIPrimitiveVertex.NeedUpdatePosition = false;
				uIPrimitiveVertex.NeedUpdateColor = false;
				uIPrimitiveVertex.NeedUpdateTexcoord = false;
			}
			if (this.needUpdateVertexAll || flag)
			{
				float[] array = new float[this.VertexCount * 3];
				int num = 0;
				for (int j = 0; j < this.VertexCount; j++)
				{
					UIPrimitiveVertex uIPrimitiveVertex2 = this.vertices[j];
					array[num++] = uIPrimitiveVertex2.X;
					array[num++] = uIPrimitiveVertex2.Y;
					array[num++] = uIPrimitiveVertex2.Z;
				}
				this.vertexBuffer.SetVertices(0, array, 0, 0, this.VertexCount);
			}
			if (this.needUpdateVertexAll || flag)
			{
				float[] array2 = new float[this.VertexCount * 4];
				int num2 = 0;
				for (int k = 0; k < this.VertexCount; k++)
				{
					UIPrimitiveVertex uIPrimitiveVertex3 = this.vertices[k];
					array2[num2++] = uIPrimitiveVertex3.Color.R;
					array2[num2++] = uIPrimitiveVertex3.Color.G;
					array2[num2++] = uIPrimitiveVertex3.Color.B;
					array2[num2++] = uIPrimitiveVertex3.Color.A;
				}
				this.vertexBuffer.SetVertices(1, array2, 0, 0, this.VertexCount);
			}
			if (texture != null)
			{
				if (this.needUpdateVertexAll || flag3)
				{
					float[] array3 = new float[this.VertexCount * 2];
					int num3 = 0;
					for (int l = 0; l < this.VertexCount; l++)
					{
						UIPrimitiveVertex uIPrimitiveVertex4 = this.vertices[l];
						array3[num3++] = uIPrimitiveVertex4.U;
						array3[num3++] = uIPrimitiveVertex4.V;
					}
					this.vertexBuffer.SetVertices(2, array3, 0, 0, this.VertexCount);
				}
			}
			else if ((this.needUpdateVertexAll || flag3) && this.vertexCount > 0)
			{
				this.vertices[0].NeedUpdateTexcoord = true;
			}
			if (this.needUpdateIndices)
			{
				if (this.indexCount > 0)
				{
					this.vertexBuffer.SetIndices(this.indices, 0, 0, this.indexCount);
				}
				this.needUpdateIndices = false;
			}
			this.needUpdateVertexAll = false;
			graphicsContext.SetVertexBuffer(0, this.vertexBuffer);
			this.updateLocalToWorld();
			Matrix4 matrix;
			UISystem.viewProjectionMatrix.Multiply(ref this.localToWorld, out matrix);
			shaderProgramUnit.ShaderProgram.SetUniformValue(shaderProgramUnit.UniformIndexOfModelViewProjection, ref matrix);
			shaderProgramUnit.ShaderProgram.SetUniformValue(shaderProgramUnit.UniformIndexOfAlpha, this.finalAlpha);
			using (List<string>.Enumerator enumerator = shaderProgramUnit.OtherUniformNames.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string current = enumerator.Current;
					if (base.ShaderUniforms.ContainsKey(current))
					{
						shaderProgramUnit.ShaderProgram.SetUniformValue(shaderProgramUnit.Uniforms[current], base.ShaderUniforms[current]);
					}
				}
			}
			int num4 = (this.indexCount > 0) ? this.indexCount : this.VertexCount;
			graphicsContext.DrawArrays(this.DrawMode, 0, num4);
		}

		private bool isExistRenderedVertex()
		{
			return this.VertexCount != 0 && this.finalAlpha >= 0.003921569f;
		}
	}
}
