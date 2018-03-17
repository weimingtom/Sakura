using Sce.Pss.Core;
using Sce.Pss.Core.Graphics;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D.Base
{
	public class EmbeddedDebugFontData
	{
		public static Vector2i CharSizei = new Vector2i(8, 8);

		public static Vector2 CharSizef = EmbeddedDebugFontData.CharSizei.Vector2();

		public static int NumChars
		{
			get
			{
				return 95;
			}
		}

		public static Texture2D CreateTexture()
		{
			uint[] array = new uint[]
			{
				0u,
				0u,
				269488144u,
				1048576u,
				2631720u,
				0u,
				679225384u,
				2631804u,
				940865552u,
				1064016u,
				270552076u,
				6317064u,
				135533576u,
				5776468u,
				1056832u,
				0u,
				269492288u,
				4202512u,
				269486084u,
				264208u,
				272126992u,
				1070136u,
				2081427456u,
				4112u,
				0u,
				135270400u,
				2080374784u,
				0u,
				0u,
				1579008u,
				270548992u,
				1032u,
				1415857208u,
				3687500u,
				269752336u,
				8130576u,
				541082680u,
				8127512u,
				809518136u,
				3687488u,
				606613536u,
				2105468u,
				1077675132u,
				3687488u,
				1006897200u,
				3687492u,
				270550140u,
				1052688u,
				943998008u,
				3687492u,
				2017739832u,
				1581120u,
				1048576u,
				4096u,
				1048576u,
				135270400u,
				202911840u,
				6303768u,
				8126464u,
				124u,
				1613764620u,
				792624u,
				541082680u,
				1048592u,
				1416905784u,
				3671156u,
				1145317392u,
				4473980u,
				1011370044u,
				3950664u,
				67389488u,
				3164164u,
				1212688412u,
				1845320u,
				1006896252u,
				8127492u,
				1006896252u,
				263172u,
				1946436664u,
				3687492u,
				2084848708u,
				4473924u,
				269488184u,
				3674128u,
				538976368u,
				1582112u,
				202646596u,
				4465684u,
				67372036u,
				8127492u,
				1414818884u,
				4473924u,
				1414286404u,
				4482148u,
				1145324600u,
				3687492u,
				1011106876u,
				263172u,
				1145324600u,
				5776468u,
				1011106876u,
				4465684u,
				939803704u,
				3687488u,
				269488252u,
				1052688u,
				1145324612u,
				3687492u,
				1145324612u,
				1058856u,
				1413760068u,
				4484180u,
				271074372u,
				4473896u,
				943998020u,
				1052688u,
				270549116u,
				8127496u,
				269488240u,
				7344144u,
				268960768u,
				16416u,
				269488156u,
				1839120u,
				4466704u,
				0u,
				0u,
				8126464u,
				1050628u,
				0u,
				1077411840u,
				7881848u,
				1278477316u,
				3427396u,
				1144520704u,
				3687428u,
				1683505216u,
				5792836u,
				1144520704u,
				3671164u,
				2081443872u,
				1052688u,
				1683488768u,
				943741028u,
				1278477316u,
				4473924u,
				270008336u,
				3674128u,
				270008336u,
				202510352u,
				337904644u,
				2364428u,
				269488152u,
				3674128u,
				1412169728u,
				5526612u,
				1278476288u,
				4473924u,
				1144520704u,
				3687492u,
				1278476288u,
				67384396u,
				1683488768u,
				1077958756u,
				1278476288u,
				263172u,
				74973184u,
				3948604u,
				138151944u,
				3164168u,
				606339072u,
				5776420u,
				1145307136u,
				1058884u,
				1413742592u,
				2643028u,
				675545088u,
				4466704u,
				1145307136u,
				943741028u,
				544997376u,
				8128528u,
				67635248u,
				3147784u,
				269488144u,
				1052688u,
				537923596u,
				790544u,
				124u,
				0u
			};
			Vector2i vector2i = new Vector2i(1024, 8);
			byte[] array2 = new byte[vector2i.X * vector2i.Y];
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i] = 0;
			}
			for (int num = 0; num != EmbeddedDebugFontData.NumChars; num++)
			{
				for (int num2 = 0; num2 != EmbeddedDebugFontData.CharSizei.Y; num2++)
				{
					for (int num3 = 0; num3 != EmbeddedDebugFontData.CharSizei.X; num3++)
					{
						uint num4 = array[num * 2];
						uint num5 = array[num * 2 + 1];
						uint num6 = (uint)(num3 + EmbeddedDebugFontData.CharSizei.X * num2);
						bool flag;
						if (num6 < 32u)
						{
							flag = (((ulong)num4 & (ulong)(1L << (int)(num6 & 31u))) != 0uL);
						}
						else
						{
							flag = (((ulong)num5 & (ulong)(1L << (int)(num6 - 32u & 31u))) != 0uL);
						}
						array2[num * EmbeddedDebugFontData.CharSizei.X + num3 + num2 * vector2i.X] = (flag ? (byte)255 : (byte)0);
					}
				}
			}
			Texture2D texture2D = new Texture2D(vector2i.X, vector2i.Y, false, (PixelFormat)8);
			texture2D.SetPixels(0, array2, (PixelFormat)8);
			texture2D.SetFilter(0, 0, 0);
			return texture2D;
		}
	}
}
