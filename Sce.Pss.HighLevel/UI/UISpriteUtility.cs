using System;

namespace Sce.Pss.HighLevel.UI
{
	public static class UISpriteUtility
	{
		public static void SetupNinePatch(UISprite sprite, float width, float height, float offsetX, float offsetY, NinePatchMargin ninePatchMargin)
		{
			if (sprite.Image != null)
			{
				int width2 = sprite.Image.Width;
				int height2 = sprite.Image.Height;
				if ((float)width2 == 0f || (float)height2 == 0f)
				{
					return;
				}
				float num = (float)width2;
				float num2 = (float)height2;
				float num3 = width - (float)(ninePatchMargin.Left + ninePatchMargin.Right);
				float num4 = height - (float)(ninePatchMargin.Top + ninePatchMargin.Bottom);
				if (num3 < 0f)
				{
					num3 = 0f;
				}
				if (num4 < 0f)
				{
					num4 = 0f;
				}
				float[] array = new float[]
				{
					0f,
					(float)ninePatchMargin.Left,
					(float)ninePatchMargin.Left + num3
				};
				float[] array2 = new float[]
				{
					0f,
					(float)ninePatchMargin.Top,
					(float)ninePatchMargin.Top + num4
				};
				float[] array3 = new float[]
				{
					(float)ninePatchMargin.Left,
					num3,
					(float)ninePatchMargin.Right
				};
				float[] array4 = new float[]
				{
					(float)ninePatchMargin.Top,
					num4,
					(float)ninePatchMargin.Bottom
				};
				float[] array5 = new float[]
				{
					0f,
					(float)ninePatchMargin.Left,
					num - (float)ninePatchMargin.Right
				};
				float[] array6 = new float[]
				{
					0f,
					(float)ninePatchMargin.Top,
					num2 - (float)ninePatchMargin.Bottom
				};
				float[] array7 = new float[]
				{
					array3[0],
					num - (float)(ninePatchMargin.Left + ninePatchMargin.Right),
					array3[2]
				};
				float[] array8 = new float[]
				{
					array4[0],
					num2 - (float)(ninePatchMargin.Top + ninePatchMargin.Bottom),
					array4[2]
				};
				if (array5[2] < 0f)
				{
					array5[2] = 0f;
				}
				if (array6[2] < 0f)
				{
					array6[2] = 0f;
				}
				if (array7[1] < 0f)
				{
					array7[1] = 0f;
				}
				if (array8[1] < 0f)
				{
					array8[1] = 0f;
				}
				if (array[0] + array3[0] > width)
				{
					if (array[0] > width)
					{
						array3[0] = 0f;
					}
					else
					{
						array3[0] = (array7[0] = width - array[0]);
					}
				}
				if (array2[0] + array4[0] > height)
				{
					if (array2[0] > height)
					{
						array4[0] = 0f;
					}
					else
					{
						array4[0] = (array8[0] = height - array2[0]);
					}
				}
				if (array[2] + array3[2] > width)
				{
					if (array[2] > width)
					{
						array3[2] = 0f;
					}
					else
					{
						array3[2] = (array7[2] = width - array[2]);
					}
				}
				if (array2[2] + array4[2] > height)
				{
					if (array2[2] > height)
					{
						array4[2] = 0f;
					}
					else
					{
						array4[2] = (array8[2] = height - array2[2]);
					}
				}
				float[] array9 = new float[]
				{
					array5[0] / num,
					array5[1] / num,
					array5[2] / num
				};
				float[] array10 = new float[]
				{
					array6[0] / num2,
					array6[1] / num2,
					array6[2] / num2
				};
				float[] array11 = new float[]
				{
					(array5[0] + array7[0]) / num,
					(array5[1] + array7[1]) / num,
					(array5[2] + array7[2]) / num
				};
				float[] array12 = new float[]
				{
					(array6[0] + array8[0]) / num2,
					(array6[1] + array8[1]) / num2,
					(array6[2] + array8[2]) / num2
				};
				for (int i = 0; i < 3; i++)
				{
					for (int j = 0; j < 3; j++)
					{
						int index = i * 3 + j;
						UISpriteUnit unit = sprite.GetUnit(index);
						unit.X = array[j] + offsetX;
						unit.Y = array2[i] + offsetY;
						unit.Width = array3[j];
						unit.Height = array4[i];
						unit.U1 = array9[j];
						unit.V1 = array10[i];
						unit.U2 = array11[j];
						unit.V2 = array12[i];
					}
				}
			}
		}

		public static void SetupHorizontalThreePatch(UISprite sprite, float width, float height, float leftMargin, float rightMargin)
		{
			if (sprite.Image != null)
			{
				int width2 = sprite.Image.Width;
				int height2 = sprite.Image.Height;
				if ((float)width2 == 0f || (float)height2 == 0f)
				{
					return;
				}
				float num = (float)sprite.Image.Width;
				float num2 = width - (leftMargin + rightMargin);
				float num3 = height;
				if (num2 < 0f)
				{
					num2 = 0f;
				}
				if (num3 < 0f)
				{
					num3 = 0f;
				}
				float[] array = new float[]
				{
					0f,
					leftMargin,
					leftMargin + num2
				};
				float[] array2 = new float[]
				{
					leftMargin,
					num2,
					rightMargin
				};
				float[] array3 = new float[]
				{
					0f,
					leftMargin,
					num - rightMargin
				};
				float[] array4 = new float[]
				{
					array2[0],
					num - (leftMargin + rightMargin),
					array2[2]
				};
				if (array3[2] < 0f)
				{
					array3[2] = 0f;
				}
				if (array4[1] < 0f)
				{
					array4[1] = 0f;
				}
				if (array[0] + array2[0] > width)
				{
					if (array[0] > width)
					{
						array2[0] = 0f;
					}
					else
					{
						array2[0] = (array4[0] = width - array[0]);
					}
				}
				if (array[2] + array2[2] > width)
				{
					if (array[2] > width)
					{
						array2[2] = 0f;
					}
					else
					{
						array2[2] = (array4[2] = width - array[2]);
					}
				}
				float[] array5 = new float[]
				{
					array3[0] / num,
					array3[1] / num,
					array3[2] / num
				};
				float[] array6 = new float[]
				{
					(array3[0] + array4[0]) / num,
					(array3[1] + array4[1]) / num,
					(array3[2] + array4[2]) / num
				};
				for (int i = 0; i < 3; i++)
				{
					UISpriteUnit unit = sprite.GetUnit(i);
					unit.X = array[i];
					unit.Y = 0f;
					unit.Width = array2[i];
					unit.Height = num3;
					unit.U1 = array5[i];
					unit.V1 = 0f;
					unit.U2 = array6[i];
					unit.V2 = 1f;
				}
			}
		}
	}
}
