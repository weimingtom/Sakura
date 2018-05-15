using Sce.Pss.Core.Imaging;
using System;

namespace Sce.Pss.HighLevel.UI
{
	public static class UIPrimitiveUtility
	{
		public static void SetupNinePatch(UIPrimitive primitive, float width, float height, float offsetX, float offsetY, NinePatchMargin ninePatchMargin)
		{
			if (ninePatchMargin.Top == 0 && ninePatchMargin.Bottom == 0)
			{
				UIPrimitiveUtility.SetupHorizontalThreePatch(primitive, width, height, offsetX, offsetY, (float)ninePatchMargin.Left, (float)ninePatchMargin.Right);
				return;
			}
			if (ninePatchMargin.Left == 0 && ninePatchMargin.Right == 0)
			{
				UIPrimitiveUtility.SetupVerticalThreePatch(primitive, width, height, offsetX, offsetY, (float)ninePatchMargin.Top, (float)ninePatchMargin.Bottom);
				return;
			}
			UIPrimitiveUtility.SetupNinePatch(primitive, width, height, offsetX, offsetY, default(ImageRect), ninePatchMargin);
		}

		public static void SetupNinePatch(UIPrimitive primitive, float width, float height, float offsetX, float offsetY, ImageRect imageRect, NinePatchMargin ninePatchMargin)
		{
			if (primitive.MaxVertexCount < 16)
			{
				throw new ArgumentOutOfRangeException("primitive", "UIPrimitive MaxVertexCount is out of range.");
			}
			if (primitive.MaxIndexCount < 28)
			{
				throw new ArgumentOutOfRangeException("primitive", "UIPrimitive MaxIndexCount is out of range.");
			}
			if (primitive.Image != null)
			{
				float[] array = new float[]
				{
					0f,
					(float)ninePatchMargin.Left,
					width - (float)ninePatchMargin.Right,
					width
				};
				float[] array2 = new float[]
				{
					0f,
					(float)ninePatchMargin.Top,
					height - (float)ninePatchMargin.Bottom,
					height
				};
				if (array[1] > width)
				{
					array[1] = width;
				}
				if (array[2] < 0f)
				{
					array[2] = 0f;
				}
				if (array[1] > array[2])
				{
					array[1] = (array[2] = (array[1] + array[2]) / 2f);
				}
				if (array2[1] > height)
				{
					array2[1] = height;
				}
				if (array2[2] < 0f)
				{
					array2[2] = 0f;
				}
				if (array2[1] > array2[2])
				{
					array2[1] = (array2[2] = (array2[1] + array2[2]) / 2f);
				}
				int num = 0;
				int num2 = 0;
				int width2 = primitive.Image.Width;
				int height2 = primitive.Image.Height;
				int num3 = width2;
				int num4 = height2;
				if (imageRect.X < width2 && imageRect.Y < height2 && imageRect.Width > 0 && imageRect.Height > 0)
				{
					if (imageRect.X > 0)
					{
						num = imageRect.X;
					}
					num3 = num + imageRect.Width;
					if (num3 > width2)
					{
						num3 = width2;
					}
					if (imageRect.Y > 0)
					{
						num2 = imageRect.Y;
					}
					num4 = num2 + imageRect.Height;
					if (num4 > height2)
					{
						num4 = height2;
					}
				}
				float num5 = (float)width2;
				float num6 = (float)height2;
				float num7 = ((float)ninePatchMargin.Left < array[1] - array[0]) ? ((float)(num + ninePatchMargin.Left)) : ((float)num + array[1] - array[0]);
				float num8 = ((float)ninePatchMargin.Right < array[3] - array[2]) ? ((float)(num3 - ninePatchMargin.Right)) : ((float)num3 - (array[3] - array[2]));
				float num9 = ((float)ninePatchMargin.Top < array2[1] - array2[0]) ? ((float)(num2 + ninePatchMargin.Top)) : ((float)num2 + array2[1] - array2[0]);
				float num10 = ((float)ninePatchMargin.Bottom < array2[3] - array2[2]) ? ((float)(num4 - ninePatchMargin.Bottom)) : ((float)num4 - (array2[3] - array2[2]));
				float[] array3 = new float[]
				{
					(float)num / num5,
					num7 / num5,
					num8 / num5,
					(float)num3 / num5
				};
				float[] array4 = new float[]
				{
					(float)num2 / num6,
					num9 / num6,
					num10 / num6,
					(float)num4 / num6
				};
				if (array3[2] < array3[0])
				{
					array3[2] = array3[0];
				}
				if (array4[2] < array4[0])
				{
					array4[2] = array4[0];
				}
				if (array3[1] > array3[3])
				{
					array3[1] = array3[3];
				}
				if (array4[1] > array4[3])
				{
					array4[1] = array4[3];
				}
				primitive.VertexCount = 16;
				primitive.SetIndices(new ushort[]
				{
					0,
					4,
					1,
					5,
					2,
					6,
					3,
					7,
					7,
					4,
					4,
					8,
					5,
					9,
					6,
					10,
					7,
					11,
					11,
					8,
					8,
					12,
					9,
					13,
					10,
					14,
					11,
					15
				});
				primitive.IndexCount = 28;
				for (int i = 0; i < 4; i++)
				{
					for (int j = 0; j < 4; j++)
					{
						UIPrimitiveVertex vertex = primitive.GetVertex(i * 4 + j);
						vertex.X = array[j] + offsetX;
						vertex.Y = array2[i] + offsetY;
						vertex.U = array3[j];
						vertex.V = array4[i];
					}
				}
			}
		}

