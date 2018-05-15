using Sce.Pss.Core;
using Sce.Pss.Core.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sce.Pss.HighLevel.UI
{
	public class UISprite : UIElement
	{
		public static bool __USE_SampleDraw = false;
		
		public string __name;
		
		private bool needUpdateVertexAll = true;

		private int unitCount;

		private UISpriteUnit[] units;

		private VertexBuffer vertexBuffer;

		public int UnitCount
		{
			get
			{
				return this.unitCount;
			}
			set
			{
				if (value < 0 || value > this.MaxUnitCount)
				{
					throw new ArgumentOutOfRangeException("UnitCount");
				}
				this.unitCount = value;
				this.needUpdateVertexAll = true;
			}
		}

		public int MaxUnitCount
		{
			get;
			private set;
		}

		public UISprite(int maxUnitCount)
		{
			maxUnitCount = ((maxUnitCount < 1) ? 1 : maxUnitCount);
			this.MaxUnitCount = maxUnitCount;
			this.unitCount = maxUnitCount;
			this.units = new UISpriteUnit[maxUnitCount];
			for (int i = 0; i < maxUnitCount; i++)
			{
				this.units[i] = new UISpriteUnit();
			}
			this.vertexBuffer = new VertexBuffer(4 * maxUnitCount, 6 * maxUnitCount - 2, new VertexFormat[]
			{
			    VertexFormat.Float3, //258u,
				VertexFormat.Float4, //259u,
				VertexFormat.Float2 //257u
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

		public UISpriteUnit GetUnit(int index)
		{
			return this.units[index];
		}
		protected internal override void Render()
		{
			if (!this.isExistRenderedUnit())
			{
				return;
			}
			
//			if (__USE_SampleDraw)
//			{
//				Sample.SampleDraw.DrawText("UISprite test", 0xffffffff, 0, 100); //FIXME:
//			}			
			
			GraphicsContext graphicsContext = UISystem.GraphicsContext;
			ShaderProgramManager.ShaderProgramUnit shaderProgramUnit = ShaderProgramManager.GetShaderProgramUnit(base.InternalShaderType);
			graphicsContext.SetShaderProgram(shaderProgramUnit.ShaderProgram);
			Texture2D texture = base.GetTexture();
			if (__USE_SampleDraw)
			{
				texture = Sample.SampleDraw.__DrawText("1234 test", 0xffffffff, 0, 0, new Matrix4(), true);
			}
			graphicsContext.SetTexture(0, texture);
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			for (int i = 0; i < this.unitCount; i++)
			{
				UISpriteUnit uISpriteUnit = this.units[i];
				flag |= uISpriteUnit.NeedUpdatePosition;
				flag2 |= uISpriteUnit.NeedUpdateColor;
				flag3 |= uISpriteUnit.NeedUpdateTexcoord;
				uISpriteUnit.NeedUpdatePosition = false;
				uISpriteUnit.NeedUpdateColor = false;
				uISpriteUnit.NeedUpdateTexcoord = false;
			}
			if (this.needUpdateVertexAll | flag)
			{
				float[] array = new float[12 * this.UnitCount];
				int num = 0;
				for (int j = 0; j < this.unitCount; j++)
				{
					UISpriteUnit unit = this.GetUnit(j);
					array[num++] = unit.X;
					array[num++] = unit.Y;
					array[num++] = unit.Z;
					
					array[num++] = unit.X;
					array[num++] = unit.Y + unit.Height;
					array[num++] = unit.Z;
					
					array[num++] = unit.X + unit.Width;
					array[num++] = unit.Y;
					array[num++] = unit.Z;
					
					array[num++] = unit.X + unit.Width;
					array[num++] = unit.Y + unit.Height;
					array[num++] = unit.Z;
				}
				this.vertexBuffer.SetVertices(0, array, 0, 0, 4 * this.UnitCount); //len==???  //>attrib['a_Position'].location == 0
			}
			if (this.needUpdateVertexAll | flag2)
			{
				UIColor[] array2 = new UIColor[4 * this.UnitCount];
				int num2 = 0;
				for (int k = 0; k < this.unitCount; k++)
				{
					UISpriteUnit uISpriteUnit2 = this.units[k];
					array2[num2++] = uISpriteUnit2.Color;
					array2[num2++] = uISpriteUnit2.Color;
					array2[num2++] = uISpriteUnit2.Color;
					array2[num2++] = uISpriteUnit2.Color;
				}
				this.vertexBuffer.SetVertices(1, array2, 0, 0, 4 * this.UnitCount); //len==4  //>attrib['a_Color'].location == 1
			}
			if (texture != null)
			{
				if (this.needUpdateVertexAll | flag3)
				{
					float[] array3 = new float[8 * this.UnitCount];
					int num3 = 0;
					for (int l = 0; l < this.unitCount; l++)
					{
						UISpriteUnit uISpriteUnit3 = this.units[l];
						array3[num3++] = uISpriteUnit3.U1;
						array3[num3++] = uISpriteUnit3.V1;
						array3[num3++] = uISpriteUnit3.U1;
						array3[num3++] = uISpriteUnit3.V2;
						array3[num3++] = uISpriteUnit3.U2;
						array3[num3++] = uISpriteUnit3.V1;
						array3[num3++] = uISpriteUnit3.U2;
						array3[num3++] = uISpriteUnit3.V2;
					}
					this.vertexBuffer.SetVertices(2, array3, 0, 0, 4 * this.UnitCount);
				}
			}
			else if ((this.needUpdateVertexAll || flag3) && this.unitCount > 0)
			{
				this.units[0].NeedUpdateTexcoord = true;
			}
			if (this.needUpdateVertexAll)
			{
				ushort[] array4 = new ushort[this.UnitCount * 6 - 2];
				ushort num4 = 0;
				int num5 = 0;
				while (true)
				{
					ushort[] arg_3E8_0 = array4;
					int expr_3DB = num5++;
					ushort expr_3E2 = num4;
					num4 = (ushort)(expr_3E2 + 1);
					arg_3E8_0[expr_3DB] = expr_3E2;
					ushort[] arg_3FA_0 = array4;
					int expr_3ED = num5++;
					ushort expr_3F4 = num4;
					num4 = (ushort)(expr_3F4 + 1);
					arg_3FA_0[expr_3ED] = expr_3F4;
					ushort[] arg_40C_0 = array4;
					int expr_3FF = num5++;
					ushort expr_406 = num4;
					num4 = (ushort)(expr_406 + 1);
					arg_40C_0[expr_3FF] = expr_406;
					array4[num5++] = num4;
					if (num5 >= array4.Length)
					{
						break;
					}
					ushort[] arg_432_0 = array4;
					int expr_425 = num5++;
					ushort expr_42C = num4;
					num4 = (ushort)(expr_42C + 1);
					arg_432_0[expr_425] = expr_42C;
					array4[num5++] = num4;
				}
				this.vertexBuffer.SetIndices(array4, 0, 0, array4.Length);
			}
			this.needUpdateVertexAll = false;
			graphicsContext.SetVertexBuffer(0, this.vertexBuffer);
			this.updateLocalToWorld();
			Matrix4 matrix;
			UISystem.viewProjectionMatrix.Multiply(ref this.localToWorld, out matrix);
			shaderProgramUnit.ShaderProgram.SetUniformValue(shaderProgramUnit.UniformIndexOfModelViewProjection, ref matrix);
			shaderProgramUnit.ShaderProgram.SetUniformValue(shaderProgramUnit.UniformIndexOfAlpha, this.finalAlpha);
//			Debug.WriteLine(">>>>>>><<<<<<<<<>>>>>>> base.ShaderUniforms.Count : " + base.ShaderUniforms.Count);
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
				
			
//			if (false)
//			{
//			    graphicsContext.Enable(EnableMode.Blend);
//			    graphicsContext.SetBlendFunc(BlendFuncMode.Add, BlendFuncFactor.SrcAlpha, BlendFuncFactor.OneMinusSrcAlpha);
//			}
			
			graphicsContext.DrawArrays((DrawMode)4, 0, this.UnitCount * 6 - 2);
//			Debug.WriteLine(">>>>>>>>>>>>>>" + (this.UnitCount * 6 - 2));
			
//			if (__USE_SampleDraw)
//			{
//				Sample.SampleDraw.__DrawText("UISprite test", 0xffffffff, 0, 0, matrix, false); //FIXME:
//			}
		}

		private bool isExistRenderedUnit()
		{
			if (this.unitCount == 0)
			{
				return false;
			}
			if (this.finalAlpha < 0.003921569f)
			{
				return false;
			}
			bool result = false;
			for (int i = 0; i < this.unitCount; i++)
			{
				UISpriteUnit uISpriteUnit = this.units[i];
				if (uISpriteUnit.Color.A * this.finalAlpha >= 0.003921569f && uISpriteUnit.Width > 0f && uISpriteUnit.Height > 0f)
				{
					result = true;
					break;
				}
			}
			return result;
		}
	}
}