		public static void SetupHorizontalThreePatch(UIPrimitive primitive, float width, float height, float offsetX, float offsetY, float leftMargin, float rightMargin)
		{
			if (primitive.MaxVertexCount < 8)
			{
				throw new ArgumentOutOfRangeException("primitive", "UIPrimitive MaxVertexCount is out of range.");
			}
			if (primitive.Image != null)
			{
				int width2 = primitive.Image.Width;
				if ((float)width2 == 0f)
				{
					return;
				}
				float num = (float)width2;
				float[] array = new float[]
				{
					0f,
					(leftMargin < width) ? leftMargin : width,
					(rightMargin < width) ? (width - rightMargin) : 0f,
					width
				};
				if (array[1] > array[2])
				{
					array[1] = (array[2] = (array[1] + array[2]) / 2f);
				}
				float[] array2 = new float[]
				{
					0f,
					height
				};
				float num2 = (leftMargin < array[1]) ? leftMargin : array[1];
				float num3 = (rightMargin < width - array[2]) ? rightMargin : (width - array[2]);
				float[] array3 = new float[]
				{
					0f,
					num2 / num,
					(num - num3) / num,
					1f
				};
				float[] array4 = new float[]
				{
					0f,
					1f
				};
				if (array3[2] < 0f)
				{
					array3[2] = 0f;
				}
				if (array3[1] > 1f)
				{
					array3[1] = 1f;
				}
				primitive.VertexCount = 8;
				primitive.SetIndices(new ushort[]
				{
					0,
					4,
					1,
					5,
					2,
					6,
					3,
					7
				});
				primitive.IndexCount = 8;
				for (int i = 0; i < 2; i++)
				{
					for (int j = 0; j < 4; j++)
					{
						UIPrimitiveVertex vertex = primitive.GetVertex(i * 4 + j);
						vertex.X = array[j] + offsetX;
						vertex.Y = array2[i] + offsetY;
						vertex.U = array3[j];
						vertex.V = array4[i];
					}
				}
			}
		}

		public static void SetupVerticalThreePatch(UIPrimitive primitive, float width, float height, float offsetX, float offsetY, float topMargin, float bottomMargin)
		{
			if (primitive.MaxVertexCount < 8)
			{
				throw new ArgumentOutOfRangeException("primitive", "UIPrimitive MaxVertexCount is out of range.");
			}
			if (primitive.Image != null)
			{
				int height2 = primitive.Image.Height;
				if ((float)height2 == 0f)
				{
					return;
				}
				float num = (float)height2;
				float[] array = new float[]
				{
					0f,
					width
				};
				float[] array2 = new float[]
				{
					0f,
					(topMargin < height) ? topMargin : height,
					(bottomMargin < height) ? (height - bottomMargin) : 0f,
					height
				};
				if (array2[1] > array2[2])
				{
					array2[1] = (array2[2] = (array2[1] + array2[2]) / 2f);
				}
				float[] array3 = new float[]
				{
					0f,
					1f
				};
				float num2 = (topMargin < array2[1]) ? topMargin : array2[1];
				float num3 = (bottomMargin < height - array2[2]) ? bottomMargin : (height - array2[2]);
				float[] array4 = new float[]
				{
					0f,
					num2 / num,
					(num - num3) / num,
					1f
				};
				if (array4[2] < 0f)
				{
					array4[2] = 0f;
				}
				if (array4[1] > 1f)
				{
					array4[1] = 1f;
				}
				primitive.VertexCount = 8;
				primitive.SetIndices(new ushort[]
				{
					0,
					1,
					2,
					3,
					4,
					5,
					6,
					7
				});
				primitive.IndexCount = 8;
				for (int i = 0; i < 4; i++)
				{
					for (int j = 1; j >= 0; j--)
					{
						UIPrimitiveVertex vertex = primitive.GetVertex(i * 2 + j);
						vertex.X = array[j] + offsetX;
						vertex.Y = array2[i] + offsetY;
						vertex.U = array3[j];
						vertex.V = array4[i];
					}
				}
			}
		}
	}
}
